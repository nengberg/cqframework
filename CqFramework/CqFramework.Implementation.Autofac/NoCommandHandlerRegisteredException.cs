using System;

namespace CqFramework.Implementation.Autofac {
	public class NoCommandHandlerRegisteredException : Exception {
		public NoCommandHandlerRegisteredException(string message) : base(message) {}
	}
}