using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasksService.Data;
using TasksService.Dtos;
using TasksService.Models;
using TaskStatus = TasksService.Models.TaskStatus; 
namespace TasksService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TasksDbContext _db;
    private readonly IHttpClientFactory _httpClientFactory;

    public TasksController(TasksDbContext db, IHttpClientFactory httpClientFactory)
    {
        _db = db;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<IEnumerable<TaskItem>> GetAll()
    {
        return await _db.Tasks.AsNoTracking().ToListAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TaskItem>> GetById(Guid id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task == null) return NotFound();
        return task;
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create([FromBody] CreateTaskDto dto)
    {
        var gateway = _httpClientFactory.CreateClient("Gateway");

        // Проверяем, что пользователь существует
        var userResponse = await gateway.GetAsync($"/users/api/users/{dto.AssigneeId}");
        if (userResponse.StatusCode == HttpStatusCode.NotFound)
            return BadRequest("Assignee user not found");

        // Проверяем, что проект существует
        var projectResponse = await gateway.GetAsync($"/projects/api/projects/{dto.ProjectId}");
        if (projectResponse.StatusCode == HttpStatusCode.NotFound)
            return BadRequest("Project not found");

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            ProjectId = dto.ProjectId,
            AssigneeId = dto.AssigneeId,
            CreatedAt = DateTime.UtcNow,
            DueDate = dto.DueDate
        };

        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpPut("{id:guid}/status")]
    public async Task<ActionResult> UpdateStatus(Guid id, [FromBody] TaskStatus status)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task == null) return NotFound();

        task.Status = status;
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
