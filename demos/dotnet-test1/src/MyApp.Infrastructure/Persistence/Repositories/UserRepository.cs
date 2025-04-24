namespace MyApp.Infrastructure.Persistence.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> IsUsernameUniqueAsync(string username)
    {
        return !await AnyAsync(u => u.Username == username);
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        return !await AnyAsync(u => u.Email == email);
    }

    public override async Task<User> AddAsync(User user)
    {
        // Additional validation before adding
        if (!await IsUsernameUniqueAsync(user.Username))
            throw new Application.Exceptions.DuplicateEntityException(nameof(User), user.Username);
        
        if (!await IsEmailUniqueAsync(user.Email))
            throw new Application.Exceptions.DuplicateEntityException(nameof(User), user.Email);

        return await base.AddAsync(user);
    }

    public override async Task UpdateAsync(User user)
    {
        // Check if username/email changes conflict with existing users
        var existing = await GetByIdAsync(user.Id);
        if (existing == null)
            throw new Application.Exceptions.NotFoundException(nameof(User), user.Id);

        if (existing.Username != user.Username && !await IsUsernameUniqueAsync(user.Username))
            throw new Application.Exceptions.DuplicateEntityException(nameof(User), user.Username);

        if (existing.Email != user.Email && !await IsEmailUniqueAsync(user.Email))
            throw new Application.Exceptions.DuplicateEntityException(nameof(User), user.Email);

        await base.UpdateAsync(user);
    }
}