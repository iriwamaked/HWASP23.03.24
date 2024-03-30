using Microsoft.AspNetCore.Mvc;

namespace HWASP.Models
{
    public class RegistrationFormModel
    {
        [FromForm (Name = "signup-username")] 
        public String UserName { get; set; }
        [FromForm(Name = "signup-password")]
        public String Password { get; set; }
    }
}
