using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Autodoro.Model
{
    [Table("logs")]
    public class Log
    {
        [Column("id")]
        [Key]
        public long Id { get; set; }

    }
}
