using StructureMap;
using StructureMap.Configuration.DSL;

namespace CqFramework.Implementation.StructureMap {
    public class Bootstrapping {
        public static void Configure(IContainer container) {
            container.Configure(x => {
                x.For<IQueryProcessor>().Use<QueryProcessor>();
                x.For<ICommandDispatcher>().Use<CommandDispatcher>();
                x.AddRegistry(new QueryHandlerImplementationsRegistry());
                x.AddRegistry(new CommandHandlerImplementationsRegistry());
            });
        }
    }

    internal class QueryHandlerImplementationsRegistry : Registry {
        public QueryHandlerImplementationsRegistry() {
            Scan(x => {
                x.AssembliesFromApplicationBaseDirectory();
                x.ConnectImplementationsToTypesClosing(typeof(IQueryHandler<,>));
                x.WithDefaultConventions();
                x.ExcludeNamespaceContainingType<IContainer>();
            });
        }
    }

    internal class CommandHandlerImplementationsRegistry : Registry {
        public CommandHandlerImplementationsRegistry() {
            Scan(x => {
                x.AssembliesFromApplicationBaseDirectory();
                x.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<>));
                x.WithDefaultConventions();
                x.ExcludeNamespaceContainingType<IContainer>();
            });
        }
    }
}
