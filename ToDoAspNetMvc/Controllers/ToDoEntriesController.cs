using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ToDoListLibrary;

namespace ToDoAspNetMvc.Controllers
{
    public class ToDoEntriesController : Controller
    {
        private readonly ApplicationContext _context;

        public ToDoEntriesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: ToDoEntries
        public async Task<IActionResult> Index()
        {
            return View(await _context.Entities.ToListAsync());
        }

        // GET: ToDoEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoEntry = await _context.Entities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoEntry == null)
            {
                return NotFound();
            }

            return View(toDoEntry);
        }

        // GET: ToDoEntries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDoEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,DueDate,Completed")] ToDoEntry toDoEntry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toDoEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toDoEntry);
        }

        // GET: ToDoEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoEntry = await _context.Entities.FindAsync(id);
            if (toDoEntry == null)
            {
                return NotFound();
            }
            return View(toDoEntry);
        }

        // POST: ToDoEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DueDate,Completed")] ToDoEntry toDoEntry)
        {
            if (id != toDoEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDoEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoEntryExists(toDoEntry.Id))
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
            return View(toDoEntry);
        }

        // GET: ToDoEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoEntry = await _context.Entities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoEntry == null)
            {
                return NotFound();
            }

            return View(toDoEntry);
        }

        // POST: ToDoEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toDoEntry = await _context.Entities.FindAsync(id);
            _context.Entities.Remove(toDoEntry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoEntryExists(int id)
        {
            return _context.Entities.Any(e => e.Id == id);
        }
    }
}
