using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WizLib_DataAccess.Data;
using WizLib_Model.Models;

namespace WizLib.Controllers
{
    public class PublisherController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PublisherController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Publisher> publishersList = _db.Publishers.ToList();
            return View(publishersList);
        }

        public IActionResult Upsert(int? id)
        {
            Publisher publisher = new Publisher();
            if (id == null)
            {
                return View(publisher);
            }
            publisher = _db.Publishers.FirstOrDefault(p => p.Publisher_Id == id);
            if (publisher == null)
            {
                return NotFound();
            }
            return View(publisher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                if (publisher.Publisher_Id == 0)
                {
                    _db.Publishers.Add(publisher);
                }
                else
                {
                    _db.Publishers.Update(publisher);
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        public IActionResult Delete(int id)
        {
            var publisher = _db.Publishers.FirstOrDefault(p => p.Publisher_Id == id);
            _db.Publishers.Remove(publisher);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
