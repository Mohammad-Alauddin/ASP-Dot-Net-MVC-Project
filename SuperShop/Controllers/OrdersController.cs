using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Rollin.Add_Entity;

namespace Rollin.Controllers
{
    public class OrdersController : Controller
    {
        private SuperShopMVCEntities1 db = new SuperShopMVCEntities1();

        // GET: Orders
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "OrderNumber_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var Order = from o in db.Orders
                          select o;

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
                Order = Order.Where(s => s.OrderNumber.ToUpper().Contains(searchString.ToUpper())
                ||
                s.OrderNumber.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "OrderNumber_desc":
                    Order = Order.OrderByDescending(s => s.OrderNumber);
                    break;
                default:
                    Order = Order.OrderBy(s => s.OrderNumber);
                    break;
            }


            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(Order.ToPagedList(pageNumber, pageSize));


            //var orders = db.Orders.Include(o => o.Customer).Include(o => o.Product);
            //return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomeID", "CustomerName");
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,CustomerID,ProductID,OrderNumber,OrderDate,TotalAmount")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomeID", "CustomerName", order.CustomerID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", order.ProductID);
            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomeID", "CustomerName", order.CustomerID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", order.ProductID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public ActionResult Edit([Bind(Include = "OrderID,CustomerID,ProductID,OrderNumber,OrderDate,TotalAmount")] Order order)
        {

            if (!HttpContext.User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomeID", "CustomerName", order.CustomerID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", order.ProductID);
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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



        public ActionResult IndexSingle(int? CustomerId, int? OrderId)
        {
            var singleSelectList = new SelectedInfoModel();

            singleSelectList.Customers = db.Orders.Include(i => i.Customer).Select(s => s.Customer).ToList();

            if (CustomerId == null)
            {
                if (Session["CustomerId"] != null)
                {
                    CustomerId = Convert.ToInt32(Session["CustomerId"].ToString());
                }
            }

            if (CustomerId != null)
            {

                Session["CustomerId"] = CustomerId;
                singleSelectList.Order = db.Orders.Where(s => s.CustomerID == CustomerId.Value).ToList();


            }

            if (OrderId != null)
            {

                singleSelectList.Product = db.Orders.Include(i => i.Product).Where(w => w.OrderID == OrderId).Select(s => s.Product).ToList();

            }





            return View(singleSelectList);
        }
    }
}
