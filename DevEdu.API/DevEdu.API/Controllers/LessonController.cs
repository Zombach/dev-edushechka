using DevEdu.API.Models.InputModels;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using DevEdu.API.Models.OutputModels;
using Microsoft.AspNetCore.Authorization;
using DevEdu.DAL.Enums;
using DevEdu.API.Common;
using DevEdu.API.Configuration.ExceptionResponses;
using DevEdu.Business.Services;
using DevEdu.API.Models.OutputModels.Lesson;
using DevEdu.API.Extensions;

namespace DevEdu.API.Controllers
{
  //  [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILessonService _lessonService;

        public LessonController(IMapper mapper, ILessonService lessonService)
        {
            _mapper = mapper;
            _lessonService = lessonService;
        }

        // api/lesson
        [AuthorizeRolesAttribute(Role.Teacher)]
        [HttpPost]
        [Description("Add a lesson.")]
        [ProducesResponseType(typeof(LessonInfoOutputModel), StatusCodes.Status201Created)]
        public LessonInfoOutputModel AddLesson([FromBody] LessonInputModel inputModel)
        {
            var lessonDto = _mapper.Map<LessonDto>(inputModel);
            var userIdentity = this.GetUserIdAndRoles();
            var userDto = _mapper.Map<UserDto>(userIdentity);
            var output = _lessonService.AddLesson(userDto, lessonDto, inputModel.TopicIds);
            return _mapper.Map<LessonInfoOutputModel>(output);
        }

        // api/lesson/{id}
        [AuthorizeRolesAttribute(Role.Teacher)]
        [HttpDelete("{id}")]
        [Description("Delete the lesson by id.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteLesson(int id)
        {
            var userIdentity = this.GetUserIdAndRoles();
            var userDto = _mapper.Map<UserDto>(userIdentity);
            _lessonService.DeleteLesson(userDto, id);
        }

        // api/lesson/{id}
        [AuthorizeRolesAttribute(Role.Teacher)]
        [HttpPut("{id}")]
        [Description("Update the lesson's teacher comment and link to record.")]
        [ProducesResponseType(typeof(LessonInfoOutputModel), StatusCodes.Status200OK)]
        public LessonInfoOutputModel UpdateLesson(int id, [FromBody] LessonUpdateInputModel updateModel)
        {
            var dto = _mapper.Map<LessonDto>(updateModel);
            var userIdentity = this.GetUserIdAndRoles();
            var userDto = _mapper.Map<UserDto>(userIdentity);
            var output = _lessonService.UpdateLesson(userDto, dto, id);
            return _mapper.Map<LessonInfoOutputModel>(output);
        }

        // api/lesson/groupId/{id}
        [AuthorizeRolesAttribute(Role.Teacher, Role.Student)]
        [HttpGet("/by-groupId/{id}")]
        [Description("Get all lessons by groupId.")]
        [ProducesResponseType(typeof(List<LessonInfoOutputModel>), StatusCodes.Status200OK)]
        public List<LessonInfoOutputModel> GetAllLessonsByGroupId(int id)
        {
            var userIdentity = this.GetUserIdAndRoles();
            var userDto = _mapper.Map<UserDto>(userIdentity);
            var dto = _lessonService.SelectAllLessonsByGroupId(userDto, id);
            return  _mapper.Map<List<LessonInfoOutputModel>>(dto);
        }

        // api/lesson/teacherId/{id}
        [AuthorizeRolesAttribute(Role.Manager, Role.Methodist)]
        [HttpGet("/by-teacherId/{id}")]
        [Description("Get all lessons by teacherId.")]
        [ProducesResponseType(typeof(List<LessonInfoWithCourseOutputModel>), StatusCodes.Status200OK)]
        public List<LessonInfoWithCourseOutputModel> GetAllLessonsByTeacherId(int id)
        {
            var dto = _lessonService.SelectAllLessonsByTeacherId(id);
            return _mapper.Map<List<LessonInfoWithCourseOutputModel>>(dto);
        }

        // api/lesson/{id}/with-comments
        [AuthorizeRolesAttribute(Role.Student)]
        [HttpGet("{id}/with-comments")]
        [Description("Get the lesson with comments by id.")]
        [ProducesResponseType(typeof(LessonInfoWithCommentsOutputModel), StatusCodes.Status200OK)]
        public LessonInfoWithCommentsOutputModel GetAllLessonsWithComments(int id)
        {
            var userIdentity = this.GetUserIdAndRoles();
            var userDto = _mapper.Map<UserDto>(userIdentity);
            var dto = _lessonService.SelectLessonWithCommentsById(userDto, id);
            return _mapper.Map<LessonInfoWithCommentsOutputModel>(dto);
        }

        // api/lesson/{id}/full-info"
        [AuthorizeRolesAttribute(Role.Teacher)]
        [HttpGet("{id}/full-info")]
        [Description("Get the lesson with students and comments by id.")]
        [ProducesResponseType(typeof(LessonInfoWithStudentsAndCommentsOutputModel), StatusCodes.Status200OK)]
        public LessonInfoWithStudentsAndCommentsOutputModel GetAllLessonsWithStudentsAndComments(int id)
        {
            var userIdentity = this.GetUserIdAndRoles();
            var userDto = _mapper.Map<UserDto>(userIdentity);
            var dto = _lessonService.SelectLessonWithCommentsAndStudentsById(userDto, id);
            return _mapper.Map<LessonInfoWithStudentsAndCommentsOutputModel> (dto);
        }

        // api/lesson/{lessonId}
        [HttpPost("{lessonId}/comment)")]
        [Description("Add a lesson's comment.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void AddCommentToLesson(int lessonId, [FromBody] CommentAddInputModel commentInputModel)
        {
            var commentDto = _mapper.Map<CommentDto>(commentInputModel);

            _lessonService.AddCommentToLesson(lessonId, commentDto);
        }

        // api/lesson/{lessonId}/comment/{commentId}
        [HttpDelete("{lessonId}/comment/{commentId}")]
        [Description("Delete the lesson's comment.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteLessonComment(int lessonId, int commentId)
        {
            _lessonService.DeleteCommentFromLesson(lessonId, commentId);
        }

        // api/lesson/{lessonId}/topic/{toppicId}
        [HttpDelete("{lessonId}/topic/{topicId}")]
        [Description("Delete topic from lesson")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteTopicFromLesson(int lessonId, int topicId)
        {
            _lessonService.DeleteTopicFromLesson(lessonId, topicId);
        }

        // api/lesson/{lessonId}/topic/{topicId}
        [HttpPost("{lessonId}/topic/{topicId}")]
        [Description("Add topic to lesson")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void AddTopicToLesson(int lessonId, int topicId)
        {
            _lessonService.AddTopicToLesson(lessonId, topicId);
        }

        // api/lesson/{lessonId}/user/{userId}
        [HttpPost("{lessonId}/user/{userId}")]
        [Description("Adds student to lesson")]
        [ProducesResponseType(typeof(StudentLessonOutputModel), StatusCodes.Status204NoContent)]
        public StudentLessonOutputModel AddStudentToLesson(int lessonId, int userId )
        {
           var output= _lessonService.AddStudentToLesson(lessonId, userId);
           return _mapper.Map<StudentLessonOutputModel>(output);
        }


        // api/lesson/{lessonId}/user/{userId}
        [HttpDelete("{lessonId}/user/{userId}")]
        [Description("Deletes student from lesson")]
        [ProducesResponseType( StatusCodes.Status204NoContent)]
        public void DeleteStudentFromLesson(int lessonId, int userId)
        {
           _lessonService.DeleteStudentFromLesson(lessonId, userId);
        }

        // api/lesson/{lessonId}/user/{userId}/feedback
        [AuthorizeRoles(Role.Student)]
        [HttpPut("{lessonId}/user/{userId}/feedback")]
        [Description("Update Feedback for lesson")]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(StudentLessonOutputModel), StatusCodes.Status200OK)]
        public StudentLessonOutputModel UpdateStudentFeedbackForLesson(int lessonId,int userId, [FromBody] FeedbackInputModel model)
        {
            var dto = _mapper.Map<StudentLessonDto>(model);
            var output =_lessonService.UpdateStudentFeedbackForLesson(lessonId, userId, dto);
            return _mapper.Map<StudentLessonOutputModel>(output);
        }

        // api/lesson/{lessonId}/user/{userId}/absenceReason
        [HttpPut("{lessonId}/user/{userId}/absenceReason")]
        [Description("Update AbsenceReason for lesson")]
        [ProducesResponseType(typeof(StudentLessonOutputModel), StatusCodes.Status200OK)]
        public StudentLessonOutputModel UpdateStudentAbsenceReasonOnLesson(int lessonId,int userId, [FromBody] AbsenceReasonInputModel model)
        {
            var dto = _mapper.Map<StudentLessonDto>(model);
            var output =_lessonService.UpdateStudentAbsenceReasonOnLesson(lessonId, userId, dto);
            return _mapper.Map<StudentLessonOutputModel>(output);
        }

        // api/lesson/{lessonId}/user/{userId}/attendance
        [HttpPut("{lessonId}/user/{userId}/attendance")]
        [Description("Update Attendance for lesson")]
        [ProducesResponseType(typeof(StudentLessonOutputModel),StatusCodes.Status200OK)]
        public StudentLessonOutputModel UpdateStudentAttendanceOnLesson(int lessonId, int userId, [FromBody] AttendanceInputModel model)
        {
            var dto = _mapper.Map<StudentLessonDto>(model);
            var output = _lessonService.UpdateStudentAttendanceOnLesson(lessonId, userId, dto);
            return _mapper.Map<StudentLessonOutputModel>(output);
        }

        // api/lesson/{lessonId}/feedback
        [HttpGet("{lessonId}/feedback")]
        [Description("Get all feedback by lesson")]
        [ProducesResponseType(typeof(List<FeedbackOutputModel>), StatusCodes.Status200OK)]
        public List<FeedbackOutputModel> GetAllFeedbackByLessonId(int lessonId)
        {
            var dto = _lessonService.SelectAllFeedbackByLessonId(lessonId);
            return _mapper.Map<List<FeedbackOutputModel>>(dto);
        }        
    }
}