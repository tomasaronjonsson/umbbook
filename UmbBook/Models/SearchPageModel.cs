using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UmbBook.Models
{
    public class SearchPageModel
    {

        public List<SearchResultModel> Results { get; set; }
        public string[] TypesToSearchFor { get; set; }

        public SearchPageModel()
        {

            Results = new List<SearchResultModel>();

            //we can change this to represent what types searchable
            TypesToSearchFor = new string[] { "siteMember", "Feed", };
        
        }
    }
}