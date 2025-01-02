using Microsoft.EntityFrameworkCore;
using Entities = SprintBoard.Entities;
using SprintBoard.Interfaces.IRepo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SprintBoard.DAL.Repositories
{
    public class TaskRepository : BaseRepository<Entities.Task>, ITaskRepository
    {
        public TaskRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Entities.Task>> GetTasksByAssigneeAsync(Guid userId)
        {
            return await _context.Tasks.Where(t => t.AssigneeId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Entities.Task>> GetTasksByStatusAsync(string status)
        {
            return await _context.Tasks.Where(t => t.Status == status).ToListAsync();
        }
    }
}
