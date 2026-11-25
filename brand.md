# 🎨 Brand Identity & Design System (`brand.md`)

> **Kurum**: İstanbul Üniversitesi (Kuruluş 1453)  
> **Ders**: Web Programlama (Bilgisayar Bilimleri)  
> **Geliştirici & Sistem Mimarı**: Nazife Türk (`nazife.turk@ogr.iu.edu.tr`)  
> **Depo**: [https://github.com/NazifeTurk/alumni](https://github.com/NazifeTurk/alumni)  
> **Tasarım Felsefesi**: *Sade, Zarif, Modern ve Güvenilir Kurumsal Kimlik*  
> **Renk Dengesi**:  
> &bull; **Yoğunluklu Ana Zemin (Arka Plan)**: `#FAF9F6` (Alabaster Sıcak Fildişi Kağıt Dokusu)  
> &bull; **Butonlar ve İnteraktif Detaylar**: `#004225` (Derin İmparatorluk Orman Yeşili)  
> **Kurumsal Görseller**:  
> &bull; **Resmi Logo**: İstanbul Üniversitesi Vektörel Amblemi (`/assets/iu-logo.svg`)  
> &bull; **Arka Plan İmzası**: Tarihi Beyazıt Giriş Kapısı Silüeti Filigranı (`/assets/iu-gate-silhouette.svg`)  
> **İlgili Belgeler**: 📐 [AI Design System Specification (`DESIGN.MD`)](DESIGN.MD) &bull; ⬅️ [README.md](README.md) &bull; 🗄️ [ddd.md](ddd.md)

---

## 📌 1. Marka Özü & Tasarım Vizyonu

**İstanbul Üniversitesi Mezun Takip Sistemi (`alumni`)**, 1453 yılından bugüne uzanan 570 yılı aşkın köklü akademik mirası, 21. yüzyılın yalın, zarif ve modern arayüz estetiğiyle buluşturur.

```text
┌────────────────────────────────────────────────────────────────────────┐
│                        İSTANBUL ÜNİVERSİTESİ                           │
│                  Mezun Takip ve Kariyer Ağı Platformu                  │
│                    "Gelenekten Geleceğe Güçlü Köprü"                   │
└───────────────────────────────────┬────────────────────────────────────┘
                                    │
       ┌────────────────────────────┼────────────────────────────┐
       ▼                            ▼                            ▼
 🏛️ 1453 Köklü Miras         🌿 Sade, Zarif ve Modern    🏛️ Beyazıt Kapısı İmzası
 Akademik ciddiyet,          #FAF9F6 Yoğun Kağıt Zemin,  Arka planda zarif bir
 saygınlık ve kurumsal güven #004225 Butonlar ve Vurgular mimari silüet filigranı
```

### 1.1 Tasarımın Dört Temel İlkesi
1. **Sade ve Zarif (Minimal & Elegant)**:
   * Görsel kirlilikten, gereksiz süslemelerden, yorucu renk curcunasından ve yapay gölgelerden tamamen arındırılmıştır.
   * Arayüzün odağında yüksek okunabilirlik, dingin bir kurumsal vakar ve modern zarafet yer alır.
2. **Modern Dokunuş (Contemporary & Crisp)**:
   * Çağdaş dijital yayıncılık ve seçkin tasarım standartları (Stripe Press, Linear, Refero Private Banking) esas alınmıştır.
   * `9999px` pürüzsüz kapsül butonlar ile `4px` keskin bento kart geometrisinin kontrastı modern bir ritim sağlar.
3. **Alabaster Yoğunluğu (`#FAF9F6` - Ana Arka Plan)**:
   * Gözü yoran çiğ dijital beyazlar veya kasvetli koyu temalar yerine; kütüphane kağıtlarını ve tarihi arşiv dokusunu anımsatan sıcak, mat ve zarif bir alabaster fildişi tonu (`#FAF9F6`) sayfanın ezici çoğunluğunu ve zeminini kaplar.
4. **Derin İmparatorluk Yeşili Vurguları (`#004225` - Butonlar ve Detaylar)**:
   * İstanbul Üniversitesi'nin kurumsal olgunluğunu, saygınlığını ve sürekliliğini temsil eden asil orman yeşili (`#004225`); butonlarda, rozetlerde, aktif sekmelerde ve tipografik detaylarda güçlü bir kontrastla kullanılır.

---

## 🏛️ 2. Kurumsal Amblem & Tarihi Giriş Kapısı Silüeti

### 2.1 İstanbul Üniversitesi Resmi Logosu
Sistemin kurumsal kimlik mührü, 1453 kuruluş tarihini taşıyan **İstanbul Üniversitesi Resmi Amblemi**dir:

```text
         ┌──────────────────────────────┐
         │     🏛️  İSTANBUL ÜNİVERSİTESİ │
         │           KURULUŞ 1453       │
         └──────────────────────────────┘
```

* **Vektörel SVG Kaynağı**: Sistem bünyesinde `src/views/assets/iu-logo.svg` olarak saklanır ve doğrudan `GET /assets/iu-logo.svg` ucundan sunulur.
* **Kesintisiz Görünüm Garantisi (Dual-Resilience)**:
  ```html
  <img src="/assets/iu-logo.svg" 
       onerror="this.onerror=null; this.src='https://upload.wikimedia.org/wikipedia/commons/1/1e/%C4%B0stanbul_%C3%9Cniversitesi_Logo.svg';" 
       alt="İstanbul Üniversitesi Logosu">
  ```
* **Kullanım Boyutları**:
  * **Yüzen Navigasyon Başlığı**: `32px × 32px` (Kapsül içinde dikey hizalı)
  * **Hero Karşılama Alanı**: `44px × 44px` (Zarif çerçeveli kurumsal rozet)
  * **Sayfa Altbilgisi (Footer)**: `36px × 36px` (Sol kurumsal imza)
  * **404 Hata Kalkanı**: `48px × 48px` (Merkezi odak noktası)

---

### 2.2 Arka Planda İstanbul Üniversitesi Tarihi Giriş Kapısı Silüeti
Üniversitemizin simgesi olan **Tarihi Beyazıt Ana Giriş Kapısı (1865-1866)** mimari silüeti, sayfa arka planında zarif bir su yolu / filigran olarak yer alır.

```text
                        ┌─────────────┐
                        │  TAÇ KÖŞK   │
                 ┌──────┴─────────────┴──────┐
                 │    BÜYÜK ZAFER KEMERİ     │
         ┌───────┤                           ├───────┐
         │ YAYA  │       ANA GEÇİT           │ YAYA  │
         │ KEMER │                           │ KEMER │
    ─────┴───────┴───────────────────────────┴───────┴───── Kaide & Merdivenler
```

* **Vektörel Silüet Dosyası**: `src/views/assets/iu-gate-silhouette.svg`
* **Sunucu Ucu**: `GET /assets/iu-gate-silhouette.svg`
* **Tasarım Standartları**:
  1. **Hafif Saydamlık & Zarafet**: Silüet `%4.5` (`opacity: 0.045`) düzeyinde tutulur. Sayfadaki metinlerin, kartların ve formların okunabilirliğini asla engellemez; arkada prestijli bir mühür gibi süzülür.
  2. **Sabit Hizalama**: Sayfanın üst kısmında (`center 30px` - `40px`) `fixed` konumlandırmayla ortalanır; kullanıcı sayfayı kaydırdıkça arkada asil ve durağan duruşunu korur.
  3. **CSS Uygulama Örneği**:
     ```css
     body {
         background-color: #FAF9F6; /* Yoğun Alabaster Zemin */
         background-image: url('/assets/iu-gate-silhouette.svg');
         background-repeat: no-repeat;
         background-position: center 40px;
         background-size: 1160px auto;
         background-attachment: fixed;
     }
     ```
  4. **HTML Watermark Katmanı**:
     ```html
     <div class="bg-gate-watermark" aria-hidden="true">
         <img src="/assets/iu-gate-silhouette.svg" alt="" onerror="this.style.display='none';">
     </div>
     ```

---

## 🎨 3. Renk Paleti ve Tasarım Jetonları (Design Tokens)

Palet, **#FAF9F6 yoğunluklu sıcak alabaster zemin** ile **#004225 derin orman yeşili detayların** kusursuz uyumu üzerine kurulmuştur. **WCAG 2.1 AAA** düzeyinde kontrast garantisi sunar:

```text
┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐
│ Alabaster Tuval  │  │   Beyaz Yüzey    │  │ İmparator Yeşili │  │   Derin Mürekkep │
│     #FAF9F6      │  │     #FFFFFF      │  │     #004225      │  │     #0F2A1D      │
│ Yoğun Arka Plan  │  │  Bento Kartları  │  │ Butonlar & Vurgu │  │   Okunaklı Başlık│
└──────────────────┘  └──────────────────┘  └──────────────────┘  └──────────────────┘
```

### 3.1 Zemin ve Yüzey Renkleri
| Jeton Adı | Hex Değeri | RGB | Rolü / Kullanım Alanı |
| :--- | :--- | :--- | :--- |
| `--color-canvas` | `#FAF9F6` | `250, 249, 246` | **Yoğunluklu ana sayfa arka planı** (Alabaster fildişi kağıt) |
| `--color-canvas-subtle` | `#F3F2EC` | `243, 242, 236` | İkincil zemin yıkaması ve bölüm gruplama alanları |
| `--color-surface-card` | `#FFFFFF` | `255, 255, 255` | Bento kartlar, veri panelleri ve form yüzeyleri |
| `--color-surface-inset` | `#F5F4EE` | `245, 244, 238` | Gömülü kod alanları, iç kuyular ve tablo başlıkları |
| `--color-surface-nav` | `rgba(250, 249, 246, 0.94)` | — | Yüzen navigasyon kapsülü (`backdrop-filter: blur(20px)`) |

### 3.2 Butonlar, Aksiyonlar ve Vurgu Jetonları
| Jeton Adı | Hex Değeri | Açıklama |
| :--- | :--- | :--- |
| `--color-primary-action` | `#004225` | **Birincil butonlar**, aktif sekmeler, ana linkler ve çağrılar |
| `--color-primary-hover` | `#065A35` | Butonların hover ve basılma anı derinleşmesi |
| `--color-primary-tint` | `#E8F0EB` | Buton ve rozetlerin açık yeşil yumuşak zemin tonu |
| `--color-primary-border` | `#C8DBCF` | Kapsül rozetlerin zarif yeşil kenarlık çizgisi |

### 3.3 Kenarlık ve Çizgi Jetonları (Hairlines)
| Jeton Adı | Hex Değeri | Kullanım Amacı |
| :--- | :--- | :--- |
| `--color-hairline` | `#E2DFD6` | Kart ve kapsüllerin 1px ultra ince sıcak taş çizgisi |
| `--color-hairline-subtle`| `#EBE8E0` | Tablo satırları ve metin içi ince bölücüler |
| `--color-hairline-hover` | `#C8DBCF` | Kartların fareyle üzerine gelindiğinde aldığı yeşilimsi sınır |

### 3.4 Yüksek Kontrastlı Tipografi Mürekkepleri
| Jeton Adı | Hex Değeri | Kontrast Oranı | Kullanım Yeri |
| :--- | :--- | :---: | :--- |
| `--color-text-display` | `#004225` | **11.2:1 (AAA)** | Anıtsal serif manşetler, tarihi sayılar |
| `--color-text-title` | `#0F2A1D` | **14.8:1 (AAA)** | Kart başlıkları, mezun isimleri, modül adları |
| `--color-text-body` | `#1C2920` | **12.5:1 (AAA)** | Birincil arayüz açıklamaları, paragraflar |
| `--color-text-muted` | `#55685C` | **5.4:1 (AA)** | Tarihler, etiketler, yardımcı rehber metinleri |
| `--color-text-light` | `#7E9084` | **3.8:1 (UI)** | Altbilgi, telif hakkı ve devre dışı öğeler |

---

## 📐 4. İkili Yarıçap (Binary Radius) ve Sıfır Gölge Mimarisi

Sade ve modern tasarım disiplini, keyfi yuvarlatmaların ve sahte derinlik yaratan gölgelerin yasaklanmasını gerektirir:

```text
┌────────────────────────────────────────────────────────────────────────┐
│                        İKİLİ YARIÇAP SİSTEMİ                           │
├───────────────────────────────────┬────────────────────────────────────┤
│         9999px (Tam Kapsül)       │            4px (Zarif Kart)        │
├───────────────────────────────────┼────────────────────────────────────┤
│ • Yüzen Navigasyon Kapsülü        │ • Bento İçerik Kartları            │
│ • Birincil & İkincil Butonlar     │ • Veri Tabloları ve Listeler       │
│ • Arama ve Form Girdi Alanları    │ • Kod ve cURL Önizleme Kutuları    │
│ • Durum ve Kategori Rozetleri     │ • 404 Hata Kalkanı Kutusu          │
│ • Üniversite Logo Çerçevesi       │ • Akademik Değerlendirme Tezgahı   │
│                                   │                                    │
│ ❌ KESİN YASAK: 8px, 12px, 16px, 24px gibi keyfi ara yarıçaplar YOKTUR. │
└───────────────────────────────────┴────────────────────────────────────┘
```

### 4.1 Kesin Sıfır Gölge Prensibi
```css
/* Küresel Kural */
* {
    box-shadow: none !important;
}
```
* **Felsefe**: Gölgeler arayüzde görsel bulanıklık ve çamurluk yaratır. Sade ve modern bir tasarımda derinlik; `#FAF9F6` tuval zemin &rarr; `#FFFFFF` kart yüzeyi &rarr; `#F5F4EE` oyuk detay alanı arasındaki **ton kontrastı** ve **1px keskin hairlines (`#E2DFD6`)** ile inşa edilir.

---

## 🔤 5. Tipografi Sistemi & Hiyerarşi

Tasarım, klasik kurumsal vakar ile modern dijital okunabilirliği dengeler:

```text
DM Serif Display      ──► Anıtsal Başlıklar & Tarihi Sayılar (1453, 50.000+)
Plus Jakarta Sans     ──► Modern ve Net Arayüz Metinleri, Formlar, Butonlar
JetBrains Mono        ──► Rotalar, Durum Kodları, API Uçları, cURL Komutları
```

### Tipografi Hiyerarşi Tablosu
| Rol | Yazı Tipi | Kalınlık | Boyut / Satır | Harf Aralığı | Kullanım Yeri |
| :--- | :--- | :---: | :--- | :---: | :--- |
| **Hero Başlık** | `DM Serif Display` | 400 | `3.25rem` / `1.1` | `-0.03em` | Sayfa ana karşılama manşeti |
| **Bölüm Başlığı** | `DM Serif Display` | 400 | `2.00rem` / `1.2` | `-0.02em` | Modül ve blok başlıkları |
| **Kart Başlığı** | `DM Serif Display` | 400 | `1.30rem` / `1.3` | `-0.01em` | Bento kart başlıkları |
| **Arayüz / Form**| `Plus Jakarta Sans` | 600 | `0.92rem` / `1.5` | `0.00em` | Butonlar, menüler, etiketler |
| **Gövde Metni** | `Plus Jakarta Sans` | 400/500 | `0.95rem` / `1.6` | `0.00em` | Açıklama paragrafları |
| **Mikro Etiket** | `Plus Jakarta Sans` | 700 | `0.75rem` / `1.2` | `+0.05em` | Büyük harf durum rozetleri |
| **Teknik Kod** | `JetBrains Mono` | 500 | `0.85rem` / `1.5` | `0.00em` | Rota adresleri, cURL, JSON |

---

## 🧩 6. UI Bileşen Standartları

### 6.1 Zarif Kapsül Butonlar
* **Birincil Buton (`.btn-primary`)**:
  * Zemin: `#004225` (Derin İmparatorluk Yeşili) &bull; Metin: `#FFFFFF`
  * Yarıçap: `9999px` &bull; Kenarlık: `1px solid #004225`
  * Hover: `#065A35` &bull; Tıklama: Hafif ton koyulaşması
* **İkincil Buton (`.btn-secondary`)**:
  * Zemin: `#FFFFFF` &bull; Metin: `#0F2A1D`
  * Yarıçap: `9999px` &bull; Kenarlık: `1px solid #E2DFD6`
  * Hover: Zemin `#F5F4EE`, Kenarlık `#C8DBCF`

### 6.2 Yüzen Navigasyon Kapsülü
* Sayfanın tepesinde ortalanmış, `border-radius: 9999px` formunda yüzer.
* Zemin: `rgba(250, 249, 246, 0.94)`, `backdrop-filter: blur(20px)`.
* Kenarlık: `1px solid #E2DFD6`.
* Sol tarafta İstanbul Üniversitesi resmi amblemi, ortada navigasyon linkleri, sağda kurumsal durum rozeti.

### 6.3 Bento İçerik Kartları
* Zemin: Mat pürüzsüz `#FFFFFF`.
* Yarıçap: `4px` (Zarif, net, disiplinli).
* Kenarlık: `1px solid #E2DFD6`.
* Gölge: Kesinlikle `none`.
* Hover: Kenarlık tonu zarifçe `#C8DBCF` yeşiline yumuşar.

---

## 📢 7. Sayfa Bazlı Tasarım Kapsamı

Projedeki her bir sayfa bu kurallara harfiyen uyacak şekilde baştan sona tasarlanmıştır:
1. **Ana Sayfa & Tezgah (`/`, `/home`, `/bench`)**: Arka planda Beyazıt Kapısı silüeti filigranı, yüzen navigasyon, bento kartlar, canlı PostgreSQL durum göstergesi, akademik değerlendirme tezgahı ve kurumsal altbilgi.
2. **Mezun Rehberi (`/users`, `/users/create`, `/users/{id}`)**: Arka planda kapı silüeti, yalın çalışma sayfası tabloları, filtreleme çipleri ve detay profilleri.
3. **Kariyer Panosu (`/jobs`, `/jobs/create`, `/jobs/{id}`)**: Arka planda kapı silüeti, şirket künyeleri, iş fırsatları ve başvuru formları.
4. **Duyurular & Etkinlikler (`/announcements`)**: Arka planda kapı silüeti, mezunlar derneği bülteni, homecoming etkinlikleri.
5. **Hakkında Sayfası (`/about`)**: Arka planda kapı silüeti, HTTP routing prensipleri, URL anatomisi ve mimari detaylar.
6. **404 Hata Kalkanı (`/404`)**: Arka planda kapı silüeti, amblemli, nazik ve ana sayfaya yönlendiren kurumsal hata yakalayıcı.

---

## 🔗 8. İlgili Belgeler ve Bağlantılar

* 📐 [Tasarım Sistemi Yapay Zeka Spesifikasyonu (`DESIGN.MD`)](DESIGN.MD)
* 📖 [Ana Sistem Mimarisi & Sözleşmesi (`README.md`)](README.md)
* 🗄️ [Veritabanı Mimarisi & Kalıcılık İspatları (`ddd.md`)](ddd.md)
* 🌐 [Canlı Şablon Dosyası (`src/views/home.html`)](src/views/home.html)
* 🏛️ [Resmi Vektörel Logo (`src/views/assets/iu-logo.svg`)](src/views/assets/iu-logo.svg)
* 🏛️ [Tarihi Giriş Kapısı Silüeti (`src/views/assets/iu-gate-silhouette.svg`)](src/views/assets/iu-gate-silhouette.svg)
