using ClassicsLiteratureAsp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassicsLiteratureAsp.Models;
using Microsoft.AspNetCore.JsonPatch;
namespace ClassicsLiteratureAsp.Controllers;

[Route("opera")]
[ApiController]
public class OpusController : Controller
{
    private readonly AppDbContext _ctx;
    
    public OpusController(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    // GET
    [HttpGet]
    public async Task<IActionResult> GetOpera()
    {
        var opera = await _ctx.Opera.Select(p => new    // This is the construction that defeated me in CS234N!
        {
            p.Id,
            p.Code,
            p.Title,
            p.Author,
            p.Language,
            LineCount = p.Lines.Count
        }).ToListAsync();
        return Ok(opera);
    }

    [HttpGet("/opera/code/{code}")]
    public async Task<IActionResult> GetOpusByCode([FromRoute] string code)
    {
        List<Opus> opus = await _ctx.Opera
            .Include(p => p.Author)
            .Where(p => p.Code == code)
            .ToListAsync();

        return Ok(opus);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOpus(int id)
    {
        // TODO: Express API uses JavaScript's Object.groubBy to arrange lines into groups of books/sections within a work. ASP.NET equivalent?
        Opus? opus = await _ctx.Opera
            .Include(p => p.Author)
            .Include(p => p.Language)
            .Include(p => p.Lines)
            .SingleOrDefaultAsync(a => a.Id == id);
        if  (opus == null)
        {
            return NotFound();
        }
        return Ok(opus);
    }

    [HttpPost]
    public async Task<IActionResult> AddOpus([FromBody] Opus opus)
    {
        _ctx.Opera.Add(opus);
        await _ctx.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOpus), new { id = opus.Id }, opus);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> EditAuthor([FromRoute] int id, [FromBody] JsonPatchDocument<Opus> opusData)
    {
        if (opusData == null)
        {
            return BadRequest();
        }

        Opus? opus = await _ctx.Opera.FindAsync(id);
        if (opus == null)
        {
            return NotFound();
        }
        
        opusData.ApplyTo(opus);
        if (!TryValidateModel(opus) || !ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _ctx.SaveChangesAsync();
        return Ok(opus);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOpus([FromRoute] int id)
    {
        Opus? opus = await _ctx.Opera.FindAsync(id);
        if (opus == null)
        {
            return NotFound();
        }
        
        _ctx.Opera.Remove(opus);
        await _ctx.SaveChangesAsync();
        return NoContent();
    }
}