﻿using System.ComponentModel.DataAnnotations;

namespace Bs.Calendar.Mvc.ViewModels
{
    public class AccountVm
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}