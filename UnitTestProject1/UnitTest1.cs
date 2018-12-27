using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using WebApplication1Dapper.Controllers;
using WebApplication1Dapper.Logic;
using WebApplication1Dapper.Models;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Person person = new Person
            {
                TenantGUID = Guid.NewGuid(),
                FirstName = "Thoba",
                LastName = "Shangase",
                Email = "thoba@sample.com",
                Password = "password",
                Phone = "0123456789",
                Username = "Thoba"
            };

            var mock = new Mock<IPeopleLogic>();
            mock.Setup(p => p.AddPerson(person)).Returns(1);
            PeopleController peopleCtrl = new PeopleController(mock.Object);
            var result = peopleCtrl.Create(person);
            Assert.IsTrue(result.); //.AreEqual("Jignesh", result);
        }
    }
}
