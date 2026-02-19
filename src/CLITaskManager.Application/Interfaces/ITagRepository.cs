using CLITaskManager.Domain.Entities;

namespace CLITaskManager.Application.Interfaces;

public interface ITagRepository
{
    Task<IEnumerable<Tag>> GetAllAsync();
    Task<Tag?> GetByIdAsync(int id);
    Task<Tag?> GetByNameAsync(string name);
    Task<Tag> AddAsync(Tag tag);
    System.Threading.Tasks.Task UpdateAsync(Tag tag);
    System.Threading.Tasks.Task DeleteAsync(Tag tag);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByNameAsync(string name);
}