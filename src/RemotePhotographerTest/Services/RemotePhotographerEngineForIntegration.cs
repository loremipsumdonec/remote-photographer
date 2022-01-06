
using Boilerplate.Features.RabbitMQ.Services;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace RemotePhotographerTest.Services
{
    public class RemotePhotographerEngineForIntegration
        : RemotePhotographerEngine
    {
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
