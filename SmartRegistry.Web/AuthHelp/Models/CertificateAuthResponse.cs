﻿using System;
using SmartRegistry.Web.AuthHelp.Enums;

namespace SmartRegistry.Web.AuthHelp.Models
{
    public class CertificateAuthResponse
    {
        public eCertResponseStatus ResponseStatus { get; set; }

        public string ResponseStatusMessage { get; set; }

        public string EGN { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string LatinNames { get; set; }
    }
}
