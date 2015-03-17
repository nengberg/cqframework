using Autofac;
using Autofac.Core.Registration;

namespace CqFramework.Implementation.Autofac {
	internal class QueryProcessor : IQueryProcessor {
		private readonly ILifetimeScope lifetimeScope;

		public QueryProcessor(ILifetimeScope lifetimeScope) {
			this.lifetimeScope = lifetimeScope;
		}

		public TResponse Process<TResponse>(IQuery<TResponse> query) {
			var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
			dynamic handler;
			try {
				handler = this.lifetimeScope.Resolve(handlerType);
			} catch(ComponentNotRegisteredException) {
				throw new NoQueryHandlerRegisteredException(string.Format("No query handler for {0} is registered", query.GetType()));
			}

			TResponse response = handler.Handle((dynamic)query);
			return response;
		}
	}
}