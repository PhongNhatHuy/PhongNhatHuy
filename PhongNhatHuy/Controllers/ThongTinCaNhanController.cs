using System.Web.Mvc;
using PhongNhatHuy.SachOnline.Models;
using System.Linq;

namespace PhongNhatHuy.SachOnline.Controllers
{
    public class ThongTinCaNhanController : Controller
    {
        private SachOnlineContext db = new SachOnlineContext();

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HoTen,Email,SoDienThoai,DiaChi,NganhHoc")] ThongTinCaNhan thongTinCaNhan)
        {
            if (ModelState.IsValid)
            {
                db.ThongTinCaNhans.Add(thongTinCaNhan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(thongTinCaNhan);
        }

        public ActionResult Index()
        {
            var thongTinCaNhans = db.ThongTinCaNhans.ToList();
            return View(thongTinCaNhans);
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