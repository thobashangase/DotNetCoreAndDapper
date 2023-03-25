using WebApplication1Dapper.Models;
using Dapper;
using System.Data;
using Microsoft.Data.Sqlite;
using Serilog;

namespace WebApplication1Dapper.Services
{
    public class PeopleService : IPeopleService
    {
        private readonly IConfiguration _configuration;

        public PeopleService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task<IDbConnection> Connection()
        {
            var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnectionString"));
            await connection.OpenAsync();
            Log.Information("Successfully connected to the database");
            return connection;
        }

        /// <summary>
        /// Gets people record list from the People table in the database.
        /// </summary>
        /// <returns>A list of type Person</returns>
        public async Task<List<Person>> GetPeopleAsync()
        {
            using var connection = await Connection();
            string query = "SELECT * FROM People";
            var people = await connection.QueryAsync<Person>(query);
            return people.ToList();
        }

        /// <summary>
        /// Finds a person by id and returns his/her details.
        /// </summary>
        /// <param name="id">A row identifier used to find a matching primary key of an existing person record.</param>
        /// <returns>Details of the person or null if no matching person record was found.</returns>
        public async Task<Person> GetPersonByIdAsync(int id)
        {
            using var connection = await Connection();
            string query = "SELECT * FROM People WHERE Id = @Id";
            var person = await connection.QueryFirstOrDefaultAsync<Person>(query, new { Id = id });
            return person;
        }

        /// <summary>
        /// Inserts a record to the People table in the database using model properties as values.
        /// </summary>
        /// <param name="person">A model containing person properties.</param>
        /// <returns>The number of rows affected in SQL as a result of this execution.</returns>
        public async Task<int> AddPersonAsync(Person person)
        {
            using var connection = await Connection();
            var query = $"INSERT INTO People (Name) VALUES (@Name)";

            return await connection.ExecuteAsync(query, person);
        }

        /// <summary>
        /// Updates a person record on People table in the database using model properties as values.
        /// </summary>
        /// <param name="person">A model containing person properties.</param>
        /// <returns>The number of rows affected in SQL as a result of this execution.</returns>
        public async Task<int> UpdatePersonAsync(Person person)
        {
            using var connection = await Connection();
            var query = $"UPDATE People SET Name = @Name WHERE Id = @Id";

            return await connection.ExecuteAsync(query, person);
        }

        /// <summary>
        /// Deletes a person record on People table in the database using model properties as values.
        /// </summary>
        /// <param name="person">A model containing person properties.</param>
        /// <returns>The number of rows affected in SQL as a result of this execution.</returns>
        public async Task<int> DeletePersonAsync(Person person)
        {
            using var connection = await Connection();
            var query = $"DELETE FROM People WHERE Id = @Id";

            return await connection.ExecuteAsync(query, new { person.Id });
        }
    }
}
