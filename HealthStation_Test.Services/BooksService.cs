using HealthStation_Test.Data;
using HealthStation_Test.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace HealthStation_Test.Services
{
    public class BooksService
    {
        private static BooksService _Instance;

        public static BooksService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new BooksService();
                }

                return (_Instance);
            }
        }

        public BooksService()
        {
        }


        public List<Book> GetAllBooks()
        {
            MyContext context = new MyContext();

            return context.Books.OrderBy(x => x.Publisher_Date).ToList();
        }

        public List<Book> SearchFeaturedBooks(int pageSize, List<int> excludeBookIDs = null)
        {
            excludeBookIDs = excludeBookIDs ?? new List<int>();

            MyContext context = new MyContext();

            return context.Books.Where(a => !excludeBookIDs.Contains(a.ID)).OrderByDescending(x => x.ID).Take(pageSize).ToList();
        }

        public List<Book> SearchBooks(List<int> categoryIDs, string searchTerm, decimal? from, decimal? to, string sortby, int? pageNo, int pageSize)
        {
            MyContext context = new MyContext();

            var Books = context.Books.AsQueryable();

            if (categoryIDs != null && categoryIDs.Count > 0)
            {
                Books = Books.Where(x => categoryIDs.Contains(x.CategoryID));
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                Books = Books.Where(x => x.Title.ToLower().Contains(searchTerm.ToLower()));
            }

            if (from.HasValue && from.Value > 0.0M)
            {
                Books = Books.Where(x => x.Price >= from.Value);
            }

            if (to.HasValue && to.Value > 0.0M)
            {
                Books = Books.Where(x => x.Price <= to.Value);
            }

            if (!string.IsNullOrEmpty(sortby) && string.Equals(sortby, "names", StringComparison.OrdinalIgnoreCase))
            {
                Books = Books.OrderBy(x => x.Title);
            }
            else //sortBy Book Date
            {
                Books = Books.OrderByDescending(x => x.ISBN);
            }

            pageNo = pageNo ?? 1;

            var skipCount = (pageNo.Value - 1) * pageSize;

            return Books.Skip(skipCount).Take(pageSize).ToList();
        }

        public int GetBookCount(List<int> categoryIDs, string searchTerm, decimal? from, decimal? to)
        {
            MyContext context = new MyContext();

            var Books = context.Books.AsQueryable();

            if (categoryIDs != null && categoryIDs.Count > 0)
            {
                Books = Books.Where(x => categoryIDs.Contains(x.CategoryID));
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                Books = Books.Where(x => x.Title.ToLower().Contains(searchTerm.ToLower()));
            }

            if (from.HasValue && from.Value > 0.0M)
            {
                Books = Books.Where(x => x.Price >= from.Value);
            }

            if (to.HasValue && to.Value > 0.0M)
            {
                Books = Books.Where(x => x.Price <= to.Value);
            }

            return Books.Count();
        }

        public Book GetBookByID(int ID)
        {
            MyContext context = new MyContext();

            return context.Books.Find(ID);
        }

        public List<Book> GetBooksByIDs(List<int> IDs)
        {
            MyContext context = new MyContext();

            return IDs.Select(id => context.Books.Find(id)).OrderBy(x => x.ID).ToList();
        }

        public IHttpActionResult SaveBook(Book Book)
        {
            MyContext context = new MyContext();
            try
            { 
            context.Books.Add(Book);

            context.SaveChanges();
            }
           catch (Exception ex)
            {
                //LogHelper.Log(ex);
                throw ex;
            }

            return null;
        }


        public void UpdateBook(Book Book)
        {
            MyContext context = new MyContext();

            var exitingBook = context.Books.Find(Book.ID);

            //context.BookPictures.RemoveRange(exitingBook.BookPictures);
            //context.BookSpecifications.RemoveRange(exitingBook.BookSpecifications);

            context.Entry(exitingBook).CurrentValues.SetValues(Book);

            //context.BookPictures.AddRange(Book.BookPictures);
            //context.BookSpecifications.AddRange(Book.BookSpecifications);

            context.SaveChanges();
        }

        public bool DeleteBook(int ID)
        {
            using (var context = new MyContext())
            {
                var Book = context.Books.Find(ID);

                context.Books.Remove(Book);

                return context.SaveChanges() > 0;
            }
        }
    }
}
