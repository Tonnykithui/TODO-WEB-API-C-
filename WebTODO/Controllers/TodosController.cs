
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTODO.Models;

namespace WebTODO.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    
    public class TodosController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public TodosController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodo()
        {
            var todo = await _dbContext.Todos.ToListAsync();
            return Ok(todo);
        }

        //public async Task<Todo> GetTodoByID(int id)
        //{
        //    var todo = await _dbContext.Todos.FindAsync(id);
        //    return todo;
        //}

        [HttpPost]
        public async Task<IActionResult> PostTodo(Todo todo)
        {
            if (todo == null)
                return BadRequest();

            await _dbContext.Todos.AddAsync(todo);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult EditTodo(int id, Todo todo)
        {
            var todoitem = _dbContext.Todos.Find(id);
            if (todoitem == null)
                return BadRequest();

            todoitem.Completed = todo != null ? todo.Completed : todoitem.Completed;
            todoitem.Description = todo != null ? todo.Description : todoitem.Description;
            todoitem.Status = todo != null ? todo.Status : todoitem.Status;
            todoitem.Title = todo.Title ?? todoitem.Title;

            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTodo(int? id)
        {
            if (id == null)
                return BadRequest("Please provide id of todo to delete");

            var todo = _dbContext.Todos.Find(id);
            if (todo == null)
                return BadRequest("No todo with this id exists");


            _dbContext.Todos.Remove(todo);
            _dbContext.SaveChanges();
            return NoContent();
        }
    }
}