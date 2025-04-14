using Microsoft.AspNetCore.Identity;

namespace ARSTAGE.Models
{
    public class AppUserModel : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImagePath { get; set; }
        public string Bio { get; set; }
        public string SocalMediaLink { get; set; }
        public DateOnly Dob { get; set; }
        public bool Gender { get; set; }
        public bool IsBan { get; set; }
        public string PasswordSalt { get; set; }


        public List<FavouriteModel> Favorites { get; set; } = new List<FavouriteModel>();
    }
}
