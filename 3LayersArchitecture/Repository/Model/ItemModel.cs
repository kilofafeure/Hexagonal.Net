using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Model
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public required ICollection<NoteModel> Notes { get; set; }
    }
}
