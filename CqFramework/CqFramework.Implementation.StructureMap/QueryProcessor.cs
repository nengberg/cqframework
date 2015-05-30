using StructureMap;

namespace CqFramework.Implementation.StructureMap
{
    internal class QueryProcessor : IQueryProcessor {
        private readonly IContainer container;

        public QueryProcessor(IContainer container) {
            this.container = container;
        }

        public TResponse Process<TResponse>(IQuery<TResponse> query) {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
			dynamic handler = this.container.TryGetInstance(handlerType);
            if (handler == null)
            {
                throw new NoQueryHandlerRegisteredException(string.Format("No query handler for {0} is registered", query.GetType()));    
            }
            
			TResponse response = handler.Handle((dynamic)query);
			return response;
        }
    }
}
