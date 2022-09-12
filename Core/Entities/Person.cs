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

        [Display(Name = "Handy Nummer")]
        public int PhoneNumber { get; set; }

        //TODO: Validierung
        [Required]
        public string Email { get; set; } = String.Empty;
        
        //[ForeignKey("DaTable_Id")]
        //public DaTable DaTable { get; set; } = new DaTable();
        //public int DaTable_Id { get; set; } = 0;
    }
}
