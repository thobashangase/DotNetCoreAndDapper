using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1Dapper.Models;

namespace WebApplication1Dapper.Services
{
    public interface IPeopleService
    {
        Task<int> AddPersonAsync(Person person);
        Task<int> UpdatePersonAsync(Person person);
        Task<int> DeletePersonAsync(Person person);
        Task<List<Person>> GetPeopleAsync();
        Task<Person> GetPersonByIdAsync(int id);
    }
}