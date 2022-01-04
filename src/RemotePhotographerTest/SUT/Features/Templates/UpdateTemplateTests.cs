using RemotePhotographer.Features.Templates.Commands;
using RemotePhotographerTest.Services;
using Boilerplate.Features.Core.Commands;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RemotePhotographerTest.SUT.Features.Templates
{
    [Collection("TemplateEngineForSmokeTests"), Trait("type", "Smoke")]
    public class UpdateTemplateTests
    {
        public UpdateTemplateTests(TemplateServiceEngineForSmoke engine)
        {
            Fixture = new TemplateServiceFixture(engine, "sv");
        }

        public TemplateServiceFixture Fixture { get; set; }

        [Fact]
        [Trait("severity", "Critical")]
        public async Task UpdateTemplate_WithValidInput_TemplateUpdated()
        {
            Fixture.CreateTemplates(1);
            var source = Fixture.Templates.First();

            var command = new UpdateTemplate(
                source.TemplateId,
                source.TemplateParentId,
                "Updated name",
                "Updated description"
            );

            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            await dispatcher.DispatchAsync(command);

            var updated = Fixture.GetTemplate(source);

            Assert.Equal(command.Name, updated.Name);
            Assert.Equal(command.Description, updated.Description);
            Assert.True(updated.Updated > source.Updated);
        }
    }
}
