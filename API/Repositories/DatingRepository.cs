using System;
using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class DatingRepository : IDatingRepository, IDisposable
{
    private readonly AppDbContext _context;
    private readonly bool _ownsContext;

    public DatingRepository()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=datingapp.db")
            .Options;
        _context = new AppDbContext(options);   
        _ownsContext = true;

        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();        
    }
    
    public DatingRepository(AppDbContext context)
    {
        _context = context;
        _ownsContext = false;
    }

    public async Task<AppUser?> GetMemberAsync(string id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IReadOnlyList<AppUser>> GetMembersAsync()
    {
       return await _context.Users.ToListAsync();
    }

    public void Dispose()
    {
        if(_ownsContext)
        {
            _context.Database.CloseConnection();
            _context.Dispose();
        }
    }
}
