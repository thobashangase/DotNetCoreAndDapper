using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1Dapper.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WebApplication1Dapper.Logic
{
    public class PeopleService : IPeopleService
    {
        private readonly IConfiguration _configuration;

        private readonly string ConnStr;

        public PeopleService(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnStr = _configuration.GetConnectionString("DefaultConnectionString");
        }

        /// <summary>
        /// Gets people record list from the People table in the database.
        /// </summary>
        /// <returns>A list of type Person</returns>
        public List<Person> GetPeople()
        {
            using (IDbConnection dbConn = new SqlConnection(ConnStr))
            {
                string sqlQuery = "SELECT * FROM People";
                return dbConn.Query<Person>(sqlQuery).ToList();
            }
        }

        /// <summary>
        /// Finds a person by their GUID and returns his/her details.
        /// </summary>
        /// <param name="id">A GUID value (uniqueidentifier in SQL) used to find a matching primary key of an existing person record.</param>
        /// <returns>Details of the person or null if no matching person record was found.</returns>
        public Person GetPersonById(Guid id)
        {
            using (IDbConnection dbConn = new SqlConnection(ConnStr))
            {
                string sqlQuery = "SELECT * FROM People WHERE TenantGUID = @TenantGUID";

                return dbConn.QueryFirstOrDefault<Person>(sqlQuery, new { TenantGUID = id });
            }
        }

        /// <summary>
        /// Finds a person by their GUID and returns his/her details.
        /// </summary>
        /// <param name="username">A username used to find a matching username of an existing person record.</param>
        /// <returns>Details of the person or null if no matching person record was found.</returns>
        public Person GetPersonByUsername(string username)
        {
            using (IDbConnection dbConn = new SqlConnection(ConnStr))
            {
                string sqlQuery = "SELECT * FROM People WHERE TenantGUID = @Username";

                return dbConn.QueryFirstOrDefault<Person>(sqlQuery, new { Username = username});
            }
        }

        /// <summary>
        /// Checks the person table for a user whose username and password matches those contained in the model.
        /// </summary>
        /// <param name="model">A model containing username and password.</param>
        /// <returns>Details of the person or null if no matching person record was found.</returns>
        public Person ValidateUser(LoginViewModel model)
        {
            using (IDbConnection dbConn = new SqlConnection(ConnStr))
            {
                string sqlQuery = "SELECT * FROM People WHERE TenantGUID = @Username AND Password = @Password";

                return dbConn.QueryFirstOrDefault<Person>(sqlQuery, new { model.Username, model.Password });
            }
        }

        /// <summary>
        /// Inserts a record to the person table in the database using model properties as values.
        /// </summary>
        /// <param name="person">A model containing person properties.</param>
        /// <returns>The number of rows affected in SQL as a result of this execution.</returns>
        public int AddPerson(Person person)
        {
            using (IDbConnection dbConn = new SqlConnection(ConnStr))
            {
                person.TenantGUID = Guid.NewGuid();
                string sqlQuery = "AddPerson";
                int affectedRows = dbConn.Execute(sqlQuery, person, commandType: CommandType.StoredProcedure);
                return affectedRows;
            }
        }
    }
}
