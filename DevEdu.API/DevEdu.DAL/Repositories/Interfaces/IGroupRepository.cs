﻿using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface IGroupRepository
    {
        int AddGroup(GroupDto groupDto);
        void DeleteGroup(int id);
        GroupDto GetGroup(int id);
        List<GroupDto> GetGroups();
        GroupDto UpdateGroup(GroupDto groupDto);
        int AddUserToGroup(int groupId, int userId, int roleId);
        int DeleteUserFromGroup(int userId, int groupId);
        int AddGroupToLesson(int groupId, int lessonId);
        void RemoveGroupFromLesson(int groupId, int lessonId);
        GroupDto ChangeGroupStatus(int groupId, int statusId);
        int AddGroupMaterialReference(int groupId, int materialId);
        int RemoveGroupMaterialReference(int groupId, int materialId);
        int AddTaskToGroup(GroupTaskDto groupTaskDto);
        void DeleteTaskFromGroup(int groupId, int taskId);
        List<GroupTaskDto> GetTaskGroupByGroupId(int groupId);
        GroupTaskDto GetGroupTask(int groupId, int taskId);
        void UpdateGroupTask(GroupTaskDto groupTaskDto);
        public List<GroupDto> GetGroupsByMaterialId(int id);
        int GetPresentGroupForStudentByUserId(int userId);
    }
}