# Unity Project

Bu proje **Unity** oyun motoru kullanılarak geliştirilmiştir.

## 📋 Gereksinimler

- **Unity Hub**: [İndir](https://unity.com/download)
- **Unity Sürümü**: Proje, aşağıdaki paketlere dayanarak modern bir Unity sürümü gerektirir
  - Universal Render Pipeline (URP) 17.0.4
  - Input System 1.18.0
  - Cinemachine 3.1.6

## 🚀 Kurulum

1. **Unity Hub**'ı açın
2. **Add** butonuna tıklayın ve bu proje klasörünü seçin
3. Unity Editor'ü önerilen sürümde yükleyin veya zaten yüklüyse projeyi açın
4. İlk açılışta paketlerin indirilmesi birkaç dakika sürebilir

## 📦 Önemli Paketler

Proje şu Unity paketlerini kullanmaktadır:

| Paket | Sürüm | Açıklama |
|-------|-------|----------|
| URP | 17.0.4 | Universal Render Pipeline |
| Input System | 1.18.0 | Yeni girdi sistemi |
| Cinemachine | 3.1.6 | Akıllı kamera sistemi |
| AI Navigation | 2.0.12 | Yapay zeka navigasyon |
| TextMeshPro | - | Gelişmiş metin renderı |

## 🎮 Özellikler

- **Universal Render Pipeline (URP)**: Modern ve optimize edilmiş render pipeline
- **Yeni Input System**: Esnek girdi yönetimi
- **Cinemachine**: Profesyonel kamera kontrolleri
- **AI Navigation**: Gelişmiş yol bulma sistemi
- **Remote Config**: Uzaktan yapılandırma desteği

## 🛠️ Geliştirme

### IDE Önerileri

- **Visual Studio** (com.unity.ide.visualstudio: 2.0.27)
- **Rider** (com.unity.ide.rider: 3.0.40)

### Test

Proje Unity Test Framework (1.6.0) içerir. Testleri çalıştırmak için:
```
Window > General > Test Runner
```

## 📁 Proje Yapısı

```
/Assets           # Tüm oyun varlıkları (sahneler, scriptler, prefablar vb.)
/Packages         # Unity paket konfigürasyonu
/ProjectSettings  # Proje ayarları
/.github          # GitHub özel yapılandırmaları
```

## 🔧 Teknik Detaylar

- **Render Pipeline**: Universal Render Pipeline (URP)
- **Input System**: New Input System (aktif)
- **Platform Desteği**: Çoklu platform desteği (Windows, Linux toolchain dahil)

## 🤝 Katkıda Bulunma

1. Bu repoyu fork edin
2. Yeni bir branch oluşturun (`git checkout -b feature/yeniOzellik`)
3. Değişikliklerinizi commit edin (`git commit -m 'yeniOzellik eklendi'`)
4. Branch'inizi push edin (`git push origin feature/yeniOzellik`)
5. Pull Request oluşturun

## 📄 Lisans

Bu proje için lisans bilgisi belirtilmemiştir. Detaylar için proje sahibiyle iletişime geçin.

## 📞 İletişim

Sorularınız için lütfen proje sahibiyle iletişime geçin veya issue açın.

---

**Not**: Bu README dosyası otomatik olarak oluşturulmuştur. Proje detayları için `Packages/manifest.json` ve `ProjectSettings` klasörlerini inceleyebilirsiniz.
