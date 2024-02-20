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
            foreach (var car in cars)
            {
                car.ImageUrl = $"{Request.Scheme}://{Request.Host.Value}/images/{car.ImageName}";
            }
           
            return cars;
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
