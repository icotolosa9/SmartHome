using System;
using System.Text.RegularExpressions;

namespace Domain
{
    public abstract class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Notification> Notifications { get; set; } = new();
        public List<Home> Homes { get; set; } = new List<Home>();

        public User(string firstName, string lastName, string email, string password)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = ValidateEmail(email);
            Password = ValidatePassword(password);
            CreationDate = DateTime.Now;
        }

        public User() { }

        private string ValidateEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                throw new ArgumentException("El correo electrónico no es válido. Debe tener el formato usuario@dominio.com.");
            }
            return email;
        }

        private string ValidatePassword(string password)
        {
            if (password.Length < 6)
            {
                throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.");
            }

            var specialCharPattern = @"[!@#$%^&*(),.?\:{ }|<>]";
            if (!Regex.IsMatch(password, specialCharPattern))
            {
                throw new ArgumentException("La contraseña debe contener al menos un carácter especial.");
            }
            return password;
        }

        public override bool Equals(object? obj)
        {
            return obj is User user &&
                   Email == user.Email;
        }
    }
}
