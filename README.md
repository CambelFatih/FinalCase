# Kullanıcı Roller ve İzinleri Açıklaması

Bu uygulama, kullanıcı rolleri ve izinlerini yönetmek için bir filtre kullanır. İlgili kod parçasını incelediğimizde, kullanıcıların kimlik doğrulama durumlarına ve rollerine göre belirli eylemleri kontrol eden `RolePermissionFilter` filtresini görebiliriz.

Bu filtre, kullanıcıların belirli roller ve izinlere sahip olup olmadığını kontrol eder. Özellikle, bu uygulama, ilk kayıt sırasında kullanıcı adını "Ataturk" olarak belirlemeniz gerektiğini ve bu kullanıcının admin olarak tüm erişimlere sahip olacağını belirtmektedir.

İlk kayıt sırasında, uygulama bu kullanıcı adını kontrol eder ve eğer kullanıcı adı "Ataturk" değilse, sadece belirli izinlere sahip bir kullanıcı olarak işlem görür.

Bu kısıtlama, uygulamanın güvenliğini artırmak ve sadece belirli roller ve izinlere sahip kullanıcıların özel eylemleri gerçekleştirmesini sağlamak amacıyla uygulanmıştır. Bu sayede, güvenlik standartlarına uygun olarak uygulamaya erişimi kontrol etmek mümkün olacaktır.

Lütfen unutmayın ki bu uygulama, kullanıcı rolleri ve izinleri üzerinde hassas bir kontrol sağlamaktadır ve bu kurallara uyum sağlamak için gerekli adımları atmanız önemlidir. Bu güvenlik önlemleri, uygulamanızın güvenliğini artırabilir ve yetkisiz erişimlere karşı koruma sağlayabilir.

---

## PostgreSQL Bağlantısı ve Migration

### PostgreSQL Bağlantısı

1. **PostgreSQL Kurulumu:** İlk olarak, bilgisayarınıza PostgreSQL'i indirip kurun. [Resmi PostgreSQL İndirme Sayfası](https://www.postgresql.org/download/) üzerinden işletim sistemine uygun olan sürümü seçip indirin ve kurulumu gerçekleştirin.

2. **Veritabanı Oluşturma:** PostgreSQL'e bir veritabanı oluşturun. Bu adım için genellikle PostgreSQL'in sunduğu araçları kullanabilir veya komut satırını kullanabilirsiniz.

3. **Bağlantı Dizesi Ayarı:** Uygulamanızın `appsettings.json` dosyasında PostgreSQL bağlantı dizesini ayarlayın. Aşağıda örnek bir bağlantı dizesi bulunmaktadır:

   ```json
   "ConnectionStrings": {
    "PostgreSQL": "User ID=postgres;Password=123;Host=localhost;Port=5432;Database=databasename;"
    },
    ```
  
## Migration Eklemek

Uygulamanızın kök dizininde, Code First yaklaşımını kullanarak veritabanınızı oluşturmak için aşağıdaki adımları takip edin:

### Package Manager Console Kullanarak Migration Eklemek:

Visual Studio'da `Tools -> NuGet Package Manager -> Package Manager Console`'yi açın ve aşağıdaki komutu kullanarak bir migration ekleyin:

```bash
Add-Migration InitialCreate
```
## Migration'ı Veritabanına Uygulamak

Uygulamanızın kök dizininde, Code First yaklaşımını kullanarak veritabanınızı oluşturmak için aşağıdaki adımları takip edin:

### Migration'ı Veritabanına Uygulamak

Ardından, aşağıdaki komutu kullanarak migration'ı veritabanına uygulayın:

```bash
Update-Database
```
