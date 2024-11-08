using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Victuz_MVC.Data;
using Victuz_MVC.Models;

namespace Victuz_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ActivitiesController> _logger;

        public ActivitiesController(ApplicationDbContext context, ILogger<ActivitiesController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/Activities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activity>>> GetActivity()
        {
            try
            {
                _logger.LogInformation("Fetching all activities from the database.");
                return await _context.Activity.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching activities.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        // GET: api/Activities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching activity with id {id}.");
                var activity = await _context.Activity.FindAsync(id);

                if (activity == null)
                {
                    _logger.LogWarning($"Activity with id {id} not found.");
                    return NotFound();
                }

                return activity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching activity with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        // PUT: api/Activities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivity(int id, Activity activity)
        {
            if (id != activity.Id)
            {
                _logger.LogWarning("Activity ID mismatch.");
                return BadRequest("Activity ID mismatch");
            }

            _context.Entry(activity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ActivityExists(id))
                {
                    _logger.LogWarning($"Activity with id {id} not found during update.");
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Concurrency error during update.");
                    return StatusCode(StatusCodes.Status500InternalServerError, "Concurrency error while updating activity");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating activity.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating activity");
            }

            return NoContent();
        }

        // POST: api/Activities
        [HttpPost]
        public async Task<ActionResult<Activity>> PostActivity(Activity activity)
        {
            try
            {
                _context.Activity.Add(activity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("New activity created successfully.");
                return CreatedAtAction(nameof(GetActivity), new { id = activity.Id }, activity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new activity.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new activity record");
            }
        }

        // DELETE: api/Activities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            try
            {
                var activity = await _context.Activity.FindAsync(id);
                if (activity == null)
                {
                    _logger.LogWarning($"Activity with id {id} not found for deletion.");
                    return NotFound();
                }

                _context.Activity.Remove(activity);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Activity with id {id} deleted successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting activity.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting activity");
            }
        }

        private bool ActivityExists(int id)
        {
            return _context.Activity.Any(e => e.Id == id);
        }
    }
}

