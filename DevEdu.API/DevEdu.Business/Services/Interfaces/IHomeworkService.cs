﻿using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface IHomeworkService
    {
        Task<HomeworkDto> GetHomeworkAsync(int homeworkId, UserIdentityInfo userInfo);
        Task<List<HomeworkDto>> GetHomeworkByGroupIdAsync(int groupId, UserIdentityInfo userInfo);
        Task<List<HomeworkDto>> GetHomeworkByTaskIdAsync(int taskId);
        Task<HomeworkDto> AddHomeworkAsync(int groupId, int taskId, HomeworkDto dto, UserIdentityInfo userInfo);
        Task DeleteHomeworkAsync(int homeworkId, UserIdentityInfo userInfo);
        Task<HomeworkDto> UpdateHomeworkAsync(int homeworkId, HomeworkDto dto, UserIdentityInfo userInfo);
    }
}