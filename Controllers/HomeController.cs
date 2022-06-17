using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Prova.Models;
using Prova.DTOs;

namespace Prova.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly ProvaContext _context;

    public HomeController(ILogger<HomeController> logger, ProvaContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Users = await GetUserSelectList();

        ViewBag.AvailableCompartments = await GetCompartmentSelectList();

        return View();
    }

    public async Task<IActionResult> Disassociate()
    {
        ViewBag.UnavailableCompartments = await GetCompartmentSelectList(false);

        return View();
    }

    [HttpPost, ActionName("Index")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(
        [Bind("UserId,CompartmentId")] AssignCompartmentFormData formData
    )
    {
        var compartmentsContext = _context.Compartment;

        if (compartmentsContext == null)
        {
            return Problem("Entity set 'ProvaContext.Compartment'  is null.");
        }

        if (formData.CompartmentId > 0 && formData.UserId > 0)
        {
            var compartment = await compartmentsContext.FindAsync(formData.CompartmentId);

            if (compartment == null)
            {
                return NotFound();
            }

            compartment.UserId = formData.UserId;

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

            ViewBag.Users = await GetUserSelectList();

            ViewBag.AvailableCompartments = await GetCompartmentSelectList();

            ViewBag.Success = "Compartimento associado com sucesso.";

            return View();
        }

        // Error message

        if (formData.CompartmentId == 0)
        {
            ModelState.AddModelError("CompartmentId", "Selecione um compartimento disponível");
        }

        if (formData.UserId == 0)
        {
            ModelState.AddModelError("UserId", "Selecione um usuário");
        }

        ViewBag.Users = await GetUserSelectList();

        ViewBag.AvailableCompartments = await GetCompartmentSelectList();

        return View(formData);
    }

    [HttpPost, ActionName("Disassociate")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Disassociate(
        [Bind("CompartmentId")] DisassociateCompartmentFormData formData
    )
    {
        var compartmentsContext = _context.Compartment;

        if (compartmentsContext == null)
        {
            return Problem("Entity set 'ProvaContext.Compartment'  is null.");
        }

        if (formData.CompartmentId > 0)
        {
            var compartment = await compartmentsContext.FindAsync(formData.CompartmentId);

            if (compartment == null)
            {
                return NotFound();
            }

            compartment.UserId = null;

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

            ViewBag.UnavailableCompartments = await GetCompartmentSelectList(false);

            ViewBag.Success = "Compartimento desassociado com sucesso.";

            return View();
        }

        if (formData.CompartmentId == 0)
        {
            ModelState.AddModelError("CompartmentId", "Selecione um compartimento disponível");
        }

        ViewBag.UnavailableCompartments = await GetCompartmentSelectList(false);

        return View(formData);
    }

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

    private bool CompartmentExists(int id)
    {
        return (_context.Compartment?.Any(e => e.Id == id)).GetValueOrDefault();
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

    private async Task<List<SelectListItem>> GetUserSelectList()
    {
        var usersContext = _context.User;

        var userSelectList = new List<SelectListItem>();

        if (usersContext == null)
        {
            return userSelectList;
        }

        var users = await usersContext.ToListAsync();

        foreach (var user in users)
        {
            userSelectList.Add(new SelectListItem { Value = user.Id.ToString(), Text = user.Name });
        }

        return userSelectList;
    }

    private async Task<List<SelectListItem>> GetCompartmentSelectList(bool available = true)
    {
        var compartmentsContext = _context.Compartment;

        var compartmentSelectList = new List<SelectListItem>();

        if (compartmentsContext == null)
        {
            return compartmentSelectList;
        }

        var compartments = new List<Compartment>();

        if (available)
        {
            compartments = await compartmentsContext.Where(c => c.UserId == null).ToListAsync();
        }
        else
        {
            compartments = await compartmentsContext.Where(c => c.UserId != null).ToListAsync();
        }

        foreach (var compartment in compartments)
        {
            var cabinet = await GetCabinet(compartment.CabinetId);

            if (cabinet != null)
            {
                compartment.Cabinet = cabinet;
            }

            if (available)
            {
                compartmentSelectList.Add(
                    new SelectListItem
                    {
                        Value = compartment.Id.ToString(),
                        Text = string.Format(
                            "Local: {0} - Número: {1} ({2}x{3}x{4}cm)",
                            compartment.Cabinet?.Name,
                            compartment.Id,
                            compartment.Width,
                            compartment.Height,
                            compartment.Depth
                        )
                    }
                );
            }
            else
            {
                var user = await GetUser(compartment.UserId);

                if (user != null)
                {
                    compartment.User = user;
                }

                compartmentSelectList.Add(
                    new SelectListItem
                    {
                        Value = compartment.Id.ToString(),
                        Text = string.Format(
                            "Local: {0} - Número: {1} ({2}x{3}x{4}cm) | Usado por: {5}",
                            compartment.Cabinet?.Name,
                            compartment.Id,
                            compartment.Width,
                            compartment.Height,
                            compartment.Depth,
                            compartment.User?.Name
                        )
                    }
                );
            }
        }

        return compartmentSelectList;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
