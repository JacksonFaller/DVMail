using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mail.Model;

namespace Mail.DataLayer.SQL.Tests
{
    [TestClass]
    public class UsersRepositoryTests
    {
        private readonly UsersRepository _usersRepository;
        private readonly List<Guid> _usersList = new List<Guid>(16);

        public UsersRepositoryTests()
        {
            _usersRepository = new UsersRepository(MainTest.ConnectionString);
        }

        [TestMethod]
        public void CreateGetUserTest()
        {
            User user = _usersRepository.CreateUser(MainTest.GetRandomUser());
            _usersList.Add(user.Id);

            var result = _usersRepository.GetUser(user.Name);
            if(result.Id != user.Id) Assert.Fail($"Expected id: {user.Id} but we got {result.Id}");

            Assert.AreEqual(user.Name, _usersRepository.GetUser(user.Name).Name);
        }

        [TestCleanup]
        public void CleanUp()
        {
            if (_usersList.Count == 0) return;

            foreach (var userId in _usersList)
            {
                _usersRepository.DeleteUser(userId);
            }
        }
    }
}
