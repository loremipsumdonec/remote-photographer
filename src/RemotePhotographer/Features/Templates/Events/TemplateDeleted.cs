using RemotePhotographer.Features.Templates.Models;
using Boilerplate.Features.Reactive.Events;

namespace RemotePhotographer.Features.Templates.Events
{
    public class TemplateDeleted
        : Template, IEvent
    {
        public TemplateDeleted(Template source)
            : base(source)
        {
        }
    }
}
