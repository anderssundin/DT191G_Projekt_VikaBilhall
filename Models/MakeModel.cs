
namespace Projekt.Models{

    public class MakeModel {

        // Define Make of a car

        //Properties

        public int Id { get; set; }

        public string? MakeOfModel { get; set; }

        public List<CarModel>? CarModels {get;set;}
    }
}