using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoWebApi.Models;

namespace ToDoWebApi.Controllers2
{
    //[Produces("application/json")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/Todo")]
    public class TodoController : Controller
    {
        private readonly TodoContext context;
        public TodoController(TodoContext ctx)
        {
            context = ctx;
            if (context.TodoItems.Count() == 0)
            {
                context.Add(new TodoItem()
                {
                    Name = "Mohammad Lotfi",
                    IsDone = false
                });
                context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return context.TodoItems.ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(int id)
        {
            var item = context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody]TodoItem todoItem)
        {
            if (todoItem==null)
            {
                return BadRequest();
            }
            context.TodoItems.Add(todoItem);
            context.SaveChanges();
            return CreatedAtRoute("GetTodo",new {id=todoItem.Id},todoItem);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id,[FromBody] TodoItem todoItem)
        {
            /*
             * --Important
             * 
             * get your object from db
             * and 
             * change property must be change
             * and update with object u maked from db 
             * not that coming from client
             * 
             * --Important
             */
            if (todoItem==null||id!=todoItem.Id)
            {
                return BadRequest();
            }
            var todo = context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (todo==null)
            {
                return NotFound();
            }

            todo.IsDone = todoItem.IsDone;
            todo.Name = todoItem.Name;

            context.TodoItems.Update(todo);
            context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = context.TodoItems.FirstOrDefault(t=>t.Id==id);
            if (todo==null)
            {
                return NotFound();
            }
            context.TodoItems.Remove(todo);
            context.SaveChanges();
            return new NoContentResult();
        }

        #region HelpCpde

        //// GET: api/Todo
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Todo/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Todo
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Todo/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //} 
        #endregion
    }
}
