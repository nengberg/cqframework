using System;

using Autofac;

namespace CqFramework.Implementation.Autofac {
	public class Bootstrapping {
		public static void Configure(ContainerBuilder containerBuilder) {
			containerBuilder.RegisterType<QueryProcessor>().As<IQueryProcessor>();
			containerBuilder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>();

			containerBuilder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
				.Where(t => t.Name.EndsWith("QueryHandler"))
				.AsImplementedInterfaces();

			containerBuilder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
				.Where(t => t.Name.EndsWith("CommandHandler"))
				.AsImplementedInterfaces();
		}
	}
}