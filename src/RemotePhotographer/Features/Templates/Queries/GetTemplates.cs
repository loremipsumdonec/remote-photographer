using RemotePhotographer.Features.Templates.Models;
using RemotePhotographer.Features.Templates.Services;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;

namespace RemotePhotographer.Features.Templates.Queries
{
    public class GetTemplates
        : Query
    {
        public GetTemplates(int offset, int fetch)
        {
            Offset = offset;
            Fetch = fetch;
        }

        public int Offset { get; set; }

        public int Fetch { get; set; }

        public bool? IsDeleted { get; set; } = false;
    }

    public class GetTemplatesModel
        : IModel
    {
        public int Offset { get; set; }

        public int Fetch { get; set; }

        public List<Template> Templates { get; set; } = new();

        public void Add(Template template)
        {
            Templates.Add(template);
        }
    }

    [Handle(typeof(GetTemplates))]
    public class GetTemplatesHandler
        : QueryHandler<GetTemplates>
    {
        private readonly ITemplateStorage _storage;

        public GetTemplatesHandler(ITemplateStorage storage)
        {
            _storage = storage;
        }

        public override Task<IModel> ExecuteAsync(GetTemplates query)
        {
            var model = new GetTemplatesModel();

            foreach(var template in _storage.List(query.Offset, query.Fetch)) 
            {
                model.Add(template);
            }

            return Task.FromResult((IModel)model);
        }
    }
}
