namespace CqFramework {
	public interface IQueryProcessor {
		TResponse Process<TResponse>(IQuery<TResponse> query);
	}
}