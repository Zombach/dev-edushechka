﻿using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IUserValidationHelper
    {
        UserDto GetUserByIdAndThrowIfNotFound(int userId);
        void CheckUserIdAndRoleIdDoesNotLessThanMinimum(int userId, int roleId);
        void ChekRoleExistence(int roleId);
        void ChekIdDoesNotLessThenMinimum(int id);
    }
}