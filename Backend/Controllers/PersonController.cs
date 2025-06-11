using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Model;
using System.Xml.Linq;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly DataContext context;
    public PersonController(DataContext c)
    {
        context = c;
    }
    [HttpGet] 
    public IActionResult GetPerson()
    {
        var people = context.PersonList!.AsQueryable();
        return Ok(people);
    }
    [HttpPost] 
    public IActionResult Create([FromBody] Person p)
    {
        var dbPerson = context.PersonList?.Find(p.Id);
        if (dbPerson == null)
        {
            context.PersonList?.Add(p);
            context.SaveChanges();
            return CreatedAtAction(nameof(GetPerson), new { p.Id }, p);
        }
        return Conflict();
    }
    [HttpPut("{id}")] 
    public IActionResult Update(int? id, [FromBody] Person p)
    {
        var dbPerson = context.PersonList!.AsNoTracking().FirstOrDefault(PersonInDB => PersonInDB.Id == p.Id);
        if (id != p.Id || dbPerson == null) return NotFound();
        context.Update(p);
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