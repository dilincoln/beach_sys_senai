using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prova.Models;

namespace Prova.Controllers
{
    public class CabinetController : Controller
    {
        private readonly ProvaContext _context;

        public CabinetController(ProvaContext context)
        {
            _context = context;
        }

        // GET: Cabinet
        public async Task<IActionResult> Index()
        {
            if (_context.Cabinet != null)
            {
                var cabinets = await _context.Cabinet.ToListAsync();

                foreach (var cabinet in cabinets)
                {
                    var compartments = await GetCompartments(cabinet.Id);

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
                    }

                    cabinet.Compartments = compartments;
                }

                return View(cabinets);
            }
            else
            {
                return Problem("Entity set 'ProvaContext.Cabinet'  is null.");
            }
        }

        // GET: Cabinet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cabinet == null)
            {
                return NotFound();
            }

            var cabinet = await _context.Cabinet.FirstOrDefaultAsync(m => m.Id == id);
            if (cabinet == null)
            {
                return NotFound();
            }

            return View(cabinet);
        }

        // GET: Cabinet/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cabinet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Name,Latitude,Longitude")] Cabinet cabinet
        )
        {
            if (ModelState.IsValid)
            {
                _context.Add(cabinet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cabinet);
        }

        // GET: Cabinet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cabinet == null)
            {
                return NotFound();
            }

            var cabinet = await _context.Cabinet.FindAsync(id);
            if (cabinet == null)
            {
                return NotFound();
            }
            return View(cabinet);
        }

        // POST: Cabinet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,Latitude,Longitude")] Cabinet cabinet
        )
        {
            if (id != cabinet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cabinet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CabinetExists(cabinet.Id))
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
            return View(cabinet);
        }

        // GET: Cabinet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cabinet == null)
            {
                return NotFound();
            }

            var cabinet = await _context.Cabinet.FirstOrDefaultAsync(m => m.Id == id);
            if (cabinet == null)
            {
                return NotFound();
            }

            return View(cabinet);
        }

        // POST: Cabinet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cabinet == null)
            {
                return Problem("Entity set 'ProvaContext.Cabinet'  is null.");
            }
            var cabinet = await _context.Cabinet.FindAsync(id);
            if (cabinet != null)
            {
                _context.Cabinet.Remove(cabinet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CabinetExists(int id)
        {
            return (_context.Cabinet?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<ICollection<Compartment>> GetCompartments(int Id)
        {
            var compartments = _context.Compartment;

            var compartmentsList = new List<Compartment>();

            if (compartments != null)
            {
                compartmentsList = await compartments.Where(c => c.CabinetId == Id).ToListAsync();
            }

            return compartmentsList;
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
    }
}
