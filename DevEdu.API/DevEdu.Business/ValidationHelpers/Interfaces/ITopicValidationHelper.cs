﻿using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ITopicValidationHelper
    {
        TopicDto CheckTopicExistence(int topicId);
        void CheckTopicsExistence(List<CourseTopicDto> topics);
        CourseTopicDto GetCourseTopicByIdAndThrowIfNotFound(int id);
        List<CourseTopicDto> GetCourseTopicBySeveralIdAndThrowIfNotFound(List<int> ids);
    }
}