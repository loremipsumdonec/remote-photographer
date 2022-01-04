using RemotePhotographer.Features.Templates.Commands;
using RemotePhotographerTest.Services;
using RemotePhotographerTest.Utility;
using Boilerplate.Features.Core.Commands;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RemotePhotographerTest.SUT.Features.Templates
{
    [Collection("TemplateEngineForSmokeTests"), Trait("type", "Smoke")]
    public class DeleteTemplateTests
    {
        public DeleteTemplateTests(TemplateServiceEngineForSmoke engine)
        {
            Fixture = new TemplateServiceFixture(engine, "sv");
        }

        public TemplateServiceFixture Fixture { get; set; }

        [Fact]
        [Trait("severity", "Critical")]
        public async Task DeleteTemplate_TemplateExists_TemplateDeleted()
        {
            Fixture.CreateTemplates(1);
            var template = Fixture.Templates.PickRandom();
            var dispatcher = Fixture.GetService<ICommandDispatcher>();

            await dispatcher.DispatchAsync(new DeleteTemplate(template.TemplateId));

            var deleted = Fixture.GetTemplate(template);

            Assert.True(deleted.IsDeleted);
            Assert.True(deleted.Deleted > DateTime.MinValue);
        }

        [Fact]
        [Trait("severity", "Critical")]
        public void DeleteTemplate_WhenTemplateDoesNotExists_()
        {
            var dispatcher = Fixture.GetService<ICommandDispatcher>();

            Assert.ThrowsAsync<ArgumentException>(
                async () => await dispatcher.DispatchAsync(new DeleteTemplate(Guid.NewGuid().ToString()))
            );
        }
    }
}
