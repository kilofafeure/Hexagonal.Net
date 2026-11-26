using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Note
    {
        public int Id { get; private set; }
        public int ItemId { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public string? Color { get; private set; }

        public Note(int id, int itemId, string message, string? color = null)
        {
            ValidateMessage(message, color);



            Id = id;
            ItemId = itemId;
            Message = message;
            Color = color;
        }

        private static void ValidateMessage(string message, string? color = null)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("The message it's mandatory", nameof(message));

            if (message.Length > 100)
                throw new ArgumentException("The message must be lower than 100 characters", nameof(message));

            if (color?.Length > 6)
                throw new ArgumentException("Color must be 6 characters or fewer.", nameof(color));
        }
    }
}
