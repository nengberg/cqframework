using System;

using Autofac;

using FakeItEasy;

using FluentAssertions;

using NUnit.Framework;

namespace CqFramework.Implementation.Autofac.Tests {
	[TestFixture]
	public class CommandDispatcherTests {
		private ILifetimeScope scope;
		private ContainerBuilder builder;
		private ICommandHandler<FakeCommand> handler;

		[SetUp]
		public void SetUp() {
			this.builder = new ContainerBuilder();
			this.handler = A.Fake<ICommandHandler<FakeCommand>>();
		}

		[Test]
		public void Dispatch_CommandHasHandler_DispatchesCorrectCommandHandler() {
			this.builder.Register(ch => this.handler).As<ICommandHandler<FakeCommand>>();
			var container = this.builder.Build();
			this.scope = container.BeginLifetimeScope();
			var dispatcher = new CommandDispatcher(this.scope);
			var command = new FakeCommand();

			dispatcher.Dispatch(command);

			A.CallTo(() => this.handler.Execute(command)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Dispatch_IncomingCommandHasNoHandler_ThrowsNoCommandHandlerRegisteredException() {
			var container = this.builder.Build();
			this.scope = container.BeginLifetimeScope();
			var dispatcher = new CommandDispatcher(this.scope);
			var command = new FakeCommand();

			Action dispatch = () => dispatcher.Dispatch(command);

			dispatch.ShouldThrow<NoCommandHandlerRegisteredException>();
		}
	}

	public class FakeCommand : ICommand {}
}