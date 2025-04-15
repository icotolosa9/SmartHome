namespace Domain
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.RegularExpressions;

    public class Company
    {
        public Guid CompanyId { get; set; }
        public string Rut { get; private set; }
        public string Name { get; private set; }
        public string LogoURL { get; private set; }
        public Guid OwnerId { get; set; }
        public CompanyOwner Owner { get; set; }
        public string? ValidatorModel { get; set; }

        public Company(string rut, string name, string logoURL)
        {
            if (!IsValidRut(rut))
                throw new ArgumentException("El rut de la compañía debe tener exactamente 12 dígitos y ser numérico.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre de la compañía no es válido.");

            if (string.IsNullOrWhiteSpace(logoURL))
                throw new ArgumentException("Se debe proporcionar un logo para la compañía.");

            CompanyId = Guid.NewGuid();
            Rut = rut;
            Name = name;
            LogoURL = logoURL;
        }

        private bool IsValidRut(string rut)
        {
            return rut.Length == 12 && Regex.IsMatch(rut, @"^\d{12}$");
        }

        public override bool Equals(object? obj)
        {
            return obj is Company company &&
                   (Name == company.Name || Rut == company.Rut);
        }
    }
}
