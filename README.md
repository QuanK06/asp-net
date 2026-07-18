# AnimeVietsub

Web xem Anime bằng ASP.NET Core MVC (.NET 8), xây dựng bám theo cấu trúc 17 Lab
"Thiết kế và lập trình Web với ASP.NET CORE MVC" (kênh Ngô Văn Linh), chuyển đổi
nghiệp vụ từ "web bán hàng" sang "web xem anime". Giao diện người dùng lấy cảm
hứng từ template **Vivid – Anime & Movies Streaming Entertainment Hub**.

## 0. Cấu trúc Solution (Lab 01)

Solution được tách làm 3 project riêng, đúng kiểu nhiều lớp (N-layer) như trong video:

```
AnimeVietsub.sln
├── AnimeVietsub.Core        -> chi chua Entity/Model (Anime, TapPhim, TheLoai...)
├── AnimeVietsub.Database    -> chi chua ApplicationDbContext, SeedData, Migrations
│                                  (tham chieu Core)
└── AnimeVietsub             -> project Web (MVC + Areas/Admin), tham chieu Database
                                   (va gian tiep tham chieu Core)
```

Khi chạy `Add-Migration`/`Update-Database` trong Package Manager Console, nhớ:
- **Default project**: chọn `AnimeVietsub.Database`
- **Startup project** (chuột phải solution → Set Startup Projects): chọn `AnimeVietsub` (project Web, vì nó giữ `appsettings.json` chứa connection string)

## 0.1 Giao diện người dùng (cập nhật)

Giao diện phần **User** (không phải Admin) hiện dùng lại nguyên bộ CSS/JS/font/ảnh của template **"Anime" (Colorlib)** — nằm ở `AnimeVietsub/wwwroot/template/`. Các trang đã chuyển sang layout này:
- Trang chủ (hero carousel + Trending/Popular/Recent/Đang chiếu + sidebar)
- Danh sách Anime theo thể loại (phân trang kiểu `product__pagination`)
- Chi tiết Anime (rating sao, theo dõi, bình luận, "có thể bạn thích")
- Trang xem tập phim (dùng **Plyr.js** có sẵn trong template làm video player, kèm 1 component React nhỏ chạy ngầm để tự lưu tiến độ xem mỗi 10 giây — đáp ứng yêu cầu Lab 17 dùng React nhưng không phá giao diện gốc)
- Đăng nhập / Đăng ký / Hồ sơ / Theo dõi / Tin tức

Khu vực **Admin** (`/Admin`) vẫn giữ giao diện dark AdminLTE-style cũ, không đổi.

## 0.2 Yêu cầu cho hosting khi deploy (Somee)
Thư mục `wwwroot/template` nặng khoảng 5.6MB (font, ảnh mẫu, thư viện JS) — nhớ tính vào giới hạn dung lượng gói Somee bạn dùng, cộng thêm dung lượng video thật sẽ upload qua trang Admin.

## 0.3 Giao diện Admin (cập nhật)

Khu vực `/Admin` hiện dùng nguyên bộ giao diện **AdminLTE 3.2.0** — nằm ở `AnimeVietsub/wwwroot/adminlte/` (đã lược bớt các file không dùng tới như source map, ảnh demo, ngôn ngữ Summernote khác để giảm dung lượng, chỉ giữ ~9MB). Toàn bộ trang Admin (Thống kê, Thể loại, Anime & Tập phim, Tin tức, Người dùng, Bình luận) đã viết lại theo đúng layout sidebar + navbar + card của AdminLTE, dùng DataTables bản đi kèm AdminLTE (Bootstrap 4) thay vì CDN Bootstrap 5 như trước.

## 0.4 Tính năng mới thêm (nhóm "Danh Mục" + nhãn VIP)

Đã thêm các trường/mục sau (theo yêu cầu mở rộng từ đồ án gốc):
- **Anime**: `DinhDangPhatSong` (TV Series/Movie/OVA/ONA/Special), `SoMua` (Season 1, 2, 3...), `PhuongThucDich` (Vietsub/Thuyết minh/Lồng tiếng) — chỉnh ở Admin → Anime & Tập phim → Thêm/Sửa Anime.
- **Lọc nâng cao** ở trang Danh sách Anime (site): lọc kết hợp Định dạng + Phương thức dịch + Trạng thái + Năm phát hành + Thể loại cùng lúc.
- **Nhãn VIP** cho tài khoản (`LaThanhVienVip`): gắn/bỏ tại Admin → Người dùng. **Không có cổng thanh toán thật** — chỉ là nhãn hiển thị, chưa giới hạn/mở khóa nội dung gì theo VIP (nếu muốn giới hạn nội dung theo VIP thật sự, cần làm thêm ở bước sau).

⚠️ **Vì có thêm cột mới trong bảng `Animes` và `AspNetUsers`, bạn phải chạy lại Migration:**
```powershell
Add-Migration ThemDanhMucVaVip -Project AnimeVietsub.Database -StartupProject AnimeVietsub
Update-Database -Project AnimeVietsub.Database -StartupProject AnimeVietsub
```
Nếu không chạy lại, app sẽ báo lỗi SQL vì cột chưa tồn tại trong CSDL thật (kể cả CSDL đã tạo trên Somee).

## 0.5 Nút Play trên poster + trang chi tiết kiểu AnimeVietsub

- **Nút Play trên poster**: xuất hiện ở card danh sách (trang chủ, danh sách anime, tìm kiếm...) và ở ảnh lớn trang chi tiết — bấm vào đưa thẳng tới tập đầu tiên (`/Anime/Xem`).
- **Trang chi tiết Anime**: thêm tab "Thông tin phim" (đủ Đạo diễn, Studio, Chất lượng, Rating, Thời lượng, Season, Số người theo dõi...) và tab "Trailer" (chỉ hiện khi Admin có nhập link YouTube embed). Sửa các trường này ở Admin → Anime & Tập phim → Thêm/Sửa.
- **Thanh điều khiển dưới Player**: Tập tiếp, Bình luận (cuộn xuống), Tắt đèn (làm tối màn hình để tập trung xem), Theo dõi, Phóng to (fullscreen video), Chụp ảnh (chụp khung hình hiện tại tải về máy), Tải về (tải file video), Lịch sử xem.

**Chưa làm** (cần thêm bảng dữ liệu riêng nếu muốn làm thật, không phải chỉnh sửa nhỏ): tab "Nhân vật" và "Hình ảnh" trong ảnh mẫu — vì cần bảng `NhanVat` (diễn viên lồng tiếng, ảnh) và bảng `ThuVienAnh` (album ảnh) hoàn toàn mới, báo mình nếu bạn muốn làm tiếp phần này.

⚠️ **Lại có thêm cột mới trong bảng `Animes`** (`DaoDien`, `Studio`, `ChatLuong`, `Rating`, `ThoiLuongPhut`, `TrailerUrl`) — nhớ chạy lại Migration:
```powershell
Add-Migration ThemThongTinPhimVaPlayer -Project AnimeVietsub.Database -StartupProject AnimeVietsub
Update-Database -Project AnimeVietsub.Database -StartupProject AnimeVietsub
```









## 0.6 Liên hệ / Góp ý / Donate / Danh hiệu người dùng

- **Liên hệ** (`/Home/LienHe`): email, số điện thoại, Facebook, TikTok — dùng Bootstrap Icons (đã thêm CDN `bootstrap-icons` vào layout).
- **Góp ý** (`/GopY`): form gửi góp ý (không cần đăng nhập), lưu vào bảng `GopYs`. Admin xem/đánh dấu xử lý/xóa tại Admin → Góp ý.
- **Donate** (`/Home/Donate`): trang ủng hộ dùng Facebook/TikTok/Email thật của bạn. Phần **chuyển khoản ngân hàng và ví Momo/ZaloPay mình để "Đang cập nhật"** vì bạn chưa cung cấp số tài khoản/số ví — tự thay nội dung trong `Views/Home/Donate.cshtml` khi có thông tin thật, đừng để "Đang cập nhật" khi public.
- **Danh hiệu người dùng**: đổi từ "Thành viên VIP" (bật/tắt) sang hệ **Danh hiệu** (`DanhHieu`, chọn 1 trong: *Fan cứng, Boss, Otaku, Mod*, hoặc để trống = thành viên thường) — gán tại Admin → Người dùng, hiện badge màu khác nhau ở trang Hồ sơ. Đây vẫn chỉ là nhãn hiển thị, **không cấp quyền thật** (vd gán "Mod" không có nghĩa họ được duyệt bình luận thật — nếu muốn "Mod" có quyền thật, cần gắn thêm Role riêng, báo mình nếu muốn làm).

⚠️ **Lại có thêm bảng/cột mới** (`GopYs`, và đổi cột `LaThanhVienVip` → `DanhHieu` trong `AspNetUsers`) — chạy lại migration:
```powershell
Add-Migration ThemGopYVaDanhHieu -Project AnimeVietsub.Database -StartupProject AnimeVietsub
Update-Database -Project AnimeVietsub.Database -StartupProject AnimeVietsub
```



## 0.7 Đổi upload video → nhập LINK video (do Somee free giới hạn dung lượng)

Vì gói Somee miễn phí không đủ dung lượng lưu file video thật, đã đổi cách quản lý Tập phim:
- Khi thêm/sửa Tập phim (Admin → Anime & Tập phim → Quản lý tập), **không còn ô upload file video** — thay bằng ô nhập **Link video**, kèm chọn **Loại nguồn**:
  - **"Link trực tiếp (.mp4)"**: dùng khi bạn có link file `.mp4` thật (vd link direct-download từ Google Drive/OneDrive, hoặc CDN video khác) — trang xem phim sẽ phát bằng thẻ `<video>`.
  - **"Nhúng (YouTube/Google Drive...)"**: dùng khi dán link dạng embed, vd `https://www.youtube.com/embed/xxxxx` — trang xem phim sẽ nhúng bằng `<iframe>`.
- Ảnh **Thumbnail vẫn upload lên server bình thường** (ảnh nhẹ, không lo dung lượng như video).
- Ở trang xem phim: nút **"Chụp ảnh"** và **"Tải về"** chỉ hiện khi tập dùng "Link trực tiếp" (vì với video nhúng iframe, trình duyệt chặn không cho chụp/tải vì lý do bảo mật cross-origin — đây là giới hạn kỹ thuật của trình duyệt, không phải lỗi code).

⚠️ **Đổi cấu trúc bảng `TapPhims`** (thêm cột `LoaiNguonVideo`) — chạy lại migration:
```powershell
Add-Migration DoiVideoThanhLink -Project AnimeVietsub.Database -StartupProject AnimeVietsub
Update-Database -Project AnimeVietsub.Database -StartupProject AnimeVietsub
```



- Visual Studio 2022 (hoặc VS Code + .NET SDK)
- .NET 8 SDK
- SQL Server (LocalDB / SQL Server Express / SQL Server đầy đủ)

## 0.8 Chỉ "Boss" mới vào được Admin + Quên mật khẩu qua Email

- **Phân quyền Admin đổi cách hoạt động**: trước đây dùng Role Identity (`[Authorize(Roles="Admin")]`), giờ đổi thành kiểm tra **danh hiệu `Boss`** (bộ lọc tự viết `AnimeVietsub/Filters/BossOnlyAttribute.cs`, gắn `[BossOnly]` lên tất cả controller trong `Areas/Admin`). Tài khoản Admin mặc định (`admin@animevietsub.local`) đã tự động được gán danh hiệu Boss khi chạy lần đầu (hoặc tự "backfill" nếu tài khoản đã có sẵn từ trước).
  - ⚠️ Vì Boss là *chìa khoá duy nhất* vào Admin, **cẩn thận đừng tự đổi danh hiệu của chính mình sang cái khác** ở Admin → Người dùng, nếu không bạn sẽ tự khoá mình ra khỏi trang Admin (lúc đó phải sửa tay trực tiếp trong database, cột `DanhHieu` bảng `AspNetUsers`).
  - Muốn thêm người khác làm Boss: vào Admin → Người dùng → chọn "Boss" ở dropdown danh hiệu của tài khoản đó.

- **Quên mật khẩu qua Email**: trang `/TaiKhoan/QuenMatKhau` → nhập email → hệ thống gửi link đặt lại mật khẩu qua email thật (SMTP) → bấm link vào `/TaiKhoan/DatLaiMatKhau` → nhập mật khẩu mới.
  - ⚠️ **Bắt buộc phải cấu hình SMTP thật** trong `appsettings.json`, mục `EmailSettings`, thì tính năng này mới gửi được email — hiện đang để giá trị mẫu (`diachi_gmail_cua_ban@gmail.com`...), **chưa gửi được** cho tới khi bạn điền đúng:
    ```json
    "EmailSettings": {
      "SmtpHost": "smtp.gmail.com",
      "SmtpPort": 587,
      "SmtpUser": "email_that_cua_ban@gmail.com",
      "SmtpPass": "app_password_16_ky_tu",
      "TenNguoiGui": "AnimeVietsub"
    }
    ```
    Nếu dùng Gmail: vào Google Account → Security → bật **2-Step Verification** → tạo **App Password** (mật khẩu ứng dụng riêng 16 ký tự, KHÔNG dùng mật khẩu Gmail thật) → dán vào `SmtpPass`.



## 1. Yêu cầu môi trường (Lab 01)

## 2. Chạy project ở local (Lab 01 - 02)

1. Mở `AnimeVietsub.sln` bằng Visual Studio.
2. Mở **Package Manager Console**. Ở thanh "Default project" chọn `AnimeVietsub.Database`, đảm bảo Startup Project là `AnimeVietsub`, rồi chạy:

   ```powershell
   Add-Migration InitialCreate -Project AnimeVietsub.Database -StartupProject AnimeVietsub
   Update-Database -Project AnimeVietsub.Database -StartupProject AnimeVietsub
   ```

3. Nhấn F5 để chạy. Lần chạy đầu tiên, `SeedData.cs` sẽ tự tạo:
   - Role `Admin`, `User`
   - Tài khoản Admin mặc định: **admin@animevietsub.local / Admin@123**
     ⚠️ Đổi mật khẩu này ngay sau khi đăng nhập lần đầu.
   - 6 thể loại mẫu

4. Vào trang quản trị tại: `/Admin` (sẽ được chuyển tới `/TaiKhoan/DangNhap` nếu chưa đăng nhập bằng tài khoản Admin).

## 3. Cấu trúc đã hoàn thành theo từng Lab

| Lab | Trạng thái | Vị trí code |
|---|---|---|
| 01 - Môi trường + CSDL | ✅ | `AnimeVietsub.Core/Models/`, `AnimeVietsub.Database/ApplicationDbContext.cs` |
| 02 - Kết nối SQL Server + Area Admin | ✅ | `AnimeVietsub/appsettings.json`, `AnimeVietsub/Areas/Admin` |
| 03 - Giao diện Admin/User | ✅ | `Views/Shared/_Layout.cshtml` (User, theme Vivid), `Areas/Admin/Views/Shared/_AdminLayout.cshtml` (Admin) |
| 04 - Ajax + JDatatable | ✅ | `Areas/Admin/Controllers/TheLoaiController.cs` (mẫu chuẩn), `AnimeQuanLyController.cs` |
| 05 - Ajax CRUD cơ bản | ✅ | như trên |
| 06 - Upload hình ảnh/video Ajax | ✅ | `AnimeQuanLyController.LuuFile()`, `QuanLyTap.cshtml` (progress bar upload video) |
| 07 - Đăng nhập | ✅ | `Controllers/TaiKhoanController.cs`, `Views/TaiKhoan/` |
| 08 - Phân quyền chức năng | ✅ | `[Authorize(Roles="Admin")]` trên các controller Admin, khóa/mở tài khoản |
| 09 - Migration + phân quyền | ✅ | `AnimeVietsub.Database/SeedData.cs` (tạo Role, seed) |
| 10 - Active menu + Chuyên mục | ✅ | `Controllers/TheLoaiController.cs` (site) |
| 11 - Quản lý sản phẩm → Anime + Tập phim | ✅ | `AnimeQuanLyController.cs` (CRUD Anime, CRUD Tập) |
| 12 - Tin tức + Summernote | ✅ | `Areas/Admin/Controllers/TinTucController.cs` |
| 13 - Đơn hàng → Theo dõi/Lịch sử xem | ✅ | `Controllers/TheoDoiController.cs` |
| 14 - Chart.js thống kê | ✅ | `Areas/Admin/Controllers/TrangChuController.cs` + `Views/TrangChu/Index.cshtml` |
| 15 - Partial View | ✅ | `Views/Anime/_DanhSachTap.cshtml`, `_BinhLuanList.cshtml` |
| 16 - Publish lên hosting | 📌 xem mục 4 bên dưới | |
| 17 - React JS giao diện | ✅ | `Views/Anime/Xem.cshtml` (Player dùng React qua CDN) |

## 4. Deploy lên Somee.com (Lab 16)

### Bước 1 - Tạo site & database trên Somee
1. Đăng ký/đăng nhập [somee.com](https://somee.com), tạo 1 **ASP.NET Website** mới (chọn hosting hỗ trợ ASP.NET Core nếu có, gói free của Somee chủ yếu hỗ trợ tốt ASP.NET Framework/MVC5 hơn; nếu gói bạn đang dùng không chạy được .NET 6, xem "Lưu ý quan trọng" bên dưới).
2. Tạo 1 **MSSQL Database** trong control panel Somee, ghi lại: server name, database name, username, password.
3. Vào mục **Connection strings** trong control panel Somee để lấy đúng chuỗi kết nối, dán vào `appsettings.json` ở khóa `DataContext_Somee`, rồi đổi `Program.cs` để dùng key này khi publish:

   ```csharp
   var connectionString = builder.Configuration.GetConnectionString("DataContext_Somee");
   ```

### Bước 2 - Publish
1. Trong Visual Studio: chuột phải vào project → **Publish** → chọn **Folder** (hoặc **Web Deploy** nếu Somee cấp thông tin Web Deploy).
2. Publish xong, upload toàn bộ nội dung thư mục publish lên Somee qua **FTP** (Somee cấp tài khoản FTP trong control panel).
3. Chạy migration để tạo bảng trên CSDL Somee — 2 cách:
   - Cách 1 (khuyên dùng khi mới học): trước khi publish, chạy `Update-Database` ở local nhưng trỏ connection string sang CSDL Somee (đổi tạm trong `appsettings.json`), sau đó đổi lại.
   - Cách 2: dùng lệnh `dotnet ef database update` từ máy local, connection string trỏ tới Somee.

### ⚠️ Lưu ý khi deploy web xem phim lên Somee (miễn phí)
- ~~Trước đây lưu file video thật trên server~~ → **đã đổi sang dùng link video** (mục 0.7), nên gói Somee free giờ chỉ cần chứa code + CSDL + ảnh poster/thumbnail (nhẹ), không còn áp lực dung lượng như lưu video thật nữa.
- Somee về bản chất host tốt cho ASP.NET (Framework) chạy trên IIS; với ASP.NET Core 8 cần server bật đúng module `AspNetCoreModuleV2` — nếu gói bạn dùng không hỗ trợ, hãy liên hệ hỗ trợ Somee hoặc cân nhắc hosting khác hỗ trợ .NET 8 tốt hơn (ví dụ Azure App Service free tier, Railway, Render).
- Nếu dùng link "Nhúng" YouTube, nhớ lấy đúng link dạng Embed (không phải link xem thường) để tránh lỗi không phát được video.

## 5. Những điểm khác biệt so với 17 Lab gốc (tổng hợp)

1. `Product` (bán hàng) → tách thành `Anime` (thông tin chung) + `TapPhim` (từng tập phim, quan hệ 1-nhiều) — bảng hoàn toàn mới.
2. Giỏ hàng/Đặt hàng (Lab 09/13 bản gốc) → đổi hẳn thành `TheoDoi` (theo dõi anime) + `LichSuXem` (xem tiếp tập dở), không có thanh toán.
3. Giao diện người dùng lấy theo template Vivid (dark theme, hero banner, poster grid) thay vì giao diện shop mặc định.
4. Có thêm Video Player (HTML5 `<video>` + React) — chức năng lõi không có ở bản gốc.
5. Lab 17 (React) chỉ áp dụng cho khu vực Player xem tập, phần còn lại vẫn giữ MVC + Ajax như tinh thần khóa học gốc.
