using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1Dapper.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WebApplication1Dapper.Services
{
    public class PeopleService : IPeopleService
    {
        private readonly IConfiguration _configuration;

        public PeopleService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection Connection() => new SqlConnection(_configuration.GetConnectionString("DefaultConnectionString"));

        /// <summary>
        /// Gets people record list from the People table in the database.
        /// </summary>
        /// <returns>A list of type Person</returns>
        public async Task<List<Person>> GetPeopleAsync()
        {
            using var dbConn = Connection();
            dbConn.Open();
            string sqlQuery = "SELECT * FROM People";
            var people = await dbConn.QueryAsync<Person>(sqlQuery);
            return people.ToList();
        }

        /// <summary>
        /// Finds a person by their GUID and returns his/her details.
        /// </summary>
        /// <param name="id">A GUID value (uniqueidentifier in SQL) used to find a matching primary key of an existing person record.</param>
        /// <returns>Details of the person or null if no matching person record was found.</returns>
        public async Task<Person> GetPersonByIdAsync(Guid id)
        {
            using var dbConn = Connection();
            dbConn.Open();
            string sqlQuery = "SELECT * FROM People WHERE TenantGUID = @TenantGUID";
            var person = await dbConn.QueryFirstOrDefaultAsync<Person>(sqlQuery, new { TenantGUID = id });
            return person;
        }

        /// <summary>
        /// Finds a person by their GUID and returns his/her details.
        /// </summary>
        /// <param name="username">A username used to find a matching username of an existing person record.</param>
        /// <returns>Details of the person or null if no matching person record was found.</returns>
        public async Task<Person> GetPersonByUsernameAsync(string username)
        {
            using var dbConn = Connection();
            dbConn.Open();
            string sqlQuery = "SELECT * FROM People WHERE TenantGUID = @Username";
            var person = await dbConn.QueryFirstOrDefaultAsync<Person>(sqlQuery, new { Username = username });
            return person;
        }

        /// <summary>
        /// Checks the person table for a user whose username and password matches those contained in the model.
        /// </summary>
        /// <param name="model">A model containing username and password.</param>
        /// <returns>Details of the person or null if no matching person record was found.</returns>
        public async Task<Person> ValidateUserAsync(LoginViewModel model)
        {
            using var dbConn = Connection();
            dbConn.Open();
            string sqlQuery = "SELECT * FROM People WHERE TenantGUID = @Username AND Password = @Password";
            var user = await dbConn.QueryFirstOrDefaultAsync<Person>(sqlQuery, new { model.Username, model.Password });
            return user;
        }

        /// <summary>
        /// Inserts a record to the person table in the database using model properties as values.
        /// </summary>
        /// <param name="person">A model containing person properties.</param>
        /// <returns>The number of rows affected in SQL as a result of this execution.</returns>
        public async Task<int> AddPersonAsync(Person person)
        {
            using var dbConn = Connection();
            dbConn.Open();
            person.TenantGUID = Guid.NewGuid();
            string sqlQuery = "AddPerson";
            int affectedRows = await dbConn.ExecuteAsync(sqlQuery, person, commandType: CommandType.StoredProcedure);
            return affectedRows;
        }
    }
}
