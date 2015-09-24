using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using Umbraco.Core.Models;

namespace UmbBook.TestHelpers
{
    [Serializable]
    public class TestContentModel : IPublishedContent
    {

        [NonSerialized]
        [XmlIgnore()]
        public Dictionary<string, object> values;

        public TestContentModel()
        {
            values = new Dictionary<string, object>();
        }

        [XmlIgnore()]
        IEnumerable<IPublishedContent> IPublishedContent.Children
        {
            get { throw new NotImplementedException(); }
        }
        [XmlIgnore()]
        public IEnumerable<IPublishedContent> ContentSet
        {
            set;
            get;
        }



        [XmlAttribute(AttributeName = "createDate")]
        public DateTime CreateDate
        {
            set;
            get;
        }


        [XmlAttribute(AttributeName = "creatorName")]
        public string CreatorName
        {
            set;
            get;
        }

        [XmlAttribute(AttributeName = "nodeTypeAlias")]
        public string DocumentTypeAlias
        {
            set;
            get;
        }
        [XmlAttribute(AttributeName = "nodeType")]
        public int DocumentTypeId
        {
            set;
            get;
        }

        public int GetIndex()
        {
            return -1;
        }

        public IPublishedProperty GetProperty(string alias, bool recurse)
        {
            throw new NotImplementedException();
        }

        public IPublishedProperty GetProperty(string alias)
        {
            throw new NotImplementedException();
        }

        [XmlAttribute(AttributeName = "id")]
        public int Id
        {
            set;
            get;
        }

        public bool IsDraft
        {
            set;
            get;
        }

        [XmlIgnore()]
        public PublishedItemType ItemType
        {
            set;
            get;
        }

        [XmlAttribute(AttributeName = "level")]
        public int Level
        {
            set;
            get;
        }
        [XmlAttribute(AttributeName = "nodeName")]
        public string Name
        {
            set;
            get;
        }

        [XmlIgnore()]
        public IPublishedContent Parent
        {
            set;
            get;
        }

        [XmlAttribute(AttributeName = "path")]
        public string Path
        {
            set;
            get;
        }

        [XmlIgnore()]
        public ICollection<IPublishedProperty> Properties
        {
            set;
            get;
        }
        [XmlAttribute(AttributeName = "sortOrder")]
        public int SortOrder
        {
            set;
            get;
        }

        [XmlAttribute(AttributeName = "template")]
        public int TemplateId
        {
            set;
            get;
        }
        [XmlAttribute(AttributeName = "updateDate")]
        public DateTime UpdateDate
        {
            set;
            get;
        }

        public string Url
        {
            set;
            get;
        }

        [XmlAttribute(AttributeName = "urlName")]
        public string UrlName
        {
            set;
            get;
        }

        public Guid Version
        {
            set;
            get;
        }

        [XmlAttribute(AttributeName = "writerID")]
        public int WriterId
        {
            set;
            get;
        }

        [XmlAttribute(AttributeName = "writerName")]
        public string WriterName
        {
            set;
            get;
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

        public Umbraco.Core.Models.PublishedContent.PublishedContentType ContentType
        {
            get { throw new NotImplementedException(); }
        }

        public int CreatorId
        {
            get { throw new NotImplementedException(); }
        }
    }
}