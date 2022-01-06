using System.Linq;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Photographer.Queries;
using RemotePhotographerTest.Services;
using Xunit;
using System.Reactive.Linq;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Photographer.Commands;
using RemotePhotographer.Features.Photographer.Models;
using RemotePhotographerTest.Utility;

namespace RemotePhotographerTest.SUT.Features.Gphoto2
{
    [Collection("RemotePhotographerEngineWithConnectCameraForIntegration"), 
        Trait("type", "Integration")]
    public class IntegrationTestsWithConnectedCamera
    {
        public IntegrationTestsWithConnectedCamera(RemotePhotographerEngineWithConnectCameraForIntegration engine)
        {
            Fixture = new PhotographerFixture(engine);
        }

        public PhotographerFixture Fixture { get; set; }

        [Fact]
        public async void GetISO_WhenCameraAvailable_ISOReturned()
        {
            var queryDispatcher = Fixture.GetService<IQueryDispatcher>();
            var model = await queryDispatcher.DispatchAsync<ISO>(new GetISO());

            Assert.False(string.IsNullOrEmpty(model.Current));
            Assert.NotEmpty(model.Values);
        }

        [Fact]
        public async void SetISO_WhenCameraAvailable_HasExpectedISOValue()
        {
            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            var queryDispatcher = Fixture.GetService<IQueryDispatcher>();

            var iso = await queryDispatcher.DispatchAsync<ISO>(new GetISO());
            var newValue = iso.Values.Skip(1).Where(v => !v.Equals(iso.Current)).PickRandom();

            await dispatcher.DispatchAsync(new SetISO(newValue));
            var updated = await queryDispatcher.DispatchAsync<ISO>(new GetISO());

            Assert.Equal(newValue, updated.Current);
        }

        [Fact]
        public async void GetShutterSpeed_WhenCameraAvailable_ShutterSpeedReturned()
        {
            var queryDispatcher = Fixture.GetService<IQueryDispatcher>();
            var model = await queryDispatcher.DispatchAsync<ShutterSpeed>(new GetShutterSpeed());

            Assert.False(string.IsNullOrEmpty(model.Current));
            Assert.NotEmpty(model.Values);
        }

        [Fact]
        public async void SetShutterSpeed_WhenCameraAvailable_HasExpectedShutterSpeedValue()
        {
            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            var queryDispatcher = Fixture.GetService<IQueryDispatcher>();

            var shutterSpeed = await queryDispatcher.DispatchAsync<ShutterSpeed>(new GetShutterSpeed());
            var newValue = shutterSpeed.Values.Where(v => !v.Equals(shutterSpeed.Current)).PickRandom();

            await dispatcher.DispatchAsync(new SetShutterSpeed(newValue));
            var updated = await queryDispatcher.DispatchAsync<ShutterSpeed>(new GetShutterSpeed());

            Assert.Equal(newValue, updated.Current);
        }

        [Fact]
        public async void GetAperture_WhenCameraAvailable_ShutterSpeedReturned()
        {
            var queryDispatcher = Fixture.GetService<IQueryDispatcher>();

            var model = await queryDispatcher.DispatchAsync<Aperture>(new GetAperture());

            Assert.False(string.IsNullOrEmpty(model.Current));
            Assert.NotEmpty(model.Values);
        }

        [Fact]
        public async void SetAperture_WhenCameraAvailable_HasExpectedApertureValue()
        {
            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            var queryDispatcher = Fixture.GetService<IQueryDispatcher>();

            var aperture = await queryDispatcher.DispatchAsync<Aperture>(new GetAperture());
            var newValue = aperture.Values.Where(v => !v.Equals(aperture.Current)).PickRandom();

            await dispatcher.DispatchAsync(new SetAperture(newValue));
            var updated = await queryDispatcher.DispatchAsync<Aperture>(new GetAperture());

            Assert.Equal(newValue, updated.Current);
        }
    }
}
