﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DevEdu.API.Models.InputModels;
using AutoMapper;
using DevEdu.DAL.Models;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using DevEdu.Business.Services;
using DevEdu.API.Models.OutputModels;
using DevEdu.API.Common;
using DevEdu.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using DevEdu.API.Extensions;

namespace DevEdu.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TopicController : Controller
    {
        private readonly ITopicService _topicService;
        private readonly IMapper _mapper;

        public TopicController(IMapper mapper, ITopicService topicService)
        {
            _topicService = topicService;
            _mapper = mapper;
        }

        //  api/topic/{id}
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [HttpGet("{id}")]
        [Description("Get topic by id")]
        [ProducesResponseType(typeof(TopicOutputModel), StatusCodes.Status200OK)]
        public TopicOutputModel GetTopicById(int id)
        {
            var output = _topicService.GetTopic(id);
            return _mapper.Map<TopicOutputModel>(output);
        }

        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [HttpGet]
        [Description("Get all topics")]
        [ProducesResponseType(typeof(List<TopicOutputModel>), StatusCodes.Status200OK)]
        public List<TopicOutputModel> GetAllTopics()
        {
            var output = _topicService.GetAllTopics();
            return _mapper.Map<List<TopicOutputModel>>(output);
        }

        //  api/topic
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [HttpPost]
        [Description("Add topic")]
        [ProducesResponseType(typeof(TopicOutputModel), (StatusCodes.Status201Created))]
        public TopicOutputModel AddTopic([FromBody] TopicInputModel model)
        {
            var dto = _mapper.Map<TopicDto>(model);
            var output = _topicService.AddTopic(dto);
            return GetTopicById(output);
        }

        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        //  api/topic/{id}
        [HttpDelete("{id}")]
        [Description("Delete topic")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteTopic(int id)
        {
            _topicService.DeleteTopic(id);
        }

        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        //  api/topic/{id}
        [HttpPut("{id}")]
        [Description("Update topic")]
        [ProducesResponseType(typeof(TopicOutputModel), StatusCodes.Status200OK)]
        public TopicOutputModel UpdateTopic(int id, [FromBody] TopicInputModel model)
        {
            var dto = _mapper.Map<TopicDto>(model);
            var output= _topicService.UpdateTopic(id, dto);
            return _mapper.Map<TopicOutputModel>(output);
        }      
    }
}
