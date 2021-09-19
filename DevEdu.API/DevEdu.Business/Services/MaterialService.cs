﻿using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupValidationHelper _groupValidationHelper;
        private readonly ITagValidationHelper _tagValidationHelper;
        private readonly ICourseValidationHelper _courseValidationHelper;
        private readonly IMaterialValidationHelper _materilaValidationHelper;
        private readonly IUserValidationHelper _userValidationHelper;

        public MaterialService(
            IMaterialRepository materialRepository,
            ICourseRepository courseRepository,
            IGroupRepository groupRepository,
            IGroupValidationHelper groupValidationHelper,
            ITagValidationHelper tagValidationHelper,
            ICourseValidationHelper courseValidationHelper,
            IMaterialValidationHelper materilaValidationHelper,
            IUserValidationHelper useraValidationHelper)
        {
            _materialRepository = materialRepository;
            _courseRepository = courseRepository;
            _groupRepository = groupRepository;
            _groupValidationHelper = groupValidationHelper;
            _tagValidationHelper = tagValidationHelper;
            _courseValidationHelper = courseValidationHelper;
            _materilaValidationHelper = materilaValidationHelper;
            _userValidationHelper = useraValidationHelper;
        }

        public List<MaterialDto> GetAllMaterials(UserIdentityInfo user)
        {
            var allMaterials = _materialRepository.GetAllMaterialsAsync();
            if (!(user.IsAdmin() || user.IsMethodist()))
            {
                return _materilaValidationHelper.GetMaterialsAllowedToUser(allMaterials, user.UserId);
            }
            return allMaterials;
        }

        public async Task<MaterialDto> GetMaterialByIdWithCoursesAndGroupsAsync(int id)
        {
            var dto = _materilaValidationHelper.GetMaterialByIdAndThrowIfNotFound(id);
            dto.Courses = await _courseRepository.GetCoursesByMaterialIdAsync(id);
            dto.Groups = _groupRepository.GetGroupsByMaterialIdAsync(id);
            return dto;
        }

        public MaterialDto GetMaterialByIdWithTags(int id, UserIdentityInfo user)
        {
            var dto = _materilaValidationHelper.GetMaterialByIdAndThrowIfNotFound(id);
            if (!(user.IsAdmin() || user.IsMethodist()))
            {
                _materilaValidationHelper.CheckUserAccessToMaterialForGetById(user.UserId, dto);
            }
            return dto;
        }

        public int AddMaterialWithGroups(MaterialDto dto, List<int> tags, List<int> groups, UserIdentityInfo user)
        {
            _materilaValidationHelper.CheckPassedValuesAreUnique(groups, nameof(groups));
            groups.ForEach(group =>
            {
                var groupDto = Task.Run(() => _groupValidationHelper.CheckGroupExistenceAsync(group)).GetAwaiter().GetResult();
                if (user.IsAdmin())
                    return;
                var currentRole = user.IsTeacher() ? Role.Teacher : Role.Tutor;
                _userValidationHelper.CheckAuthorizationUserToGroup(group, user.UserId, currentRole);
            });
            var materialId = AddMaterial(dto, tags);
            groups.ForEach(group => _groupRepository.AddGroupMaterialReferenceAsync(group, materialId));
            return materialId;
        }

        public int AddMaterialWithCourses(MaterialDto dto, List<int> tags, List<int> courses)
        {
            _materilaValidationHelper.CheckPassedValuesAreUnique(courses, nameof(courses));
            courses.ForEach(course => _courseValidationHelper.GetCourseByIdAndThrowIfNotFoundAsync(course));

            var materialId = AddMaterial(dto, tags);
            courses.ForEach(course => _courseRepository.AddCourseMaterialReferenceAsync(course, materialId));
            return materialId;
        }

        public async Task<MaterialDto> UpdateMaterialAsync(int id, MaterialDto dto, UserIdentityInfo user)
        {
            var material = await GetMaterialByIdWithCoursesAndGroupsAsync(id);
            CheckAccessToMaterialByRole(material, user);

            dto.Id = id;
            _materialRepository.UpdateMaterialAsync(dto);
            return _materialRepository.GetMaterialByIdAsync(dto.Id);
        }

        public async Task DeleteMaterialAsync(int id, bool isDeleted, UserIdentityInfo user)
        {
            var material = await GetMaterialByIdWithCoursesAndGroupsAsync(id);
            CheckAccessToMaterialByRole(material, user);
            _materialRepository.DeleteMaterialAsync(id, isDeleted);
        }

        public void AddTagToMaterial(int materialId, int tagId)
        {
            CheckMaterialAndTagExistence(materialId, tagId);
            _materialRepository.AddTagToMaterialAsync(materialId, tagId);
        }
        public void DeleteTagFromMaterial(int materialId, int tagId)
        {
            CheckMaterialAndTagExistence(materialId, tagId);
            _materialRepository.DeleteTagFromMaterialAsync(materialId, tagId);
        }

        public List<MaterialDto> GetMaterialsByTagId(int tagId, UserIdentityInfo user)
        {
            _tagValidationHelper.GetTagByIdAndThrowIfNotFound(tagId);

            var allMaterialsByTag = _materialRepository.GetMaterialsByTagIdAsync(tagId);
            if (!(user.IsAdmin() || user.IsMethodist()))
            {
                return _materilaValidationHelper.GetMaterialsAllowedToUser(allMaterialsByTag, user.UserId);
            }
            return allMaterialsByTag;
        }

        private int AddMaterial(MaterialDto dto, List<int> tags)
        {
            if (tags == null || tags.Count == 0)
                return _materialRepository.AddMaterialAsync(dto);

            _materilaValidationHelper.CheckPassedValuesAreUnique(tags, nameof(tags));
            tags.ForEach(tag => _tagValidationHelper.GetTagByIdAndThrowIfNotFound(tag));

            var materialId = _materialRepository.AddMaterialAsync(dto);
            tags.ForEach(tag => _materialRepository.AddTagToMaterialAsync(materialId, tag));
            return materialId;
        }

        private void CheckAccessToMaterialByRole(MaterialDto material, UserIdentityInfo user)
        {
            if (!user.IsAdmin())
            {
                if (user.IsMethodist())
                {
                    _materilaValidationHelper.CheckMethodistAccessToMaterialForDeleteAndUpdate(user.UserId, material);
                }
                else
                {
                    _materilaValidationHelper.CheckTeacherAccessToMaterialForDeleteAndUpdate(user.UserId, material);
                }
            }
        }

        private void CheckMaterialAndTagExistence(int materialId, int tagId)
        {
            _materilaValidationHelper.GetMaterialByIdAndThrowIfNotFound(materialId);
            _tagValidationHelper.GetTagByIdAndThrowIfNotFound(tagId);
        }
    }
}