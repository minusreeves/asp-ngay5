using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using qlsanpham.Models;

namespace qlsanpham.Controllers
{
    public class CategoryController : Controller
    {
        // Giả lập csdl
       public static List<Category> categories = new List<Category> 
       { 
          new Category{CategoryId=1, Name="Laptop", Description="High Quality"},
          new Category{CategoryId=2, Name="Console", Description="On Discount"},
          new Category{CategoryId=3, Name="Mobile phone", Description="On Sale"}
       };
        public ActionResult Index(string search, string sortOrder)
        {
            var categoryList = categories;
                if (!String.IsNullOrEmpty(search)) { 
                    categoryList=categories.Where(s=>s.Name!=null&&s.Name.Contains(search,StringComparison.OrdinalIgnoreCase)).ToList();  //kểm tra từ khóa có null và tên có từ khóa timf liếm không nếu đúng thì ko phân biệt hoa thường chuyển kq thnhf ds
            }
                // sắp xếp 
            switch (sortOrder)
            {
                case "name_asc":
                    categoryList = categoryList.OrderBy(s => s.Name).ToList();
                    break;

                case "name_desc":
                    categoryList = categoryList.OrderByDescending(s => s.Name).ToList();
                    break;

                case "id_asc":
                    categoryList = categoryList.OrderBy(s => s.CategoryId).ToList();
                    break;

                case "id_desc":
                    categoryList = categoryList.OrderByDescending(s => s.CategoryId).ToList();
                    break;
            }
            ViewBag.Search = search;// ViewBag dung để truyền dữ liệu từ ctl sang view 
            ViewBag.SortOrder = sortOrder;//
            return View(categoryList);// trả dl sang gd
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            var detcategory = categories.FirstOrDefault(d => d.CategoryId == id);//Tìm phần tử đầu tiên thỏa mãn điều kiện.
            if (detcategory == null)
            {
                return NotFound();
            }
            return View(detcategory);
        }

        // GET: CategoryController/Create
        public ActionResult Create() 
        {

            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            category.CategoryId = categories.Count + 1;//động tạo ID mới tự động tăng 
            categories.Add(category);
            return RedirectToAction(nameof(Index));//Chuyển người dùng về trang Index.
        }
        public ActionResult Edit(int id)
        {
            var edcategory = categories.FirstOrDefault(e=>e.CategoryId==id);
            if (edcategory == null)
            {
                return NotFound();
            }
            return View(edcategory);//Tìm danh mục theo ID và hiển thị dữ liệu cũ lên form sửa.
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category ca)
        {
           var ed= categories.FirstOrDefault(e=>e.CategoryId==id);
            if (ed == null)
            {
                return NotFound();
            }
            ed.Name=ca.Name;//Lấy tên mới từ form và cập nhật vào đối tượng cũ.
            ed.Description=ca.Description;
            return RedirectToAction(nameof (Index));
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            var delcategory=categories.FirstOrDefault(de=>de.CategoryId==id);
            if (delcategory == null)
            {
                return NotFound();
            }
            return View(delcategory);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Category ca)
        {
           categories.RemoveAll(ca=>ca.CategoryId==id);//Xóa tất cả phần tử thỏa mãn điều kiện ID bằng ID cần xóa.
            return RedirectToAction(nameof(Index));//Chuyển về danh sách sau khi xóa
        }
    }
}
