using System;

namespace CqFramework.Implementation.Autofac {
	public class NoQueryHandlerRegisteredException : Exception {
		public NoQueryHandlerRegisteredException(string message) : base(message) {}
	}
}