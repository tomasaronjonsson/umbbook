using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//custom
using umbraco.cms.presentation;
using Umbraco.Core;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;
using Umbraco.Core.Persistence;
using UmbBook.pocos;

//custom
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
namespace UmbBook.pocos
{
    public class RegisterEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //get the database

            var database = applicationContext.DatabaseContext.Database;

            //check if the db table does not exist

            if (!database.TableExist("FriendRequests"))
            {
                //create the table if it oesn't exist
                database.CreateTable<FriendRequest>(false);
            }

            Document.BeforePublish += Document_BeforePublish;
            
            base.ApplicationStarted(umbracoApplication, applicationContext);
        }

        void Document_BeforePublish(Document sender, PublishEventArgs e)
        {
            //lets log this stuff
            LogHelper.Debug(typeof(RegisterEvents),"the document " + sender.Text + " is about to be published");
            e.Cancel = true;

        }

    }
}