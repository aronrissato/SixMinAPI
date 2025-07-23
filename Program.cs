using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SixMinAPI.Data;
using SixMinAPI.Dtos;
using SixMinAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sqlConBuilder = new SqlConnectionStringBuilder();

sqlConBuilder.ConnectionString = builder.Configuration.GetConnectionString("SQLDbConnection");
sqlConBuilder.UserID = builder.Configuration["UserId"];
sqlConBuilder.Password = builder.Configuration["Password"];

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(sqlConBuilder.ConnectionString));
builder.Services.AddScoped<ICommandRepo, CommandRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("api/v1/commands",
    async (ICommandRepo repo, IMapper mapper) =>
    {
        var commands = await repo.GetAllCommandsAsync();
        return Results.Ok(mapper.Map<IEnumerable<CommandReadDto>>(commands));
    });

app.MapGet("api/v1/commands/{id}",
    async (ICommandRepo repo, IMapper mapper, int id) =>
    {
        var command = await repo.GetCommandByIdAsync(id);
        return command == null
            ? Results.NotFound()
            : Results.Ok(mapper.Map<CommandReadDto>(command));
    });

app.MapPost("api/v1/commands",
    async (ICommandRepo repo, IMapper mapper, CommandCreateDto commandCreateDto) =>
    {
        var commandModel = mapper.Map<Command>(commandCreateDto);
        await repo.CreateCommandAsync(commandModel);
        await repo.SaveChangesAsync();

        var commandReadDto = mapper.Map<CommandReadDto>(commandModel);

        return Results.Created($"api/v1/commands/{commandReadDto.Id}", commandReadDto);
    });

app.MapPut("api/v1/commands/{id}",
    async (ICommandRepo repo, IMapper mapper, int id, CommandUpdateDto commandUpdateDto) =>
    {
        var command = await repo.GetCommandByIdAsync(id);
        if (command == null)
            return Results.NotFound();

        mapper.Map(commandUpdateDto, command);
        await repo.SaveChangesAsync();

        return Results.NoContent();
    });

app.MapDelete("api/v1/commands/{id}", async (ICommandRepo repo, int id) =>
{
    var command = await repo.GetCommandByIdAsync(id);
    if (command == null)
        return Results.NotFound();
    
    repo.DeleteCommand(command);
    await repo.SaveChangesAsync();
 
    return Results.NoContent();
});


app.Run();