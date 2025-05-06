Field Expense Manager API
Bu API, saha personelinin masraf yönetim süreçlerini dijitalleştirmek ve kolaylaştırmak amacıyla geliştirilmiştir. Personelin masraf girişlerini hızlandırır, yöneticilerin onay süreçlerini kolaylaştırır ve ödemeleri hızlandırır.



Ana Özellikler

Personel Rolü İçin:

•	Masraf Oluşturma: Kategori, tutar, tarih, açıklama ve opsiyonel olarak fiş/fatura ekleyerek yeni masraf girişi yapabilme (POST /api/Expenses).


•	Masrafları Listeleme: Sadece kendi girdiği masrafları ve durumlarını (Beklemede, Onaylandı, Reddedildi) listeleyebilme (GET /api/Expense/my).


•	Masraf Detayı Görme: Kendi girdiği bir masrafın tüm detaylarını (reddedildiyse sebebi dahil) görebilme (GET /api/Expenses/ExpenseById/{id}).


•	Masraf Geçmişi Raporu: Kendi masraf geçmişini (opsiyonel tarih filtresiyle) alabilme (GET /api/Reports/MyExpenseHistory).


Admin Rolü İçin:

•	Tüm Masrafları Listeleme: Sistemdeki tüm personelin masraflarını detaylarıyla listeleyebilme (GET /api/Expenses/GetAllPersonnel).

•	Masraf Detayı Görme: Herhangi bir masrafın tüm detaylarını (eki dahil) görebilme (GET /api/Expenses/ExpenseById/{id}).

•	Masraf Onaylama: Beklemede olan bir masrafı onaylayıp, (simüle edilmiş) ödeme sürecini tetikleyebilme ve ödeme kaydı oluşturabilme (PUT /api/Expenses/{id}/approve).

•	Masraf Reddetme: Beklemede olan bir masrafı sebep belirterek reddedebilme ve ödeme kaydını buna göre oluşturabilme (PUT /api/Expenses/{id}/reject).

•	Masraf Silme (Soft Delete): Bir masraf kaydını pasif hale getirebilme (DELETE /api/Expenses/{id}).

•	Kategori Yönetimi: Masraf kategorilerini ekleyebilme, listeleyebilme, güncelleyebilme ve silebilme (soft delete) (/api/ExpenseCategory CRUD).

•	Kullanıcı Yönetimi: Yeni personel kullanıcıları ekleyebilme, mevcut kullanıcıları listeleyebilme, güncelleyebilme ve pasif hale getirebilme (/api/Users CRUD).

•	Şirket Raporları:

o	Ödeme Yoğunluğu (Günlük/Haftalık/Aylık) (GET /api/Reports/PaymentDensity)

o	Personel Bazlı Harcama (Günlük/Haftalık/Aylık) (GET /api/Reports/PersonnelSpending)

o	Onay/Red Durumu (Günlük/Haftalık/Aylık) (GET /api/Reports/ApprovalStatus)

Kullanılan Teknolojiler

•	.NET 8 / ASP.NET Core Web API
•	Entity Framework Core 8 (Code First)
•	SQL Server
•	Dapper (Raporlama için)
•	CQRS (MediatR ile)
•	FluentValidation
•	AutoMapper
•	JWT (Yetkilendirme)
•	Swagger/OpenAPI

Kurulum ve Çalıştırma

1.	Gereksinimler: .NET 8 SDK, SQL Server.
2.	
3.	Yapılandırma: appsettings.Development.json dosyasındaki ConnectionStrings:DefaultConnection ve JwtSettings bölümlerini kendi ortamınıza göre düzenleyin.
4.	
5.	Veritabanı: API projesi dizininde dotnet ef database update --project FieldExpenseManager.Infrastructure --startup-project FieldExpenseManager.Api komutunu çalıştırın.
6.	
7.	Çalıştırma: API projesi dizininde dotnet run komutunu çalıştırın.
8.	
9.	API Erişimi: Uygulama çalışırken tarayıcıda /swagger adresine gidin.

API Kullanımı

Detaylı endpoint açıklamaları ve testler için Swagger arayüzünü (/swagger) kullanın. Genel akış: Login ol -> Token al -> Token'ı Swagger'da "Authorize" et -> Diğer endpoint'leri kullan.

Örnek Kullanıcılar:

•	Admin: burcu@gmail.com / AdminSifre123!

•	Personel: personel@gmail.com / PersonelSifre789.

•	Personel: personel@gmail.com / PersonelSifre7897.

 API Test ve Dokümantasyon
 
Bu proje, API'yi kolayca test etmeniz ve anlamanız için gerekli dokümanları ve bir Postman koleksiyonunu içermektedir. Bu dosyalara proje klonlandıktan sonra doğrudan proje klasörleri içinden erişebilirsiniz.

Postman Koleksiyonu:

Projenin `docs/postman/` klasörü altında `FieldExpenseManager.postman_collection.json ` adıyla yer almaktadır. 

    **Nasıl Kullanılır?:**
        1.  Postman uygulamasını açın.
        2.  `File` (Dosya) menüsünden `Import...` seçeneğine tıklayın.
        3.  Projenizdeki yukarıda belirtilen `.json` uzantılı koleksiyon (ve varsa ortam) dosyasını seçerek Postman'e yükleyin.
        4.  Ortam dosyası yüklediyseniz, Postman'de sağ üst köşeden ilgili ortamı seçmeyi unutmayın.

Diğer Dokümanlar

    * Proje ile ilgili ek açıklamalar, mimari bilgileri veya diğer yardımcı dokümanlar `docs/documentation` klasörü  altında bulabilirsiniz. Bu dosyalara doğrudan göz atabilirsiniz.
Bu dosyalar, API'yi daha hızlı anlamanıza ve testlerinizi kolayca yapmanıza yardımcı olmak için projeye dahil edilmiştir.

