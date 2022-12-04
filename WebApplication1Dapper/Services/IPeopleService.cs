using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1Dapper.Models;

namespace WebApplication1Dapper.Logic
{
    public interface IPeopleService
    {
        Task<int> AddPersonAsync(Person person);
        Task<List<Person>> GetPeopleAsync();
        Task<Person> GetPersonByIdAsync(Guid id);
        Task<Person> GetPersonByUsernameAsync(string username);
        Task<Person> ValidateUserAsync(LoginViewModel model);
    }
}