using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Model;

namespace backend.Controllers;

[ApiController] [Route("api/[controller]")] 
public class EventsController : ControllerBase {
    private readonly DataContext context;
    public EventsController(DataContext c)  {
        context = c;
    }
    [HttpGet] public IActionResult GetEvents() {        
        var events = context.EventList!.AsQueryable();
        return Ok(events);

    }   
    [HttpPost] public IActionResult Create([FromBody] Event e) {
        var dbEvent = context.EventList?.Find(e.Id); 
        if (dbEvent == null) {
            context.EventList?.Add(e); 
            context.SaveChanges();
            return CreatedAtAction(nameof(GetEvents), new { e.Id }, e);
        }
        return Conflict();
    }
    [HttpPut("{id}")] public IActionResult Update(int? id, [FromBody] Event e) {
        var dbEvent = context.EventList!.AsNoTracking().FirstOrDefault(eventInDB => eventInDB.Id == e.Id);
        if (id != e.Id || dbEvent == null) return NotFound();     
        context.Update(e);
        context.SaveChanges();
        return NoContent();
    }
    [HttpDelete("{id}")] public IActionResult Delete(int id) {
        var eventToDelete = context.EventList?.Find(id);
        if (eventToDelete == null) return NotFound();        
        context.EventList?.Remove(eventToDelete);
        context.SaveChanges();
        return NoContent();
    }

}

