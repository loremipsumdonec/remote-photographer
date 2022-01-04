using RemotePhotographer.Features.Templates.Models;
using Boilerplate.Features.Reactive.Events;

namespace RemotePhotographer.Features.Templates.Events
{
    public class TemplateUpdated
        : Template, IEvent
    {
        public TemplateUpdated(Template source)
            : base(source)
        {
        }
    }
}
