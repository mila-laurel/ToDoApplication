using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ToDoListLibrary;

namespace ToDoAspNetMvc.Controllers;

public class ToDoListsController : Controller
{
    private readonly ApplicationContext _context;

    public ToDoListsController(ApplicationContext context)
    {
        _context = context;
    }

    // GET: ToDoLists
    public async Task<IActionResult> Index()
    {
        return View(await _context.Lists
            .Include(l => l.EntryList)
            .ToListAsync());
    }

    // GET: ToDoLists/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var toDoList = await _context.Lists
            .Include(l => l.EntryList)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (toDoList == null)
        {
            return NotFound();
        }

        return View(toDoList);
    }

    // GET: ToDoLists/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ToDoLists/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Hide")] ToDoList toDoList)
    {
        if (ModelState.IsValid)
        {
            _context.Add(toDoList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(toDoList);
    }

    // GET: ToDoLists/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var toDoList = await _context.Lists.FindAsync(id);
        if (toDoList == null)
        {
            return NotFound();
        }
        return View(toDoList);
    }

    // POST: ToDoLists/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Hide")] ToDoList toDoList)
    {
        if (id != toDoList.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(toDoList);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoListExists(toDoList.Id))
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
        return View(toDoList);
    }

    // GET: ToDoLists/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var toDoList = await _context.Lists
            .FirstOrDefaultAsync(m => m.Id == id);
        if (toDoList == null)
        {
            return NotFound();
        }

        return View(toDoList);
    }

    // POST: ToDoLists/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var toDoList = await _context.Lists.FindAsync(id);
        _context.Lists.Remove(toDoList);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ToDoListExists(int id)
    {
        return _context.Lists.Any(e => e.Id == id);
    }
}
