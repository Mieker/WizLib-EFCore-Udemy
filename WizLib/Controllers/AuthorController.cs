using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WizLib_DataAccess.Data;
using WizLib_Model.Models;

namespace WizLib.Controllers
{
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AuthorController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Author> authorsList = _db.Authors.ToList();
            return View(authorsList);
        }

        public IActionResult Upsert(int? id)
        {
            Author author = new Author();
            if (id == null)
            {
                return View(author);
            }
            author = _db.Authors.FirstOrDefault(a => a.Author_Id == id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Author author)
        {
            if (ModelState.IsValid)
            {
                if (author.Author_Id == 0)
                {
                    _db.Authors.Add(author);
                }
                else
                {
                    _db.Authors.Update(author);
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        public IActionResult Delete(int id)
        {
            var author = _db.Authors.FirstOrDefault(a => a.Author_Id == id);
            _db.Authors.Remove(author);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
