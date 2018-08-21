using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebShopSana.Models;

namespace WebShopSana.Controllers
{
    public class ProductsController : Controller
    {
        private WebShopSanaEntities db = new WebShopSanaEntities();
        List<Products> inMemoryProducts = new List<Products>();

        // GET: Products
        public ActionResult SetDataStorage(bool PersistentStorage)
        {

            Session["sessionDataStorage"] = PersistentStorage.ToString();           

            return RedirectToAction("Index");
        }

        // GET: Products
        public ActionResult Index()
        {   
            if ( string.IsNullOrEmpty( Session["sessionDataStorage"] as string) || Session["sessionDataStorage"].ToString().Equals("True"))
            {                
                return View(db.Products.ToList());
            }
            else
            {
                if (Session["sessionInMemoryProducts"] == null)
                {
                    return View(inMemoryProducts);
                }

                inMemoryProducts = (List<Products>)Session["sessionInMemoryProducts"];
                return View(inMemoryProducts);
            }            
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductNumber,Title,Price")] Products products)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Session["sessionDataStorage"] as string) || Session["sessionDataStorage"].ToString().Equals("True"))
                {
                    db.Products.Add(products);
                    db.SaveChanges();
                }
                else
                {
                    if (Session["sessionInMemoryProducts"] != null)
                    {
                        inMemoryProducts = (List<Products>)Session["sessionInMemoryProducts"];
                    }
                    inMemoryProducts.Add(products);
                    Session["sessionInMemoryProducts"] = inMemoryProducts;
                }

                
                return RedirectToAction("Index");
            }

            return View(products);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductNumber,Title,Price")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
