using Core.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text;

namespace Core.Entities
{
    public class Person : EntityObject
    {
        [Display(Name = "Vorname")]
        public string FirstName { get; set; } = String.Empty;
        [Display(Name = "Nachname"), Required(ErrorMessage = "Nachname muss eingegeben werden"), ValidLastName]
        public string LastName { get; set; } = String.Empty;

        // TODO: Nummer überprüfen
        [Display(Name = "Handy Nummer")]
        public string PhoneNumber { get; set; } = String.Empty;

        //TODO: Emails validieren...
        [Required]
        public string Email { get; set; } = String.Empty;

        [ForeignKey("Table_Id")]
        public DaTable Table { get; set; } = new DaTable();
        public int Table_Id { get; set; } = 0;
    }
}
