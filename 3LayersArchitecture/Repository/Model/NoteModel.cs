using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Model
{
    public class NoteModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string? Color { get; set; }
        public ItemModel Item { get; set; } = null!;

    }
}
