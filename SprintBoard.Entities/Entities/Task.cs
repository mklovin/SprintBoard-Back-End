using System;

namespace SprintBoard.Entities
{
    public class Task
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public Guid? AssigneeId { get; set; }
        public string Sprint { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public User Assignee { get; set; }
    }
}
