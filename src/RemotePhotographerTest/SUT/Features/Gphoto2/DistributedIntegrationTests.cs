using System.Linq;
using RemotePhotographer.Features.Photographer.Commands;
using RemotePhotographer.Features.Photographer.Queries;
using RemotePhotographerTest.Services;
using Xunit;
using System.Reactive.Linq;
using RemotePhotographer.Features.Gphoto2.Services;
using Boilerplate.Features.MassTransit.Services;

namespace RemotePhotographerTest.SUT.Features.Gphoto2
{
    [Collection("RemotePhotographerEngineForIntegration"), 
        Trait("type", "Integration")]
    public class DistributedIntegrationTests
    {
        public DistributedIntegrationTests(RemotePhotographerEngineForIntegration engine)
        {
            Fixture = new PhotographerFixture(engine);
        }

        public PhotographerFixture Fixture { get; set; }

        [Fact]
        public async void GetCameras_WhenCameraAvailable_HasACamera()
        {
            var dispatcher = Fixture.GetService<IDistributedQueryDispatcher>();
            var model = await dispatcher.DispatchAsync<GetCamerasModel>(new GetCameras());

            Assert.Single(model.Cameras);
        }

        [Fact]
        public async void GetCameras_WhenCameraAvailable_CameraHasExpectedName()
        {
            string expected = "Canon EOS 450D (PTP mode)";

            var dispatcher = Fixture.GetService<IDistributedQueryDispatcher>();
            var model = await dispatcher.DispatchAsync<GetCamerasModel>(new GetCameras());

            Assert.Equal(expected, model.Cameras.First().Name);
        }

        [Fact]
        public async void CameraContextManager_WhenCameraAvailable_HasCameraContext()
        {
            var cameraContextManager = Fixture.GetService<ICameraContextManager>();
            var dispatcher = Fixture.GetService<IDistributedCommandDispatcher>();

            await dispatcher.DispatchAsync(new ConnectCamera());
            Assert.NotNull(cameraContextManager.CameraContext);
            
            await dispatcher.DispatchAsync(new DisconnectCamera());
            Assert.NotNull(cameraContextManager.CameraContext);
        }
    }
}
