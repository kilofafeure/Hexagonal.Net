using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Item(int id, string title, bool isCompleted)
    {
        public int Id { get; set; } = id;
        public string Title { get; set; } = title;
        public bool IsCompleted { get; set; } = isCompleted;
    }
}
