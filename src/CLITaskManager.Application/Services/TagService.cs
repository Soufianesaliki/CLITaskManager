using CLITaskManager.Application.Exceptions;
using CLITaskManager.Domain.Entities;
using CLITaskManager.Domain.Interfaces;

namespace CLITaskManager.Application.Services;

public class TagService
{
    private readonly ITagRepository _tagRepository;

    public TagService(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async System.Threading.Tasks.Task<IEnumerable<Tag>> GetAllTagsAsync()
    {
        return await _tagRepository.GetAllAsync();
    }

    public async System.Threading.Tasks.Task<Tag> GetTagByIdAsync(int id)
    {
        var tag = await _tagRepository.GetByIdAsync(id);
        
        if (tag == null)
        {
            throw new NotFoundException("Tag", id);
        }
        
        return tag;
    }

    public async System.Threading.Tasks.Task<Tag> GetTagByNameAsync(string name)
    {
        var tag = await _tagRepository.GetByNameAsync(name);
        
        if (tag == null)
        {
            throw new NotFoundException("Tag", name);
        }
        
        return tag;
    }

    public async System.Threading.Tasks.Task<Tag> CreateTagAsync(string name)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Tag name cannot be empty.");
        }

        if (name.Length > 50)
        {
            throw new ValidationException("Tag name cannot exceed 50 characters.");
        }

        // Business rule: Tag names must be unique
        if (await _tagRepository.ExistsByNameAsync(name))
        {
            throw new BusinessRuleException($"Tag '{name}' already exists.");
        }

        var tag = new Tag
        {
            Name = name.Trim().ToLower()  // Store tags in lowercase for consistency
        };

        return await _tagRepository.AddAsync(tag);
    }

    public async System.Threading.Tasks.Task<Tag> UpdateTagAsync(int id, string newName)
    {
        // Get existing tag
        var tag = await _tagRepository.GetByIdAsync(id);
        if (tag == null)
        {
            throw new NotFoundException("Tag", id);
        }

        // Validation
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ValidationException("Tag name cannot be empty.");
        }

        if (newName.Length > 50)
        {
            throw new ValidationException("Tag name cannot exceed 50 characters.");
        }

        var normalizedName = newName.Trim().ToLower();

        // Business rule: Tag names must be unique
        if (normalizedName != tag.Name && await _tagRepository.ExistsByNameAsync(normalizedName))
        {
            throw new BusinessRuleException($"Tag '{newName}' already exists.");
        }

        tag.Name = normalizedName;
        await _tagRepository.UpdateAsync(tag);
        
        return tag;
    }

    public async System.Threading.Tasks.Task DeleteTagAsync(int id)
    {
        var tag = await _tagRepository.GetByIdAsync(id);
        
        if (tag == null)
        {
            throw new NotFoundException("Tag", id);
        }

        await _tagRepository.DeleteAsync(tag);
    }

    public async System.Threading.Tasks.Task<Tag> GetOrCreateTagAsync(string name)
    {
        var normalizedName = name.Trim().ToLower();
        
        var existingTag = await _tagRepository.GetByNameAsync(normalizedName);
        if (existingTag != null)
        {
            return existingTag;
        }

        return await CreateTagAsync(normalizedName);
    }
}