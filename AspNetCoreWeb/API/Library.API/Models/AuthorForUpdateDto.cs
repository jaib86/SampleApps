﻿using System;

namespace Library.API.Models
{
    public class AuthorForUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string Genre { get; set; }
    }
}