using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt.Models
{

    public class CarModel
    {

        // Define carmodel

        //Properties

        public int Id { get; set; }
        [Required]
        public string? Model { get; set; }

        public string? Make { get; set; }

        public string? Gearbox { get; set; }

        public string? Fuel { get; set; }

        public int? Milage { get; set; }

        public string? Description { get; set; }

        public int? Price { get; set; }

        public string? ImageName { get; set; }

        [NotMapped]
        [Display(Name = "Bild")]
        public IFormFile? ImageFile { get; set; }


        //Foreign key

        public int MakeModelId { get; set; }
        public MakeModel? MakeModel {get; set;}

    }

}