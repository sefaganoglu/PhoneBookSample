# PhoneBookSample
Bu örnek bir telefon rehberi uygulamasıdır. Bu projede mikroservis mimarisi, .NET Core, PostgreSQL ve RabbitMQ kullanılmıştır.

# Nasıl Çalıştırılır
- Her bir mikroservise ait appsettings.json dosyalarının içerisinde bulunan ConnectionString bilgileri PostgreSQL Server ile çalışmaya uygun hale getirilir.
- ReportService'e ait appsettings.json içerisinde ek olarak "RabbitMQConfig:RabbitMQConnection" ve "ContactApiConfig:BaseUrl" bilgileri düzenlenir.
- "RabbitMQConfig:RabbitMQConnection" için örnek değer şu şekildedir (Port ve Ip değiştirilebilir): amqp://username:password@localhost:5672
- "ContactApiConfig:BaseUrl" için örnek değer şu şekildedir (Protokol, Port ve Ip değiştirilebilir): https://localhost:7196/api/

# Mikroservisler ve Endpointler
### ContactService ve Endpointleri
Bu servis, telefon rehberinde yeni kişi oluşturma, bu kişiyi çağırma, düzenleme, silme ve kişi listesini çekme işlemleri için kullanılır. Endpointler aşağıdaki gibidir.
- List: Telefon rehberindeki kişilerin listesini çekmek için kullanılır.
- Get: Telefon rehberindeki herhangi bir kişinin iletişim bilgileri de dahil bütün detaylarını çekmek için kullanılır.
- Post: Telefon rehberine yeni bir kişi eklemek için kullanılır.
- Put: Telefon rehberindeki bir kişinin iletişim bilgileri hariç diğer bilgilerini düzenlemek için kullanılır.
- Delete: Telefon rehberinden bir kişiyi silmek için kullanılır.

### ReportService ve Endpointleri
Bu servis, telefon rehberindeki kişilerin lokasyon bilgilerine göre gruplanmış halde kişi ve telefon no sayısını gösteren raporu CSV dosya formatında indirebilmek için kullanılır. İlk önce rapor isteği gönderilir. Bu istek veritabanına kaydolur, kuyruğa düşer (RabbitMQ) ve rapor hazırlanana kadar bekler. Rapor hazırlandığı zaman ise indirmeye hazır demektir.
- List: Rapor isteklerinin listesini çekmek için kullanılır.
- Post: Yeni bir rapor isteği oluşturmak için kullanılır. Bu çalıştırıldığında RabbitMQ kuyruğuna yeni bir istek gönderir.
- Download: Hazırlanan raporun indirilebilmesi için kullanılır.
