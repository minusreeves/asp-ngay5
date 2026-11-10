using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using qlsanpham.Models;

namespace qlsanpham.Controllers
{
    // Giả lập CSDL
   
    public class ProductController : Controller
    {
        public static List<Product> products = new List<Product>
    {
        new Product{ProductId=1, Name="Nitro 5",Description="tốt",Price=5.6f, CategoryId=1},
        new Product{ProductId=2, Name="PS5",Description="tốt",Price=8.888f, CategoryId=2},
        new Product{ProductId=3, Name="ROG Phone 8",Description="tốt",Price=4.6f, CategoryId=3}
    };
        // GET: ProductController
        public ActionResult Index(string search, string sortOrder, int page = 1)
        {
            var data= from p in products
                join c in CategoryController.categories on p.CategoryId equals c.CategoryId
                select new ViewModel
                {
                    Id = p.ProductId,
                    Name = p.Name,
                    DateTime = c.CreatedDate,
                    Description=p.Description,
                    Price=p.Price,
                    CategoryName=c.Name,
                    CategoryId =c.CategoryId

                };

            var l=data.ToList();
            if(!String .IsNullOrEmpty(search))
            {
                l=l.Where(p=>p.Name!=null&&p.Name.Contains(search,StringComparison.OrdinalIgnoreCase)).ToList();
            }
            switch (sortOrder)
            {
                case "name_asc":
                    l = l.OrderBy(p => p.Name).ToList();
                    break;

                case "name_desc":
                    l = l.OrderByDescending(p => p.Name).ToList();
                    break;

                case "id_asc":
                    l = l.OrderBy(p => p.Id).ToList();
                    break;

                case "id_desc":
                    l = l.OrderByDescending(p => p.Id).ToList();
                    break;
            }
              int tongprd = l.Count;
            int pageSize = 1; // Mỗi trang hiển thị 1 sản phẩm
            int st = (int)Math.Ceiling((double)tongprd / pageSize);//// Tính tổng số trang
            page = st == 0 ? 1 : Math.Clamp(page, 1, st);//kt trang hiện tại có nằm trong khoảng
            l = l.Skip((page - 1) * pageSize).Take(pageSize) .ToList();// phân trang
            ViewBag.Search = search;
            ViewBag.SortOrder = sortOrder;
            ViewBag.Sotrang = st;
            ViewBag.CurrentPage = page;
            return View(l);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            var detProduct = products.FirstOrDefault(d => d.ProductId == id);
            if (detProduct == null)
            {
                return NotFound();
            }
            var detca=CategoryController.categories.FirstOrDefault(c=>c.CategoryId==id);
            if (detca == null)
            {
                return NotFound();
            }

            var vmd = new ViewModel
            {
                Id = detProduct.ProductId,
                Name = detProduct.Name,
                DateTime = detca.CreatedDate,
                Description = detProduct.Description,
                Price = detProduct.Price,
                CategoryName = detca.Name,
                CategoryId = detca.CategoryId
            };
            return View(vmd);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CategoryController.categories, "CategoryId", "Name");
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.ProductId = products.Count + 1;
                products.Add(product);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId=new SelectList(CategoryController.categories,"CategoryId","Name",product.CategoryId);
            return View(product);
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            var edproduct =products.FirstOrDefault(ed=>ed.ProductId==id);
            if (edproduct == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(CategoryController.categories, "CategoryId", "Name", edproduct.CategoryId);
            return View(edproduct);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,Product product)
        {
            var ed=products.FirstOrDefault(e=>e.ProductId==id);
            if (ed == null)
            {
                return NotFound ();
            }
           ed.Name= product.Name;
            ed.Description= product.Description;
            ed.Price= product.Price;
            ed.CategoryId = product.CategoryId;
            return RedirectToAction (nameof(Index));
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            var delproduct = products.FirstOrDefault(del=>del.ProductId==id);
            if (delproduct == null)
            {
                return NotFound();
            }
            var ca= CategoryController.categories.FirstOrDefault(d=>d.CategoryId==delproduct.CategoryId);
            ViewBag.Name = ca?.Name;
            return View(delproduct);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id,Product product)
        {
           products.RemoveAll(d=>d.ProductId==id);
            return RedirectToAction(nameof(Index));
        }
    }
}
