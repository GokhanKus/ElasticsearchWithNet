# **ElasticSearch**
Bu proje, ElasticSearch kullanılarak farklı sorgu modellerinin ve CRUD işlemlerinin uygulandığı bir örnek projedir. Projede, ElasticSearch sorgularını öğrenmek, 
CRUD işlemlerini gerçekleştirmek ve küçük ölçekli örnek projeler geliştirmek için farklı branch'ler ve commit'ler üzerinden ilerleme kaydedilmiştir.
Proje boyunca, Elastic.Clients.Elasticsearch kütüphanesi kullanılmıştır.

##### Projeye geçmeden önce Docker-compose.yml dosyasında konfigurasyon ayarı ve docker compose up komutuyla docker container'ı ayağa kaldırma işlemi..

<p align="center">
  <img src="https://github.com/user-attachments/assets/32f3371b-5ef9-491b-8784-9d1c0d44d883" alt="Image 1" width="53%" style="margin-right: 10px;">
  <img src="https://github.com/user-attachments/assets/6183f629-4100-4bf4-a219-9d01970c348c" alt="Image 2" width="44%">
</p>

![image](https://github.com/user-attachments/assets/e614c512-7f81-467f-bef1-d9213807895a)

# Branchler ve Açıklamaları

## **1 - CRUD-Operations-Elastic.Client.Elasticsearch**

#### Açıklama:
- ElasticSearch üzerinde CRUD işlemleri (Create, Read, Update, Delete) gerçekleştirilmiştir.
- Elastic.Clients.Elasticsearch kütüphanesi kullanılarak temel CRUD işlemleri basit ve etkili bir şekilde uygulanmıştır.
- ![image](https://github.com/user-attachments/assets/af739fc9-9891-4055-9e75-0ca5e916dcfe)

#### İçerik:
- BaseController ile ortak kodlar minimize edilmiştir.
- CreateActionResult ile genel cevap yapısı oluşturulmuştur.
![image](https://github.com/user-attachments/assets/48a4ef28-097d-4d19-a20f-59fabb7309ad)
- Swagger üzerinden ElasticSearch'e CRUD işlemleri yapılmıştır.
![image](https://github.com/user-attachments/assets/d1ae9bbb-911b-4b87-82db-f8241d587ea1)

#### Kullanılan Örnekler:
- Ürün kayıtları oluşturulmuş (Product, ProductFeature modelleri).
- CRUD operasyonlarının sonuçları Swagger üzerinden ve Kibana UI üzerinden test edilmiştir.
BURAYA EKRAN GÖRÜNTÜSÜ GELECEKBURAYA EKRAN GÖRÜNTÜSÜ GELECEKBURAYA EKRAN GÖRÜNTÜSÜ GELECEKBURAYA EKRAN GÖRÜNTÜSÜ GELECEKBURAYA EKRAN GÖRÜNTÜSÜ GELECEKBURAYA EKRAN GÖRÜNTÜSÜ GELECEK

## **2 - Query-Operations-Elastic.Client.Elasticsearch**
#### Açıklama:
- Kibana UI tarafından hazır olarak alınan ECommerce.db üzerine sorgular yapılmıştır.
- ElasticSearch sorguları üzerine yoğunlaşılan branch'tir. Kibana üzerinde yazılan sorguların .NET tarafına taşınarak uygulanması sağlanmıştır.
  
### Kullanılan Sorgular:

![image](https://github.com/user-attachments/assets/befe4206-45a1-45f8-8d3d-791f85967a62)

#### TERM LEVEL QUERIES
- **TermQueryAsync:** Belirli bir alan için tam eşleşen belgeleri getirir.
- **TermsQueryAsync:** Birden fazla değere tam eşleşen belgeleri getirir.
- **PrefixQueryAsync:** Belirli bir metinle başlayan belgeleri sorgular
- **RangeQueryAsync:** Sayısal, tarih veya metinsel bir aralıkta yer alan belgeleri getirir.
- **MatchAllQueryAsync:** Tüm belgeleri sorgular ve getirir.
- **PaginationQueryAsync:** Belgeleri sayfalama mantığıyla (belirli bir aralıkta) getirir.
- **WildCardQueryAsync:** Belirli bir desenle eşleşen belgeleri sorgular (*, ? gibi joker karakterler kullanılarak).
- **FuzzyQueryAsync:** Yazım hatalarını tolere ederek, benzer kelimelerle eşleşen belgeleri getirir.

#### FULL-TEXT QUERIES
- **MatchQueryFullTextAsync:** Belirli bir kelime veya ifadeyle eşleşen belgeleri getirir.
- **MultiMatchQueryFullTextAsync:** Birden fazla alan üzerinde aynı kelime veya ifadeyi arar.
- **MatchBoolPrefixFullTextAsync:** Belirli bir kelime önekiyle başlayan belgeleri sorgular.
- **MatchPhraseQueryFullTextAsync:** Kelimelerin sırasını dikkate alarak tam bir ifadeyle eşleşen belgeleri getirir.
- **MatchPhrasePrefixQueryFullTextAsync:** Bir ifadenin başlangıcıyla eşleşen belgeleri getirir
- **MatchPhrasePrefixQueryByProductNameFullTextAsync:** Ürün ismi üzerinden belirli bir kelime önekiyle eşleşen belgeleri sorgular.
- **CompoundQueryExampleOneAsync:** Birden fazla sorguyu birleştirerek daha karmaşık sorgular oluşturur. (Must, MustNot, Should, Filter)

## **3 - ExampleApp (Blog_App)**
#### Açıklama:
- ElasticSearch kullanılarak basit bir MVC blog uygulaması geliştirilmiştir. Blog içerikleri sorgulanabilir, filtrelenebilir.
- SearchAsync gibi metotlarla dinamik aramalar yapılmıştır.
![image](https://github.com/user-attachments/assets/b47ef157-c6c3-47ad-bdec-60f064d114a5)
![image](https://github.com/user-attachments/assets/ebcf6e7f-c43a-4ecc-937e-d54bc57f1a43)
![image](https://github.com/user-attachments/assets/1d978d37-4ebb-4294-bd18-1ed8fb548108)
SearchAsync metodu in BlogRepository.cs
![image](https://github.com/user-attachments/assets/99191cb4-65ca-4dac-bd74-58c5cd79e1cc)

## **4 - ExampleApp (ECommerce_App)**
#### Açıklama:
- ElasticSearch kullanılarak basit bir e-ticaret uygulaması geliştirilmiştir.
- Ürünler hazır dbset olan Ecommerceden alınıp, Ürün sorgulama, sayfalama ve filtreleme işlemleri uygulanmıştır.
#### Proje Özellikleri:
##### Ürün Sorgulama:
- Ürünler birden fazla kritere göre sorgulanabilir.
- SearchAsync metodu, dinamik aramalar için özelleştirilmiştir.
##### Sayfalama (Pagination):
- Arama sonuçlarının kullanıcıya belirli sayfalarda gösterilmesi sağlanmıştır.
##### Filtreleme:
- Ürünler belirli kriterlere (örneğin fiyat, kategori) göre filtrelenebilir.
##### Örnek Kullanım:
- Belirli bir fiyat aralığında, belirli bir kategoriye ait ürünlerin listelenmesi.
- Sayfalama özelliği ile sonuçların daha kullanıcı dostu bir şekilde sunulması.

### Ekran Görüntüleri
![image](https://github.com/user-attachments/assets/af6f8e6a-3447-4ace-81a9-846c694414a7)

#### Score değerine göre filtreleme yapan örnek bir sorgu;
![image](https://github.com/user-attachments/assets/90bec930-cd00-487d-9194-c811bc2330d1)

### Sorgu Kriterleri;
- İsminde 'Eddie' veya soyisminde 'Underwood' geçecek
- Category alanı Men's Clothing olacak
- Gender Male olacak
- Order Datesi 01.01.2025 tarihiyle 20.01.2025 tarihi arasında olanları getirecek

