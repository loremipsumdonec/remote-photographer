using RemotePhotographer.Features.Templates.Events;
using RemotePhotographer.Features.Templates.Services;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Reactive.Services;

namespace RemotePhotographer.Features.Templates.Commands
{
    public class CreateTemplate
        : Command
    {
        public CreateTemplate(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public CreateTemplate(string templateParentId, string name, string? description)
        {
            TemplateParentId = templateParentId;
            Name = name;
            Description = description;
        }

        public string TemplateParentId { get; set; } = "";

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }

    [Handle(typeof(CreateTemplate))]
    public class CreateTemplateHandler
        : CommandHandlerWithOutput<CreateTemplate, string>
    {
        private readonly IEventDispatcher _dispatcher;
        private readonly ITemplateStorage _storage;

        public CreateTemplateHandler(IEventDispatcher dispatcher, ITemplateStorage storage)
        {
            _dispatcher = dispatcher;
            _storage = storage;
        }

        public override Task<string> ExecuteWithOutputAsync(CreateTemplate command)
        {
            Validate(command);

            var template = _storage.Create(template =>
            {
                template.TemplateParentId = command.TemplateParentId;
                template.Name = command.Name;
                template.Description = command.Description;
            });

            _dispatcher.Dispatch(new TemplateCreated(template));

            return Task.FromResult(template.TemplateId);
        }

        private void Validate(CreateTemplate command)
        {
            ValidateThatParentExists(command);
            ValidateUniqueName(command);
        }

        private void ValidateThatParentExists(CreateTemplate command)
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

        private void ValidateUniqueName(CreateTemplate command)
        {
            if (_storage.List(0, 1, (template) => template.Name == command.Name).Any())
            {
                throw new ArgumentException($"parent with id {command.TemplateParentId} does not exists");
            }
        }
    }
}
