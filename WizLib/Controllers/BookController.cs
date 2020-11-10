using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WizLib_DataAccess.Data;
using WizLib_Model.Models;
using WizLib_Model.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            List<Book> objList = _db.Books.Include(b => b.Publisher)
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .ToList();
            //List<Book> objList = _db.Books.ToList();
            //foreach (var obj in objList)
            //{
            //    //obj.Publisher = _db.Publishers.FirstOrDefault(p => p.Publisher_Id == obj.Publisher_Id);
            //    _db.Entry(obj).Reference(o => o.Publisher).Load();
            //    _db.Entry(obj).Collection(o => o.BookAuthors).Load();
            //    foreach (var bookAuthor in obj.BookAuthors)
            //    {
            //        _db.Entry(bookAuthor).Reference(ba => ba.Author).Load();
            //    }
            //}
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BookVM obj)
        {
                if (obj.Book.Book_Id == 0)
                {
                    _db.Books.Add(obj.Book);
                }
                else
                {
                    _db.Books.Update(obj.Book);
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            BookVM obj = new BookVM();
            
            if (id == null)
            {
                return View(obj);
            }
            obj.Book = _db.Books.Include(b => b.BookDetail).FirstOrDefault(b => b.Book_Id == id);
            //obj.Book.BookDetail = _db.BookDetails.FirstOrDefault(bd => bd.BookDetail_Id == obj.Book.BookDetail_Id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(BookVM obj)
        {
            if (obj.Book.BookDetail.BookDetail_Id == 0)
            {
                _db.BookDetails.Add(obj.Book.BookDetail);
                _db.SaveChanges();

                var BookFromDb = _db.Books.FirstOrDefault(b => b.Book_Id == obj.Book.Book_Id);
                BookFromDb.BookDetail_Id = obj.Book.BookDetail.BookDetail_Id;
                _db.SaveChanges();
            }
            else
            {
                _db.BookDetails.Update(obj.Book.BookDetail);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var book = _db.Books.FirstOrDefault(b => b.Book_Id == id);
            _db.Books.Remove(book);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ManageAuthors(int id)
        {
            BookAuthorVM obj = new BookAuthorVM
            {
                BookAuthorList = _db.BookAuthors.Include(ba => ba.Author).Include(ba => ba.Book)
                    .Where(ba => ba.Book_Id == id).ToList(),
                BookAuthor = new BookAuthor()
                {
                    Book_Id = id
                },
                Book = _db.Books.FirstOrDefault(b => b.Book_Id == id),
            };
            List<int> tempListOfAssignedAuthors = obj.BookAuthorList.Select(ba => ba.Author_Id).ToList();
            var tempList = _db.Authors.Where(a => !tempListOfAssignedAuthors.Contains(a.Author_Id)).ToList();

            obj.AuthorList = tempList.Select(i => new SelectListItem
            {
                Text = i.FullName,
                Value = i.Author_Id.ToString()
            });
            return View(obj);
        }

        [HttpPost]
        public IActionResult ManageAuthors(BookAuthorVM bookAuthorVM)
        {
            if (bookAuthorVM.BookAuthor.Book_Id != 0 && bookAuthorVM.BookAuthor.Author_Id != 0)
            {
                _db.BookAuthors.Add(bookAuthorVM.BookAuthor);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(ManageAuthors), new { @id = bookAuthorVM.BookAuthor.Book_Id });
        }

        [HttpPost]
        public IActionResult RemoveAuthors(int authorId, BookAuthorVM bookAuthorVM)
        {
            int bookId = bookAuthorVM.Book.Book_Id;
            BookAuthor bookAuthor = _db.BookAuthors.FirstOrDefault(
                ba => ba.Author_Id == authorId && ba.Book_Id == bookId);

            _db.BookAuthors.Remove(bookAuthor);
            _db.SaveChanges();

            return RedirectToAction(nameof(ManageAuthors), new { @id = bookId });
        }

        public IActionResult PlayGround()
        {
            var bookTemp = _db.Books.FirstOrDefault();
            bookTemp.Price = 100;

            var bookCollection = _db.Books;
            double totalPrice = 0;

            foreach (var book in bookCollection)
            {
                totalPrice += book.Price;
            }

            var bookList = _db.Books.ToList();
            foreach (var book in bookList)
            {
                totalPrice += book.Price;
            }

            var bookCollection2 = _db.Books;
            var bookCount1 = bookCollection2.Count();

            var bookCount2 = _db.Books.Count();

            IEnumerable<Book> BookList1 = _db.Books;
            var FilteredBook1 = BookList1.Where(b => b.Price > 500).ToList();

            IQueryable<Book> BookList2 = _db.Books;
            var fileredBook2 = BookList2.Where(b => b.Price > 500).ToList();


            //var category = _db.Categories.FirstOrDefault();
            //_db.Entry(category).State = EntityState.Modified;

            //_db.SaveChanges();


            //Updating Related Data
            //var bookTemp1 = _db.Books.Include(b => b.BookDetail).FirstOrDefault(b => b.Book_Id == 4);
            //bookTemp1.BookDetail.NumberOfChapters = 2222;
            //bookTemp1.Price = 12345;
            //_db.Books.Update(bookTemp1);
            //_db.SaveChanges();


            //var bookTemp2 = _db.Books.Include(b => b.BookDetail).FirstOrDefault(b => b.Book_Id == 4);
            //bookTemp2.BookDetail.Weight = 3333;
            //bookTemp2.Price = 123456;
            //_db.Books.Attach(bookTemp2);
            //_db.SaveChanges();


            //VIEWS
            //var viewList = _db.BookDetailsFromViews.ToList();
            //var viewList1 = _db.BookDetailsFromViews.FirstOrDefault();
            //var viewList2 = _db.BookDetailsFromViews.Where(u => u.Price > 500);

            //RAW SQL

            //var bookRaw = _db.Books.FromSqlRaw("Select * from dbo.books").ToList();

            //SQL Injection attack prone
            //int id = 2;
            //var bookTemp1 = _db.Books.FromSqlInterpolated($"Select * from dbo.books where Book_Id={id}").ToList();

            //var booksSproc = _db.Books.FromSqlInterpolated($" EXEC dbo.getAllBookDetails {id}").ToList();

            //.NET 5 only
            //var BookFilter1 = _db.Books.Include(e => e.BookAuthors.Where(p => p.Author_Id == 5)).ToList();
            //var BookFilter2 = _db.Books.Include(e => e.BookAuthors.OrderByDescending(p => p.Author_Id).Take(2)).ToList();


            //return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Index));
        }
    }
}
