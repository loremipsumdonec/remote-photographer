using RemotePhotographer.Features.Templates.Models;
using RemotePhotographer.Features.Templates.Queries;
using Boilerplate.Features.Core.Queries;

namespace RemotePhotographer.Features.Templates.Schema
{
    public class TemplatesQuery
    {
        public Task<Template> Template(string templateId, [Service] IQueryDispatcher dispatcher) 
        {
            return dispatcher.DispatchAsync<Template>(
                new GetTemplate(templateId)
            );
        }

        public Task<GetTemplatesModel> Templates(int offset, int fetch, [Service] IQueryDispatcher dispatcher)
        {
            return dispatcher.DispatchAsync<GetTemplatesModel>(new GetTemplates(offset, fetch));
        }
    }
}
