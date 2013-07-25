﻿using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using Bs.Calendar.Core;
using System.Web.Mvc;
using Bs.Calendar.DataAccess;
using Bs.Calendar.DataAccess.Bases;
using Bs.Calendar.Models;
using Bs.Calendar.Mvc.Controllers;
using Bs.Calendar.Mvc.Server;
using Bs.Calendar.Mvc.Services;
using Bs.Calendar.Mvc.ViewModels;
using Bs.Calendar.Tests.Int.TestHelpers;
using FluentAssertions;
using NUnit.Framework;

namespace Bs.Calendar.Tests.Int
{
    [TestFixture]
    class PagingAndSortingTests
    {
        private RepoUnit _unit;
        private UsersController _usersController;
        private int _pageSize;

        [TestFixtureSetUp]
        public void SetUp()
        {
            DiMvc.Register();
            Resolver.RegisterType<IUserRepository, UserRepository>();

            _unit = new RepoUnit();
            _unit.User.Save(new User { Email = "aaa@bbb.com", FirstName = "aaa", LastName = "ddd" });
            _unit.User.Save(new User { Email = "ccc@ddd.com", FirstName = "aaa", LastName = "bbb" });

            var userService = new UserService(_unit);
            userService.PageSize = _pageSize = 1;

            _usersController = new UsersController(userService);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            var user1 = _unit.User.Get(user => user.Email.Equals(
                "aaa@bbb.com", StringComparison.InvariantCulture));
            var user2 = _unit.User.Get(user => user.Email.Equals(
                "ccc@ddd.com", StringComparison.InvariantCulture));
            _unit.User.Delete(user1);
            _unit.User.Delete(user2);
        }

        [Test]
        public void Can_Paginate_Users()
        {
            //act
            var usersView = _usersController.List(null, null, 2) as PartialViewResult;
            var users = usersView.Model as UsersVm;

            //assert
            users.Users.Count().ShouldBeEquivalentTo(_pageSize);
        }

        [Test]
        public void Can_Sort_Users() {
            //arrange
            var user = _unit.User.Load().OrderBy(n => n.FirstName).ThenBy(n => n.LastName).First();

            //act
            var usersView = _usersController.List(null, "Name", 1) as PartialViewResult;
            var users = usersView.Model as UsersVm;

            //assert
            users.Users.Count().ShouldBeEquivalentTo(_pageSize);
            users.Users.First().Email.ShouldBeEquivalentTo(user.Email);
        }
    }
}
