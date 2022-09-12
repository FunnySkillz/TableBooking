using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Validations
{
    public class ValidLastName: ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return false;
            string lastName = (string)value;
            if (lastName.Contains(' '))
            {
                ErrorMessage = "Nachname darf keine Leerzeichen beinhalten";
                return false;
            }
            return true;
        }

        //protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        //{
        //    if (value == null)
        //        return ValidationResult.;
        //    string lastName = (string)value;
        //    if (lastName.Contains(' '))
        //    {
        //        return new ValidationResult("Nachname darf keine Leerzeichen beinhalten", new List<string>({nameof(Person.LastName) });
        //    }
        //    return ValidationResult.Success;
        //}

    }
}
