using HealthStation_Test.Data;
using HealthStation_Test.Entities;
using HealthStation_Test.Services;
using HealthStation_Test.Shared;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HealthStation_Test.WEBAPI.Controllers
{
    public class LibraryController : ApiController
    {
        private static NLog.Logger LogHelper = LogManager.GetCurrentClassLogger();
        private readonly BooksService _BookService;
        private readonly BookedHistorieservice _BookedhistoryService;

        public LibraryController()
        {
            this._BookService = new BooksService();
            this._BookedhistoryService = new BookedHistorieservice();
        }
        /// <summary>
        /// Get all available books in the shlve
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Staff")]
        [HttpPost]
        [Route("api/lib/addbook")]
        public IHttpActionResult Addbook([FromBody] Book request)
        {
       
            if (ModelState.IsValid)
            {
                var log = string.Format("Request: {0}, Response: {1}",  SerializationServices.SerializeJson(request), BadRequest());
                LogHelper.Info(log);
                return BadRequest();
            }
            try
            {
                var log = string.Format("Request: {0}",  SerializationServices.SerializeJson(request));
                LogHelper.Info(log);
                var response = _BookService.SaveBook(request);
                log = string.Format("Response: {0}", SerializationServices.SerializeJson(response));
                LogHelper.Info(log);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.Debug(ex);
                return InternalServerError(ex);
            }


        }

        //[Authorize(Roles = "Administrator, Staff, User")]
        [HttpGet]
        [Route("api/lib/getbook")]
        public IHttpActionResult GetAllbook()
        {

            
            try
            {
                
                var response = _BookService.GetAllBooks();
               var log = string.Format("Response: {0}", SerializationServices.SerializeJson(response));
                LogHelper.Info(log);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.Debug(ex);
                return InternalServerError(ex);
            }


        }
        /// <summary>
        /// list of books with check-in/check-out. it can be searched with isbn, title, status
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Staff, User")]
        [HttpGet]
        [Route("api/lib/getbooked")]
        public IHttpActionResult GetAllbooked(string isbn, string title, bool status)
        {


            try
            {
                //var log = string.Format("Request: {0}", SerializationServices.SerializeJson(request));
                //LogHelper.Info(log);
                var response = _BookedhistoryService.GetAllBookHistory(isbn, title, status);
                var log = string.Format("Response: {0}", SerializationServices.SerializeJson(response));
                LogHelper.Info(log);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.Debug(ex);
                return InternalServerError(ex);
            }


        }
        /// <summary>
        /// add user's booked books to bookedhistory table. Here checkbox must be checked(status = true) 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Administrator, Staff, User")]
        [HttpPost]
        [Route("api/lib/addbookedbooks")]
        public IHttpActionResult booked( List<BookedHistory> request)
        {

            MyContext db = new MyContext();
            if (ModelState.IsValid)
            {
                var log = string.Format("Request: {0}, Response: {1}",  SerializationServices.SerializeJson(request), BadRequest());
                LogHelper.Info(log);
                return BadRequest();
            }
            try
            {
                //to check if the selected book still available
                var resutw = "No Available book for ID {0}";
                foreach (var reqw in request)
                {
                    var qty = db.Books.Where(x => x.ID == reqw.BookID).ToList().FirstOrDefault();
                    var cuntr = db.BookedHistories.Where(x => x.ID == reqw.BookID && x.Status == true).Count();
                    
                    int qtynum = qty.Quantity;
                    if(cuntr > qtynum)
                    {
                        return Ok(resutw);
                    }
                }
            }
            catch(Exception x)
            {
                LogHelper.Debug(x);
                throw x;
            }
           
            try
            {
                //add selected book to bookedhistory and reduce the quantity of selected book 
                foreach(var req in request)
                {
                    
                    _BookedhistoryService.SaveBookedHistory(req);
                    var qty = db.Books.Where(x => x.ID == req.BookID).ToList().FirstOrDefault();
                    var cuntr = db.BookedHistories.Where(x => x.ID == req.BookID && x.Status == true).Count();

                    int qtynum = qty.Quantity;
                    var updBook = qtynum - cuntr;

                    Book nm = new Book();
                    {
                        nm.Author_Name = qty.Author_Name;
                        nm.CategoryID = qty.CategoryID;
                        nm.ID = qty.ID;
                        nm.ISBN = qty.ISBN;
                        nm.Price = qty.Price;
                        nm.Quantity = updBook;
                        nm.Publisher_Date = qty.Publisher_Date;
                        nm.Title = qty.Title;
                    }


                    _BookService.UpdateBook(nm);



                    
                }
               
                
            }
            catch (Exception ex)
            {
                LogHelper.Debug(ex);
                return InternalServerError(ex);
            }

            return Ok("successfully added");
        }

        /// <summary>
        /// add user's returned books to bookedhistory table. Here checkbox must be unchecked(Status = false)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Administrator, Staff, User")]
        [HttpPost]
        [Route("api/lib/returnbook")]
        public IHttpActionResult Returnbook(List<BookedHistory> request)
        {

            MyContext db = new MyContext();
            if (ModelState.IsValid)
            {
                var log = string.Format("Request: {0}, Response: {1}", SerializationServices.SerializeJson(request), BadRequest());
                LogHelper.Info(log);
                return BadRequest();
            }
           
            try
            {
                //add unselected book to bookedhistory and add the quantity of unselected book 
                foreach (var req in request)
                {

                    _BookedhistoryService.UpdateBookedHistory(req);
                    var qty = db.Books.Where(x => x.ID == req.BookID).ToList().FirstOrDefault();
                    //var cuntr = db.BookedHistories.Where(x => x.ID == req.BookID && x.Status == false).Count();

                    int qtynum = qty.Quantity;
                    var updBook = qtynum + 1;

                    Book nm = new Book();
                    {
                        nm.Author_Name = qty.Author_Name;
                        nm.CategoryID = qty.CategoryID;
                        nm.ID = qty.ID;
                        nm.ISBN = qty.ISBN;
                        nm.Price = qty.Price;
                        nm.Quantity = updBook;
                        nm.Publisher_Date = qty.Publisher_Date;
                        nm.Title = qty.Title;
                    }


                    _BookService.UpdateBook(nm);




                }


            }
            catch (Exception ex)
            {
                LogHelper.Debug(ex);
                return InternalServerError(ex);
            }

            return Ok("successfully Returned");
        }
    }
}
