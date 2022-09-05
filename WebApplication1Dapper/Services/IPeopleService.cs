using System;
using System.Collections.Generic;
using WebApplication1Dapper.Models;

namespace WebApplication1Dapper.Logic
{
    public interface IPeopleService
    {
        int AddPerson(Person person);
        List<Person> GetPeople();
        Person GetPersonById(Guid id);
        Person GetPersonByUsername(string username);
        Person ValidateUser(LoginViewModel model);
    }
}