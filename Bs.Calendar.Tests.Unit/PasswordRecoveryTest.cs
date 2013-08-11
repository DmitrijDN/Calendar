﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using Bs.Calendar.Mvc.ViewModels;
using Bs.Calendar.Rules;
using Bs.Calendar.Rules.Emails;
using FluentAssertions;
using Moq;
using Bs.Calendar.Core;
using Bs.Calendar.DataAccess;
using Bs.Calendar.Models;
using Bs.Calendar.Mvc.Server;
using Bs.Calendar.Mvc.Services;
using Bs.Calendar.Tests.Unit.FakeObjects;
using NUnit.Framework;

namespace Bs.Calendar.Tests.Unit
{
    [TestFixture]
    public class PasswordRecoveryTest
    {
        private List<User> _users;
        private AccountService _accountService;
        private RepoUnit _repoUnit;

        [SetUp]
        public void SetUp()
        {
            _users = new List<User>()
            {
                new User {Id = 1, Email = "12345@gmail.com", FirstName = "Saveli", LastName = "Bondini", Role = Roles.Simple, Live = LiveStatuses.Active},
                new User {Id = 2, Email = "5678@gmail.com", FirstName = "Dima", LastName = "Rossi", Role = Roles.Simple, Live = LiveStatuses.Active},
                new User {Id = 3, Email = "9999@gmail.com", FirstName = "Dima", LastName = "Prohorov", Role = Roles.Simple, Live = LiveStatuses.Active}
            };

            DiMvc.Register();
            Ioc.RegisterType<IUserRepository, FakeUserRepository>();

            _repoUnit = new RepoUnit();
            _users.ForEach(u => _repoUnit.User.Save(u));

            Ioc.RegisterInstance<RepoUnit>(_repoUnit);

            _accountService = Ioc.Resolve<AccountService>();
        }

        [Test]
        [ExpectedException(typeof(WarningException))]
        public void Should_Throw_Exception_On_Simplexistent_Email()
        {
            //arrange
            var alienEmail = "alien@gmail.com";

            //act
            _accountService.PasswordRecovery(alienEmail, "");
        }

        [Test]
        public void Can_Find_Existent_Email()
        {
            //arrange
            var existentUser = _users[0];

            //act
            _accountService.PasswordRecovery(existentUser.Email, "localhost/");

            //assert
            existentUser.PasswordRecovery.Should().NotBeNull();
            existentUser.PasswordRecovery.PasswordHash.Should().NotBeNullOrEmpty();
            existentUser.PasswordRecovery.PasswordSalt.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void Can_Form_Recovery_Link_And_Send_Email()
        {
            //arrange

            var mailMessage = new MailMessage();

            var url = "localhost/";

            var moq = new Mock<IEmailSender>(Ioc.Resolve<IEmailProvider>());
            moq.Setup(e => e.Send("", "", It.IsAny<string>())).Callback<MailMessage>(m => mailMessage = m);
            Ioc.RegisterInstance<IEmailSender>(moq.Object);

            //act

            _accountService.PasswordRecovery(_users[0].Email, url);
            var expectedUrl = string.Format("{0}PasswordReset/{1}/{2}", url, _users[0].Id,_users[0].PasswordRecovery.PasswordHash);

            //assert
            
            moq.Verify(e => e.Send("","",It.IsAny<string>()), Times.Once());
            mailMessage.Body.Should().Contain(expectedUrl);
            mailMessage.To.Contains(new MailAddress(_users[0].Email)).Should().BeTrue();
        }

        [Test]
        [ExpectedException(typeof(WarningException))]
        public void Should_Throw_Exception_On_Invalid_Token()
        {
            //arrange
            var user = _users[0];
            user.PasswordRecovery = new PasswordRecovery {Date = DateTime.Now, PasswordHash = "valid"};

            //act
            _accountService.CheckToken(user.Id, "invalid");
        }

        [Test]
        [ExpectedException(typeof(WarningException))]
        public void Should_Throw_Exception_On_Invalid_Link_Date() {
            //arrange
            var user = _users[0];
            user.PasswordRecovery = new PasswordRecovery { Date = DateTime.Now - new TimeSpan(24, 0, 0), PasswordHash = "valid" };

            //act
            _accountService.CheckToken(user.Id, "valid");
        }

        [Test]
        public void Should_Return_Model_On_Valid_Token_And_Date()
        {
            //arrange
            var user = _users[0];
            user.PasswordRecovery = new PasswordRecovery { Date = DateTime.Now, PasswordHash = "valid" };

            //act
            var model = _accountService.CheckToken(user.Id, "valid");

            //assert
            model.Should().NotBeNull();
            model.Email.ShouldBeEquivalentTo(user.Email);
        }

        [Test]
        public void Should_Change_Password_And_Clear_Hash()
        {
            //arrange
            var user = _users[0];
            user.PasswordRecovery = new PasswordRecovery { Date = DateTime.Now, PasswordHash = "valid" };

            //act
            _accountService.ResetPassword(new AccountVm { Email = user.Email, Password = "1234567"});

            //assert
            user.PasswordRecovery.PasswordHash.Should().BeEmpty();
            user.PasswordRecovery.PasswordSalt.Should().BeEmpty();
            user.PasswordHash.ShouldBeEquivalentTo(Ioc.Resolve<ICryptoProvider>().GetHashWithSalt("1234567", "salt"));
        }
    }
}