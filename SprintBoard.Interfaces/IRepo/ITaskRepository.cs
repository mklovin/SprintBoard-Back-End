using Entities = SprintBoard.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SprintBoard.Interfaces.IRepo
{
    public interface ITaskRepository : IBaseRepository<Entities.Task>
    {
        Task<IEnumerable<Entities.Task>> GetTasksByAssigneeAsync(Guid userId);
        Task<IEnumerable<Entities.Task>> GetTasksByStatusAsync(string status);
    }
}
