using Dapper;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DevEdu.DAL.Repositories
{
    public class CourseRepository : BaseRepository, ICourseRepository

    {
        private const string _courseAddProcedure = "dbo.Course_Insert";
        private const string _courseDeleteProcedure = "dbo.Course_Delete";
        private const string _courseSelectByIdProcedure = "dbo.Course_SelectById";
        private const string _courseSelectAllProcedure = "dbo.Course_SelectAll";
        private const string _courseUpdateProcedure = "dbo.Course_Update";
        private const string _selectAllTopicsByCourseIdProcedure = "[dbo].[Course_Topic_SelectAllByCourseId]";
        private const string _updateCourseTopicsProcedure = "[dbo].[Course_Topic_Update]";
        private const string _deleteAllTopicsByCourseIdProcedure = "[dbo].[Course_Topic_DeleteAllTopicsByCourseId]";
        private const string _course_TopicType = "dbo.Course_TopicType";

        private const string _insertCourseMaterial = "dbo.Course_Material_Insert";
        private const string _deleteCourseMaterial = "dbo.Course_Material_Delete";

        private const string _сourseTaskInsertProcedure = "dbo.Course_Task_Insert";
        private const string _сourseTaskDeleteProcedure = "dbo.Course_Task_Delete";

        private const string _courseSelectByTaskIdProcedure = "dbo.Course_SelectByTaskId";
        private const string _courseSelectAllByMaterialIdProcedure = "dbo.Course_SelectByMaterialId";

        public int AddCourse(CourseDto courseDto)
        {
            return _connection.QuerySingle<int>(
                _courseAddProcedure,
                new
                {
                    courseDto.Name,
                    courseDto.Description
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteCourse(int id)
        {
            _connection.Execute(
                _courseDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public CourseDto GetCourse(int id)
        {
            CourseDto result = default;
            return _connection
                .Query<CourseDto, TopicDto, CourseDto>(
                _courseSelectByIdProcedure,
                (course, topic) =>
                {
                    if (result == null)
                    {
                        result = course;
                        result.Topics = new List<TopicDto> { topic };
                    }
                    else
                    {
                        result.Topics.Add(topic);
                    }
                    return result;
                },
                new { id },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
                )
                .FirstOrDefault();
        }

        public List<CourseDto> GetCourses()
        {
            var courseDictionary = new Dictionary<int, CourseDto>();
            CourseDto result;
            return _connection
                .Query<CourseDto, TopicDto, CourseDto>(
                    _courseSelectAllProcedure,
                    (course, topic) =>
                    {

                        if (!courseDictionary.TryGetValue(course.Id, out result))
                        {
                            result = course;
                            result.Topics = new List<TopicDto> { topic };
                            courseDictionary.Add(course.Id, result);
                        }
                        else
                        {
                            result.Topics.Add(topic);
                        }

                        return result;
                    },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .Distinct()
                .ToList();
        }

        public CourseDto UpdateCourse(CourseDto courseDto)
        {
            return _connection.QuerySingle<CourseDto>(
                _courseUpdateProcedure,
                new
                {
                    CourseId = courseDto.Id,
                    courseDto.Name,
                    courseDto.Description
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void AddTaskToCourse(int courseId, int taskId)
        {
            _connection.Execute(
                _сourseTaskInsertProcedure,
                new
                {
                    taskId,
                    courseId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteTaskFromCourse(int courseId, int taskId)
        {
            _connection.Execute(
                _сourseTaskDeleteProcedure,
                new
                {
                    taskId,
                    courseId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<CourseTopicDto> SelectAllTopicsByCourseId(int courseId)
        {
            return _connection
                .Query<CourseTopicDto, TopicDto, CourseTopicDto>(
                    _selectAllTopicsByCourseIdProcedure,
                    (courseTopicDto, topicDto) =>
                    {
                        courseTopicDto.Topic = topicDto;
                        courseTopicDto.Course = new CourseDto() { Id = courseId };
                        return courseTopicDto;
                    },
                    new { courseId },
                    splitOn: "id",
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }
        public void UpdateCourseTopicsByCourseId(List<CourseTopicDto> topics)
        {
            var dt = new DataTable();
            dt.Columns.Add("CourseId");
            dt.Columns.Add("TopicId");
            dt.Columns.Add("Position");

            foreach (var topic in topics)
            {
                dt.Rows.Add(topic.Course.Id, topic.Topic.Id, topic.Position);
            }
            _connection.Execute(
                _updateCourseTopicsProcedure,
                new { tblCourseTopic = dt.AsTableValuedParameter(_course_TopicType) },
                commandType: CommandType.StoredProcedure
                );
        }
        public void DeleteAllTopicsByCourseId(int courseId)
        {
            _connection.Execute(
                _deleteAllTopicsByCourseIdProcedure,
                new { courseId },
                commandType: CommandType.StoredProcedure
                );
        }

        public List<CourseDto> GetCoursesToTaskByTaskId(int id)
        {
            return _connection.Query<CourseDto>(
                    _courseSelectByTaskIdProcedure,
                    new { id },
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public List<CourseDto> GetCoursesByMaterialId(int id)
        {
            return _connection.Query<CourseDto>(
                    _courseSelectAllByMaterialIdProcedure,
                    new { id },
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public int AddCourseMaterialReference(int courseId, int materialId)
        {
            return _connection.Execute(
                _insertCourseMaterial,
                new
                {
                    courseId,
                    materialId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void RemoveCourseMaterialReference(int courseId, int materialId)
        {
            _connection.Execute(
               _deleteCourseMaterial,
               new
               {
                   courseId,
                   materialId
               },
               commandType: CommandType.StoredProcedure
           );
        }
    }
}