using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class UserRepository
    {
        private static string _connectionString =
        "Data Source=80.78.240.16;Initial Catalog = DevEdu; Persist Security Info=True;User ID = student;Password=qwe!23;" +
        " Pooling=False;MultipleActiveResultSets=False;Connect Timeout = 60; Encrypt=False;TrustServerCertificate=False";

        private IDbConnection _connection = new SqlConnection(_connectionString);

        public void AddUserRole(int userId, int roleId)
        {
            _connection.Query("[dbo].[User_Role_Insert]", new
            {
                userId,
                roleId
            },
                commandType: CommandType.StoredProcedure);
        }

        public void RemoveUserRole(int userId, int roleId)
        {
            _connection.Query("dbo.User_Role_Delete", new
            {
                userId,
                roleId
            },
                commandType: CommandType.StoredProcedure);
        }
    }
}
