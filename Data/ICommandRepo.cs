using SixMinAPI.Models;

namespace SixMinAPI.Data;

public interface ICommandRepo
{
    Task SaveChangesAsync();
    Task<Command?> GetCommandByIdAsync(int id);
    Task<IEnumerable<Command>> GetAllCommandsAsync();
    Task CreateCommandAsync(Command command);
    
    void DeleteCommand(Command command);
}