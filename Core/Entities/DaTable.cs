using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class DaTable : EntityObject
    {
        [Required]
        public string QRCode { get; set; } = String.Empty;

        [Required]
        public int TableNumber { get; set; }

        public bool IsBooked { get; set; }

        //[ForeignKey("Person_Id")]
        //public Person Person { get; set; } = new Person();
        //public int Person_Id { get; set; } = 0;

    }
}
