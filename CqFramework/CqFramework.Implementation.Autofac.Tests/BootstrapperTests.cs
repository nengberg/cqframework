using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Autofac;

using NUnit.Framework;

namespace CqFramework.Implementation.Autofac.Tests {
	[TestFixture]
	public class BootstrapperTests {
		[Test]
		public void Autofac_CanResolve_AllQueryHandlers() {
			var containerBuilder = new ContainerBuilder();
			Bootstrapping.Configure(containerBuilder);
			var container = containerBuilder.Build();

			var unRegisteredTypes = new List<Type>();
			foreach(var allAssemblies in AppDomain.CurrentDomain.GetAssemblies().Where(IsNotTestAssembly)) {
				foreach(var queryType in allAssemblies.GetTypes().Where(t => t.IsClass && t.IsClosedTypeOf(typeof(IQuery<>)))) {
					var isRegistered = false;

					var resultType = queryType.GetInterfaces()
						.Single(i => i.IsClosedTypeOf(typeof(IQuery<>)))
						.GetGenericArguments().Single();

					var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, resultType);
					isRegistered = container.IsRegistered(handlerType);
					if(!isRegistered) {
						unRegisteredTypes.Add(handlerType);
					}
				}
			}
			var sb = PrintUnregisteredTypes(unRegisteredTypes);
			Assert.IsTrue(!unRegisteredTypes.Any(), 
				string.Format("There are {0} query handlers that is not registered.\r\n{1}", unRegisteredTypes.Count, sb));
		}

		[Test]
		public void Autofac_CanResolve_AllCommandHandlers() {
			var containerBuilder = new ContainerBuilder();
			Bootstrapping.Configure(containerBuilder);
			var container = containerBuilder.Build();

			var unRegisteredTypes = new List<Type>();

			foreach(var allAssemblies in AppDomain.CurrentDomain.GetAssemblies().Where(IsNotTestAssembly)) {
				foreach(var command in allAssemblies.GetTypes().Where(t => t.IsClass && typeof(ICommand).IsAssignableFrom(t))) {
					var isRegistered = false;

					var handlerType = typeof(ICommandHandler<>).MakeGenericType(command);
					isRegistered = container.IsRegistered(handlerType);
					if(!isRegistered) {
						unRegisteredTypes.Add(handlerType);
					}
				}
			}

			var sb = PrintUnregisteredTypes(unRegisteredTypes);
			Assert.IsTrue(!unRegisteredTypes.Any(), 
				string.Format("There are {0} command handlers that is not registered.\r\n{1}", unRegisteredTypes.Count, sb));
		}

		private static StringBuilder PrintUnregisteredTypes(List<Type> unRegisteredTypes) {
			var sb = new StringBuilder();
			foreach(var unRegisteredType in unRegisteredTypes) {
				sb.Append(unRegisteredType.FullName);
				sb.AppendLine();
			}
			return sb;
		}

		private static bool IsNotTestAssembly(Assembly assembly) {
			return assembly != Assembly.GetExecutingAssembly();
		}
	}
}