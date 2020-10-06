using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using PagedList;
using Rollin.Add_Entity;


namespace Rollin.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private SuperShopMVCEntities1 db = new SuperShopMVCEntities1();
        // Insert Update Delete for Jason

        // GET: Customers
        [AllowAnonymous]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

           

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "CustomerName_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var Customer = from c in db.Customers
                           select c;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;


            if (!String.IsNullOrEmpty(searchString))
            {
                Customer = Customer.Where(s => s.CustomerName.ToUpper().Contains(searchString.ToUpper())
                ||
                s.City.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "CustomerName_desc":
                    Customer = Customer.OrderByDescending(s => s.CustomerName);
                    break;               
                default:
                    Customer = Customer.OrderBy(s => s.CustomerName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(Customer.ToPagedList(pageNumber, pageSize));
        }

        //public ActionResult IndexSingle(int? CustomerId, int? OrderId)
        //{
        //    var singleSelectList = new SelectedInfoModel();

        //    singleSelectList.Customers = db.Orders.Include(i => i.Customer).Select(s => s.Customer).ToList();

        //    if (CustomerId == null)
        //    {
        //        if (Session["CustomerId"] != null)
        //        {
        //            CustomerId = Convert.ToInt32(Session["CustomerId"].ToString());
        //        }
        //    }

        //    if (CustomerId != null)
        //    {

        //        Session["CustomerId"] = CustomerId;
        //        singleSelectList.Order = db.Orders.Where(s => s.CustomerID == CustomerId.Value ).ToList();


        //    }

        //    if (OrderId != null)
        //    {

        //        singleSelectList.Product = db.Orders.Include(i => i.Product).Where(w => w.OrderID == OrderId).Select(s => s.Product).ToList();

        //    }





        //    return View(singleSelectList);
        //}
        //--------------------------------------------------- Insert Multi Data Using Ajax

        [Authorize]
        public ActionResult DataInsert()
        {
            return View();
        }



        [HttpPost]
        [Authorize]
        public JsonResult DataInsert(string CustomerJason)
        {
            var js = new JavaScriptSerializer();

            Customer[] customer = js.Deserialize<Customer[]>(CustomerJason);

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Customers.AddRange(customer);
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    return Json("Data are inserted in Institute Table");
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return Json("There is a Probem arise");
                }


            }
        }








        // GET: Customers/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public ActionResult Create([Bind(Include = "CustomeID,CustomerName,City,Country,Phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        [Authorize]

        public ActionResult Edit(int? id)
        {

            if (!HttpContext.User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public ActionResult Edit([Bind(Include = "CustomeID,CustomerName,City,Country,Phone")] Customer customer)
        {

            if (!HttpContext.User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Home");
            }


            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        [Authorize]

        public ActionResult Delete(int? id)
        {

            if (!HttpContext.User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]

        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
       
}
