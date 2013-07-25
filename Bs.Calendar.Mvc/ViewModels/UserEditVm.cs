﻿using System.ComponentModel.DataAnnotations;
using Bs.Calendar.Models;

namespace Bs.Calendar.Mvc.ViewModels
{
    public class UserEditVm
    {
        public UserEditVm(int userId,string firstName,string lastName, string email, Roles role, State state)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Role = role;
            State = state;
        }

        public UserEditVm(User user)
        {
            UserId = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Role = user.Role;
            State = user.State;
        }
        
        public UserEditVm()
        {
        }

        public int UserId { get; set; }

        [StringLength(50),
        Required(ErrorMessage = "First name is required!"),
        Display(Name = "First name")]
        public string FirstName { get; set; }

        [StringLength(50),
        Required(ErrorMessage = "Last name is required!"),
        Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "An email is required!"),
        StringLength(200)]
        public string Email { get; set; }

        public string UserLogin
        {
            get { return Email; }
        }

        public Roles Role { get; set; }

        public State State { get; set; }
    }
}