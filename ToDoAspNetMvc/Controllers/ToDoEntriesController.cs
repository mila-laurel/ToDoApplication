﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ToDoAspNetMvc.Models;
using ToDoListLibrary;

namespace ToDoAspNetMvc.Controllers;

public class ToDoEntriesController : Controller
{
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;

    public ToDoEntriesController(ApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: ToDoEntries
    public async Task<IActionResult> Index(string compare = null)
    {
        switch (compare)
        {
            case "equals":
                return View(await _context.Entities
            .Where(e => e.DueDate.Date == DateTime.Today)
            .ToListAsync());
            case "less":
                return View(await _context.Entities
            .Where(e => e.DueDate.Date < DateTime.Today)
            .ToListAsync());
            default:
                return View(await _context.Entities
            .ToListAsync());
        }
    }

    // GET: ToDoEntries/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var toDoEntry = await _context.Entities
            .Include(l => l.Fields)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (toDoEntry == null)
        {
            return NotFound();
        }

        return View(toDoEntry);
    }

    // GET: ToDoEntries/Create
    public IActionResult Create(int? owner)
    {
        ViewBag.Owner = owner;
        return View();
    }

    // POST: ToDoEntries/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("OwnerId,Id,Title,Description,DueDate,Completed,Fields")] ToDoEntryViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var toDoEntry = _mapper.Map<ToDoEntry>(vm);
            toDoEntry.CreatedOn = DateTime.Now;
            _context.Add(toDoEntry);
            foreach (var field in vm.Fields)
            {
                var entityField = _mapper.Map<CustomField>(field);
                entityField.ToDoEntryId = toDoEntry.Id;
                _context.Add(entityField);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), "ToDoLists", new { id = toDoEntry.OwnerId });
        }
        return View(vm);
    }

    // GET: ToDoEntries/Create
    public async Task<IActionResult> CreateCopy(int? initial)
    {
        if (initial == null)
        {
            return NotFound();
        }

        var toDoEntry = await _context.Entities.Include(l => l.Fields).FirstOrDefaultAsync(e => e.Id == initial);
        if (toDoEntry == null)
        {
            return NotFound();
        }
        var todolists = await _context.Lists.ToListAsync();
        var vm = _mapper.Map<ToDoEntryViewModel>(toDoEntry);
        vm.ToDoLists = todolists.Select(l => new SelectListItem { Text = l.Title, Value = l.Id.ToString() }).ToList();
        return View(vm);
    }

    // POST: ToDoEntries/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCopy([Bind("OwnerId,Title,Description,DueDate,Completed,Fields")] ToDoEntryViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var toDoEntry = _mapper.Map<ToDoEntry>(vm);
            toDoEntry.CreatedOn = DateTime.Now;
            _context.Add(toDoEntry);
            foreach (var field in vm.Fields)
            {
                var entityField = _mapper.Map<CustomField>(field);
                toDoEntry.Fields.Add(entityField);
                _context.Add(entityField);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), "ToDoLists", new { id = toDoEntry.OwnerId });
        }
        return View(vm);
    }

    // GET: ToDoEntries/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var toDoEntry = await _context.Entities.Include(l => l.Fields).FirstOrDefaultAsync(e => e.Id == id);
        if (toDoEntry == null)
        {
            return NotFound();
        }
        var vm = _mapper.Map<ToDoEntryViewModel>(toDoEntry);
        return View(vm);
    }

    // POST: ToDoEntries/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("OwnerId,Id,Title,Description,DueDate,Completed,Fields")] ToDoEntryViewModel vm)
    {
        var toDoEntry = await _context.Entities.Include(l => l.Fields).FirstOrDefaultAsync(e => e.Id == id);
        if (toDoEntry == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _mapper.Map(vm, toDoEntry);

                foreach (var modelField in vm.Fields)
                {
                    var existingField = toDoEntry.Fields.Where(f => f.Id == modelField.Id && f.Id != default)
                        .FirstOrDefault();
                    if (existingField != null)
                    {
                        _mapper.Map(modelField, existingField);
                    }
                    else
                    {
                        var newField = _mapper.Map<CustomField>(modelField);
                        newField.ToDoEntryId = id;
                        _context.Add(newField);
                    }
                }
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
            return RedirectToAction(nameof(Details), "ToDoLists", new { id = toDoEntry.OwnerId });
        }
        return View(vm);
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
        var ownerId = toDoEntry.OwnerId;
        _context.Entities.Remove(toDoEntry);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Details), "ToDoLists", new { id = ownerId });
    }

    public async Task<IActionResult> MarkCompleted(int id)
    {
        var toDoEntry = await _context.Entities.FindAsync(id);
        if (toDoEntry == null)
        {
            return NotFound();
        }
        var vm = new MarkCompletedViewModel { Id = toDoEntry.Id, Title = toDoEntry.Title, IsCompleted = toDoEntry.Completed == Status.Completed };
        return View(vm);
    }

    [HttpPost, ActionName("MarkCompleted")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkCompletedConfirmed(int id, [Bind("Id,IsCompleted")] MarkCompletedViewModel vm)
    {
        var toDoEntry = await _context.Entities.FindAsync(id);
        if (ModelState.IsValid)
        {
            toDoEntry.Completed = vm.IsCompleted ? Status.Completed : Status.InProgress; 
            await _context.SaveChangesAsync();
            if (toDoEntry == null)
            {
                return NotFound();
            }
        }

        return View(vm);
    }

    private bool ToDoEntryExists(int id)
    {
        return _context.Entities.Any(e => e.Id == id);
    }
}
