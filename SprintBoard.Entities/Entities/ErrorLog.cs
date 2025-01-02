using System;

namespace SprintBoard.Entities
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
