﻿using System;
using System.Configuration;
using Bs.Calendar.Rules;

namespace Bs.Calendar.Mvc.Services
{
    public class MvcConfig : IConfig
    {
        private bool? _sendEmail;
        private string _teamHeaderPattern;

        public bool SendEmail
        {
            get
            {
                if (_sendEmail == null)
                {
                    var parameter = ConfigurationManager.AppSettings["SendEmail"];
                    _sendEmail = parameter != null && parameter.Equals("true", StringComparison.OrdinalIgnoreCase);
                }

                return _sendEmail.Value;
            }
            set { _sendEmail = value; }
        }

        public string TeamHeaderPattern
        {
            get
            {
                if (_teamHeaderPattern == null)
                {
                    _teamHeaderPattern = ConfigurationManager.AppSettings["TeamHeaderPattern"];
                }
                return _teamHeaderPattern;
            }
        }
    }
}