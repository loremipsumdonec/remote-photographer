using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.RabbitMQ.Services;
using RemotePhotographer.Features.Photographer.Commands;
using System;
using System.IO;
using System.Runtime.InteropServices;
using Xunit;

namespace RemotePhotographerTest.Services
{
    public class RemotePhotographerEngineWithConnectCameraForIntegration
        : RemotePhotographerEngine
    {
        public override void Start()
        {
            base.Start();

            var commandDispatcher = (ICommandDispatcher)Services.GetService(typeof(ICommandDispatcher));
            var status = commandDispatcher.DispatchAsync(new ConnectCamera())
                .GetAwaiter()
                .GetResult();

            Assert.True(status);
        }

        public override void Stop()
        {
            base.Stop();

            var commandDispatcher = (ICommandDispatcher)Services.GetService(typeof(ICommandDispatcher));
            commandDispatcher.DispatchAsync(new DisconnectCamera())
                .GetAwaiter()
                .GetResult();
        }

        protected override DistributedServiceEngine CreateDistributedServiceEngine()
        {
            var configuration = GetConfiguration();
            var rabbitMQReadinessProbe = new RabbitMQReadinessProbe(configuration.GetSection("message.broker-service:parameters"));

            string path = Path.Combine(Environment.CurrentDirectory, "integration");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new DockerComposeThroughWSLDistributedServiceEngine(path, rabbitMQReadinessProbe);
            }

            return new DockerComposeDistributedServiceEngine(path, rabbitMQReadinessProbe);
        }
    }
}
