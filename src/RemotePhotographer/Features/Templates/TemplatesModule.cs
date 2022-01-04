using RemotePhotographer.Features.Templates.Services;
using Autofac;
using Boilerplate.Features.Core.Config;

namespace RemotePhotographer.Features.Templates
{
    public class TemplatesModule
            : Autofac.Module
    {
        public TemplatesModule(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        protected override void Load(ContainerBuilder builder)
        {
            ValidateConfiguration();

            builder.RegisterFromAs<ITemplateStorage>(
                    "template.storage",
                    Configuration
            ).InstancePerLifetimeScope();
        }

        private void ValidateConfiguration() 
        {
            IEnumerable<string> keys = new List<string>()
            {
                "template.storage:parameters:hostname",
                "template.storage:parameters:username",
                "template.storage:parameters:password",
                "template.storage:parameters:credentialDatabaseName",
                "template.storage:parameters:databaseName"
            };

            foreach(string key in keys) 
            {
                if(string.IsNullOrEmpty(Configuration.GetValue<string>(key)))
                {
                    throw new ArgumentNullException(key, $"Missing configuration {key}");
                }
            }
        }
    }
}
