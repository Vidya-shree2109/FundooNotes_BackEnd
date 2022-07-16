using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Model
{
    public class NoteModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Reminder { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        public bool IsArchived { get; set; }
        public bool IsPinned { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }
    }
}
