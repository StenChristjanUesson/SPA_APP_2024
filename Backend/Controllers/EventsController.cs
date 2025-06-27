using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Model;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers;

[ApiController] 
[Route("api/[controller]")]
[Authorize]
public class EventsController : ControllerBase {
    private readonly DataContext context;
    public EventsController(DataContext c)  {
        context = c;
    }
    [HttpPost("reg_event")]
    public IActionResult RegisterToEvent(string username, int eID)
    {
        var exists = context.EventList.FirstOrDefault(e => e.Id == eID);
        if (exists == null) return BadRequest("No such event!");
        var user = context.Users.FirstOrDefault(u=>u.Username == username);
        if (user == null) return BadRequest("No such user to register");
        exists.RegisteredUsers.Add(user);
        context.SaveChanges();
        return Ok("Success!");
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

