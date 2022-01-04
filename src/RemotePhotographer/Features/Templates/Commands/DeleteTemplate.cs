using RemotePhotographer.Features.Templates.Events;
using RemotePhotographer.Features.Templates.Services;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Reactive.Services;

namespace RemotePhotographer.Features.Templates.Commands
{
    public class DeleteTemplate
        : Command
    {
        public DeleteTemplate(string templateId)
        {
            TemplateId = templateId;
        }

        public string TemplateId { get; set; }
    }

    [Handle(typeof(DeleteTemplate))]
    public class DeleteTemplateHandler
        : CommandHandler<DeleteTemplate>
    {
        private readonly IEventDispatcher _dispatcher;
        private readonly ITemplateStorage _storage;

        public DeleteTemplateHandler(IEventDispatcher dispatcher, ITemplateStorage storage)
        {
            _dispatcher = dispatcher;
            _storage = storage;
        }

        public override Task<bool> ExecuteAsync(DeleteTemplate command)
        {
            Validate(command);

            var template = _storage.Delete(command.TemplateId);

            _dispatcher.Dispatch(new TemplateDeleted(template));

            return Task.FromResult(true);
        }

        private void Validate(DeleteTemplate command)
        {
            ValidateThatTemplateExists(command);
        }

        private void ValidateThatTemplateExists(DeleteTemplate command)
        {
            if (_storage.Get(command.TemplateId) is null)
            {
                throw new ArgumentException($"template with id {command.TemplateId} does not exists");
            }
        }
    }
}
