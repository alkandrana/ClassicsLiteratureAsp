using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ClassicsLiteratureAsp.Models;

[Index(nameof(Name), IsUnique = true)]
public class Language
{
    public int Id { get; set; }
    [Required] public string Name { get; set; }
}