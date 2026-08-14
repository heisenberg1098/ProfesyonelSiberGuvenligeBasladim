# Profesyonel-Siber-Guvenlige-Basladim-Ilk_Projem

# Güvenlik Uygulaması

Bu projeyi C# ile geliştirdiğim basit bir dosya izleme ve güvenlik uygulaması olarak hazırladım.

Amacım önemli dosyalarımda izinsiz bir değişiklik olduğunda bunu fark edebilmek. Bunun için bilgisayarımda belirlediğim `.txt` dosyalarını arka planda takip ediyorum.

## Proje Nasıl Çalışıyor?

Uygulama belirlediğim klasörü sürekli izliyor.

Benim önemli dosyalarım şu klasörde:

`Onemli_Dosyalar`

Bu klasörün içerisinde şu anda:

* `Rapor_2026.txt`
* `Sifrelerim.txt`

dosyalarını kullanıyorum.

C# tarafında `FileSystemWatcher` kullandım. Böylece dosyaları sürekli kontrol etmek yerine Windows'tan gelen dosya değişikliği olaylarını dinliyorum.

Örneğin `Rapor_2026.txt` dosyasını açıp değiştirip kaydettiğimde uygulama bunu algılıyor.

## Firebase Kullanımı

Projenin kontrol kısmında Firebase Firestore kullanıyorum.

Firebase içerisinde ayrı bir `siber_izin` koleksiyonu oluşturdum.

Yapı şu şekilde:

```text
siber_izin
└── primary
    └── dosya_degisiklik_izni
```

`dosya_degisiklik_izni` değeri `true` veya `false` oluyor.

Ben bunu daha sonra kendi blog sitemdeki admin panelinden değiştireceğim.

Mantık basit:

```text
Dosya değişti
      ↓
C# değişikliği algıladı
      ↓
Firebase'deki izin kontrol edildi
      ↓
true  → Değişiklik normal
false → İzinsiz değişiklik olarak kabul et
```

İzin `false` olduğunda uygulama Firebase'e izinsiz giriş tarihini kaydediyor.

## Neden FileSystemWatcher Kullandım?

İlk başta dosyanın açıldığını doğrudan yakalamaya çalıştım. Fakat Windows'ta bir `.txt` dosyasının açılması ile dosyanın değiştirilmesi aynı şey değil.

Bu yüzden projeyi biraz değiştirdim.

Benim için önemli olan senaryo şu:

> Önemli dosyalarımdan biri değiştirilip kaydedilirse uygulama bunu algılasın.

Bu nedenle `FileSystemWatcher` ve `LastWrite` olayını kullandım.


## Şu Anki Durum

Şu anda uygulama:

* Belirlediğim klasörü izliyor.
* `.txt` dosyalarını takip ediyor.
* Dosya değişikliğini algılıyor.
* Değişen dosyanın adını konsola yazıyor.
* Firebase'den değişiklik iznini kontrol ediyor.
* İzin kapalıysa olayı Firebase'e kaydetmek için hazırlanmış durumda.

## Projenin Sonraki Aşaması

Bir sonraki aşamada bu sistemi kendi blog sitemin admin paneline bağlamak istiyorum.

Admin panelinde:

* Değişiklik Aç
* Değişiklik Kapat

şeklinde iki seçenek olacak.

Ben değişiklik yapacağım zaman izni açacağım. İşim bittikten sonra kapatacağım.

Eğer izin kapalıyken önemli dosyalardan biri değiştirilirse bunu şüpheli bir işlem olarak değerlendireceğim.

Bu projeyi özellikle hazır bir güvenlik sistemi kullanmak yerine C# ile kendim öğrenerek geliştirmeye çalışıyorum. Amacım sadece çalışan bir uygulama yapmak değil, kullandığım teknolojilerin ne yaptığını da anlamak.

