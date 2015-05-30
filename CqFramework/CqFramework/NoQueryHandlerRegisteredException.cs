using System;

namespace CqFramework {
	public class NoQueryHandlerRegisteredException : Exception {
		public NoQueryHandlerRegisteredException(string message) : base(message) {}
	}
}