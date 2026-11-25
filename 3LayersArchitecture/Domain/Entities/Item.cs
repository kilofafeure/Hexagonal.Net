using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Item
    {
        public Item(int id, string title, bool isCompleted)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);

            Id = id;
            Title = title;
            IsCompleted = isCompleted;
        }

        public int Id { get; private set; }
        public string Title { get; private set; }
        public bool IsCompleted { get; private set; }
    }
}
