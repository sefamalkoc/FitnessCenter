# FitnessCenter - Spor Salonu ve Randevu Yönetim Sistemi

FitnessCenter, modern bir spor salonu yönetim ve randevu sistemidir. ASP.NET Core MVC mimarisi üzerine inşa edilen bu uygulama, spor salonu sahiplerinin tesislerini, eğitmenlerini ve servislerini yönetmelerini; üyelerin ise kolayca randevu almalarını sağlar.

## 🚀 Özellikler

- **Üyelik Sistemi**: 
  - Rol tabanlı yetkilendirme (Admin ve Üye).
  - Güvenli kayıt ve giriş ekranları.
- **Yönetim Paneli (Admin)**:
  - Spor salonu şubelerinin yönetimi.
  - Verilen hizmetlerin (Yoga, Crossfit vb.) yönetimi.
  - Eğitmenlerin uzmanlık alanlarına göre listelenmesi ve yönetimi.
- **Randevu Sistemi**:
  - Üyeler için servis ve eğitmen bazlı randevu oluşturma.
  - Randevu çakışma kontrolü (aynı saate ikinci randevu engellenir).
- **Yapay Zeka (AI) Entegrasyonu**:
  - OpenAI API kullanılarak üyelere özel spor ve beslenme önerileri.
- **Raporlama**:
  - Admin için randevu ve kullanıcı istatistiklerini içeren raporlar.
- **Responsive Tasarım**:
  - Bootstrap 5 ile tüm cihazlarda uyumlu arayüz.

## 🛠 Kullanılan Teknolojiler

- **Backend**: C# / .NET 8.0 (ASP.NET Core MVC)
- **Veritabanı**: Microsoft SQL Server
- **ORM**: Entity Framework Core
- **Kimlik Doğrulama**: Microsoft ASP.NET Core Identity
- **AI Servis**: OpenAI API
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap 5

## 📦 Kurulum

Projeyi yerel makinenizde çalıştırmak için aşağıdaki adımları izleyin:

1. **Projeyi Klonlayın**:
   ```bash
   git clone <repo-url>
   cd FitnessCenter
   ```

2. **Veritabanı Yapılandırması**:
   `appsettings.json` dosyasındaki `DefaultConnection` bağlantı dizesini kendi SQL Server ayarlarınıza göre güncelleyin.

3. **Veritabanını Oluşturun**:
   Terminalde veya Package Manager Console'da aşağıdaki komutu çalıştırarak tabloları oluşturun:
   ```bash
   dotnet ef database update
   ```

4. **Uygulamayı Çalıştırın**:
   ```bash
   dotnet run
   ```

## 🔑 Varsayılan Giriş Bilgileri (Seed Data)

Uygulama ilk çalıştığında otomatik olarak aşağıdaki yönetici hesabını oluşturur:

- **E-posta**: `b221210043@sakarya.edu.tr`
- **Şifre**: `sau`

## 📂 Proje Yapısı

- `Controllers/`: HTTP isteklerini yöneten ve mantığı koordine eden sınıflar.
- `Models/`: Veritabanı tablolarını ve ViewModelleri temsil eden sınıflar.
- `Views/`: Kullanıcı arayüzü (HTML/Razor) dosyaları.
- `Data/`: DbContext ve SeedData sınıfları.
- `Services/`: AI entegrasyonu gibi harici servis mantığı.
- `wwwroot/`: Statik dosyalar (CSS, JS, Resimler).

---
*Bu proje bir web programlama ödevi kapsamında geliştirilmiştir.*
