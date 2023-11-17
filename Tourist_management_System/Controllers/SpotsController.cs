using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tourist_management_System.Models;

namespace Tourist_management_System.Controllers
{
    public class SpotsController : Controller
    {
        private readonly TravelDbContext _context;

        public SpotsController(TravelDbContext context)
        {
            _context = context;
        }       
        public async Task<IActionResult> Index()
        {
              return _context.Spots != null ? 
                          View(await _context.Spots.ToListAsync()) :
                          Problem("Entity set 'TravelDbContext.Spots'  is null.");
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Spots == null)
            {
                return NotFound();
            }

            var spot = await _context.Spots
                .FirstOrDefaultAsync(m => m.SpotId == id);
            if (spot == null)
            {
                return NotFound();
            }

            return View(spot);
        }      
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpotId,SpotName")] Spot spot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(spot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(spot);
        }     
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Spots == null)
            {
                return NotFound();
            }

            var spot = await _context.Spots.FindAsync(id);
            if (spot == null)
            {
                return NotFound();
            }
            return View(spot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SpotId,SpotName")] Spot spot)
        {
            if (id != spot.SpotId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpotExists(spot.SpotId))
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
            return View(spot);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Spots == null)
            {
                return NotFound();
            }

            var spot = await _context.Spots
                .FirstOrDefaultAsync(m => m.SpotId == id);
            if (spot == null)
            {
                return NotFound();
            }

            return View(spot);
        }        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Spots == null)
            {
                return Problem("Entity set 'TravelDbContext.Spots'  is null.");
            }
            var spot = await _context.Spots.FindAsync(id);
            if (spot != null)
            {
                _context.Spots.Remove(spot);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpotExists(int id)
        {
          return (_context.Spots?.Any(e => e.SpotId == id)).GetValueOrDefault();
        }
    }
}
