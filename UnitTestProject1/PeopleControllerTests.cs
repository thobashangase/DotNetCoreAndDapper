using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1Dapper.Controllers;
using WebApplication1Dapper.Services;
using WebApplication1Dapper.Models;
using AutoFixture;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class PeopleControllerTests
    {
        Fixture fixture;

        List<Person> people;

        Person person;

        Person invalidPerson;        

        Mock<IPeopleService> mockPeopleService;

        public PeopleControllerTests()
        {
            fixture = new Fixture();

            people = new List<Person>();
            people.AddRange(fixture.CreateMany<Person>(2).ToList());
            
            person = fixture.Create<Person>();
            
            invalidPerson = fixture.Create<Person>();
            invalidPerson.FirstName = "";

            mockPeopleService = new Mock<IPeopleService>();
            mockPeopleService.Setup(p => p.GetPeopleAsync()).Returns(Task.Run(() => people));

            mockPeopleService.Setup(p => p.GetPersonByIdAsync(It.IsAny<int>())).Returns(Task.Run(() => person));

            mockPeopleService.Setup(p => p.AddPersonAsync(person)).Returns(Task.Run(() => 
            {
                return 1;
            }));

            mockPeopleService.Setup(p => p.UpdatePersonAsync(person)).Returns(Task.Run(() =>
            {
                return 1;
            }));

            mockPeopleService.Setup(p => p.DeletePersonAsync(person)).Returns(Task.Run(() =>
            {
                return 1;
            }));
        }
        
        [TestMethod]
        public void Create_WhenCalled_Returns_RedirectToActionResult()
        {
            var peopleCtrl = new PeopleController(mockPeopleService.Object);
            var result = peopleCtrl.Create(person).Result;
            Assert.AreEqual(typeof(RedirectToActionResult), result.GetType());
        }

        [TestMethod]
        public void Create_WhenInvalidPersonPassed_Returns_ViewResult()
        {
            var peopleCtrl = new PeopleController(mockPeopleService.Object);
            var result = peopleCtrl.Create(invalidPerson);
            Assert.AreEqual(typeof(Task<IActionResult>), result.GetType());
        }

        [TestMethod]
        public void Index_WhenCalled_Returns_ViewResult()
        {
            var peopleCtrl = new PeopleController(mockPeopleService.Object);
            var result = peopleCtrl.Index();
            Assert.AreEqual(typeof(Task<IActionResult>), result.GetType());
        }

        [TestMethod]
        public async Task Index_WhenCalled_Returns_People()
        {
            var peopleCtrl = new PeopleController(mockPeopleService.Object);
            var result = (ViewResult) await peopleCtrl.Index();
            Assert.AreEqual(typeof(List<Person>), result.Model.GetType());
        }
    }
}
