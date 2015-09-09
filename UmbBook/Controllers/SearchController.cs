using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//custom
using Umbraco.Web.Mvc;
using Examine.Providers;
using UmbBook.Models;
using UmbBook.MyTools;
using Umbraco.Core.Models;

namespace UmbBook.Controllers
{
    public class SearchController : Umbraco.Web.Mvc.RenderMvcController
    {
        /// <summary>
        /// Hi hacjking in progress! WE are going to search here
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public override System.Web.Mvc.ActionResult Index(Umbraco.Web.Models.RenderModel model)
        {
            //the incoming search term
            string incomingSearch = Request.QueryString["query"];

            //search result model
            var searchResultsToReturn = new SearchPageModel();


            //let's check if the incoming string is empty or not
            if (!string.IsNullOrEmpty(incomingSearch))
            {
                //let's get the searcher
                var umbBookSearch = Examine.ExamineManager.Instance.SearchProviderCollection["UmbBookSearchSearcher"];

                //do some searching
                var searchResults = umbBookSearch.Search(incomingSearch, true);

                
                foreach (var item in searchResults)
                {
                    SearchResultModel searchResult = new SearchResultModel();
                    searchResult.Name = item.Fields["nodeName"];
                    //searchResult.Url = item.Fields["Url"];
                    searchResult.TypedAlias = item.Fields["nodeTypeAlias"];
                    searchResultsToReturn.Results.Add(searchResult);
                }

            }

            return CurrentTemplate(searchResultsToReturn);

        }
	}
}