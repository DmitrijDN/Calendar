﻿using System.ComponentModel.DataAnnotations;
using Bs.Calendar.Models.Bases;

namespace Bs.Calendar.Models
{
    public class User : BaseEntity 
    {
        [StringLength(LENGTH_NAME)]
        public string Email { get; set; }

        [StringLength(LENGTH_NAME)]
        public string FirstName { get; set; }

        [StringLength(LENGTH_NAME)]
        public string LastName { get; set; }

        public Roles Role { get; set; }

        public string PasswordKeccakHash { get; set; }

        public string PasswordMd5Hash { get; set; }

        public State State { get; set; }
    }
}
