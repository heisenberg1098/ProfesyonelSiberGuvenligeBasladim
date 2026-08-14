using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;

var klasor = @"C:\Users\muham\OneDrive\Masaüstü\C# Projeleri\Guvenlik_Uygulamasi\Guvenlik_Uygulamasi\Onemli_Dosyalar";

var proje = "webproje-dd9f2";
var apiKey = "KENDİ APINIZI YAZIN BENİMKİ ÖZEL";

var url = $"https://firestore.googleapis.com/v1/projects/{proje}/databases/(default)/documents/siber_izin/primary?key={apiKey}";
using var http = new HttpClient();

using var watcher = new FileSystemWatcher(klasor);

watcher.Filter = "*.txt";
watcher.NotifyFilter = NotifyFilters.LastWrite;

watcher.Changed += async (sender, e) =>
{
    Console.WriteLine("Dosya değişikliği algılandı.");
    Console.WriteLine("Dosya: " + e.Name);

    try
    {
        var cevap = await http.GetAsync(url);
        var json = await cevap.Content.ReadAsStringAsync();

        if (!cevap.IsSuccessStatusCode)
        {
            Console.WriteLine("Firebase okuma hatası: " + cevap.StatusCode);
            return;
        }

        using var doc = JsonDocument.Parse(json);

        var izin = doc.RootElement
            .GetProperty("fields")
            .GetProperty("dosya_degisiklik_izni")
            .GetProperty("booleanValue")
            .GetBoolean();

        Console.WriteLine("Değişiklik izni: " + izin);

        if (izin)
        {
            Console.WriteLine("Değişiklik izni açık.");
            return;
        }

        var tarih = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        var veri = new
        {
            fields = new
            {
                izinsiz_giris_tarihi = new
                {
                    stringValue = tarih
                }
            }
        };

        var body = new StringContent(
            JsonSerializer.Serialize(veri),
            Encoding.UTF8,
            "application/json"
        );

        var sonuc = await http.PatchAsync(url, body);

        if (sonuc.IsSuccessStatusCode)
        {
            Console.WriteLine("İzinsiz giriş tarihi Firebase'e yazıldı.");
        }
        else
        {
            Console.WriteLine("Firebase yazma hatası: " + sonuc.StatusCode);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Hata: " + ex.Message);
    }
};

watcher.EnableRaisingEvents = true;

Console.WriteLine("Dosya izleme başladı.");
Console.WriteLine("Klasör: " + klasor);
Console.WriteLine("Program çalışıyor...");

Console.ReadLine();