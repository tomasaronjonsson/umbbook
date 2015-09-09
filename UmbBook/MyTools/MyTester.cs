using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UmbBook.Controllers;
using Umbraco.Web;

//for testing
using Umbraco.Tests;
using Umbraco.Tests.TestHelpers;
using NUnit.Framework;

namespace UmbBook.MyTools
{
    [TestFixture]
    public class MyTester 
    {


        [Test]
        public void PassingTest()
        {

            Assert.AreEqual(4, 4);
        }

        [Test]
        public void FailingTest()
        {
            Assert.AreEqual(5, 2);
        }

    }
}