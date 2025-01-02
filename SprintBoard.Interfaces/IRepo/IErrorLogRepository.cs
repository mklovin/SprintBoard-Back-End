using SprintBoard.Entities;
using System.Threading.Tasks;

namespace SprintBoard.Interfaces.IRepo
{
    public interface IErrorLogRepository
    {
        System.Threading.Tasks.Task LogErrorAsync(ErrorLog errorLog);
    }
}
