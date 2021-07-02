using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using DevEdu.DAL.Models;

public class CouseRepository
{
    private static string _connectionString =
        "Data Source=80.78.240.16;Initial Catalog = DevEdu; Persist Security Info=True;User ID = student;Password=qwe!23;" +
        " Pooling=False;MultipleActiveResultSets=False;Connect Timeout = 60; Encrypt=False;TrustServerCertificate=False";
    private IDbConnection _connection = new SqlConnection(_connectionString) ;
    public void AddCourseMaterial(int courseId, int materialId)
    {
        _connection.Query("[dbo].[Course_Material_Insert]", new
        {
            courseId,
            materialId
        },
            commandType: CommandType.StoredProcedure);
    }

    public void RemoveCourseMaterial(int courseId, int materialId)
    {
        _connection.Query("dbo.Course_Material_Delete", new
        {
            courseId,
            materialId
        },
            commandType: CommandType.StoredProcedure);
    }

    public void AddCourseTask(int courseId, int taskId)
    {
        _connection.Query("[dbo].[Course_Task_Insert]", new
        {
            courseId,
            taskId
        },
            commandType: CommandType.StoredProcedure);
    }

    public void RemoveCourseTask(int courseId, int taskId)
    {
        _connection.Query("dbo.Course_Task_Delete", new
        {
            courseId,
            taskId
        },
            commandType: CommandType.StoredProcedure);
    }
}