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

        public Note(int id, int itemId, string message)
        {
            ValidateMessage(message);

            Id = id;
            ItemId = itemId;
            Message = message;           
        }

        public void UpdateMessage(string newMessage)
        {
            ValidateMessage(newMessage);

            Message = newMessage;
        }

        private void ValidateMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("The message it's mandatory", nameof(message));

            if (message.Length > 100)
                throw new ArgumentException("The message must be lower than 100 characters", nameof(message));
        }
    }
}
