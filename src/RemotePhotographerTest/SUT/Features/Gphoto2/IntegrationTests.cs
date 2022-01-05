using System;
using System.Linq;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Photographer.Commands;
using RemotePhotographer.Features.Photographer.Queries;
using RemotePhotographerTest.Services;
using Xunit;

namespace RemotePhotographerTest.SUT.Features.Gphoto2
{
    [Collection("RemotePhotographerEngineForSmokeTests"), Trait("type", "Integration")]
    public class IntegrationTests
    {
        public IntegrationTests(RemotePhotographerEngineForSmoke engine)
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
        public async void ConnectCamera_WhenCameraAvailable_CameraManagerHasCameraContext()
        {
            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            await dispatcher.DispatchAsync(new ConnectCamera());
            await dispatcher.DispatchAsync(new CaptureImage());
            await dispatcher.DispatchAsync(new DisconnectCamera());
        }
    }
}
