using ClassicsLiteratureAsp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassicsLiteratureAsp.Models;
using Microsoft.AspNetCore.JsonPatch;
namespace ClassicsLiteratureAsp.Controllers;

[Route("languages")]
[ApiController]
public class LanguageController : Controller
{
    private readonly AppDbContext _ctx;
    
    public LanguageController(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    // GET
    [HttpGet]
    public async Task<IActionResult> GetLanguages()
    {
        List<Language> languages = await _ctx.Languages.ToListAsync();
        return Ok(languages);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLanguage([FromRoute] int id)
    {
        Language? language = await _ctx.Languages.FindAsync(id);
        if  (language == null)
        {
            return NotFound();
        }
        return Ok(language);
    }

    [HttpPost]
    public async Task<IActionResult> AddLanguage([FromBody] Language language)
    {
        _ctx.Languages.Add(language);
        await _ctx.SaveChangesAsync();
        return CreatedAtAction(nameof(GetLanguage), new { id = language.Id }, language);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> EditLanguage([FromRoute] int id, [FromBody] JsonPatchDocument<Language> languageData)
    {
        if (languageData == null)
        {
            return BadRequest();
        }

        Language? language = await _ctx.Languages.FindAsync(id);
        if (language == null)
        {
            return NotFound();
        }
        
        languageData.ApplyTo(language);
        if (!TryValidateModel(language) || !ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _ctx.SaveChangesAsync();
        return Ok(language);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLanguage([FromRoute] int id)
    {
        Language? language = await _ctx.Languages.FindAsync(id);
        if (language == null)
        {
            return NotFound();
        }
        
        _ctx.Languages.Remove(language);
        await _ctx.SaveChangesAsync();
        return NoContent();
    }
}