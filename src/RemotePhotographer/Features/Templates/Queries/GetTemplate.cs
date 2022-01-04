using RemotePhotographer.Features.Templates.Services;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;

namespace RemotePhotographer.Features.Templates.Queries
{
    public class GetTemplate
        : Query
    {
        public GetTemplate(string templateId)
        {
            TemplateId = templateId;
        }

        public string TemplateId { get; set; }
    }

    [Handle(typeof(GetTemplate))]
    public class GetTemplateHandler
        : QueryHandler<GetTemplate>
    {
        private readonly ITemplateStorage _storage;

        public GetTemplateHandler(ITemplateStorage storage)
        {
            _storage = storage;
        }

        public override Task<IModel> ExecuteAsync(GetTemplate query)
        {
            var template = _storage.Get(query.TemplateId);

            return Task.FromResult((IModel)template);
        }
    }
}
