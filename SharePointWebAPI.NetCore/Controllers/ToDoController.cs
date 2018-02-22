using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SharePoint.Client;
using WebAPI.NetCore.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.NetCore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class ToDoController : Controller
    {
        private readonly ToDoContext _context;
        public ToDoController(ToDoContext context)
        {
            _context = context;
            if (!_context.ToDoItems.Any())
            {
                _context.ToDoItems.Add(new ToDoItem { Name = "ToDo 1" });
                _context.SaveChanges();
            }
        }
        // GET api/<controller>/5
        [HttpGet("{id}", Name = "GetToDo")]
        public IActionResult Get(int id)
        {
            ToDoItem item = _context.ToDoItems.FirstOrDefault(i => i.Id == id);
            if (item == null)
                return NotFound();
            return new ObjectResult(item);
        }
        [HttpGet]
        public IEnumerable<ToDoItem> Get() => _context.ToDoItems.ToList();
        [HttpGet]
        public IEnumerable<ToDoItem> Get1() => new List<ToDoItem>();
        // POST api/<controller>
        /// <summary>
        /// Creates a ToDo Item
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        /// </remarks>
        /// <param name="todo"></param>
        /// <returns>A newly created ToDo item</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the input parameter is null or empty</response>
        [HttpPost]
        [ProducesResponseType(typeof(ToDoItem), 201)]
        [ProducesResponseType(typeof(ToDoItem), 400)]
        public IActionResult Post([FromBody]ToDoItem todo)
        {
            if (todo == null)
                return BadRequest();
            _context.ToDoItems.Add(todo);
            _context.SaveChanges();
            return CreatedAtRoute("GetToDo", new { id = todo.Id }, todo);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody]ToDoItem todo)
        {
            if (todo == null || todo.Id != id)
                return BadRequest();
            ToDoItem item = _context.ToDoItems.FirstOrDefault(i => i.Id == id);
            if (item == null)
                return NotFound();
            item.IsComplete = todo.IsComplete;
            item.Name = todo.Name;
            _context.ToDoItems.Update(item);
            _context.SaveChanges();
            return new NoContentResult();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            ToDoItem todo = _context.ToDoItems.FirstOrDefault(i => i.Id == id);
            if (todo == null)
                return NotFound();
            _context.ToDoItems.Remove(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}