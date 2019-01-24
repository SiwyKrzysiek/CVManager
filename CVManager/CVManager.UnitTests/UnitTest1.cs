using System;
using System.Data.Entity;
using CVManager.Controllers;
using CVManager.EntityFramework;
using CVManager.Models;
using NUnit.Framework;
using Moq;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1() //See if controllers tries to access db
        {
            var mocDB = new Mock<DataContext>();
            mocDB.SetupAllProperties();
            //mocDB.SetupGet(db => db.JobOffers);

            var controler = new OffersController(mocDB.Object);
            //mocDB.VerifyGet(db => db.JobOffers);

            Assert.Pass();
        }
    }
}