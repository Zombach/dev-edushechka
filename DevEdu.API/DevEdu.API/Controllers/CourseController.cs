﻿using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Configuration.ExceptionResponses;
using DevEdu.API.Extensions;
using DevEdu.API.Models;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DevEdu.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICourseService _courseService;

        public CourseController(
            IMapper mapper,
            ICourseService courseService)
        {
            _mapper = mapper;
            _courseService = courseService;
            _mapper = mapper;
        }

        [HttpGet("{id}/simple")]
        [Description("Get course by id with topics")]
        [ProducesResponseType(typeof(CourseInfoShortOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<CourseInfoShortOutputModel> GetCourseSimpleAsync(int id)
        {
            var course = await _courseService.GetCourseAsync(id);
            return _mapper.Map<CourseInfoShortOutputModel>(course);
        }

        [HttpGet("{id}/full")]
        [Description("Get course by id full")]
        [AuthorizeRoles(Role.Teacher, Role.Methodist, Role.Tutor)]
        [ProducesResponseType(typeof(CourseInfoFullOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<CourseInfoFullOutputModel> GetCourseFullAsync(int id)
        {
            var userToken = this.GetUserIdAndRoles();
            var course = await _courseService.GetFullCourseInfoAsync(id, userToken);
            return _mapper.Map<CourseInfoFullOutputModel>(course);
        }

        [HttpGet]
        [Description("Get all courses")]
        [ProducesResponseType(typeof(CourseInfoShortOutputModel), StatusCodes.Status200OK)]
        public async Task<List<CourseInfoShortOutputModel>> GetAllCoursesWithGroupsAsync()
        {
            var courses = await _courseService.GetCoursesAsync();
            return _mapper.Map<List<CourseInfoShortOutputModel>>(courses);
        }

        [HttpPost]
        [Description("Create new course")]
        [AuthorizeRoles(Role.Manager, Role.Teacher, Role.Methodist)]
        [ProducesResponseType(typeof(CourseInfoShortOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<CourseInfoShortOutputModel>> AddCourseAsync([FromBody] CourseInputModel model)
        {
            var dto = _mapper.Map<CourseDto>(model);
            var course = await _courseService.AddCourseAsync(dto);
            var output = _mapper.Map<CourseInfoShortOutputModel>(course);
            return Created(new Uri($"api/Course/{output.Id}/full", UriKind.Relative), output);
        }

        [HttpDelete("{id}")]
        [Description("Delete course by id")]
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCourseAsync(int id)
        {
            await _courseService.DeleteCourseAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Description("Update course by Id")]
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [ProducesResponseType(typeof(CourseInfoShortOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<CourseInfoShortOutputModel> UpdateCourseAsync(int id, [FromBody] CourseInputModel model)
        {
            var dto = _mapper.Map<CourseDto>(model);
            var updDto = await _courseService.UpdateCourseAsync(id, dto);
            return _mapper.Map<CourseInfoShortOutputModel>(updDto);
        }

        //  api/course/{courseId}/material/{materialId}
        [AuthorizeRoles(Role.Methodist)]
        [HttpPost("{courseId}/material/{materialId}")]
        [Description("Add material to course")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddCourseMaterialReferenceAsync(int courseId, int materialId)
        {
            await _courseService.AddCourseMaterialReferenceAsync(courseId, materialId);
            return NoContent();
        }

        //  api/course/{courseId}/material/{materialId}
        [AuthorizeRoles(Role.Methodist)]
        [HttpDelete("{courseId}/material/{materialId}")]
        [Description("Remove material from course")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoveCourseMaterialReferenceAsync(int courseId, int materialId)
        {
            await _courseService.RemoveCourseMaterialReferenceAsync(courseId, materialId);
            return NoContent();
        }

        [AuthorizeRoles(Role.Methodist)]
        [HttpPost("{courseId}/task/{taskId}")]
        [Description("Add task to course")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddTaskToCourseAsync(int courseId, int taskId)
        {
            await _courseService.AddTaskToCourseAsync(courseId, taskId);
            return NoContent();
        }

        [AuthorizeRoles(Role.Methodist)]
        [HttpDelete("{courseId}/task/{taskId}")]
        [Description("Delete task from course")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoveTaskFromCourseAsync(int courseId, int taskId)
        {
            await _courseService.DeleteTaskFromCourseAsync(courseId, taskId);
            return NoContent();
        }

        // api/course/{courseId}/topic/{topicId}
        [HttpPost("{courseId}/topic/{topicId}")]
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [ProducesResponseType(typeof(CourseTopicOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [Description("Add topic to course")]
        public async Task<CourseTopicOutputModel> AddTopicToCourseAsync(int courseId, int topicId, [FromBody] CourseTopicInputModel inputModel)
        {
            var dto = _mapper.Map<CourseTopicDto>(inputModel);
            var id = await _courseService.AddTopicToCourseAsync(courseId, topicId, dto);
            dto = await _courseService.GetCourseTopicByIdAsync(id);
            return _mapper.Map<CourseTopicOutputModel>(dto);
        }

        [HttpPost("{courseId}/add-topics")]
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [ProducesResponseType(typeof(List<CourseTopicOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [Description("Add topics to course")]
        public async Task<List<CourseTopicOutputModel>> AddTopicsToCourseAsync(int courseId, [FromBody] List<CourseTopicUpdateInputModel> inputModel)
        {
            var dto = _mapper.Map<List<CourseTopicDto>>(inputModel);
            var id = await _courseService.AddTopicsToCourseAsync(courseId, dto);
            dto = await _courseService.GetCourseTopicBySeveralIdAsync(id);
            return _mapper.Map<List<CourseTopicOutputModel>>(dto);
        }

        // api/course/{courseId}/topic/{topicId}
        [HttpDelete("{courseId}/topic/{topicId}")]
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [Description("Delete topic from course")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTopicFromCourseAsync(int courseId, int topicId)
        {
            await _courseService.DeleteTopicFromCourseAsync(courseId, topicId);
            return NoContent();
        }

        [HttpGet("{courseId}/topics")]
        [Description("Get all topics by course id")]
        [ProducesResponseType(typeof(List<CourseTopicOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<List<CourseTopicOutputModel>> SelectAllTopicsByCourseIdAsync(int courseId)
        {
            var list = await _courseService.SelectAllTopicsByCourseIdAsync(courseId);
            return _mapper.Map<List<CourseTopicOutputModel>>(list);
        }

        // api/course/{courseId}/program
        [HttpPut("{courseId}/program")]
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [Description("updates topics in the course")]
        [ProducesResponseType(typeof(List<CourseTopicOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<List<CourseTopicOutputModel>> UpdateCourseTopicsByCourseIdAsync(int courseId, [FromBody] List<CourseTopicUpdateInputModel> topics)
        {
            var listDto = _mapper.Map<List<CourseTopicDto>>(topics);
            var listId = await _courseService.UpdateCourseTopicsByCourseIdAsync(courseId, listDto);
            listDto = await _courseService.GetCourseTopicBySeveralIdAsync(listId);
            return _mapper.Map<List<CourseTopicOutputModel>>(listDto);
        }
    }
}