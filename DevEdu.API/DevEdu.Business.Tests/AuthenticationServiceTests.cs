﻿using DevEdu.Business.Configuration;
using DevEdu.Business.Services;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System;

namespace DevEdu.Business.Tests
{
    class AuthenticationServiceTests
    {
        private AuthenticationService _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new AuthenticationService(new Mock<IUserRepository>().Object,
                                             new Mock<IAuthOptions>().Object);
        }

        [Test]
        public void HashPassword_PasswordAndSalt_ReturnSalt()
        {
            //Given 
            string password = "password";
            byte[] salt = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            string expected = "AAECAwQFBgcICQoLDA0ODyT2cCjwnE2JIl0Ka2bvFeMtEwX+";
           
            //When
            var actual = _sut.HashPasswordAsync(password, salt);
           
            //Than
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void HashPassword_WrongSalt_ReturnError()
        {
            string password = "password";
            byte[] salt = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

            Assert.ThrowsAsync<ArgumentException>(() => _sut.HashPasswordAsync(password, salt));
        }

        [Test]
        public void Verify_CorrectPassword_GetTrue()
        {
            //Given 
            var expected = true;
            string hashedPassword = "AAECAwQFBgcICQoLDA0ODyT2cCjwnE2JIl0Ka2bvFeMtEwX+";
            string userPassword = "password";
            
            //When
            var actual = _sut.VerifyAsync(hashedPassword, userPassword);
          
            //Than
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Verify_IncorrectPassword_GetException()
        {
            //Given 
            string hashedPassword = "AAECAwQFBgcICQoLDA0ODyT2cCjwnE2JIl0Ka2bvFeMtEwX";
            string userPassword = "password";

            Assert.ThrowsAsync<FormatException>(() => _sut.VerifyAsync(hashedPassword, userPassword)); ;
        }
    }
}
