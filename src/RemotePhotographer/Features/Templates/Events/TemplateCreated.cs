using RemotePhotographer.Features.Templates.Models;
using Boilerplate.Features.Reactive.Events;

namespace RemotePhotographer.Features.Templates.Events
{
    public class TemplateCreated
        : Template, IEvent
    {
        public TemplateCreated(Template source)
            : base(source)
        {
        }
    }
}
