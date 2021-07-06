﻿using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using DevEdu.Business.Servicies;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;
        
        public TaskController(IMapper mapper, ITaskService taskService)
        {
            _taskService = taskService;
            _mapper = mapper;
        }

        //  api/Task/1
        [HttpGet("{taskId}")]
        public TaskDto GetTask(int taskId)
        {
            var taskDto = _taskService.GetTaskById(taskId);
            return taskDto;
        }

        //  api/Task
        [HttpGet]
        public List<TaskDto> GetAllTasks()
        {
            var taskDtos = _taskService.GetTasks();
            return taskDtos;
        }

        // api/task
        [HttpPost]
        public int AddTask([FromBody] TaskInputModel model)
        {
            var taskDto = _mapper.Map<TaskDto>(model);
            return _taskService.AddTask(taskDto);
        }


        // api/task/{taskId}
        [HttpPut("{taskId}")]
        public void UpdateTask(int taskId, [FromBody] TaskInputModel model)
        {
            TaskDto taskDto = _mapper.Map<TaskDto>(model);
            _taskService.UpdateTask(taskId, taskDto);
        }

        // api/task/{taskId}
        [HttpDelete("{taskId}")]
        public void DeleteTask(int taskId)
        {
            _taskService.DeleteTask(taskId);
        }

        // api/task/{taskId}/tag/{tagId}
        [HttpPost("{taskId}/tag/{tagId}")]
        public int AddTagToTask(int taskId, int tagId)
        {
            return 1;
        }

        // api/task/{taskId}/tag/{tagId}
        [HttpDelete("{taskId}/tag/{tagId}")]
        public string DeleteTagFromTask(int taskId, int tagId)
        {
            return $"deleted tag task with {taskId} taskId";
        }

        // api/task/{taskId}/student/{studentId}
        [HttpPost("{taskId}/student/{studentId}")]
        public string AddStudentAnswerOnTask(int taskId, int studentId, string taskAnswer)  // to inputModel
        {
            return $"add answer for task {taskId} id";
        }

        // api/task/{taskId}/student/{studentId}
        [HttpPut("{taskId}/student/{studentId}")]  // to inputModel
        public string UpdateStudentAnswerOnTask(int studentId, int taskId, string taskAnswer)
        {
            return $"update task with {taskId} id by {taskAnswer}";
        }

        // api/task/{taskId}/student/{studentId}
        [HttpDelete("{taskId}/student/{studentId}")]
        public string DeleteStudentAnswerOnTask(int taskId, int studentId)
        {
            return $"deleted answer for task {taskId} id";
        }

        // api/task/{taskId}/student/{studentId}/change-status/{statusId}
        [HttpPut("{taskId}/student/{studentId}/change-status/{statusId}")]
        public int UpdateStatusOfStudentAnswer(int taskId, int studentId, int statusId)
        {
            return statusId;
        }

        // api/task/{taskId}/student/{studentId}/comment}
        [HttpPost("{taskId}/student/{studentId}/comment")]
        public int AddCommentOnStudentAnswer(int taskId, int studentId, [FromBody] CommentAddInputModel inputModel)
        {
            return taskId;
        }
    }
}