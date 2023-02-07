using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using System.Linq;
using NuGet.Protocol;
using static NuGet.Packaging.PackagingConstants;

namespace TaskManager.Controllers
{
    public class TaskManagerController : Controller
    {

       
        private readonly ApplicationDBContext _context;

        public TaskManagerController(ApplicationDBContext context)
        {
           // _logger = logger;
            _context = context;
        }
        // GET: TaskManagerController
        public async Task<IActionResult> Index(string currentFilter, string SearchString, string sortOrder, int? pageNumber)
        {
            
            ViewData["CurrentFilter"] = SearchString;

            //sort order
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSort"] = String.IsNullOrEmpty(sortOrder) ? "name" : "name_Task";

            // ViewData["NameSort"] = sortOrder == "name" ? "default": "";

            if (SearchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                SearchString = currentFilter;
            }

            var items = from i in _context.ToDoItems
                        select i;
             
            if (!String.IsNullOrEmpty(SearchString))
            {
                items = items.Where(i => i.Name.Contains(SearchString)|| i.status.Contains(SearchString));
               
            }

            switch (sortOrder)
            {
                case "name":
                   items= items.OrderByDescending(i => i.Name);
                    break;
                case "name_Task":
                    items= items.OrderBy(i => i.Name);
                    break;
               

            }


            int pageSize = 5;

            return View(await PaginatedList<ToDoItem>.CreateAsync(items.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        public ActionResult CreateItem()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateItem(ToDoItem model)
        {

            if (ModelState.IsValid)
            {

                var item = new ToDoItem
                {

                    Name = model.Name,
                    createdOn = DateTime.Now,
                    updatedOn = DateTime.Now,
                    status = model.status,

                };
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(model);
        }
       
        public ActionResult EditItem([FromRoute] string ID)
        {
            var items = _context.ToDoItems.Where(i => i.ID == ID).FirstOrDefault();

            var editItems = new editTaskModel
            {
                ID = ID,
                Name= items.Name,
                updatedOn=DateTime.Now,
                status = items.status,
                

            };
            return View(editItems);
        }

        [HttpPost]
        public async Task<IActionResult> EditItem(editTaskModel model)
        {
            if (ModelState.IsValid)
            {
                var items = _context.ToDoItems.Where(i => i.ID == model.ID).FirstOrDefault();

                if (items == null)
                {
                    return NotFound();
                }

                items.Name = model.Name;
                items.updatedOn = model.updatedOn;
                items.status = model.status;

                _context.ToDoItems.Update(items);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View (model);
        }

        public ActionResult DeleteItem([FromRoute] string ID)
        {
            if (ID == null)
            {
                return NotFound();
            }
            var items = _context.ToDoItems.Where(i => i.ID ==ID).FirstOrDefault();

            if (items == null)
            {
                return NotFound();
            }
            return View(items);
        }
        [HttpPost, ActionName("DeleteItem")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmItem([FromRoute] string ID)
        {
            var item = _context.ToDoItems.Where(i => i.ID == ID).FirstOrDefault();

            _context.ToDoItems.Remove(item);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        // GET: TaskManagerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TaskManagerController/Create
        public ActionResult Create()
        {
            return View();
        }
        
        // POST: TaskManagerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TaskManagerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TaskManagerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TaskManagerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TaskManagerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
