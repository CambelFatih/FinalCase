# FinalCase
Patika Final Case
### **1. Kullanıcı İşlemleri**

1. Sistem üzerinde 2 farklı rolde kullanıcı olabilir: Admin ve Bayi.
2. Bayi sadece sipariş oluşturabilir ve ürünleri görebilir. Ürünlerin stok durumlarını görebilir.
3. Bayi sipariş verirken x üründen farklı miktarlarda sipariş verebilir.
4. Tek sipariş de birden fazla ürün kalemi olabilir.
5. Bayiler sadece kendi siparişlerini görebilir ve listeleyebilir.
6. Bayiler ve sistem kullanıcısı 2 farklı rol ile sisteme login olur.
7. JWT token üzerinde rol bilgisi ile yetkilendirme sağlanabilir.
8. Bayi siparişlerini ilgili kriterlere göre filtreleyebilir.

### **2. Ödeme**

1. Bayiler ödeme sırasında sistem tarafından sağlanan ödeme yöntemlerinden birisini tercih edebilir.
2. Ödeme seçenekleri EFT, havale, kredi kartı, açık hesap olabilir.
3. Ödeme seçeneklerinden EFT havale seçildiğinde sipariş sırasında herhangi bir geliştirme ihtiyacı yoktur. Sipariş numarası oluştuktan sonra şirketin cari hesaplarına o sipariş numarası ve sipariş tutarı kadar bir ödeme gelinceye kadar sipariş ödeme bekliyor statüsünde kalır.
4. Şirketin cari hesaplarını belli aralıklarla kontrol edip ödeme bekleyen siparişler için ödemenin gelip gelmediğini kontrol eden bir schedule yazılabilir. (Opsiyonel)
5. Ödeme seçeneği kredi kartı olduğunda bir simülasyon ile girilen kart bilgilerinden ödeme tahsil edilir.
6. Açık hesap için her bayinin bir limiti vardır. Bayinin giriş yapmış olduğu sipariş tutarı kadar limiti varsa ödeme adımı başarılı gerçekleşir.
7. Açık hesap bakiyesi yani limiti yeterli değil ise ödeme onaylanmaz. Sipariş oluşmaz.
8. Şirkete ödeme yaptığı tutar kadar limiti yükselir ve tüm borç ödendiğinde bakiyesi sıfırlanmış olur.
9. Ödeme yöntemi sipariş modellerinde olmalıdır.


### **3. Raporlar**

1. Bayi siparişleri için bir rapor geliştirilmeli. Bayi kendi siparişlerini ve detaylarını görebilir.
2. Şirket günlük haftalık ve aylık raporlar ile sipariş yoğunluğu raporlanmalı.
3. Şirket Bayi bazlı günlük haftalık ve aylık sipariş yoğunluğunu raporlayabilmeli.
4. Rapor tarafında veri tabanı üzerinde view sp kullanımı yapılabilir.
5. Rapor için Dapper kullanımı yapılmalıdır.
6. Şirket ürün stok raporu ile envanter durumunu raporlayabilmelidir.
7. Stokları azalan ürünler için bir rapor hazırlanmalıdır. Min stok adeti 10 ise. (Stok miktarı min adet altına düşen ürünler için bildirim yapılabilir. SMS- Email. Opsiyonel)

### **4. Bayi İşlemleri**
1. Bayiler için tanımlanan modelde adres ve fatura bilgileri olmalıdır. Bir bayi sipariş girişi yaptığında sipariş beyan edilen o bilgiler ile oluşturulur.
2. Her bayi için bir kar marjı vardır ve bu admin tarafından bayi özelinde tanımlanır. Her bayi sistem tarafından tanımlanan fiyat üzerinden bu kar payı uygulanmış hali ile fiyatları görür. Örneğin şirketin A ürünü için tanımladığı fiyat 100 TRY iken K bayisi için marj %13 ise bayi kendi sipariş sisteminde ürün fiyatını 113 TRY olarak görecektir.
3. Bayilere göre bu kar payı değişken olup yapılan anlaşmalara bağlıdır.
4. KDV gibi vergiler hesaplamalara dahil edilebilir. (opsiyonel)
5. Bayi kendi siparişleri ve durumlarını görebilir. Raporlayabilir.


## **5. Sipariş İşlemleri**

1. Sipariş sırasında ürünlerin stok durumları kontrol edilir. Stok da yeterli adet ürün yoksa sipariş oluşmaz.
2. Sipariş sırasında ilgili ürünlerin stokları düşülerek stoklar güncellenir.
3. Sipariş ilk oluştuğunda şirketin onayına düşer.
4. Şirket siparişi onayladıktan sonra fatura numarası vb. bilgiler oluşur ve sipariş artık iptal edilemez.
5. Sipariş onaylanana kadar bayi tarafından iptal edilebilir.
6. Şirket sebep göstermeksizin siparişi onaylamadan iptal edebilir.
7. Stoklar sipariş sırasında azalırken Admin kullanıcılar yeni ürün tedarik durumunda stokları güncelleyebilir.


## **6. Mesaj**

1. Şirket ve bayi arasında iletişimin sağlanması ve haberleşme amaçlı bir mesajlaşma tasarımı yapılmalı ve Şirket ve bayiler arasında bire bir konuşma alt yapısı sağlanmalı.
2. Şirket tüm bayilere bilgilendirme duyuru veya mesaj gönderebilir.
3. Bayi sadece şirket yani admin ile iletişime geçip mesaj gönderebilir.
4. Bayiler kendi arasında haberleşemez ve birbirlerinden haberdar olamaz.


## **NOT**

1. Modeller tasarlanırken isimlendirme standartlarına dikkat edilmeli.
2. Modellerdeki alanlar sistemin taleplerini karşılayacak şekilde eksiksiz olmalı.
3. Talep edilen tüm işlem setlerini karşılayan API metodları geliştirilmeli ve dokümante edilmeli.
4. Talep edilen tüm işlem setleri bir API metodu olarak tasarlanmalı en basit hali ile test edilebilmelidir.
5. Modeller için API'ler üzerinde gerekli validasyonları ekleyip gerekli kontrolleri gerçekleştiriniz. Örneğin kar marjı sıfırdan küçük olamaz. Ürün fiyatı sıfır veya daha küçük olamaz. Her üründen min adet 1 olmak zorundadır vb.


## **İhtiyaçlar**

1. Yetkilendirme için JWT token altyapısı
2. Masraf API'leri, oluşturma, aktif masraf talepleri, geçmiş masraf talepleri API'leri
3. Tüm modeller için GET, GETBYID, POST, PUT, DELETE metodları hazırlanmalıdır.
4. Tüm modeller için Controller ve metodlar eksiksiz olmalıdır.
5. Proje iyi dokümante edilmelidir.
6. Angular template kurulumu
7. Modüllerin karşılığı olan ekleme, listeleme ve düzenleme sayfalarının oluşturulması.
8. Componentleri besleyecek olan data akışları için servisler yazılmalıdır.


## Tech Stack
- Veritabanı (PostgreSQL, MSSQL)
- JWT token (Yetkilendirme)
- EF-Repositroy - Unitofwork
- Postman veya Swagger
- RabbitMQ (Opsiyonel)

## **Teslim Kriterleri**

1. Proje kapsamında sadece Swagger üzerinde bu senaryonun uçtan uca çalışması beklenmektedir.
2. Postman veya herhangi bir API dokümantasyon aracı kullanarak sistem dokümante edilmelidir.
3. CodeFirst veya DbFirst yaklaşımlarından birisi ile ilerleyebilirsiniz.
4. Code first geliştirme yaptıysanız yeni bir veritabanı oluştuğunda migrationların çalıştığından emin olunuz.
5. Db first geliştirme yaptıysanız proje tesliminde veritabanı yedekleri ve scriptleri ekleyiniz.
6. Projenizin başarılı şekilde derlendiğinden emin olunuz.
7. Angular'da oluşturduğunuz component yapısındaki listeleme, düzenleme, ekleme ve silme akışlarının düzgün şekilde çalıştığından emin olunuz.
8. Angular kodunuzda build işlemini sorunsuz bir şekilde gerçekleştirin.
9. Yetkilendirme işlemlerinin düzgün çalıştığından emin olunuz.


## **Kriterler**

1. Değişken isimleri anlamlı, amaçlarına uygun verilmiştir.
2. Metot isimleri, metotların amaçlarını net bir şekilde ifade etmektedir.
3. Sınıfların içindeki metot sayısı azdır ve amaca yöneliktir.
4. İç içe geçmiş ifadeler bulunmamaktadır ve kod karmaşıklığı düşüktür.
5. Kod parçaları tekrarlanmamıştır.
6. Kod, okunması zorlaştıran karmaşık koşullar içermez.
7. Uzun metotlar (25 satırdan uzun olmamalıdır).
8. Hardcoded değerler const olarak isimlendirilmiş ve kullanılmıştır.
9. Kodlama standardı tüm kodlarda tutarlı bir şekilde uygulanmıştır.
10. Sınıfların içindeki metotlar tek bir sorumluluk alanına odaklıdır.
11. Obje arası bağımlılıklar enjekte edilmiştir.
12. Dependency Injection kullanılmıştır.
13. Sınıflar arası bağımlılıklar minimum seviyede tutulmasına dikkat edilmelidir.
    * İnterface gibi abstraction lar ihtiyaç olduğu için kullanılmış. Gereksiz abstraction eklenmemiş.
    * İnterface içeriği az ve tek sorumluluğa özgü metot imzası barındıracak şekilde tasarlanmış. Gereksiz design pattern kullanımı yapılmamış.
    * Gerçekten bir problem çözmek için kullanılmış.
    * Open-Closed Prensibine dikkat edilmiş. Kod genişlemeye açık, değişikliğe kapalı şeklinde tasarlanmış. Web/REST standartlarına dikkat edilmiş.
    * Rest Api de açık metot parametreler için defansif validation kodları yazılmış olmalıdır.
