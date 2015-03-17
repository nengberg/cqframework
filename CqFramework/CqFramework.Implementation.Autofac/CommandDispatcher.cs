using Autofac;
using Autofac.Core.Registration;

namespace CqFramework.Implementation.Autofac {
	internal class CommandDispatcher : ICommandDispatcher {
		private readonly ILifetimeScope lifetimeScope;

		public CommandDispatcher(ILifetimeScope lifetimeScope) {
			this.lifetimeScope = lifetimeScope;
		}

		public void Dispatch<TCommand>(TCommand command) where TCommand : ICommand {
			try {
				var handler = this.lifetimeScope.Resolve<ICommandHandler<TCommand>>();
				handler.Execute(command);
			} catch(ComponentNotRegisteredException) {
				throw new NoCommandHandlerRegisteredException(string.Format("No command handler for {0} is registered", command.GetType()));
			}
		}
	}
}