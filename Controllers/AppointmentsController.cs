using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessCenter.Data;
using FitnessCenter.Models;

namespace FitnessCenter.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var appointments = _context.Appointments
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .AsQueryable();

            if (!User.IsInRole("Admin"))
            {
                // Members see only their own appointments
                appointments = appointments.Where(a => a.MemberId == user.Id);
            }

            return View(await appointments.ToListAsync());
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name");
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "Name");
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,Time,TrainerId,ServiceId")] Appointment appointment)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            appointment.MemberId = user.Id;
            appointment.Status = AppointmentStatus.Pending;

            // Manual Validation Logic
            if (ModelState.IsValid)
            {
                // 1. Check if Trainer is available (Simple: Not booked at that time)
                bool isConflict = await _context.Appointments.AnyAsync(a =>
                    a.TrainerId == appointment.TrainerId &&
                    a.Date.Date == appointment.Date.Date &&
                    a.Time == appointment.Time &&
                    a.Status != AppointmentStatus.Cancelled);

                if (isConflict)
                {
                    ModelState.AddModelError("", "The selected trainer is already booked for this time slot.");
                }
                else
                {
                    // 2. Ideally check Trainer Working Hours here (Mock check)
                    // If we had parsed WorkingHours, we would check. For now, assume ok or check simple range (9-17)
                    if (appointment.Time.Hours < 9 || appointment.Time.Hours > 17)
                    {
                        ModelState.AddModelError("Time", "Appointments must be between 09:00 and 17:00.");
                    }
                    else
                    {
                        _context.Add(appointment);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "Name", appointment.TrainerId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            // Authorization check
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (!User.IsInRole("Admin") && appointment.MemberId != user.Id)
            {
                return Forbid();
            }

            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "Name", appointment.TrainerId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Time,TrainerId,ServiceId,Status")] Appointment appointment)
        {
            if (id != appointment.Id) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var existingAppointment = await _context.Appointments.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            if (existingAppointment == null) return NotFound();

            // Authorization check
            if (!User.IsInRole("Admin") && existingAppointment.MemberId != user.Id)
            {
                return Forbid();
            }

            // Keep the original MemberId
            appointment.MemberId = existingAppointment.MemberId;

            if (ModelState.IsValid)
            {
                // Check for conflicts (excluding current appointment)
                bool isConflict = await _context.Appointments.AnyAsync(a =>
                    a.Id != appointment.Id &&
                    a.TrainerId == appointment.TrainerId &&
                    a.Date.Date == appointment.Date.Date &&
                    a.Time == appointment.Time &&
                    a.Status != AppointmentStatus.Cancelled);

                if (isConflict)
                {
                    ModelState.AddModelError("", "The selected trainer is already booked for this time slot.");
                }
                else if (appointment.Time.Hours < 9 || appointment.Time.Hours > 17)
                {
                    ModelState.AddModelError("Time", "Appointments must be between 09:00 and 17:00.");
                }
                else
                {
                    try
                    {
                        _context.Update(appointment);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AppointmentExists(appointment.Id))
                            return NotFound();
                        else
                            throw;
                    }
                }
            }

            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "Name", appointment.TrainerId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Member)
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null) return NotFound();

            // Authorization check
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (!User.IsInRole("Admin") && appointment.MemberId != user.Id)
            {
                return Forbid();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            // Authorization check
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (!User.IsInRole("Admin") && appointment.MemberId != user.Id)
            {
                return Forbid();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Appointments/Approve/5 (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            appointment.Status = AppointmentStatus.Approved;
            _context.Update(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Appointments/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            // Authorization check
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (!User.IsInRole("Admin") && appointment.MemberId != user.Id)
            {
                return Forbid();
            }

            appointment.Status = AppointmentStatus.Cancelled;
            _context.Update(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
