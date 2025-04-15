using System;
using System.Collections.Generic;

namespace Domain
{
    public class HomeOwner : User
    {
        public string ProfilePhoto { get; set; }

        public HomeOwner(string firstName, string lastName, string email, string password, string profilePhoto)
        : base(firstName, lastName, email, password)
        {
            Role = "homeOwner";
            ProfilePhoto = ValidateProfilePhoto(profilePhoto);
        }

        private string ValidateProfilePhoto(string profilePhoto)
        {
            if (string.IsNullOrWhiteSpace(profilePhoto))
            {
                throw new ArgumentException("Se debe proporcionar una foto de perfil.");
            }
            return profilePhoto;
        }
    }
}
