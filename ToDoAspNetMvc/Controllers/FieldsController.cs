using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDoListLibrary;

namespace ToDoAspNetMvc.Controllers
{
    public class FieldsController : Controller
    {
        private readonly ApplicationContext _context;

        public FieldsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Fields/Create
        public IActionResult Create(int? owner)
        {
            ViewBag.Owner = owner;
            return View();
        }

        // POST: Fields/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OwnerId,Id,Name,Value")] CustomField field)
        {
            if (ModelState.IsValid)
            {
                _context.Add(field);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "ToDoEntry", new { id = field.OwnerId });
            }
            return View(field);
        }
    }
}
