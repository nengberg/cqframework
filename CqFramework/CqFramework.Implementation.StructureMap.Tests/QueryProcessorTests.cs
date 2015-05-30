using System;

using FakeItEasy;

using FluentAssertions;

using NUnit.Framework;

using StructureMap;

namespace CqFramework.Implementation.StructureMap.Tests {
	[TestFixture]
	public class QueryProcessorTests {
		private IContainer container;

		[SetUp]
		public void SetUp() {
			this.container = new Container();
			Bootstrapping.Configure(this.container);
		}

		[Test]
		public void Process_IncomingQuery_CorrectQueryHandlerIsInvoked() {
			var query = new FakeQuery();
			var handler = SetUpAndRegisterHandler<FakeQuery, FakeQueryResult>(query);
			var queryProcessor = new QueryProcessor(this.container);

			queryProcessor.Process(query);

			A.CallTo(() => handler.Handle(query)).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Process_IncomingQueryHasNoHandler_ThrowsNoQueryHandlerException() {
			var query = new FakeQuery();
			var queryProcessor = new QueryProcessor(this.container);

			Action request = () => queryProcessor.Process(query);

			request.ShouldThrow<NoQueryHandlerRegisteredException>();
		}

		private IQueryHandler<TQuery, TResponse> SetUpAndRegisterHandler<TQuery, TResponse>(TQuery query)
			where TQuery : IQuery<TResponse> {
			var handler = A.Fake<IQueryHandler<TQuery, TResponse>>();
			this.container.Configure(x => { x.For<IQueryHandler<TQuery, TResponse>>().Use(handler); });

			return handler;
		}
	}

	public class FakeQuery : IQuery<FakeQueryResult> {}

	public class FakeQueryResult {}
}