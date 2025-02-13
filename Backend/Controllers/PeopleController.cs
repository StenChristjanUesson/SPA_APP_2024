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
        var People = context.PersonList!.AsQueryable();
        return Ok(People);
    }
    [HttpPost]
    public IActionResult Create([FromBody] Person e)
    {
        var dbPerson = context.PersonList?.Find(e.Name);
        if (dbPerson == null)
        {
            context.PersonList?.Add(e);
            context.SaveChanges();
            return CreatedAtAction(nameof(GetEvents), new { e.Name }, e);
        }
        return Conflict();
    }
    [HttpPut("{id}")]
    public IActionResult Update(string? name, [FromBody] Person e)
    {
        var dbPerson = context.PersonList!.AsNoTracking().FirstOrDefault(PersonInDB => PersonInDB.Name == e.Name);
        if (name != e.Name || dbPerson == null) return NotFound();
        context.Update(e);
        context.SaveChanges();
        return NoContent();
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(Person e)
    {
        var PersonToDelete = context.PersonList?.Find(e);
        if (PersonToDelete == null) return NotFound();
        context.PersonList?.Remove(PersonToDelete);
        context.SaveChanges();
        return NoContent();
    }

}

