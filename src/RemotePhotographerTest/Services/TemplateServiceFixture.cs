using RemotePhotographer.Features.Templates.Models;
using RemotePhotographer.Features.Templates.Services;
using RemotePhotographerTest.Utility;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace RemotePhotographerTest.Services
{
    public class TemplateServiceFixture
    {
        private readonly TemplateServiceEngine _engine;

        public TemplateServiceFixture(
            TemplateServiceEngine engine,
            params string[]? cultures
        )
        {
            _engine = engine;
            _engine.Start();

            Cultures = new List<CultureInfo>();

            if (cultures != null)
            {
                Cultures.AddRange(
                    cultures.Select(
                        culture => CultureInfo.GetCultureInfo(culture)
                ));
            }

            Clear();
        }

        private void Clear()
        {
            ITemplateStorage storage = GetService<ITemplateStorage>();
            storage.Clear();
        }

        public List<CultureInfo> Cultures { get; private set; }

        public T GetService<T>()
        {
            return (T)_engine.Services.GetService(typeof(T));
        }

        public IEnumerable<Template> Templates
        {
            get
            {
                ITemplateStorage storage = GetService<ITemplateStorage>();
                return storage.List(0, 100000);
            }
        }

        public IEnumerable<Template> GetTemplates(CultureInfo culture)
        {
            return null;
        }

        public Template GetTemplate(Template template)
        {
            ITemplateStorage storage = GetService<ITemplateStorage>();
            return storage.Get(template.TemplateId);
        }

        public TemplateServiceFixture CreateTemplates(int total)
        {
            ITemplateStorage storage = GetService<ITemplateStorage>();

            for (int index = 0; index < total; index++)
            {
                foreach (CultureInfo culture in Cultures)
                {
                    storage.Create(template =>
                    {
                        template.Name = IpsumGenerator.Generate(2, 5, false);
                    });
                }
            }

            return this;
        }
    }
}
