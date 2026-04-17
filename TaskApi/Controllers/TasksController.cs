using Microsoft.AspNetCore.Mvc;
using TaskApi.Models;
namespace TaskApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase {
    private static List<TaskItem> _tasks = new() {
        new TaskItem {
            Id = 1,
            Title = "Изучить ASP.NET Core",
            Priority = "High",
            isCompleted = true
        },
        new TaskItem {
            Id = 2,
            Title = "Сделать лабораторную №28",
            Priority = "High",
            isCompleted = false
        },
        new TaskItem {
            Id = 3,
            Title = "Написать README",
            Priority = "Normal",
            isCompleted = false
        },
    };
    private static int _nextId = 4;

    [HttpGet]
    public ActionResult<IEnumerable<TaskItem>> GetAll([FromQuery] bool? completed = null) {
        var result = _tasks.AsEnumerable();
        if (completed.HasValue)
            result = result.Where(t => t.isCompleted == completed.Value);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public ActionResult<TaskItem> GetById(int id) {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task is null)
            return NotFound(new { Message = $"Задача с id = {id} не найдена" });
        return Ok(task);
    }

    [HttpPost]
    public ActionResult<TaskItem> Create([FromBody] CreateTaskDto dto) {
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest(new { Message = "Поле Title обязательно для заполнения" });
        var newTask = new TaskItem {
            Id = _nextId++,
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            isCompleted = false,
            CreatedAt = DateTime.Now
        };
        _tasks.Add(newTask);
        return CreatedAtAction(nameof(GetById), new { id = newTask.Id }, newTask);
    }

    [HttpPut("{id}")]
    public ActionResult<TaskItem> Update(int id, [FromBody] UpdateTaskDto dto) {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task is null)
            return NotFound(new { Message = $"Задача с id={id} не найдена" });
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest(new { Message = "Поле Title не может быть пустым" });
        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Priority = dto.Priority;
        task.isCompleted = dto.isCompleted;
        return Ok(task);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id) {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task is null)
            return NotFound(new { Message = $"Задача с id={id} не найдена" });
        _tasks.Remove(task);
        return NoContent();
    }

    [HttpPatch("{id}/complete")]
    public ActionResult<TaskItem> MarkComplete(int id) {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task is null)
            return NotFound(new { Message = $"Задача с id={id} не найдена" });
        task.isCompleted = !task.isCompleted;
        return Ok(task);
    }

}