using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.Models;

namespace Projekt
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CarApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CarApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarModel>>> GetCars()
        {

            var cars = await _context.Cars
         .Include(c => c.MakeModel)
         .ToListAsync();

            var carsWithMakeOfModel = cars.Select(c => new CarModel
            {
                Id = c.Id,
                Model = c.Model,
                Year = c.Year,
                Gearbox = c.Gearbox,
                Fuel = c.Fuel,
                Milage = c.Milage,
                Description = c.Description,
                Price = c.Price,
                ImageName = c.ImageName,
                ImageFile = c.ImageFile,
                MakeModelId = c.MakeModelId,
                MakeModel = new MakeModel
                {
                    Id = c.MakeModel?.Id ?? 0,
                    MakeOfModel = c.MakeModel?.MakeOfModel
                }
            });

            return carsWithMakeOfModel.ToList();
        }


        // GET: api/CarApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CarModel>> GetCarModel(int id)
        {
            var carModel = await _context.Cars.FindAsync(id);

            if (carModel == null)
            {
                return NotFound();
            }

            return carModel;
        }

        // PUT: api/CarApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarModel(int id, CarModel carModel)
        {
            if (id != carModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(carModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CarApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CarModel>> PostCarModel(CarModel carModel)
        {
            _context.Cars.Add(carModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCarModel", new { id = carModel.Id }, carModel);
        }

        // DELETE: api/CarApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarModel(int id)
        {
            var carModel = await _context.Cars.FindAsync(id);
            if (carModel == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(carModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarModelExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
