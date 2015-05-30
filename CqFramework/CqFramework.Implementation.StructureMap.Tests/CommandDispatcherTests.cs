using System;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using StructureMap;

namespace CqFramework.Implementation.StructureMap.Tests
{
    [TestFixture]
    public class CommandDispatcherTests
    {
        private IContainer container;
        private ICommandHandler<FakeCommand> handler;

        [SetUp]
        public void SetUp()
        {
            this.container = new Container();
            Bootstrapping.Configure(container);
            this.handler = A.Fake<ICommandHandler<FakeCommand>>();
        }

        [Test]
        public void Dispatch_CommandHasHandler_DispatchesCorrectCommandHandler()
        {
            this.container.Configure(x =>
            {
                x.For<ICommandHandler<FakeCommand>>().Use(this.handler);
            });
            
            var dispatcher = new CommandDispatcher(this.container);
            var command = new FakeCommand();

            dispatcher.Dispatch(command);

            A.CallTo(() => this.handler.Execute(command)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Dispatch_IncomingCommandHasNoHandler_ThrowsNoCommandHandlerRegisteredException()
        {
            var dispatcher = new CommandDispatcher(this.container);
            var command = new FakeCommand();

            Action dispatch = () => dispatcher.Dispatch(command);

            dispatch.ShouldThrow<NoCommandHandlerRegisteredException>();
        }
    }

    public class FakeCommand : ICommand { }
}
