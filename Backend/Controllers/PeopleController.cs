using backend.Model;
using Backend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PeopleController : Controller
    {
        private readonly DataContext context;
        public PeopleController(DataContext c)
        {
            context = c;
        }
        [HttpGet]
        public IActionResult GetPeople()
        {
            var people = context.PeopleList!.AsQueryable();
            return Ok(people);

        }
        [HttpPost]
        public IActionResult Create([FromBody] People e)
        {
            //var eventToBeAttending = context.EventList.Find();
            var dbEvent = context.PeopleList?.Find(e.Id);
            if (dbEvent == null)
            {
                context.PeopleList?.Add(e);
                context.SaveChanges();
                return CreatedAtAction(nameof(GetPeople), new { e.Id }, e);
            }
            return Conflict();
        }
        [HttpPut("{id}")]
        public IActionResult Update(int? id, [FromBody] People e)
        {
            var person = context.PeopleList!.AsNoTracking().FirstOrDefault(p => p.Id == e.Id);
            if (id != e.Id || person == null) return NotFound();
            person = e;
            context.PeopleList.Update(person);
            context.SaveChanges();
            return CreatedAtAction(nameof(GetPeople), new { e.Id }, e);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var person = context.PeopleList?.Find(id);
            if (person == null) return NotFound();
            context.PeopleList?.Remove(person);
            context.SaveChanges();
            return NoContent();
        }
    }
}
