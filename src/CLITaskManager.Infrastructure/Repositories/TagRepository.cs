using Microsoft.EntityFrameworkCore;
using CLITaskManager.Domain.Entities;
using CLITaskManager.Domain.Interfaces;
using CLITaskManager.Infrastructure.Data;

namespace CLITaskManager.Infrastructure.Repositories;

public class TagRepository : ITagRepository
{
    public readonly AppDbContext _context;

    public TagRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await _context.Tags
            .OrderBy(t => t.Name)
            .ToListAsync();
    }
    public async Task<Tag?> GetByIdAsync(int id)
    {
        return await _context.Tags.FindAsync(id);
    }
    public async Task<Tag?> GetByNameAsync(string name)
    {
        return await _context.Tags
            .FirstOrDefaultAsync(t => t.Name == name);
    }
    public async Task<Tag> AddAsync(Tag tag)
    {
        tag.CreatedAt = DateTime.UtcNow;
        tag.UpdatedAt = DateTime.UtcNow;

        await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();

        return tag;
    }
    public async System.Threading.Tasks.Task UpdateAsync(Tag tag)
    {
        tag.UpdatedAt = DateTime.UtcNow;

        _context.Tags.Update(tag);
        await _context.SaveChangesAsync();
    }
    public async System.Threading.Tasks.Task DeleteAsync(Tag tag)
    {
        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
    }
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Tags.AnyAsync(t => t.Id == id);
    }
    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _context.Tags.AnyAsync(t => t.Name == name);
    }
}