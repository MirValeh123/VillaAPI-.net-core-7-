using System.ComponentModel.DataAnnotations;

namespace VillaApi.Models
{
    public class Villa
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public int Price { get; set; }

        public int Area { set; get; }

        public DateTime CreatedDate { get; set; }
    }
}
