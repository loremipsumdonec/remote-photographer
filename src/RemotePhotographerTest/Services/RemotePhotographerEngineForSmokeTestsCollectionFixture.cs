using Xunit;

namespace RemotePhotographerTest.Services
{
    [CollectionDefinition("RemotePhotographerEngineForSmokeTests")]
    public class RemotePhotographerEngineForSmokeTestsCollectionFixture
        : ICollectionFixture<RemotePhotographerEngineForSmoke>
    {
    }
}
