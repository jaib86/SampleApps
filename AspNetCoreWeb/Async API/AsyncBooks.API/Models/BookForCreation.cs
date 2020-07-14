﻿using System;

namespace AsyncBooks.API.Models
{
    public class BookForCreation
    {
        public Guid AuthorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}