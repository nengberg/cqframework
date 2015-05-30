using System;

using Autofac;

using FakeItEasy;

using FluentAssertions;

using NUnit.Framework;

namespace CqFramework.Implementation.Autofac.Tests {
	[TestFixture]
	public class QueryProcessorTests {
		private ILifetimeScope scope;
		private ContainerBuilder builder;

		[SetUp]
		public void SetUp() {
			this.builder = new ContainerBuilder();
		}

		[Test]
		public void Process_IncomingQuery_CorrectQueryHandlerIsInvoked() {
			var query = new FakeQuery();
			var handler = SetUpAndRegisterHandler<FakeQuery, FakeQueryResult>(query);
			var queryProcessor = new QueryProcessor(this.scope);

			queryProcessor.Process(query);

			A.CallTo(() => handler.Handle(query)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Process_IncomingQueryHasNoHandler_ThrowsNoQueryHandlerException() {
			var query = new FakeQuery();
			var container = this.builder.Build();
			this.scope = container.BeginLifetimeScope();
			var queryProcessor = new QueryProcessor(this.scope);

			Action request = () => queryProcessor.Process(query);

			request.ShouldThrow<NoQueryHandlerRegisteredException>();
		}

		private IQueryHandler<TQuery, TResponse> SetUpAndRegisterHandler<TQuery, TResponse>(TQuery query)
			where TQuery : IQuery<TResponse> {
			var handler = A.Fake<IQueryHandler<TQuery, TResponse>>();
			this.builder.Register(qh => handler).As<IQueryHandler<TQuery, TResponse>>();
			var container = this.builder.Build();
			this.scope = container.BeginLifetimeScope();
			return handler;
		}
	}

	public class FakeQuery : IQuery<FakeQueryResult> {}

	public class FakeQueryResult {}
}