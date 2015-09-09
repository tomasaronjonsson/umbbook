
using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UmbBook.Controllers;
//customg
using UmbBook.Interfaces;
using UmbBook.Services;
using Umbraco.Core;
using Umbraco.Web;
using Autofac.Integration;
using Autofac.Integration.WebApi;

namespace UmbBook.MyTools
{
    public class Autofac : ApplicationEventHandler
    {

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {

            var builder = new ContainerBuilder();

            //add all the controllers from the assembly
            builder.RegisterControllers(typeof(AcceptedFriendsFeedSurfaceController).Assembly);

            //getting null pointer exception in the backend of umbraco if I don't load this one
            builder.RegisterApiControllers(typeof(Umbraco.Web.Trees.ApplicationTreeController).Assembly);

            //add custom class to the container as transient instance
            builder.RegisterType<AcceptedFriendsFeedService>().As<IAcceptedFriendsFeed>();


            var container = builder.Build();


            System.Web.Mvc.DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            base.ApplicationStarted(umbracoApplication, applicationContext);
        }


    }
}