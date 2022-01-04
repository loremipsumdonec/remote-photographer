using RemotePhotographer.Features.Templates.Commands;
using RemotePhotographerTest.Services;
using RemotePhotographerTest.Utility;
using Boilerplate.Features.Core.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RemotePhotographerTest.SUT.Features.Templates
{
    [Collection("TemplateEngineForSmokeTests"), Trait("type", "Smoke")]
    public class CreateTemplateTests
    {
        public CreateTemplateTests(TemplateServiceEngineForSmoke engine)
        {
            Fixture = new TemplateServiceFixture(engine, "sv");
        }

        public TemplateServiceFixture Fixture { get; set; }

        [Fact]
        [Trait("severity", "Critical")]
        public async Task CreateTemplate_WithValidInput_TemplateCreated()
        {
            var command = new CreateTemplate(
                IpsumGenerator.Generate(2, 3, false),
                IpsumGenerator.Generate(8, 12, false)
            );

            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            await dispatcher.DispatchAsync(command);

            Assert.Single(Fixture.Templates);

            var template = Fixture.Templates.First();

            Assert.Equal(command.Name, template.Name);
            Assert.Equal(command.Description, template.Description);
        }

        [Fact]
        [Trait("severity", "Critical")]
        public async Task CreateTemplate_WithParent_TemplateHasParent()
        {
            Fixture.CreateTemplates(1);

            var command = new CreateTemplate(
                Fixture.Templates.First().TemplateId,
                IpsumGenerator.Generate(2, 3, false),
                IpsumGenerator.Generate(8, 12, false)
            );

            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            await dispatcher.DispatchAsync(command);

            var template = Fixture.Templates.Last();

            Assert.Equal(command.TemplateParentId, template.TemplateParentId);
        }

        [Fact]
        [Trait("severity", "Critical")]
        public async Task CreateTemplate_WithParentThatDoesNotExists_ThrowArgumentException()
        {
            var command = new CreateTemplate(
                Guid.NewGuid().ToString(),
                IpsumGenerator.Generate(2, 3, false),
                IpsumGenerator.Generate(8, 12, false)
            );

            var dispatcher = Fixture.GetService<ICommandDispatcher>();

            await Assert.ThrowsAsync<ArgumentException>(
                async () => await dispatcher.DispatchAsync(command)
            );
        }

        [Fact]
        [Trait("severity", "Critical")]
        public async Task CreateTemplate_WithSameNameAsOtherTemplate_ThrowsDu()
        {
            Fixture.CreateTemplates(1);

            var command = new CreateTemplate(
                Fixture.Templates.First().Name,
                IpsumGenerator.Generate(8, 12, false)
            );

            var dispatcher = Fixture.GetService<ICommandDispatcher>();

            await Assert.ThrowsAsync<ArgumentException>(
                async () => await dispatcher.DispatchAsync(command)
            );
        }
    }
}
