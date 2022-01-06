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
using System.Collections.Generic;
using Boilerplate.Features.Reactive.Services;
using System.Reactive.Linq;
using RemotePhotographer.Features.Photographer.Events;

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
            int takePhotos = 1;

            List<string> images = new List<string>();

            var commandDispatcher = Fixture.GetService<ICommandDispatcher>();
            var queryDispatcher = Fixture.GetService<IQueryDispatcher>();
            var eventHub = Fixture.GetService<IEventHub>();

            eventHub.Connect((stream) => 
                stream.Where(e => e is ImageCaptured)
                .Select(e=> e as ImageCaptured)
                .Subscribe(e => images.Add(e.Path))
            );

            await commandDispatcher.DispatchAsync(new ConnectCamera());

            /*
            var shutterSpeed = await queryDispatcher.DispatchAsync<ShutterSpeed>(new GetShutterSpeed());
            var newShutterSpeed = shutterSpeed.Values.PickRandom();

            await commandDispatcher.DispatchAsync(new SetShutterSpeed(newShutterSpeed));

            var iso = await queryDispatcher.DispatchAsync<ISO>(new GetISO());
            var newISO = iso.Values.Skip(1).PickRandom();

            await commandDispatcher.DispatchAsync(new SetISO(newISO));
            */

            for(int index = 0; index < takePhotos; index++) 
            {
                await commandDispatcher.DispatchAsync(new CaptureImage());
            }
            
            foreach(string image in images) 
            {
                await queryDispatcher.DispatchAsync<GetImageModel>(new GetImage(image));   
            }

            await commandDispatcher.DispatchAsync(new DisconnectCamera());
        }
    }
}
