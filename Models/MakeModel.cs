
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Projekt.Models
{

    public class MakeModel
    {

        // Define Make of a car

        //Properties

        public int Id { get; set; }

        [Required]
        [Display(Name = "Tillverkare")]
        public string? MakeOfModel { get; set; }
        
        [JsonIgnore]
        public List<CarModel>? CarModels { get; set; }
    }
}