using System;
using System.Linq;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Core.Queries;
using Boilerplate.Features.Core.Services;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Commands;
using RemotePhotographer.Features.Photographer.Models;
using RemotePhotographer.Features.Photographer.Queries;
using RemotePhotographerTest.Services;
using RemotePhotographerTest.Utility;
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

        [Fact]
        public async void GetISO_WhenCameraAvailable_ISOReturned()
        {
            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            var queryDispatcher = Fixture.GetService<IQueryDispatcher>();
            await dispatcher.DispatchAsync(new ConnectCamera());
            
            var iso = await queryDispatcher.DispatchAsync<ISO>(new GetISO());

            await dispatcher.DispatchAsync(new DisconnectCamera());
        }

        [Fact]
        public async void SetISO_WhenCameraAvailable_HasExpectedISOValue()
        {
            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            var queryDispatcher = Fixture.GetService<IQueryDispatcher>();
            await dispatcher.DispatchAsync(new ConnectCamera());
            
            var iso = await queryDispatcher.DispatchAsync<ISO>(new GetISO());
            var newValue = iso.Values.Skip(1).Where(v=> !v.Equals(iso.Current)).PickRandom();

            await dispatcher.DispatchAsync(new SetISO(newValue));
            var updated = await queryDispatcher.DispatchAsync<ISO>(new GetISO());            
            await dispatcher.DispatchAsync(new DisconnectCamera());

            Assert.Equal(newValue, updated.Current);
        }

        [Fact]
        public async void GetShutterSpeed_WhenCameraAvailable_ShutterSpeedReturned()
        {
            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            var queryDispatcher = Fixture.GetService<IQueryDispatcher>();
            await dispatcher.DispatchAsync(new ConnectCamera());
            
            var shutterSpeed = await queryDispatcher.DispatchAsync<ShutterSpeed>(new GetShutterSpeed());

            await dispatcher.DispatchAsync(new DisconnectCamera());
        }

        [Fact]
        public async void SetShutterSpeed_WhenCameraAvailable_HasExpectedShutterSpeedValue()
        {
            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            var queryDispatcher = Fixture.GetService<IQueryDispatcher>();
            await dispatcher.DispatchAsync(new ConnectCamera());
            
            var shutterSpeed = await queryDispatcher.DispatchAsync<ShutterSpeed>(new GetShutterSpeed());
            var newValue = shutterSpeed.Values.Where(v=> !v.Equals(shutterSpeed.Current)).PickRandom();

            await dispatcher.DispatchAsync(new SetShutterSpeed(newValue));
            var updated = await queryDispatcher.DispatchAsync<ShutterSpeed>(new GetShutterSpeed());            
            await dispatcher.DispatchAsync(new DisconnectCamera());

            Assert.Equal(newValue, updated.Current);
        }

        [Fact]
        public async void GetAperture_WhenCameraAvailable_ShutterSpeedReturned()
        {
            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            var queryDispatcher = Fixture.GetService<IQueryDispatcher>();
            await dispatcher.DispatchAsync(new ConnectCamera());
            
            var aperture = await queryDispatcher.DispatchAsync<Aperture>(new GetAperture());

            await dispatcher.DispatchAsync(new DisconnectCamera());
        }

        [Fact]
        public async void SetAperture_WhenCameraAvailable_HasExpectedApertureValue()
        {
            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            var queryDispatcher = Fixture.GetService<IQueryDispatcher>();
            await dispatcher.DispatchAsync(new ConnectCamera());
            
            var aperture = await queryDispatcher.DispatchAsync<Aperture>(new GetAperture());
            var newValue = aperture.Values.Where(v=> !v.Equals(aperture.Current)).PickRandom();

            await dispatcher.DispatchAsync(new SetAperture(newValue));
            var updated = await queryDispatcher.DispatchAsync<Aperture>(new GetAperture());            
            await dispatcher.DispatchAsync(new DisconnectCamera());

            Assert.Equal(newValue, updated.Current);
        }

        [Fact]
        public async void Lorem() 
        {
            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            var manager = Fixture.GetService<ICameraContextManager>();
            var service = Fixture.GetService<IModelService>();

            await dispatcher.DispatchAsync(new ConnectCamera());

            var isoStatus = CameraService.gp_camera_get_single_config(
                manager.CameraContext.Camera, "iso", out IntPtr isoWidget, manager.CameraContext.Context
            );

            var apertureStatus = CameraService.gp_camera_get_single_config(
                manager.CameraContext.Camera, "aperture", out IntPtr apertureWidget, manager.CameraContext.Context
            );

            var shutterSpeedStatus = CameraService.gp_camera_get_single_config(
                manager.CameraContext.Camera, "shutterspeed", out IntPtr shutterSpeedWidget, manager.CameraContext.Context
            );

            var iso = await service.CreateModelAsync<ISO>(isoWidget);
            var aperture = await service.CreateModelAsync<Aperture>(apertureWidget);
            var shutterSpeed = await service.CreateModelAsync<ShutterSpeed>(shutterSpeedWidget);

            await dispatcher.DispatchAsync(new DisconnectCamera());
        }
    }
}
