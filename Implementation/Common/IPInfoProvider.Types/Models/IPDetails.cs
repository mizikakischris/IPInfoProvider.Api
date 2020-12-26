using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPInfoProvider.Types.Models
{
    [Table("IPDetails")]
    public class IPDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string  City { get; set; }

        [Column("Country_Name", TypeName = "nvarchar(50)")]
        public string Country { get; set; }

        [Column("Continent_Name", TypeName = "nvarchar(50)")]
        public string Continent { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }

        [Column("IP_Adress", TypeName = "nvarchar(50)")]
        public string IP { get; set; }
    }
}
