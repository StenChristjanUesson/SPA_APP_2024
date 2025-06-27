using backend.Model;
using Backend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   // [Authorize]
    public class PrivateNotesController : Controller
    {
        private readonly DataContext context;
        public PrivateNotesController(DataContext c)
        {
            context = c;
        }
        [HttpGet("getall")]
        public IActionResult GetAllNotes(string username)
        {
            var user = context.Users.Include(u => u.PrivateNotes).FirstOrDefault(u => u.Username == username);
            if (user == null) return BadRequest("No such user");
            var allNotes = user.PrivateNotes;
            return Ok(allNotes);
        }
        [HttpPost("add")] 
        public IActionResult AddNote(string username,[FromBody] PrivateNotes note)
        {
            var user = context.Users.Include(u => u.PrivateNotes).FirstOrDefault(u => u.Username == username);
            if (user == null) return BadRequest("No such user");
            user.PrivateNotes.Add(note);
            context.SaveChanges();
            return Ok("Success!");
        }
        [HttpDelete("delete")]
        public IActionResult DeleteNote(string username,int id)
        {
            var user = context.Users.Include(u => u.PrivateNotes).FirstOrDefault(u => u.Username == username);
            if (user == null) return BadRequest("No such user");
            var note = user.PrivateNotes.FirstOrDefault(n => n.Id == id);
            if (note == null) return NotFound("No such note");
            user.PrivateNotes.Remove(note);
            context.SaveChanges();
            return NoContent();
        }
        [HttpPut("update")]
        public IActionResult Update(string username, [FromBody] PrivateNotes note)
        {
            var user = context.Users.Include(u => u.PrivateNotes).FirstOrDefault(u => u.Username == username);
            if (user == null) return NotFound("No such user");
            var existingNote = user.PrivateNotes.FirstOrDefault(n => n.Id == note.Id);
            if (existingNote == null) return NotFound("No such note");
            existingNote.Name = note.Name;
            existingNote.Description = note.Description;
            context.SaveChanges();
            return Ok("Success!");
        }
    }
}
