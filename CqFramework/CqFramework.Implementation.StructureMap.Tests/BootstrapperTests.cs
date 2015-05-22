using System;
using NUnit.Framework;
using StructureMap;

namespace CqFramework.Implementation.StructureMap.Tests
{
    [TestFixture]
    public class BootstrapperTests
    {
        [Test]
        public void Structuremap_CanResolve_QueryProcessor()
        {
            var container = new Container();

            Bootstrapping.Configure(container);

            var queryProcessor = container.GetInstance<IQueryProcessor>();

            Assert.IsInstanceOf<QueryProcessor>(queryProcessor);
        }

        [Test]
        public void Structuremap_CanResolve_CommandDispatcher()
        {
            var container = new Container();

            Bootstrapping.Configure(container);

            var commandDispatcher = container.GetInstance<ICommandDispatcher>();

            Assert.IsInstanceOf<CommandDispatcher>(commandDispatcher);
        }
    }
}
