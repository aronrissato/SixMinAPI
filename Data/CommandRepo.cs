﻿using Microsoft.EntityFrameworkCore;
using SixMinAPI.Models;

namespace SixMinAPI.Data;

public class CommandRepo : ICommandRepo
{
    private readonly AppDbContext _context;

    public CommandRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Command?> GetCommandByIdAsync(int id)
    {
        return await _context.Commands.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Command>> GetAllCommandsAsync()
    {
        return await _context.Commands.ToListAsync();
    }

    public async Task CreateCommandAsync(Command command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        await _context.Commands.AddAsync(command);
    }

    public void DeleteCommand(Command command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        _context.Commands.Remove(command);
    }
}