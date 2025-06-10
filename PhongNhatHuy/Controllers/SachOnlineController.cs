using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PhongNhatHuy.SachOnline.Models;

namespace PhongNhatHuy.SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        private SachOnlineDbEntities data = new SachOnlineDbEntities();

        // GET: SachOnline
        public ActionResult Index()
        {
            var model = new SachIndexViewModel
            {
                SachMoi = LaySachMoi(6),
                NXBList = data.NHAXUATBAN.ToList()
            };
            return View(model);
        }

        // Action cho trang About
        public ActionResult About()
        {
            return View();
        }

        // Action cho trang Contact
        public ActionResult Contact()
        {
            return View();
        }

        // Action cho ChuDePartial
        public ActionResult ChuDePartial()
        {
            var listChuDe = from cd in data.CHUDE select cd;
            return PartialView(listChuDe);
        }

        // Action cho NhaXuatBanPartial
        public ActionResult NhaXuatBanPartial()
        {
            var listNhaXuatBan = from nxb in data.NHAXUATBAN select nxb;
            return PartialView(listNhaXuatBan);
        }

        // Action cho SachBanNhieuPartial
        public ActionResult SachBanNhieuPartial()
        {
            var listSachBanNhieu = LaySachBanNhieu(6);
            return PartialView(listSachBanNhieu);
        }

        // Action cho Html.Action() để render partial view
        public ActionResult SachTheoNXBPartial()
        {
            var nxbList = data.NHAXUATBAN.ToList();
            return PartialView(nxbList);
        }

        // Action xử lý logic khi người dùng click để xem sách theo NXB
        public ActionResult SachTheoNXB(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var lstSach = data.SACH.Where(s => s.MaNXB == id).ToList();
            return View(lstSach);
        }

        // Action cho SliderPartial
        public ActionResult SliderPartial()
        {
            return PartialView();
        }

        // SachTheoChuDe
        public ActionResult SachTheoChuDe(int id)
        {
            var kq = (from s in data.SACH where s.MaCD == id select s).ToList();
            return View(kq);
        }

        public ActionResult ChiTietSach(int id)
        {
            var sach = data.SACH.FirstOrDefault(s => s.MaSach == id);
            if (sach == null)
            {
                return HttpNotFound("Không tìm thấy sách với MaSach = " + id);
            }
            return View(sach);
        }

        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection collection)
        {
            var sHoTen = collection["HoTen"];
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];
            var sMatKhauNhapLai = collection["MatKhauNhapLai"];
            var sEmail = collection["Email"];
            var sDienThoai = collection["DienThoai"];
            var sNgaySinh = collection["NgaySinh"];
            var sDiaChi = collection["DiaChi"];

            if (string.IsNullOrEmpty(sHoTen))
            {
                ViewData["err1"] = "Họ tên không được rỗng";
            }
            else if (string.IsNullOrEmpty(sTenDN))
            {
                ViewData["err2"] = "Tên đăng nhập không được rỗng";
            }
            else if (string.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err3"] = "Phải nhập mật khẩu";
            }
            else if (string.IsNullOrEmpty(sMatKhauNhapLai))
            {
                ViewData["err4"] = "Phải nhập lại mật khẩu";
            }
            else if (sMatKhau != sMatKhauNhapLai)
            {
                ViewData["err4"] = "Mật khẩu nhập lại không khớp";
            }
            else if (string.IsNullOrEmpty(sEmail))
            {
                ViewData["err5"] = "Email không được rỗng";
            }
            else if (string.IsNullOrEmpty(sDienThoai))
            {
                ViewData["err6"] = "Số điện thoại không được rỗng";
            }
            else if (string.IsNullOrEmpty(sNgaySinh))
            {
                ViewData["err7"] = "Ngày sinh không được rỗng";
            }
            else
            {
                try
                {
                    DateTime ngaySinh;
                    if (!DateTime.TryParse(sNgaySinh, out ngaySinh))
                    {
                        ViewData["err7"] = "Định dạng ngày sinh không hợp lệ (dd/MM/yyyy hoặc MM/dd/yyyy)";
                        return View();
                    }

                    if (data.KHACHHANG.SingleOrDefault(n => n.TaiKhoan == sTenDN) != null)
                    {
                        ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
                    }
                    else if (data.KHACHHANG.SingleOrDefault(n => n.Email == sEmail) != null)
                    {
                        ViewBag.ThongBao = "Email đã được sử dụng";
                    }
                    else
                    {
                        KHACHHANG kh = new KHACHHANG
                        {
                            HoTen = sHoTen,
                            TaiKhoan = sTenDN,
                            MatKhau = sMatKhau,
                            Email = sEmail,
                            DiaChi = sDiaChi,
                            DienThoai = sDienThoai,
                            NgaySinh = ngaySinh
                        };

                        data.KHACHHANG.Add(kh);
                        data.SaveChanges();
                        return RedirectToAction("DangNhap");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ThongBao = "Đã xảy ra lỗi khi lưu dữ liệu: " + ex.Message;
                }
            }

            return View();
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];

            if (string.IsNullOrEmpty(sTenDN))
            {
                ViewData["err1"] = "Tên đăng nhập không được rỗng";
            }
            else if (string.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err2"] = "Mật khẩu không được rỗng";
            }
            else
            {
                try
                {
                    var khachHang = data.KHACHHANG.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == sMatKhau);
                    if (khachHang != null)
                    {
                        // Lưu thông tin đăng nhập vào Session
                        Session["TaiKhoan"] = khachHang;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ThongBao = "Đã xảy ra lỗi: " + ex.Message;
                }
            }

            return View();
        }
        /// <summary>
        /// LaySachMoi
        /// </summary>
        /// <param name="count">int</param>
        /// <returns>List</returns>
        private List<SACH> LaySachMoi(int count)
        {
            return data.SACH.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }

        /// <summary>
        /// LaySachBanNhieu
        /// </summary>
        /// <param name="count">int</param>
        /// <returns>List</returns>
        private List<SACH> LaySachBanNhieu(int count)
        {
            return data.SACH.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                data.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}