using EventApp.Data;
using EventApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SQLitePCL;
using System.Linq;
using System.Threading.Tasks;

namespace EventApp.Controllers
{
    public class ToDoListsController : Controller
    {
        private readonly DataContext _context;

        public ToDoListsController(DataContext context)
        {
            _context = context;
        }

        /**
         * Tüm yapılacakları listeliyor
         */
        public async Task<IActionResult> Index()
        {
            return View(await _context.ToDoLists.OrderBy(x=>x.DateNotify).ToListAsync());
        }

        // GET: ToDoLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoList = await _context.ToDoLists
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

        /**
         * Burada kişi nextday/nextmonth/nextyear seçinekleri ile aynı eventi 1gün/1ay/1yıl şeklinde kaydedebilir.
         * Hatta gün-ay-yılı istediği süre bazında da artırabilir.
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ToDo,DateAdded,DateNotify,IsNotify,NextDay,NextMonth,NextYear")] ToDoList toDoList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toDoList);
                await _context.SaveChangesAsync();

                var todo = _context.ToDoLists.Find(toDoList.Id);

                if (todo.NextDay)
                {
                    _context.Add(todo.AddDay());
                    await _context.SaveChangesAsync();
                }
                if (todo.NextMonth)
                {
                    _context.Add(todo.AddMonth());
                    await _context.SaveChangesAsync();
                }
                if (todo.NextYear)
                {
                    _context.Add(todo.AddYear());
                    await _context.SaveChangesAsync();
                }
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

            var toDoList = await _context.ToDoLists.FindAsync(id);
            if (toDoList == null)
            {
                return NotFound();
            }
            return View(toDoList);
        }

        // POST: ToDoLists/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ToDo,DateAdded,DateNotify,IsNotify,NextDay,NextMonth,NextYear")] ToDoList toDoList)
        {
            if (id != toDoList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    toDoList.IsNotify = false;
                    toDoList.NextDay = false;
                    toDoList.NextMonth = false;
                    toDoList.NextYear = false;
                    _context.Update(toDoList);
                    await _context.SaveChangesAsync();
                    var todo = _context.ToDoLists.Find(toDoList.Id);
                    if (todo.NextDay)
                    {
                        _context.Add(todo.AddDay());
                        await _context.SaveChangesAsync();
                    }
                    if (todo.NextMonth)
                    {
                        _context.Add(todo.AddMonth());
                        await _context.SaveChangesAsync();
                    }
                    if (todo.NextYear)
                    {
                        _context.Add(todo.AddYear());
                        await _context.SaveChangesAsync();
                    }
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

            var toDoList = await _context.ToDoLists
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
            var toDoList = await _context.ToDoLists.FindAsync(id);
            _context.ToDoLists.Remove(toDoList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Seen(int id)
        {
            var seen= await _context.ToDoLists.FindAsync(id);
            seen.IsNotify = true;
            _context.Update(seen);          
            await  _context.SaveChangesAsync();
            return View();
        }

        private bool ToDoListExists(int id)
        {
            return _context.ToDoLists.Any(e => e.Id == id);
        }
    }
}
