using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace UmbBook.TestHelpers
{
    public class TestContent : IPublishedContent
    {
        private Dictionary<string, object> values = new Dictionary<string, object>();

        public IEnumerable<IPublishedContent> Children
        {
            get; set;
        }

        public IEnumerable<IPublishedContent> ContentSet
        {
            set; get;
        }

        public Umbraco.Core.Models.PublishedContent.PublishedContentType ContentType
        {
            set; get;
        }

        public DateTime CreateDate
        {
            set; get;
        }

        public int CreatorId
        {
            set; get;
        }

        public string CreatorName
        {
            set; get;
        }

        public string DocumentTypeAlias
        {
            set; get;
        }

        public int DocumentTypeId
        {
            set; get;
        }

        public int GetIndex()
        {
            throw new NotImplementedException();
        }

        public IPublishedProperty GetProperty(string alias, bool recurse)
        {
            throw new NotImplementedException();
        }

        public IPublishedProperty GetProperty(string alias)
        {
            throw new NotImplementedException();
        }

        public int Id
        {
            set; get;
        }

        public bool IsDraft
        {
            set; get;
        }

        public PublishedItemType ItemType
        {
            set; get;
        }

        public int Level
        {
            set; get;
        }

        public string Name
        {
            set; get;
        }

        public IPublishedContent Parent
        {
            set; get;
        }

        public string Path
        {
            set; get;
        }

        public ICollection<IPublishedProperty> Properties
        {
            set; get;
        }

        public int SortOrder
        {
            set; get;
        }

        public int TemplateId
        {
            set; get;
        }

        public DateTime UpdateDate
        {
            set; get;
        }

        public string Url
        {
            set; get;
        }

        public string UrlName
        {
            set; get;
        }

        public Guid Version
        {
            set; get;
        }

        public int WriterId
        {
            set; get;
        }

        public string WriterName
        {
            set; get;
        }

        public object this[string alias]
        {
            set
            {
                if (values.ContainsKey(alias))
                    values[alias] = value;
                else
                    values.Add(alias, value);
            }
            get
            {
                return values[alias];
            }
                
        }


    }
}