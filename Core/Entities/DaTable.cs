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
        public string TableName { get; set; } = String.Empty;
        
        [Required]
        public string QRCode { get; set; } = String.Empty;

        [ForeignKey("Person_Id")]
        public Person Person { get; set; } = new Person();
        public int Person_Id { get; set; } = 0;


        public DaTable()
        {
        }

        public DaTable(string tableName, string qRCode, Person person, int person_Id)
        {
            TableName = tableName;
            QRCode = qRCode;
            Person = person;
            Person_Id = person_Id;
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}
