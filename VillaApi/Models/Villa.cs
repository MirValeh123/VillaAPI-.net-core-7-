using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VillaApi.Models
{
    public class Villa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public int Price { get; set; }

        public int Area { set; get; }

        //public double Rate { get; set; }

        //public string ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
