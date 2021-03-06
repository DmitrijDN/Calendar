﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bs.Calendar.Models.Bases;

namespace Bs.Calendar.Models
{
    public class User : BaseEntity 
    {
        [StringLength(LENGTH_NAME)]
        public string Email { get; set; }

        [StringLength(LENGTH_NAME)]
        public string FullName { get; set; }

        [StringLength(LENGTH_NAME)]
        public string FirstName { get; set; }

        [StringLength(LENGTH_NAME)]
        public string LastName { get; set; }

        public Roles Role { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public DateTime? BirthDate { get; set; }

        public virtual PasswordRecovery PasswordRecovery { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }

        public LiveStatuses Live { get; set; }
        public ApproveStates ApproveState { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}
