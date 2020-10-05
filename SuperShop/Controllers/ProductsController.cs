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
    public class ProductsController : Controller
    {
        private SuperShopMVCEntities1 db = new SuperShopMVCEntities1();

        // GET: Products
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "ProductName_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var Product = from p in db.Products
                           select p;

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
                Product = Product.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                ||
                s.ProductName.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "ProductName_desc":
                    Product = Product.OrderByDescending(s => s.ProductName);
                    break;
                default:
                    Product = Product.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(Product.ToPagedList(pageNumber, pageSize));


            //var products = db.Products.Include(p => p.Category);
            //return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        [Authorize]
        public ActionResult Create()
        {

            if (!HttpContext.User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ProductID,CategoryID,ProductName,UnitPrice,imageUrl")] Product product, HttpPostedFileBase imageFileCreate)
        {
            if (!HttpContext.User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                imageFileCreate.SaveAs(Server.MapPath("~/imagePro") + "/" + imageFileCreate.FileName);
                string filePath = "~/imagePro/" + imageFileCreate.FileName;
                product.imageUrl = filePath;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Products/Edit/5
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
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ProductID,CategoryID,ProductName,UnitPrice,imageUrl")] Product product, HttpPostedFileBase imageFileCreate)
        {
            if (!HttpContext.User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (imageFileCreate.ContentLength > 0 && ModelState.IsValid)
            {

                imageFileCreate.SaveAs(Server.MapPath("~/imagePro") + "/" + imageFileCreate.FileName);
                string filePath = "~/imagePro/" + imageFileCreate.FileName;
                product.imageUrl = filePath;
                db.Products.Add(product);
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Products/Delete/5
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
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!HttpContext.User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Home");
            }

            Product product = db.Products.Find(id);
            System.IO.File.Delete(Server.MapPath(product.imageUrl));

            db.Products.Remove(product);
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
