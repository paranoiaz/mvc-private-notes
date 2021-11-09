using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Models
{
    public class User : IdentityUser
    {
        public List<Note> Notes { get; set; } = new List<Note>();
    }
}