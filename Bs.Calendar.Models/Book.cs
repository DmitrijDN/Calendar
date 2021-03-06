﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bs.Calendar.Models
{
    public class Book : Bases.BaseEntity
    {
        [StringLength(LENGTH_NAME)]
        public string Code { get; set; }

        [StringLength(LENGTH_NAME)]
        public string Title { get; set; }

        [StringLength(LENGTH_NAME)]
        public string Author { get; set; }

        public string Description { get; set; }

        public ICollection<BookHistoryItem> BookHistories { get; set; }

        public string ReaderName { get; set; }

        public bool HasCover { get; set; }

        public ICollection<Tag> Tags { get; set; } 
    }
}
