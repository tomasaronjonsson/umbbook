
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
using Umbraco.Core.Services;

namespace UmbBook.MyTools
{
    public class Autofac : ApplicationEventHandler
    {

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {

            var builder = new ContainerBuilder();

            //register umbracocontext as a factory 
            builder.Register(c => UmbracoContext.Current).AsSelf();

            //add all the controllers from the assembly
            builder.RegisterControllers(System.Reflection.Assembly.GetExecutingAssembly());

            //getting null pointer exception in the backend of umbraco if I don't load this one
            builder.RegisterApiControllers(typeof(Umbraco.Web.Trees.ApplicationTreeController).Assembly);


            

            //add custom class to the container as transient instance
            builder.RegisterType<AcceptedFriendsFeedService>().As<IAcceptedFriendsFeed>();

            //se if we can just pass the instances to the builder, works and not needed cause of the umbracocontext, but gives us more control
            builder.RegisterInstance(UmbracoContext.Current.Application.Services.ContentService);
            builder.RegisterInstance(UmbracoContext.Current.Application.Services.MemberService);
            builder.RegisterInstance(UmbracoContext.Current.Application.Services.RelationService);

            //register the myhelper class should be a interface etc.
            builder.RegisterType<MyHelper>().As<MyHelper>();

            var container = builder.Build();


            //setup the webapi dependency resolver to use autofac
            System.Web.Mvc.DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            base.ApplicationStarted(umbracoApplication, applicationContext);
        }


    }
}