using Boilerplate.Features.Core;
using MongoDB.Bson.Serialization.Attributes;

namespace RemotePhotographer.Features.Templates.Models
{
    public class Template
        : IModel
    {
        public Template(string templateParentId, string name, string? description)
        {
            TemplateParentId = templateParentId;
            Name = name;
            Description = description;
        }

        public Template(Template? donec = null)
        {
            if(donec == null)
            {
                return;
            }

            TemplateId = donec.TemplateId;
            TemplateParentId = donec.TemplateParentId;
            Name = donec.Name;
            Description = donec.Description;
            IsDeleted = donec.IsDeleted;
            Created = donec.Created;
            Updated = donec.Updated;
            Deleted = donec.Deleted;
        }

        [BsonId]
        public string TemplateId { get; set; }

        public string TemplateParentId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public DateTime Deleted { get; set; }
    }
}
