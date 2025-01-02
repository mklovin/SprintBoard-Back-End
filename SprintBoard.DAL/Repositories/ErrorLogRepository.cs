using SprintBoard.Entities;
using SprintBoard.Interfaces.IRepo;
using System.Threading.Tasks;

namespace SprintBoard.DAL.Repositories
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        private readonly AppDbContext _context;

        public ErrorLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task LogErrorAsync(ErrorLog errorLog)
        {
            await _context.ErrorLogs.AddAsync(errorLog);
            await _context.SaveChangesAsync();
        }
    }
}
