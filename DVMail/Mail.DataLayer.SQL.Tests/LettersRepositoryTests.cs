using System;
using System.Collections.Generic;
using System.Linq;
using Mail.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Mail.DataLayer.SQL.Tests.MainTest;

namespace Mail.DataLayer.SQL.Tests
{
    [TestClass]
    public class LettersRepositoryTests
    {
        private readonly LettersRepository _lettersRepository;
        private readonly UsersRepository _usersRepository;

        private readonly List<Guid> _usersList = new List<Guid>(16);
        private readonly List<Guid> _lettersList = new List<Guid>(16);

        public LettersRepositoryTests()
        {
            _lettersRepository = new LettersRepository(ConnectionString);
            _usersRepository = new UsersRepository(ConnectionString);
        }
        [TestMethod]
        public void CreateGetLetterText()
        {
            User addressee =
                _usersRepository.CreateUser(GetRandomUser());
            _usersList.Add(addressee.Id);

            User addresser =
                _usersRepository.CreateUser(GetRandomUser());
            _usersList.Add(addresser.Id);

            var result = _lettersRepository.CreateLetter(GetRandomLetter(addresser.Id, addressee.Id));
            _lettersList.Add(result.Id);
            Assert.AreEqual(_lettersRepository.GetLetter(result.Id).Subject, result.Subject);
        }

        [TestMethod]
        public void SetIsReadLetterTest()
        {
            User addressee =
                _usersRepository.CreateUser(GetRandomUser());
            _usersList.Add(addressee.Id);

            User addresser =
                _usersRepository.CreateUser(GetRandomUser());
            _usersList.Add(addresser.Id);

            var result = _lettersRepository.CreateLetter(GetRandomLetter(addresser.Id, addressee.Id));
            _lettersList.Add(result.Id);

            _lettersRepository.SetIsRead(result.Id, addressee.Id, true);
            Assert.IsTrue(_lettersRepository.GetLetter(result.Id).IsRead);
        }

        [TestMethod]
        public void GetUsersLettersTest()
        {
            User addressee = _usersRepository.CreateUser(GetRandomUser());
            _usersList.Add(addressee.Id);

            User addresser = _usersRepository.CreateUser(GetRandomUser());
            _usersList.Add(addresser.Id);

            for (int i = 0; i < 3; i++)
            {
                var letter1 = _lettersRepository.CreateLetter(GetRandomLetter(addresser.Id, addressee.Id));
                _lettersList.Add(letter1.Id);
            }

            List<Letter> result = _lettersRepository.GetUsersInbox(addressee.Id).ToList();
            if(result.Count != _lettersList.Count)
                Assert.Fail($"Expected number of letters: {_lettersList.Count}, but we got {result.Count}");

            foreach (var usersLetter in result)
            {
                if(!_lettersList.Contains(usersLetter.Id)) Assert.Fail($"Unexpected letter with id {usersLetter.Id}");
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            foreach (var letterId in _lettersList)
            {
                _lettersRepository.DeleteLetter(letterId);
            }

            foreach (var userId in _usersList)
            {
                _usersRepository.DeleteUser(userId);
            }
        }
    }
}
