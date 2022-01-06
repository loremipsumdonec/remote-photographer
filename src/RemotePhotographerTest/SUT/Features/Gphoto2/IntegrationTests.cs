using System.Linq;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Photographer.Commands;
using RemotePhotographer.Features.Photographer.Queries;
using RemotePhotographerTest.Services;
using Xunit;
using System.Reactive.Linq;
using RemotePhotographer.Features.Gphoto2.Services;
using Boilerplate.Features.MassTransit.Services;

namespace RemotePhotographerTest.SUT.Features.Gphoto2
{
    [Collection("RemotePhotographerEngineWithConnectCameraForIntegration"), 
        Trait("type", "Integration")]
    public class IntegrationTests
    {
        public IntegrationTests(RemotePhotographerEngineWithConnectCameraForIntegration engine)
        {
            Fixture = new PhotographerFixture(engine);
        }

        public PhotographerFixture Fixture { get; set; }

        [Fact]
        public async void GetCameras_WhenCameraAvailable_HasACamera()
        {
            var dispatcher = Fixture.GetService<IQueryDispatcher>();
            var model = await dispatcher.DispatchAsync<GetCamerasModel>(new GetCameras());

            Assert.Single(model.Cameras);
        }

        [Fact]
        public async void GetCameras_WhenCameraAvailable_CameraHasExpectedName()
        {
            string expected = "Canon EOS 450D (PTP mode)";

            var dispatcher = Fixture.GetService<IQueryDispatcher>();
            var model = await dispatcher.DispatchAsync<GetCamerasModel>(new GetCameras());

            Assert.Equal(expected, model.Cameras.First().Name);
        }

        [Fact]
        public async void CameraContextManager_WhenCameraAvailable_HasCameraContext()
        {
            var cameraContextManager = Fixture.GetService<ICameraContextManager>();
            var dispatcher = Fixture.GetService<ICommandDispatcher>();

            await dispatcher.DispatchAsync(new ConnectCamera());
            Assert.NotNull(cameraContextManager.CameraContext);
            
            await dispatcher.DispatchAsync(new DisconnectCamera());
            Assert.NotNull(cameraContextManager.CameraContext);
        }
    }
}
