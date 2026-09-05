using ClassicsLiteratureAsp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassicsLiteratureAsp.Models;
using Microsoft.AspNetCore.JsonPatch;
namespace ClassicsLiteratureAsp.Controllers;

[Route("authors")]
[ApiController]
public class AuthorController : Controller
{
    private readonly AppDbContext _ctx;
    
    public AuthorController(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    // GET
    [HttpGet]
    public async Task<IActionResult> GetAuthors()
    {
        List<Author> authors = await _ctx.Authors.ToListAsync();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuthor(int id)
    {
        Author? author = await _ctx.Authors.Include(a => a.Opera).SingleOrDefaultAsync(a => a.Id == id);
        if  (author == null)
        {
            return NotFound();
        }
        return Ok(author);
    }

    [HttpPost]
    public async Task<IActionResult> AddAuthor([FromBody] Author author)
    {
        _ctx.Authors.Add(author);
        await _ctx.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> EditAuthor([FromRoute] int id, [FromBody] JsonPatchDocument<Author> authorData)
    {
        if (authorData == null)
        {
            return BadRequest();
        }

        Author? author = await _ctx.Authors.FindAsync(id);
        if (author == null)
        {
            return NotFound();
        }
        
        authorData.ApplyTo(author);
        if (!TryValidateModel(author) || !ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _ctx.SaveChangesAsync();
        return Ok(author);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        Author? author = await _ctx.Authors.FindAsync(id);
        if (author == null)
        {
            return NotFound();
        }
        
        _ctx.Authors.Remove(author);
        await _ctx.SaveChangesAsync();
        return NoContent();
    }
}