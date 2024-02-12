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
            if (_context.Cars == null)
            {
                return NotFound();
            }

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
                // Add image url to response
                ImageUrl = $"{Request.Scheme}://{Request.Host.Value}/images/{c.ImageName}",
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
            var carModel = await _context.Cars
            .Include(c => c.MakeModel)
            .FirstOrDefaultAsync(d => d.Id == id);




            if (carModel == null)
            {
                return NotFound();
            }
            // Add image url to response
            carModel.ImageUrl = $"{Request.Scheme}://{Request.Host.Value}/images/{carModel.ImageName}";
            return carModel;
        }

        
    }
}
