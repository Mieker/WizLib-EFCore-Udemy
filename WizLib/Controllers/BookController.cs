using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WizLib_DataAccess.Data;
using WizLib_Model.Models;
using WizLib_Model.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WizLib.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Book> objList = _db.Books.ToList();
            return View(objList);
        }

        public IActionResult Upsert(int? id)
        {
            BookVM obj = new BookVM();
            obj.PublisherList = _db.Publishers.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Publisher_Id.ToString()
            });
            if (id == null)
            {
                return View(obj);
            }
            obj.Book = _db.Books.FirstOrDefault(b => b.Book_Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Upsert(Author author)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (author.Author_Id == 0)
        //        {
        //            _db.Authors.Add(author);
        //        }
        //        else
        //        {
        //            _db.Authors.Update(author);
        //        }
        //        _db.SaveChanges();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(author);
        //}

        //public IActionResult Delete(int id)
        //{
        //    var author = _db.Authors.FirstOrDefault(a => a.Author_Id == id);
        //    _db.Authors.Remove(author);
        //    _db.SaveChanges();

        //    return RedirectToAction(nameof(Index));
        //}
    }
}
