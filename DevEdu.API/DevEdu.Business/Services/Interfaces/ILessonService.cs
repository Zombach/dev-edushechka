﻿using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface ILessonService
    {
        void AddCommentToLesson(int lessonId, int commentId);
        int AddLesson(LessonDto lessonDto);
        void DeleteCommentFromLesson(int lessonId, int commentId);
        void DeleteLesson(int id);
        List<LessonDto> SelectAllLessonsByGroupId(int id);
        List<LessonDto> SelectAllLessonsByTeacherId(int id);
        LessonDto SelectLessonById(int id);
        LessonDto SelectLessonWithCommentsById(int id);
        LessonDto SelectLessonWithCommentsAndStudentsById(int id);
        void UpdateLesson(int id, LessonDto lessonDto);
        void DeleteTopicFromLesson(int lessonId, int topicId);
        void AddTopicToLesson(int lessonId, int topicId);
    }
}