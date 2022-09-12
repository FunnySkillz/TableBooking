using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Booking : EntityObject
    {
        [ForeignKey("Person_Id")]
        public Person Person { get; set; } = new Person();
        public int Person_Id { get; set; } = 0;


        [ForeignKey("Table_Id")]
        public DaTable Table { get; set; } = new DaTable();
        public int Table_Id { get; set; }

        public DateTime Date { get; set; }

        public Booking()
        {
            Date = DateTime.Now;
        }
    }
}
