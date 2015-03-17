using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
			foreach(var unRegisteredType in unRegisteredTypes) {
				Console.Write(unRegisteredType.FullName);
				Console.WriteLine();
			}
			Assert.IsTrue(!unRegisteredTypes.Any(), string.Format("There are {0} query handlers that is not registered", unRegisteredTypes.Count));
		}

		private bool IsNotTestAssembly(Assembly assembly) {
			return assembly != Assembly.GetExecutingAssembly();
		}
	}
}