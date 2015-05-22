using StructureMap;

namespace CqFramework.Implementation.StructureMap {
    internal class CommandDispatcher : ICommandDispatcher {
        private readonly IContainer container;

        public CommandDispatcher(IContainer container) {
            this.container = container;
        }

        public void Dispatch<TCommand>(TCommand command) where TCommand : ICommand {
            var handler = this.container.TryGetInstance<ICommandHandler<TCommand>>();
            if (handler == null)
            {
                throw new NoCommandHandlerRegisteredException(string.Format("No command handler for {0} is registered",
                    command.GetType()));
            }
            handler.Execute(command);
        }
    }
}
