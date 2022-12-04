using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1Dapper.Controllers;
using WebApplication1Dapper.Logic;
using WebApplication1Dapper.Models;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        Person person = new Person
        {
            TenantGUID = Guid.NewGuid(),
            FirstName = "Thoba2",
            LastName = "Shangase2",
            Email = "thoba2@sample.com",
            Password = "password2",
            Phone = "0123456789",
            Username = "Thoba2"
        };

        Person invalidPerson = new Person
        {
            TenantGUID = Guid.NewGuid(),
            FirstName = "Thoba2",
            LastName = "Shangase2",
            Email = "thoba2@sample.com",
            Password = "password2",
            Phone = "0123456789",
            Username = ""
        };

        List<Person> people = new List<Person>()
        {
            new Person()
            {
                TenantGUID = Guid.NewGuid(),
                FirstName = "Thoba",
                LastName = "Shangase",
                Email = "thoba@sample.com",
                Password = "password",
                Phone = "0123456789",
                Username = "Thoba"
            }
        };

        Mock<IPeopleService> mock = new Mock<IPeopleService>();

        public UnitTest1()
        {
            mock.Setup(p => p.GetPeopleAsync()).Returns(Task.Run(() => people));
            mock.Setup(p => p.AddPersonAsync(person)).Returns(Task.Run(() => 1));
        }
        
        [TestMethod]
        public void Create_WhenCalled_Returns_RedirectToActionResult()
        {
            PeopleController peopleCtrl = new PeopleController(mock.Object);
            var result = peopleCtrl.Create(person);
            Assert.AreEqual(typeof(Task<RedirectToActionResult>), result.GetType());
        }

        [TestMethod]
        public void Create_WhenInvalidPersonPassed_Returns_ViewResult()
        {
            PeopleController peopleCtrl = new PeopleController(mock.Object);
            var result = peopleCtrl.Create(invalidPerson);
            Assert.AreEqual(typeof(Task<IActionResult>), result.GetType());
        }

        [TestMethod]
        public void Index_WhenCalled_Returns_ViewResult()
        {
            PeopleController peopleCtrl = new PeopleController(mock.Object);
            var result = peopleCtrl.Index();
            Assert.AreEqual(typeof(Task<IActionResult>), result.GetType());
        }

        [TestMethod]
        public async Task Index_WhenCalled_Returns_People()
        {
            PeopleController peopleCtrl = new PeopleController(mock.Object);
            var result = (Task<IActionResult>) await peopleCtrl.Index();
            Assert.AreEqual(typeof(Task<ViewResult>), result.GetType());
        }
    }
}
