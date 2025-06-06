using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Model;
using System.Xml.Linq;

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
        var person = context.PersonList!.AsQueryable();
        return Ok(person);
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
        var dbPerson = context.PersonList!.AsNoTracking().FirstOrDefault(PersonInDB => PersonInDB.Id == e.Id);
        if (id != e.Id || dbPerson == null) return NotFound();
        context.Update(e);
        context.SaveChanges();
        return NoContent();
    }
    [HttpDelete("{id}")] 
    public IActionResult Delete(int id)
    {
        var PersonToDelete = context.PersonList?.Find(id);
        if (PersonToDelete == null) return NotFound();
        context.PersonList?.Remove(PersonToDelete);
        context.SaveChanges();
        return NoContent();
    }
}