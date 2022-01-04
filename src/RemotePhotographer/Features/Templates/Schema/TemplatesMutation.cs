using RemotePhotographer.Features.Templates.Commands;
using RemotePhotographer.Features.Templates.Models;
using RemotePhotographer.Features.Templates.Queries;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Core.Queries;

namespace RemotePhotographer.Features.Templates.Schema
{
    public class TemplatesMutation
    {
        public async Task<Template> CreateTemplate(
            string templateParentId,
            string name,
            string? description,
            [Service] ICommandDispatcher dispatcher,
            [Service] IQueryDispatcher queryDispatcher)
        {

            var command = new CreateTemplate(templateParentId, name, description);

            if (await dispatcher.DispatchAsync(command))
            {
                string templateId = (string)command.CommandResult.Output;
                return await queryDispatcher.DispatchAsync<Template>(new GetTemplate(templateId));
            }

            throw command.CommandResult.Exception;
        }

        public async Task<Template> UpdateTemplate(
            string templateId,
            string templateParentId,
            string name,
            string? description,
            [Service] ICommandDispatcher dispatcher,
            [Service] IQueryDispatcher queryDispatcher) 
        {

            var command = new UpdateTemplate(templateId, templateParentId, name, description);

            if (await dispatcher.DispatchAsync(command))
            {
                return await queryDispatcher.DispatchAsync<Template>(new GetTemplate(templateId));
            }

            throw command.CommandResult.Exception;
        }

        public async Task<Template> DeleteTemplate(
            string templateId,
            [Service] ICommandDispatcher dispatcher,
            [Service] IQueryDispatcher queryDispatcher)
        {
            var command = new DeleteTemplate(templateId);

            if (await dispatcher.DispatchAsync(command))
            {
                return await queryDispatcher.DispatchAsync<Template>(new GetTemplate(templateId));
            }

            throw command.CommandResult.Exception;
        }
    }
}
