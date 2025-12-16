using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessCenter.Data;
using FitnessCenter.Models;

namespace FitnessCenter.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TrainersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Trainers.Include(t => t.Specialties).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var trainer = await _context.Trainers.Include(t => t.Specialties).FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null) return NotFound();
            return View(trainer);
        }

        public IActionResult Create()
        {
            ViewData["Services"] = _context.Services.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Availability")] Trainer trainer, int[] sentSelectedServices)
        {
            if (ModelState.IsValid)
            {
                if (sentSelectedServices != null)
                {
                    foreach (var serviceId in sentSelectedServices)
                    {
                        var service = await _context.Services.FindAsync(serviceId);
                        if (service != null)
                        {
                            trainer.Specialties.Add(service);
                        }
                    }
                }
                _context.Add(trainer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Services"] = _context.Services.ToList();
            return View(trainer);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var trainer = await _context.Trainers.Include(t => t.Specialties).FirstOrDefaultAsync(x => x.Id == id);
            if (trainer == null) return NotFound();
            
            ViewData["Services"] = _context.Services.ToList();
            return View(trainer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Availability")] Trainer trainer, int[] sentSelectedServices)
        {
            if (id != trainer.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var trainerToUpdate = await _context.Trainers.Include(t => t.Specialties).FirstOrDefaultAsync(t => t.Id == id);
                    if (trainerToUpdate == null) return NotFound();

                    trainerToUpdate.Name = trainer.Name;
                    trainerToUpdate.Availability = trainer.Availability;
                    
                    // Update Specialties
                    trainerToUpdate.Specialties.Clear();
                    if (sentSelectedServices != null)
                    {
                        foreach (var serviceId in sentSelectedServices)
                        {
                            var service = await _context.Services.FindAsync(serviceId);
                            if (service != null)
                            {
                                trainerToUpdate.Specialties.Add(service);
                            }
                        }
                    }

                    _context.Update(trainerToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerExists(trainer.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Services"] = _context.Services.ToList();
            return View(trainer);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var trainer = await _context.Trainers.FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null) return NotFound();
            return View(trainer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TrainerExists(int id)
        {
            return _context.Trainers.Any(e => e.Id == id);
        }
    }
}
