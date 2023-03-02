using Microsoft.AspNetCore.Mvc;
using pvp.Data.Dto;
using pvp.Data.Entities;
using pvp.Data.Repositories;

namespace pvp.Controllers 
{
    [ApiController]
    [Route("api/Task")]
    public class TaskController :ControllerBase
    {
        private readonly ITaskRepository _taskRepository; 
        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> Create(CreateTaskDto createTaskDto) 
        {
            var task = new Uzduotys
            {
                Pavadinimas = createTaskDto.Name,
                Aprasymas = createTaskDto.Description,
                Difficulty = createTaskDto.Sudetingumas,
                Patvirtinta = createTaskDto.Confirmed,
                Mokomoji = createTaskDto.Educational,
                Data = createTaskDto.Date,
                Tipas_id = createTaskDto.Type_id,
                UserId = "1111"
            }
            await _taskRepository.CreateAsync(task);
            return Created("", new TaskDto(task.id, task.Pavadinimas, task.Aprasymas, task.Difficulty, task.Patvirtinta, task.Mokomoji, task.Data, task.Tipas_id));
        }
    }
}