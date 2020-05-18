using HealthStation_Test.Data;
using HealthStation_Test.Entities;
using HealthStation_Test.WEBAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace HealthStation_Test.Services
{
   public class BookedHistorieservice
    {
        #region Define as Singleton
        private static BookedHistorieservice _Instance;

        public static BookedHistorieservice Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new BookedHistorieservice();
                }

                return (_Instance);
            }
        }

        public BookedHistorieservice()
        {
        }
        #endregion
        public void SaveBookedHistory(BookedHistory BookedHistory)
        {
            MyContext context = new MyContext();

            context.BookedHistories.Add(BookedHistory);

            context.SaveChanges();
        }

        public void UpdateBookedHistory(BookedHistory Book)
        {
            MyContext context = new MyContext();

            var exitingBook = context.Books.Find(Book.ID);


            context.Entry(exitingBook).CurrentValues.SetValues(Book);

  
            context.SaveChanges();
        }

        public BookedHistory GetBookedHistoryByID(int ID)
        {
            MyContext context = new MyContext();

            return context.BookedHistories.Find(ID);
        }
        public List<BookedBooksMD> GetAllBookHistory( string isbn, string title, bool status)
        {
            MyContext context = new MyContext();

            //return context.BookedHistories.OrderByDescending(x =>x.Status == true).ToList();

            var result = context.Books.Select(x => new  BookedBooksMD{ 
                ISBN = x.ISBN,
                Publisher_Date = x.Publisher_Date,
                Price = x.Price,
                status = x.Bookedhistories.Where(u => u.Status).Select(y => y.Status).SingleOrDefault(),
            }).AsQueryable();

            if (status)
            {
                result = result.Where(x => x.status.Equals(status));
            }

            if (isbn != null)
            {
                result = result.Where(x => x.ISBN == isbn);
            }

            if (title != null)
            {
                result = result.Where(x => x.Title == title);
            }

            return result.ToList();
        }
    
        public int SearchBookedHistoriesCount(Int16 UserId, int? BookedHistoryID, Boolean BookedHistoriestatus)
        {
            MyContext context = new MyContext();

            var BookedHistories = context.BookedHistories.AsQueryable();

            if (BookedHistoryID.HasValue && BookedHistoryID.Value > 0)
            {
                BookedHistories = BookedHistories.Where(x => x.ID == BookedHistoryID.Value);
            }

            if (UserId != 0)
            {
                BookedHistories = BookedHistories.Where(x => x.UserID.Equals(UserId));
            }

            if (BookedHistoriestatus == true)
            {
                BookedHistories = BookedHistories.Where(x => x.Status.Equals(BookedHistoriestatus));
            }

            return BookedHistories.Count();
        }

        public string CheckifUserexceddate(DateTime dateretune, int userid)
        {
            MyContext context = new MyContext();

           // var BookedHistories = context.BookedHistories.AsQueryable();

           try
            { 
                var booklist = context.BookedHistories.Where(x => x.UserID == userid).FirstOrDefault();
            if(dateretune > booklist.ReturnDate)
            {
                return "Pay 500 NGN for late return";
            }
            else
            {
                return "Return";
            }

            }
            catch(Exception x)
            {
                throw x;
            }




            //return BookedHistories.Count();
        }
        public bool AddBookedHistory(BookedHistory BookedHistoryHistory)
        {
            MyContext context = new MyContext();

            context.BookedHistories.Add(BookedHistoryHistory);

            return context.SaveChanges() > 0;
        }
        public List<BookedHistory> GetUserBookedHistories(Int16 UserId, int? BookedHistoryID, int? BookedHistoriestatus, int? pageNo, int pageSize)
        {
            MyContext context = new MyContext();

            var BookedHistories = context.BookedHistories.Where(x => x.UserID.Equals(UserId));

            if (BookedHistoryID.HasValue)
            {
                BookedHistories = BookedHistories.Where(x => x.ID == BookedHistoryID.Value);
            }

            //if (BookedHistoriestatus.HasValue)
            //{
            //    BookedHistories = BookedHistories.Where(x => x.BookedHistories.BookedHistoryByDescending(y => y.ModifiedOn).FirstOrDefault().BookedHistoriestatus == BookedHistoriestatus);
            //}

            pageNo = pageNo ?? 1;

            var skipCount = (pageNo.Value - 1) * pageSize;

            return BookedHistories.OrderByDescending(x => x.Status == true).Skip(skipCount).Take(pageSize).ToList();
        }
    }
}
