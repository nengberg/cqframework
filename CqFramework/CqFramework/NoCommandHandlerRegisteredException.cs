using System;

namespace CqFramework {
	public class NoCommandHandlerRegisteredException : Exception {
		public NoCommandHandlerRegisteredException(string message) : base(message) {}
	}
}