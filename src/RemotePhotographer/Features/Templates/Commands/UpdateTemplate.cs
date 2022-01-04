using RemotePhotographer.Features.Templates.Events;
using RemotePhotographer.Features.Templates.Services;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Reactive.Services;

namespace RemotePhotographer.Features.Templates.Commands
{
    public class UpdateTemplate
        : Command
    {
        public UpdateTemplate(
            string templateId,
            string templateParentId,
            string? name,
            string? description)
        {
            TemplateId = templateId;
            TemplateParentId = templateParentId;
            Name = name;
            Description = description;
        }

        public string TemplateId { get; set; }

        public string TemplateParentId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }

    [Handle(typeof(UpdateTemplate))]
    public class UpdateTemplateHandler
        : CommandHandler<UpdateTemplate>
    {
        private readonly IEventDispatcher _dispatcher;
        private readonly ITemplateStorage _storage;

        public UpdateTemplateHandler(IEventDispatcher dispatcher, ITemplateStorage storage)
        {
            _dispatcher = dispatcher;
            _storage = storage;
        }

        public override Task<bool> ExecuteAsync(UpdateTemplate command)
        {
            Validate(command);

            var template = _storage.Update(command.TemplateId, template =>
            {
                template.TemplateParentId = command.TemplateParentId;
                template.Name = command.Name;
                template.Description = command.Description;
            });

            _dispatcher.Dispatch(new TemplateUpdated(template));

            return Task.FromResult(true);
        }

        private void Validate(UpdateTemplate command)
        {
            ValidateThatParentExists(command);
            ValidateThatTemplateExists(command);
            ValidateUniqueName(command);
        }

        private void ValidateThatTemplateExists(UpdateTemplate command)
        {
            if (_storage.Get(command.TemplateId) is null)
            {
                throw new ArgumentException($"template with id {command.TemplateId} does not exists");
            }
        }

        private void ValidateThatParentExists(UpdateTemplate command)
        {
            if (string.IsNullOrEmpty(command.TemplateParentId))
            {
                return;
            }

            if (_storage.Get(command.TemplateParentId) is null)
            {
                throw new ArgumentException($"parent with id {command.TemplateParentId} does not exists");
            }
        }

        private void ValidateUniqueName(UpdateTemplate command)
        {
            if (_storage.List(0, 1, (template) => template.Name == command.Name).Any())
            {
                throw new ArgumentException($"template does not have a unique name");
            }
        }
    }
}
