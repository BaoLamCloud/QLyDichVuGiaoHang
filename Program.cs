using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLy_dịch_vụ_giao_hàng
{

    class DonHang
    {
        public string MaDonHang { get; set; }
        public string NguoiGui { get; set; }
        public string SDTNguoiGui { get; set; }
        public string DiaChiNguoiGui { get; set; }
        public string NguoiNhan { get; set; }
        public string SDTNguoiNhan { get; set; }
        public string DiaChiNguoiNhan { get; set; }
        public string LoaiHang { get; set; }
        public string TrangThai { get; set; }
        public double KhoangCach { get; set; }
        public double KhoiLuong { get; set; }
        public string PhuongThucThanhToan { get; set; } // Thêm thuộc tính phương thức thanh toán


        public static string TaoMaDonHangNgauNhien()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; // Ký tự tạo mã tự động
            Random random = new Random();  //Random sinh mã ngẫu nhiên
            return new string(Enumerable.Repeat(chars, 5) // Độ dài mã đơn hàng là 5 ký tự
                .Select(s => s[random.Next(s.Length)]).ToArray()); //Chọn ngẫu nhiên từ tập ký tự
        }

        public DonHang(string maDonHang, string nguoiGui, string sdtNguoiGui, string diaChiNguoiGui,
                      string nguoiNhan, string sdtNguoiNhan, string diaChiNguoiNhan,
                      string loaiHang, double khoangCach, double khoiLuong)
        {
            MaDonHang = maDonHang;
            NguoiGui = nguoiGui;
            SDTNguoiGui = sdtNguoiGui;
            DiaChiNguoiGui = diaChiNguoiGui;
            NguoiNhan = nguoiNhan;
            SDTNguoiNhan = sdtNguoiNhan;
            DiaChiNguoiNhan = diaChiNguoiNhan;
            LoaiHang = loaiHang;
            KhoangCach = khoangCach;
            KhoiLuong = khoiLuong;
            TrangThai = "Đơn hàng mới";
            PhuongThucThanhToan = "Chưa thanh toán"; // Giá trị mặc định
        }

        public double TinhCuocVanChuyen()
        {
            double phiCoBan = 20000;
            double phiKhoangCach = KhoangCach * 500;
            double phiKhoiLuong = KhoiLuong * 5000;
            return phiCoBan + phiKhoangCach + phiKhoiLuong;
        }

        public void CapNhatTrangThai(string trangThaiMoi)
        {
            TrangThai = trangThaiMoi;
        }

        public void HienThiThongTin()
        {
            Console.WriteLine("=== THÔNG TIN ĐƠN HÀNG ===");
            Console.WriteLine("Mã đơn hàng: " + MaDonHang);
            Console.WriteLine("Người gửi: " + NguoiGui + " SĐT: " + SDTNguoiGui);
            Console.WriteLine("Địa chỉ người gửi: " + DiaChiNguoiGui);
            Console.WriteLine("Người nhận: " + NguoiNhan + " SĐT: " + SDTNguoiNhan);
            Console.WriteLine("Địa chỉ người nhận: " + DiaChiNguoiNhan);
            Console.WriteLine("Loại hàng: " + LoaiHang);
            Console.WriteLine("Khoảng cách: " + KhoangCach + " km");
            Console.WriteLine("Khối lượng: " + KhoiLuong + " kg");
            Console.WriteLine("Trạng thái: " + TrangThai);
            Console.WriteLine("Cước vận chuyển: " + TinhCuocVanChuyen() + " VNĐ");
            Console.WriteLine("Phương thức thanh toán: " + PhuongThucThanhToan);
        }
    }

    class DichVuGiaoHang
    {
        private static List<User> danhSachNguoiDung = new List<User>(); // Danh sách người dùng
        private static User currentUser = null; // Người dùng hiện tại (đăng nhập)
        private static List<DonHang> danhSachDonHang  = new List<DonHang>(); //Danh sách đơn hàng
        private static double tongDoanhThu = 0;  // Tổng doanh thu từ các đơn hàng hoàn thành

        static void DangKy()
        {
            Console.Write("Nhập tên người dùng: ");
            string username = Console.ReadLine();

            // Kiểm tra xem tên người dùng đã tồn tại chưa
            if (danhSachNguoiDung.Any(u => u.Username == username))
            {
                Console.WriteLine("Tên người dùng đã tồn tại! Vui lòng chọn tên khác.");
                return;
            }

            Console.Write("Nhập mật khẩu: ");
            string password = Console.ReadLine();

            Console.WriteLine("Chọn vai trò người dùng:");
            Console.WriteLine("1. Admin");
            Console.WriteLine("2. Shipper");
            Console.WriteLine("3. Người gửi");
            Console.WriteLine("4. Người nhận");
            Console.Write("Lựa chọn (1-4): ");
            string roleChoice = Console.ReadLine();
            string role;

            // Kiểm tra nếu chọn vai trò admin và đã có admin rồi
            if (roleChoice == "1" && danhSachNguoiDung.Any(u => u.Role == "admin"))
            {
                Console.WriteLine("Tài khoản Admin đã tồn tại! Chỉ được phép đăng ký một tài khoản Admin.");
                return;
            }

            switch (roleChoice)
            {
                case "1":
                    role = "admin";
                    break;
                case "2":
                    role = "shipper";
                    break;
                case "3":
                    role = "người gửi";
                    break;
                case "4":
                    role = "người nhận";
                    break;
                default:
                    role = "người gửi"; // Mặc định là người gửi nếu lựa chọn không hợp lệ
                    break;
            }

            User newUser = new User(username, password, role);
            danhSachNguoiDung.Add(newUser);

            Console.WriteLine("Đăng ký thành công!");
        }

        static void DangNhap()
        {
            Console.Write("Nhập tên người dùng: ");
            string username = Console.ReadLine(); // Nhập tên người dùng

            Console.Write("Nhập mật khẩu: ");
            string password = Console.ReadLine(); // Nhập mật khẩu

            // Tìm người dùng trong danh sách
            User user = danhSachNguoiDung.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                currentUser = user; // Gán người dùng hiện tại
                Console.WriteLine("Đăng nhập thành công với vai trò: " + user.Role);
            }
            else
            {
                Console.WriteLine("Tên người dùng hoặc mật khẩu không đúng!");
            }
        }

        static bool CoChuHoaKhongHopLe(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (i == 0 || (i > 0 && input[i - 1] == ' ')) continue; // Bỏ qua chữ cái đầu từ
                if (char.IsUpper(input[i])) return true; // Nếu có chữ hoa không ở đầu từ
            }
            return false; //// Không có chữ hoa không hợp lệ
        }



        static bool KiemTraKyTuDacBiet(string input)
        {
            if (input == input.ToUpper() && input.Any(char.IsLetter)) // Nếu toàn bộ là chữ in hoa
                return true;
            if (input.Contains("     ")) // Nếu có nhiều hơn 4 khoảng trắng liên tiếp
                return true;

            foreach (char c in input)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c)) // Nếu có ký tự không phải chữ cái hoặc khoảng trắng
                    return true;
            }
            return false; // Không có ký tự đặc biệt
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Console.WriteLine("CHƯƠNG TRÌNH QUẢN LÝ DỊCH VỤ GIAO HÀNG");
            Console.WriteLine("=======================================");

            while (true)
            {
                Console.Clear();
                HienThiMenu();
                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        Console.Clear();
                        DangKy();
                        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                        Console.ReadKey();
                        break;
                    case "2":
                        Console.Clear();
                        DangNhap();
                        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                        Console.ReadKey();
                        break;
                    case "3":
                        if (currentUser != null && (currentUser.Role == "admin" || currentUser.Role == "người gửi"))
                        {
                            Console.Clear();
                            ThemDonHangMoi();
                            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                            Console.ReadKey();
                        }
                        break;
                    case "4":
                        if (currentUser != null && (currentUser.Role == "admin" || currentUser.Role == "shipper"))
                        {
                            Console.Clear();
                            CapNhatTrangThaiDonHang();
                            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                            Console.ReadKey();
                        }
                        break;
                    case "5":
                        if (currentUser != null && currentUser.Role == "admin")
                        {
                            Console.Clear();
                            ThongKe();
                            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                            Console.ReadKey();
                        }
                        break;
                    case "6":
                        if (currentUser != null)
                        {
                            Console.Clear();
                            TimKiemDonHang();
                            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                            Console.ReadKey();
                        }
                        break;
                    case "7":
                        if (currentUser != null)
                        {
                            currentUser = null;
                            Console.WriteLine("Đăng xuất thành công!");
                            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                            Console.ReadKey();
                        }
                        break;
                    case "0":
                        Console.Clear();
                        Console.WriteLine("Cảm ơn bạn đã sử dụng chương trình!");
                        return;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ!");
                        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void TimKiemDonHang()
        {
            if (danhSachDonHang.Count == 0)
            {
                Console.WriteLine("Chưa có đơn hàng nào!");
                Console.WriteLine("\nNhấn 0 để quay lại hoặc bất kỳ phím nào để tiếp tục...");
                if (Console.ReadLine() == "0") return;
                return;
            }

            Console.Write("Nhập mã đơn hàng hoặc tên người nhận để tìm kiếm (0 để quay lại): ");
            string tuKhoa = Console.ReadLine().ToLower();
            if (tuKhoa == "0") return; // Quay lại menu chính

            var ketQua = danhSachDonHang.Where(dh => dh.MaDonHang.ToLower().Contains(tuKhoa) ||
                                                    dh.NguoiNhan.ToLower().Contains(tuKhoa)).ToList();

            if (ketQua.Count > 0)
            {
                Console.WriteLine("\nKết quả tìm kiếm:");
                foreach (var donHang in ketQua)
                {
                    donHang.HienThiThongTin();
                }
            }
            else
            {
                Console.WriteLine("Không tìm thấy đơn hàng nào phù hợp!");
            }
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
        }

        static void HienThiMenu()
        {
            Console.WriteLine("\n===== MENU =====");

            if (currentUser == null)
            {
                Console.WriteLine("1. Đăng ký");
                Console.WriteLine("2. Đăng nhập");
            }
            else
            {
                Console.WriteLine($"Xin chào, {currentUser.Username} ({currentUser.Role})");

                switch (currentUser.Role)
                {
                    case "admin":
                        Console.WriteLine("3. Tạo đơn hàng mới");
                        Console.WriteLine("4. Cập nhật trạng thái đơn hàng");
                        Console.WriteLine("5. Xem thống kê");
                        Console.WriteLine("6. Tìm kiếm đơn hàng");
                        break;
                    case "shipper":
                        Console.WriteLine("4. Cập nhật trạng thái đơn hàng");
                        Console.WriteLine("6. Tìm kiếm đơn hàng");
                        break;
                    case "người gửi":
                        Console.WriteLine("3. Tạo đơn hàng mới");
                        Console.WriteLine("6. Tìm kiếm đơn hàng");
                        break;
                    case "người nhận":
                        Console.WriteLine("6. Tìm kiếm đơn hàng");
                        break;
                }

                Console.WriteLine("7. Đăng xuất");
            }

            Console.WriteLine("0. Thoát");
            Console.Write("Chọn chức năng: ");
        }



        static void ThemDonHangMoi()
        {
            //Console.Write("\nNhập mã đơn hàng: ");
            //string maDonHang = Console.ReadLine();
            string maDonHang = DonHang.TaoMaDonHangNgauNhien();
            Console.WriteLine("Mã đơn hàng tự động tạo: " + maDonHang);

            string nguoiGui;
            do
            {
                Console.Write("Nhập tên người gửi: ");
                nguoiGui = Console.ReadLine();
                if (KiemTraKyTuDacBiet(nguoiGui))
                    Console.WriteLine("Tên không được chứa ký tự đặc biệt!");
                else if (CoChuHoaKhongHopLe(nguoiGui))
                    Console.WriteLine("Chữ cái in hoa chỉ được ở đầu từ!");
            }
            while (KiemTraKyTuDacBiet(nguoiGui) || CoChuHoaKhongHopLe(nguoiGui));

            string sdtNguoiGui;
            while (true)
            {
                Console.Write("Nhập số điện thoại người gửi (tối đa 10 số): ");
                sdtNguoiGui = Console.ReadLine();
                if (sdtNguoiGui.Length != 10 || !sdtNguoiGui.All(char.IsDigit))
                {
                    Console.WriteLine("Số điện thoại phải đủ 10 chữ số và chỉ chứa số! Vui lòng nhập lại.");
                    continue;
                }
                break;
            }

            Console.Write("Nhập địa chỉ người gửi: ");
            string diaChiNguoiGui = Console.ReadLine();

            string nguoiNhan;
            do
            {
                Console.Write("Nhập tên người nhận: ");
                nguoiNhan = Console.ReadLine();
                if (KiemTraKyTuDacBiet(nguoiNhan))
                    Console.WriteLine("Tên không được chứa ký tự đặc biệt!");
                else if (CoChuHoaKhongHopLe(nguoiNhan))
                    Console.WriteLine("Chữ cái in hoa chỉ được ở đầu từ!");
            } while (KiemTraKyTuDacBiet(nguoiNhan) || CoChuHoaKhongHopLe(nguoiNhan));

            string sdtNguoiNhan;
            while (true)
            {
                Console.Write("Nhập số điện thoại người nhận (tối đa 10 số): ");
                sdtNguoiNhan = Console.ReadLine();
                if (sdtNguoiNhan.Length != 10 || !sdtNguoiNhan.All(char.IsDigit))
                {
                    Console.WriteLine("Số điện thoại phải đủ 10 chữ số và chỉ chứa số! Vui lòng nhập lại.");
                    continue;
                }
                break;
            }

            Console.Write("Nhập địa chỉ người nhận: ");
            string diaChiNguoiNhan = Console.ReadLine();

            Console.Write("Nhập loại hàng hóa: ");
            string loaiHang = Console.ReadLine();
            while (KiemTraKyTuDacBiet(loaiHang))
            {
                Console.Write("Loại hàng không được chứa ký tự đặc biệt! Nhập lại: ");
                loaiHang = Console.ReadLine();
            }

            double khoangCach;
            while (true)
            {
                Console.Write("Nhập khoảng cách (km): ");
                if (double.TryParse(Console.ReadLine(), out khoangCach) && khoangCach > 0) break;
                Console.WriteLine("Khoảng cách phải là số hợp lệ và lớn hơn 0!");
            }

            double khoiLuong;
            while (true)
            {
                Console.Write("Nhập khối lượng (kg): ");
                if (double.TryParse(Console.ReadLine(), out khoiLuong) && khoiLuong > 0) break;
                Console.WriteLine("Khối lượng phải là số hợp lệ và lớn hơn 0!");
            }

            DonHang donHangMoi = new DonHang(maDonHang, nguoiGui, sdtNguoiGui, diaChiNguoiGui,
                                          nguoiNhan, sdtNguoiNhan, diaChiNguoiNhan,
                                          loaiHang, khoangCach, khoiLuong);
            danhSachDonHang.Add(donHangMoi);
            donHangMoi.HienThiThongTin();
            Console.WriteLine("Đơn hàng đã được tạo thành công!");
        }

        static void CapNhatTrangThaiDonHang()
        {
            if (danhSachDonHang.Count == 0)
            {
                Console.WriteLine("Chưa có đơn hàng nào!");
                return;
            }

            Console.Write("\nNhập mã đơn hàng cần cập nhật: ");
            string maDonHang = Console.ReadLine();
            DonHang donHang = danhSachDonHang.FirstOrDefault(dh => dh.MaDonHang == maDonHang);

            if (donHang != null)
            {
                Console.WriteLine("Chọn trạng thái mới:");
                Console.WriteLine("1. Đơn hàng mới");
                Console.WriteLine("2. Đang giao");
                Console.WriteLine("3. Đã giao thành công");
                Console.Write("Lựa chọn (1-3): ");
                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        donHang.CapNhatTrangThai("Đơn hàng mới");
                        donHang.PhuongThucThanhToan = "Chưa thanh toán"; // Reset khi quay lại trạng thái mới
                        break;
                    case "2":
                        donHang.CapNhatTrangThai("Đang giao");
                        donHang.PhuongThucThanhToan = "Chưa thanh toán"; // Reset khi đang giao
                        break;
                    case "3":
                        donHang.CapNhatTrangThai("Đã giao thành công");
                        tongDoanhThu += donHang.TinhCuocVanChuyen();

                        // Yêu cầu chọn phương thức thanh toán
                        Console.WriteLine("Chọn phương thức thanh toán:");
                        Console.WriteLine("1. Tiền mặt");
                        Console.WriteLine("2. Chuyển khoản");
                        Console.Write("Lựa chọn (1-2): ");
                        string luaChonThanhToan = Console.ReadLine();

                        switch (luaChonThanhToan)
                        {
                            case "1":
                                donHang.PhuongThucThanhToan = "Tiền mặt";
                                break;
                            case "2":
                                donHang.PhuongThucThanhToan = "Chuyển khoản";
                                break;
                            default:
                                Console.WriteLine("Lựa chọn không hợp lệ! Mặc định là Tiền mặt.");
                                donHang.PhuongThucThanhToan = "Tiền mặt";
                                break;
                        }
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ!");
                        return;
                }

                Console.WriteLine("Cập nhật trạng thái thành công!");
                donHang.HienThiThongTin();
            }
            else
            {
                Console.WriteLine("Không tìm thấy đơn hàng với mã: " + maDonHang);
            }
        }

        static void ThongKe()
        {
            int soDonHoanThanh = danhSachDonHang.Count(dh => dh.TrangThai == "Đã giao thành công");

            Console.WriteLine("\n=== THỐNG KÊ ===");
            Console.WriteLine("Tổng số đơn hàng: " + danhSachDonHang.Count);
            Console.WriteLine("Số đơn hàng hoàn thành: " + soDonHoanThanh);
            Console.WriteLine("Tổng doanh thu: " + tongDoanhThu + " VNĐ");
        }
    }
    class User
    {
        public string Username { get; set; } // Tên người dùng
        public string Password { get; set; } // Mật khẩu
        public string Role { get; set; }  //Vai trò (admin, shipper, người gửi, người nhận)

        public User(string username, string password, string role)
        {
            Username = username; // Gán tên người dùng
            Password = password; // Gán mật khẩu
            Role = role; // Gán vai trò
        }
    }
}