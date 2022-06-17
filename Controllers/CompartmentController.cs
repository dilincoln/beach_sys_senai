using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Prova.Models;

namespace Prova.Controllers
{
    public class CompartmentController : Controller
    {
        private readonly ProvaContext _context;

        public CompartmentController(ProvaContext context)
        {
            _context = context;
        }

        // GET: Compartment
        public async Task<IActionResult> Index()
        {
            var compartmentContext = _context.Compartment;

            if (compartmentContext == null)
            {
                return Problem("Entity set 'ProvaContext.Compartment'  is null.");
            }

            var compartments = await compartmentContext.ToListAsync();

            foreach (var compartment in compartments)
            {
                var UserId = compartment.UserId;

                if (UserId != null)
                {
                    var user = await GetUser(UserId);

                    if (user != null)
                    {
                        compartment.User = user;
                    }
                }

                var CabinetId = compartment.CabinetId;

                var cabinet = await GetCabinet(CabinetId);

                if (cabinet != null)
                {
                    compartment.Cabinet = cabinet;
                }
            }

            return View(compartments);
        }

        // GET: Compartment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Compartment == null)
            {
                return NotFound();
            }

            var compartment = await _context.Compartment.FirstOrDefaultAsync(m => m.Id == id);

            if (compartment == null)
            {
                return NotFound();
            }

            var UserId = compartment.UserId;

            if (UserId != null)
            {
                var user = await GetUser(UserId);

                if (user != null)
                {
                    compartment.User = user;
                }
            }

            return View(compartment);
        }

        // GET: Compartment/Create
        public async Task<IActionResult> Create()
        {
            var cabinetSelectList = await this.GetCabinets();

            ViewData["CabinetId"] = cabinetSelectList;

            return View();
        }

        // POST: Compartment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,CabinetId,Width,Height,Depth")] Compartment compartment
        )
        {
            if (compartment.CabinetId == 0)
            {
                ModelState.AddModelError("CabinetId", "Armário obrigatório");
            }

            if (ModelState.IsValid)
            {
                _context.Add(compartment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var cabinetSelectList = await GetCabinets();

            ViewData["CabinetId"] = cabinetSelectList;

            return View(compartment);
        }

        // GET: Compartment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Compartment == null)
            {
                return NotFound();
            }

            var compartment = await _context.Compartment.FindAsync(id);

            if (compartment == null)
            {
                return NotFound();
            }

            var cabinetSelectList = await this.GetCabinets();

            ViewData["CabinetId"] = cabinetSelectList;

            return View(compartment);
        }

        // POST: Compartment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,CabinetId,Width,Height,Depth")] Compartment compartment
        )
        {
            if (id != compartment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compartment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompartmentExists(compartment.Id))
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

            return View(compartment);
        }

        // GET: Compartment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Compartment == null)
            {
                return NotFound();
            }

            var compartment = await _context.Compartment.FirstOrDefaultAsync(m => m.Id == id);
            if (compartment == null)
            {
                return NotFound();
            }

            return View(compartment);
        }

        // POST: Compartment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Compartment == null)
            {
                return Problem("Entity set 'ProvaContext.Compartment'  is null.");
            }
            var compartment = await _context.Compartment.FindAsync(id);
            if (compartment != null)
            {
                _context.Compartment.Remove(compartment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompartmentExists(int id)
        {
            return (_context.Compartment?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // Async function to get the list of cabinets
        private async Task<List<SelectListItem>> GetCabinets()
        {
            var cabinetsContext = _context.Cabinet;

            if (cabinetsContext == null)
            {
                return new List<SelectListItem>();
            }

            var cabinets = await cabinetsContext.ToListAsync();

            var cabinetsSelectList = cabinets.Select(
                c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }
            );

            return cabinetsSelectList.ToList();
        }

        // Async function to get User
        private async Task<User?> GetUser(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var userContext = _context.User;

            if (userContext == null)
            {
                return null;
            }

            var user = await userContext.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        // Async function to get Cabinet
        private async Task<Cabinet?> GetCabinet(int id)
        {
            var cabinetContext = _context.Cabinet;

            if (cabinetContext == null)
            {
                return null;
            }

            var cabinet = await cabinetContext.FirstOrDefaultAsync(u => u.Id == id);

            return cabinet;
        }
    }
}
