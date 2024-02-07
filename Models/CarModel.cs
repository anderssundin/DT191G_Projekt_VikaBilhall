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
        [Display(Name = "Modellnamn")]
        public string? Model { get; set; }

           [Display(Name = "År")]
        public string? Year { get; set; }

        [Display(Name = "Växellåda")]
        public string? Gearbox { get; set; }
        [Display(Name = "Drivmedel")]
        public string? Fuel { get; set; }
        [Display(Name = "Miltal")]
        public int? Milage { get; set; }
        [Display(Name = "Beskrivning")]
        public string? Description { get; set; }
        [Display(Name = "Pris")]
        public int? Price { get; set; }
        [Display(Name = "Bild")]
        public string? ImageName { get; set; }

        [NotMapped]
        [Display(Name = "Bild")]
        public IFormFile? ImageFile { get; set; }


        //Foreign key
        [Display(Name = "Tillverkare")]
        public int MakeModelId { get; set; }
         [Display(Name = "Tillverkare")]
        public MakeModel? MakeModel { get; set; }

    }

}