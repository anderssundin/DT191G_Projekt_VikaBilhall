using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.Models;

namespace Projekt.Controllers
{
    [Authorize]
    public class CarController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string wwwRootPath;

        public CarController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            wwwRootPath = hostEnvironment.WebRootPath;

        }

        // GET: Car
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Cars.Include(c => c.MakeModel);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Car/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carModel = await _context.Cars
                .Include(c => c.MakeModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carModel == null)
            {
                return NotFound();
            }

            return View(carModel);
        }

        // GET: Car/Create
        public IActionResult Create()
        {
            ViewData["MakeModelId"] = new SelectList(_context.Make, "Id", "MakeOfModel");
            return View();
        }

        // POST: Car/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Model,Year,Gearbox,Fuel,Milage,Description,Price,MakeModelId,ImageFile")] CarModel carModel)
        {

            if (ModelState.IsValid)
            {

                // Check for image

                if (carModel.ImageFile != null)
                {
                    // Generate filename
                    string fileName = Path.GetFileNameWithoutExtension(carModel.ImageFile.FileName);
                    // Get file extension
                    string extension = Path.GetExtension(carModel.ImageFile.FileName);

                    // Set imagename
                    carModel.ImageName = fileName = fileName.Replace(" ", String.Empty) + DateTime.Now.ToString("yyyymmssfff") + extension;

                    // set path to image
                    string path = Path.Combine(wwwRootPath + "/images", fileName);

                    // store in filesystem

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await carModel.ImageFile.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    carModel.ImageName = "empty.jpg";
                }


                _context.Add(carModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MakeModelId"] = new SelectList(_context.Make, "Id", "Id", carModel.MakeModelId);
            return View(carModel);
        }

        // GET: Car/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carModel = await _context.Cars.FindAsync(id);
            if (carModel == null)
            {
                return NotFound();
            }
            ViewData["MakeModelId"] = new SelectList(_context.Make, "Id", "MakeOfModel", carModel.MakeModelId);
            return View(carModel);
        }

        // POST: Car/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Model,Year,Gearbox,Fuel,Milage,Description,Price,MakeModelId,ImageFile")] CarModel carModel)
        {
            if (id != carModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Hämta befintlig bil från databasen
                    var existingCar = await _context.Cars.FindAsync(id);
                    if (existingCar != null)
                    {
                        // Uppdatera bara de egenskaper som behöver ändras
                        existingCar.Model = carModel.Model;
                        existingCar.Year = carModel.Year;
                        existingCar.Gearbox = carModel.Gearbox;
                        existingCar.Fuel = carModel.Fuel;
                        existingCar.Milage = carModel.Milage;
                        existingCar.Description = carModel.Description;
                        existingCar.Price = carModel.Price;
                        existingCar.MakeModelId = carModel.MakeModelId;

                        // Om en ny bild har valts, uppdatera bildinformationen
                        if (carModel.ImageFile != null)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(carModel.ImageFile.FileName);
                            string extension = Path.GetExtension(carModel.ImageFile.FileName);
                            existingCar.ImageName = fileName.Replace(" ", String.Empty) + DateTime.Now.ToString("yyyymmssfff") + extension;

                            string path = Path.Combine(wwwRootPath + "/images", existingCar.ImageName);

                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                await carModel.ImageFile.CopyToAsync(fileStream);
                            }
                        }
                    }
                    // Spara ändringarna i databasen
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarModelExists(carModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MakeModelId"] = new SelectList(_context.Make, "Id", "Id", carModel.MakeModelId);
            return View(carModel);
        }


        // GET: Car/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carModel = await _context.Cars
                .Include(c => c.MakeModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carModel == null)
            {
                return NotFound();
            }

            return View(carModel);
        }

        // POST: Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carModel = await _context.Cars.FindAsync(id);
            if (carModel != null)
            {
                _context.Cars.Remove(carModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarModelExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
