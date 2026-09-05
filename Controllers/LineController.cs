using ClassicsLiteratureAsp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassicsLiteratureAsp.Models;
using Microsoft.AspNetCore.JsonPatch;
namespace ClassicsLiteratureAsp.Controllers;

[Route("lines")]
[ApiController]
public class LineController : Controller
{
    private readonly AppDbContext _ctx;
    
    public LineController(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet("/lines/opus/{opusId}")]
    public async Task<IActionResult> GetLinesByOpus([FromRoute] int opusId)
    {
        List<Line> lines = await _ctx.Lines
            .Where(l => l.OpusId == opusId)
            .ToListAsync();
        return Ok(lines);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLine([FromRoute] int id)
    {
        Line? line = await _ctx.Lines.FindAsync(id);
        if  (line == null)
        {
            return NotFound();
        }
        return Ok(line);
    }

    [HttpPost]
    public async Task<IActionResult> AddLine([FromBody] Line line)
    {
        _ctx.Lines.Add(line);
        await _ctx.SaveChangesAsync();
        return CreatedAtAction(nameof(GetLine), 
            new { id = line.Id }, line);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> EditLine([FromRoute] int id, [FromBody] JsonPatchDocument<Line> lineData)
    {
        if (lineData == null)
        {
            return BadRequest();
        }

        Line? line = await _ctx.Lines.FindAsync(id);
        if (line == null)
        {
            return NotFound();
        }
        
        lineData.ApplyTo(line);
        if (!TryValidateModel(line) || !ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _ctx.SaveChangesAsync();
        return Ok(line);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLine(int id)
    {
        Line? line = await _ctx.Lines.FindAsync(id);
        if (line == null)
        {
            return NotFound();
        }
        
        _ctx.Lines.Remove(line);
        await _ctx.SaveChangesAsync();
        return NoContent();
    }
}