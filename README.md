# Kütüphane Takip Sistemi - Full Stack Proje


✅ Tam Katmanlı Mimari:
Repository, Service, Controller, DTO ve Interface yapılarıyla kurumsal standartlarda katmanlı bir backend mimarisi.

✅ JWT Token ile Güvenli Kimlik Doğrulama:
Login sonrası alınan token ile güvenli API erişimi ve kullanıcı doğrulama süreci.

✅ Full Stack Yapı:
Hem backend hem de frontend tarafı senkronize ve profesyonelce yönetilen eksiksiz bir uygulama.

✅ React ile Modern Arayüz:
Sidebar navigasyon, dinamik sayfa geçişleri ve kullanıcı dostu arayüz.

✅ Admin Yönetimi:
Yönetici giriş sistemi ve admin bilgilerini token üzerinden alma.

✅ Kullanıcı yönetimi:
-Kard üretimi-okuma
-Kitap kiralama yönetimi
-Geciken kitaplardan dolayı ceza yazma sistemi


✅ Kullanıcı / Admin / Kitaplar / Yazar / Yayımcı / Kategori Modülleri:
CRUD işlemleriyle entegre tam yönetim sistemi.

✅ Kütüphane Raf Sistemi (Geliştirme Aşamasında):
Kullanıcıların kendi kitap raf yapılarını oluşturabilecekleri modüler sistem.

✅ DTO Kullanımı ile Temiz Veri Akışı:
Veri transferinde sade ve güvenli yapı.

✅ Unit Test Altyapısı:
Servis ve repository katmanlarında yazılmış örnek testlerle yazılım kalitesi güvence altına alınmış.

✅ MySQL Veritabanı Desteği:
XAMPP ile kolayca kurulup yönetilebilen, proje ile entegre veri sistemi.

✅ Yapılandırılmış ve Yorumlanmış Kod:
Her katmanda okunabilir, geliştirilebilir ve genişletilebilir kod dizaynı.

✅ Kolay Kurulum ve Geliştirici Dostu:
Kurulum adımları basit, yapı modüler ve yeni geliştiricilere açık.

![image](https://github.com/user-attachments/assets/1725fc9a-4e23-4cf6-bc6c-050bec8a5947)
*****
![image](https://github.com/user-attachments/assets/daf7c39e-ff35-4114-9308-917fedc058e4)
*****
![image](https://github.com/user-attachments/assets/2f1f563c-2b9e-4156-bb8f-c3cf38d48a4f)

![image](https://github.com/user-attachments/assets/fdc1e63f-0855-41e4-95e5-0c19882a6848)

![image](https://github.com/user-attachments/assets/353cb1e3-efe7-442c-a369-87ce0ac8d3eb)


## Proje Hakkında

Bu proje, **kullanıcı**, **yazar**, **yayımcı**, **kategori**, **kitap** ve **admin** yönetimini içeren tam katmanlı, JWT tabanlı kimlik doğrulama sistemine sahip bir **Kütüphane Takip Sistemi**dir.  
Ayrıca geliştirilmekte olan **raflık sistemi** ile kullanıcıların kitaplarını organize edebileceği bir yapı oluşturulmaktadır.

---

## Teknoloji ve Mimari

- Backend: **ASP.NET Core 8.0**
- Veri Tabanı: **MySQL (XAMPP Apache Server üzerinde)**
- Frontend: **React** (Side bar yapısı, login, kayıt, ekleme, güncelleme sayfaları dahil)
- Katmanlar:
  - **Repository Layer** (veri erişimi ve CRUD işlemleri)
  - **Service Layer** (iş mantığı ve servisler)
  - **Controller Layer** (API endpoint'leri)
  - **Interface Katmanları** (bağımlılıkların soyutlanması)
  - **DTO'lar** (Data Transfer Object'ler - veri taşımak için)
- Kimlik Doğrulama: **JWT Token Authentication**
- Testler: **Unit Testler** (backend servisler ve repository için)
- Full stack: Backend ile frontend tamamen entegre ve gerçek zamanlı çalışır.

---

## Özellikler

- **Kullanıcı Yönetimi:** Kayıt, giriş, token bazlı oturum.
- **Yazar/Yayımcı/Kategori Yönetimi:** CRUD işlemleri.
- **Admin Paneli:** Yetkili kullanıcıların yönetimi ve rol kontrolü.
- **Raflık Sistemi:** Kullanıcıya özel kitap rafı tasarlama ve yönetme (geliştirme aşamasında).
- **JWT ile Güvenlik:** API erişiminde güvenlik ve token kontrolü.
- **Frontend:** React tabanlı dinamik sayfalar, responsive side bar, giriş ve kayıt formları.
- **Unit Test:** Kod kalitesini artırmak için backend servislerinde kapsamlı testler.

---

## Kurulum

1. **Backend**

   - MySQL ve XAMPP Apache'yi kurup çalıştırın.
   - Projenin backend klasörüne gidin.
   - `appsettings.json` dosyasını veritabanı bağlantınıza göre güncelleyin.
   - Terminalde:
     ```
     dotnet restore
     dotnet build
     dotnet ef database update
     dotnet run
     ```
   - API `https://localhost:7195` üzerinde çalışacaktır.

2. **Frontend**

   - Frontend klasörüne gidin.
   - Kitapsin.Client yoluyla terminale girin
   - Node.js yüklü ise:
     ```
     npm install
     npm start
     ```
   - Uygulama `https://localhost:50860` adresinde açılacaktır.

---

## Kullanım

- İlk olarak kayıt olabilirsiniz (şu an AllowAnonymous).
- Giriş yaptıktan sonra token alır ve admin yetkileriyle API'ye erişirsiniz.
- Yazar, yayımcı, kategori ve kitap yönetimini gerçekleştirebilirsiniz.
- Raflık sisteminde kendi kitap rafınızı oluşturabilirsiniz (aktif geliştirme aşaması).
- Side bar ve sayfalar arası geçiş React Router ile yapılır.

---

## Proje Mimarisi

```text
Client (React) <--> API Controllers <--> Services <--> Repositories <--> MySQL DB
                     |                  |               |
                   DTOs               Interfaces     Entity Models
