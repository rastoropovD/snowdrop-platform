using Microsoft.Extensions.DependencyInjection;
using Snowdrop.DAL.Extensions;

namespace Snowdrop.DAL.Tests.Core
{
    public abstract class BaseTest
    {
        protected ServiceProvider Services { get; }

        protected BaseTest()
        {
            ServiceCollection collection = new ServiceCollection();
            collection.AddSnowdropContextInMemory("SnowdropMemory");
            Services = collection.BuildServiceProvider();
        }
    }
}