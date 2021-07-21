﻿using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IStudentAnswerOnTaskService
    {
        void AddStudentAnswerOnTask(StudentAnswerOnTaskDto studentResponse);
        void DeleteStudentAnswerOnTask(StudentAnswerOnTaskDto dto);
        List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTasks();
        StudentAnswerOnTaskDto GetStudentAnswerOnTaskByTaskIdAndStudentId(StudentAnswerOnTaskDto dto);
        void ChangeStatusOfStudentAnswerOnTask(int taskId, int studentId, int statusId);
        void UpdateStudentAnswerOnTask(StudentAnswerOnTaskDto dto);
        void AddCommentOnStudentAnswer(int taskstudentId, int commentId);
    }
}
