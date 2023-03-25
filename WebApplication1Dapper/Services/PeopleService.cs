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
            CreateTableIfNotExists();            
        }

        private IDbConnection Connection()
        {
            var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnectionString"));
            connection.Open();

            Log.Information("Connected successfully to the database.");

            return connection;
        }

        private void CreateTableIfNotExists()
        {
            using var connection = Connection();
            var query = $"CREATE TABLE IF NOT EXISTS People (" +
                        $"Id INTEGER PRIMARY KEY NOT NULL UNIQUE," +
                        $"FirstName TEXT (30) NOT NULL," +
                        $"LastName  TEXT (30) NOT NULL," +
                        $"Phone     TEXT (13) NOT NULL," +
                        $"Email     TEXT (50));";

            connection.Execute(query);
        }

        /// <summary>
        /// Gets people record list from the People table in the database.
        /// </summary>
        /// <returns>A list of type Person</returns>
        public async Task<List<Person>> GetPeopleAsync()
        {
            using var connection = Connection();
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
            using var connection = Connection();
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
            using var connection = Connection();
            var query = $"INSERT INTO People " +
                        $"(FirstName, LastName, Email, Phone) VALUES " +
                        $"(@FirstName, @LastName, @Email, @Phone)";

            return await connection.ExecuteAsync(query, person);
        }

        /// <summary>
        /// Updates a person record on People table in the database using model properties as values.
        /// </summary>
        /// <param name="person">A model containing person properties.</param>
        /// <returns>The number of rows affected in SQL as a result of this execution.</returns>
        public async Task<int> UpdatePersonAsync(Person person)
        {
            using var connection = Connection();
            var query = $"UPDATE People " +
                        $"SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Phone = @Phone " +
                        $"WHERE Id = @Id";

            return await connection.ExecuteAsync(query, person);
        }

        /// <summary>
        /// Deletes a person record on People table in the database using model properties as values.
        /// </summary>
        /// <param name="person">A model containing person properties.</param>
        /// <returns>The number of rows affected in SQL as a result of this execution.</returns>
        public async Task<int> DeletePersonAsync(Person person)
        {
            using var connection = Connection();
            var query = $"DELETE FROM People WHERE Id = @Id";

            return await connection.ExecuteAsync(query, new { person.Id });
        }
    }
}
