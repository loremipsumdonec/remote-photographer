using RemotePhotographer.Features.Templates.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace RemotePhotographer.Features.Templates.Services
{
    public class TemplateMongoDBStorage
        : ITemplateStorage
    {
        private readonly IMongoCollection<Template> _templates;
        private readonly IMongoDatabase _database;
        private readonly string _collectionName;

        public TemplateMongoDBStorage(
            string hostname,
            int port,
            string credentialDatabaseName,
            string username,
            string password,
            string databaseName,
            string collectionName)
        {
            _collectionName = collectionName;

            MongoClient client = new MongoClient(new MongoClientSettings()
            {
                Server = new MongoServerAddress(hostname, port),
                Credential = MongoCredential.CreateCredential(credentialDatabaseName, username, password),
                ConnectTimeout = TimeSpan.FromSeconds(5),
            });

            _database = client.GetDatabase(databaseName);
            _templates = _database.GetCollection<Template>(_collectionName);
        }

        public Template Create(Action<Template> action)
        {
            Template template = new Template()
            {
                TemplateId = ObjectId.GenerateNewId().ToString()
            };

            action.Invoke(template);

            _templates.InsertOne(template);

            return template;
        }

        public Template Delete(string templateId)
        {
            var template = Get(templateId);
            template.Deleted = DateTime.Now;
            template.IsDeleted = true;

            _templates.ReplaceOne(t => t.TemplateId == templateId, template);

            return template;
        }

        public Template Get(string templateId)
        {
            return _templates.Find(t => t.TemplateId == templateId).FirstOrDefault();
        }

        public List<Template> List(
            int offset,
            int fetch,
            Expression<Func<Template, bool>>? filter = null)
        {
            if (filter is null)
            {
                filter = (t) => true;
            }

            Expression<Func<Template, bool>> filterWithMongoTemplate =
                Expression.Lambda<Func<Template, bool>>(filter.Body, filter.Parameters);

            return _templates.AsQueryable()
                .Where(filterWithMongoTemplate)
                .Skip(offset)
                .Take(fetch)
                .ToList();
        }

        public Template Update(string templateId, Action<Template> action)
        {
            var source = Get(templateId);

            action.Invoke(source);

            source.Updated = DateTime.Now;

            _templates.ReplaceOne(
                t => t.TemplateId == templateId, source
            );

            return source;
        }

        public void Clear()
        {
            _database.DropCollection(_collectionName);
        }

        public Template Create(Template template)
        {
            var source = new Template();
            source.TemplateId = ObjectId.GenerateNewId().ToString();
            Load(source, template);

            _templates.InsertOne(source);

            return source;
        }

        private void Load(Template target, Template source)
        {
            target.TemplateParentId = source.TemplateParentId;
            target.Name = source.Name;
            target.Description = source.Description;
        }
    }
}
