using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Model;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeopleController : ControllerBase
{
    private readonly DataContext context;
    public PeopleController(DataContext c)
    {
        context = c;
    }
    [HttpGet]
    public IActionResult GetEvents()
    {
        var People = context.PersonList!.AsQueryable();
        return Ok(People);
    }
    [HttpPost]
    public IActionResult Create([FromBody] Person e)
    {
        var dbPerson = context.PersonList?.Find(e.Id);
        if (dbPerson == null)
        {
            context.PersonList?.Add(e);
            context.SaveChanges();
            return CreatedAtAction(nameof(GetEvents), new { e.Id }, e);
        }
        return Conflict();
    }
    [HttpPut("{id}")]
    public IActionResult Update(int? id, [FromBody] Person e)
    {
        var dbPerson = context.EventList!.AsNoTracking().FirstOrDefault(PersonInDB => PersonInDB.Id == e.Id);
        if (id != e.Id || dbPerson == null) return NotFound();
        context.Update(e);
        context.SaveChanges();
        return NoContent();
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var PersonToDelete = context.EventList?.Find(id);
        if (PersonToDelete == null) return NotFound();
        context.EventList?.Remove(PersonToDelete);
        context.SaveChanges();
        return NoContent();
    }

}

