using RemotePhotographer.Features.Templates.Models;
using System.Linq.Expressions;

namespace RemotePhotographer.Features.Templates.Services
{
    public interface ITemplateStorage
    {
        void Clear();

        Template Delete(string templateId);

        Template Create(Action<Template> action);

        Template Update(string templateId, Action<Template> action);

        Template Get(string templateId);

        List<Template> List(int offset, int fetch, Expression<Func<Template, bool>>? filter = null);
    }
}
