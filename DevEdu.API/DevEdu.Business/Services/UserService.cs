﻿using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Servicies
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int AddUser(UserDto dto)
        {
           var addedUserId = _userRepository.AddUser(dto);
            
            if(dto.Roles == null || dto.Roles.Count == 0)
            {
                return addedUserId;
            }

            foreach(var role in dto.Roles)
            {
                AddUserRole(addedUserId, (int)role);
            }
            return addedUserId;
        }

        public UserDto SelectUserById(int id) => _userRepository.SelectUserById(id);

        public List<UserDto> SelectUsers() => _userRepository.SelectUsers();

        public UserDto UpdateUser(UserDto dto)
        {
            _userRepository.UpdateUser(dto);
            return _userRepository.SelectUserById(dto.Id);
        }

        public void DeleteUser(int id) => _userRepository.DeleteUser(id);

        public int AddUserRole(int userId, int roleId) => _userRepository.AddUserRole(userId, roleId);

        public void DeleteUserRole(int userId, int roleId) => _userRepository.DeleteUserRole(userId, roleId);
    }
}