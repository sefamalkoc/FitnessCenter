using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessCenter.Data;
using FitnessCenter.Models;

namespace FitnessCenter.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Reports/available-trainers?date=2023-10-25
        [HttpGet("available-trainers")]
        public async Task<ActionResult<IEnumerable<object>>> GetAvailableTrainers(DateTime date)
        {
            // LINQ Requirement: Filter data
            // Return list of trainers who have less than 8 appointments on that day (example logic)
            // or simply return all with their specialty count.
            
            var busyTrainerIds = await _context.Appointments
                .Where(a => a.Date.Date == date.Date && a.Status != AppointmentStatus.Cancelled)
                .Select(a => a.TrainerId)
                .Distinct()
                .ToListAsync();

            var availableTrainers = await _context.Trainers
                .Include(t => t.Specialties)
                .Where(t => !busyTrainerIds.Contains(t.Id)) // Simplistic availability check: Not booked at all? 
                                                            // Or better: Just return list and let frontend decide.
                                                            // Requirement: "Available Trainers for a specific Date"
                .Select(t => new {
                    t.Id,
                    t.Name,
                    Specialties = t.Specialties.Select(s => s.Name).ToList(),
                    Availability = t.Availability
                })
                .ToListAsync();

            return Ok(availableTrainers);
        }

        // LINQ Example 2: List of all Trainers with their Specialties
        [HttpGet("trainers-specialties")]
        public async Task<ActionResult<IEnumerable<object>>> GetTrainersWithSpecialties()
        {
            var data = await _context.Trainers
                .Include(t => t.Specialties)
                .Select(t => new {
                    TrainerName = t.Name,
                    Specialties = t.Specialties.Select(s => s.Name).ToList()
                })
                .ToListAsync();

            return Ok(data);
        }
    }
}
