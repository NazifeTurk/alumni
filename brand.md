# 🎨 Brand Identity & Design System (`brand.md`)

> **Course**: Web Programming (Web Programlama) — Istanbul University  
> **Instructor**: Assoc. Prof. Dr. Emre Akadal  
> **Student**: Nazife Türk (`nazife.turk@ogr.iu.edu.tr`)  
> **Repository**: [https://github.com/NazifeTurk/alumni](https://github.com/NazifeTurk/alumni)  
> **Design Benchmarks**:
> 1. [Refero AngelList Design System](https://styles.refero.design/style/c75603c7-492d-4c26-9744-9acc22fe6225) &bull; *Binary Radii & Zero Shadows*
> 2. [Refero Cord.com Design System](https://styles.refero.design/style/485ae5fb-8f25-4aa3-a4e4-1deb1590d7d6) &bull; *Midnight Harbor & Light Mist*
> 3. [Refero Resume.io Design System](https://styles.refero.design/style/41e65213-1c4a-41c4-b715-50427fa8926e) &bull; *Fresh Worksheet Precision*  
> **VoltAgent Specification**: 📐 [AI Design System Specification (`DESIGN.MD`)](DESIGN.MD)  
> **Cross-Doc Navigation**: ⬅️ [Return to Main Documentation (README.md)](README.md) &bull; 🗄️ [Data Design Documentation (ddd.md)](ddd.md)

---

## 📌 1. Brand Overview & Institutional Philosophy

The **Istanbul University Alumni Tracking System (`alumni`)** bridges over five centuries of academic excellence (established in 1453) with modern, institutional private-banking-grade software engineering.

```text
┌────────────────────────────────────────────────────────────────────────┐
│                        ISTANBUL UNIVERSITY                             │
│                  Alumni Tracking & Career Network                      │
│                  "Gelenekten Geleceğe Güçlü Köprü"                     │
└───────────────────────────────────┬────────────────────────────────────┘
                                    │
       ┌────────────────────────────┼────────────────────────────┐
       ▼                            ▼                            ▼
 🏛️ 1453 Legacy              💎 Trustworthy & Minimal    🤝 Alumni Solidarity
 Academic Heritage,          Atmospheric Light Blue,     Mentorship, Careers,
 Preeminence & Trust         DM Serif + Jakarta Sans     Homecoming & Unity
```

### Brand Personality & Core Values
* **Light Blue Intensive & Calming**: An open atmospheric light blue wash (`#eaf4fb`) that instills institutional tranquility, focus, and transparency.
* **Deeply Trustworthy & Authoritative**: High-contrast abyssal navy typography (`#04263e`) paired with classical monumental serifs (`DM Serif Display`), reminiscent of prestigious financial and academic institutions.
* **Radical Minimalism**: Zero ornamental drop shadows. Depth and structure are defined exclusively through disciplined tonal stepping and hairline borders.
* **Architectural Rigor**: Strict binary corner radius system (capsule `9999px` vs card `4px`), eliminating all arbitrary radii.

---

## 🎓 2. Logomark, Crest & Visual Identity

### 2.1 Primary Crest Anatomy
The emblem synthesizes the iconic academic graduation mortarboard, Istanbul University's historical gate motif, and modern cloud telemetry:

```text
       ┌───────────────┐
       │     🎓        │  ◄── Academic Cap: Educational Excellence (1453)
       │  İSTANBUL     │  ◄── University Title: Deep Institutional Authority
       │  ÜNİVERSİTESİ │  ◄── Historical Foundation: Timeless Heritage
       └───────────────┘
```

* **Icon Symbol**: `🎓` (Academic Cap) housed within a pill capsule (`radius: 9999px`) on floating light-blue glass.
* **Pill Dimensions**: 40px $\times$ 40px circular badge in navigation; 48px $\times$ 48px in hero modules.
* **Crest Stroke**: `1px solid #bae6fd` on `#ffffff` crisp surface.
* **Hover Micro-interaction**: Subtle `border-color: #0284c7` transition (150ms ease).

### 2.2 Clear Space & Safe Margin
To maintain visual authority and readability:
* Maintain a minimum margin of **`16px`** around the logomark in all screen sizes.
* Never place cluttered background textures or high-saturation gradients behind the primary mark.

---

## 🎨 3. Color Palette & Refero-Inspired Design Tokens

The palette is engineered directly from the [Refero Design Benchmark](https://styles.refero.design/style/c75603c7-492d-4c26-9744-9acc22fe6225), optimized for a **Light Blue Intensive, Minimal, and Trustworthy** aesthetic meeting **WCAG 2.1 AAA** contrast standards:

```text
┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐
│   Atmosphere Blue│  │   Card Surface   │  │   Abyssal Navy   │  │  Cerulean Action │
│     #eaf4fb      │  │     #ffffff      │  │     #04263e      │  │     #0284c7      │
│  Primary Canvas  │  │  Flat Paper Card │  │ High-Contrast Text│ │  Primary Button  │
└──────────────────┘  └──────────────────┘  └──────────────────┘  └──────────────────┘
```

### 3.1 Structural & Surface Palette
| Token Name | Hex Code | RGB | Role / Application |
| :--- | :--- | :--- | :--- |
| `--color-canvas` | `#eaf4fb` | `234, 244, 251` | Primary atmospheric light blue page background |
| `--color-canvas-subtle` | `#f2f8fd` | `242, 248, 253` | Secondary page canvas and container backdrop |
| `--color-surface-card` | `#ffffff` | `255, 255, 255` | Crisp white paper surface for bento cards and modals |
| `--color-surface-elevated`| `#f0f7fe` | `240, 247, 254` | Inset wells, table headers, and metric container surfaces |
| `--color-hairline` | `#bae6fd` | `186, 230, 253` | Crisp 1px sky hairline border for cards and dividers |
| `--color-hairline-subtle`| `#e0f2fe` | `224, 242, 254` | Light internal divider between list rows |

### 3.2 High-Contrast Authoritative Typography Tokens
| Token Name | Hex Code | Contrast on `#ffffff` | Application |
| :--- | :--- | :---: | :--- |
| `--color-text-display` | `#04263e` | **15.4:1 (AAA)** | Monumental headlines, statement metrics, page titles |
| `--color-text-body` | `#1e3a52` | **10.8:1 (AAA)** | Primary readable interface copy and paragraph descriptions |
| `--color-text-muted` | `#486f8a` | **5.3:1 (AA)** | Secondary helper text, subtitles, datetimes |
| `--color-text-light` | `#6a92ad` | **3.8:1 (UI)** | Disabled states, footer metadata, copyright stamps |

### 3.3 Semantic Action & Accent Indicators
| Semantic Role | Hex Code | Background Tint | Meaning & Association |
| :--- | :--- | :--- | :--- |
| **Cerulean Primary** | `#0284c7` | `#e0f2fe` | Primary action buttons, active tabs, link underlines |
| **Cerulean Hover** | `#0369a1` | `#bae6fd` | Focused and hovered interactive triggers |
| **Lavender Dawn** | `#6366f1` | `#e0e7ff` | Mentorship badge, academic networking highlight |
| **Bio Mint Active** | `#059669` | `#d1fae5` | PostgreSQL live health status, 200 OK responses |
| **Imperial Gold** | `#b45309` | `#fef3c7` | 1453 Historical Heritage, Academic Bench pill |
| **Rose Rejection** | `#e11d48` | `#ffe4e6` | 404 Guardian Shield, 5xx Guardian alert |

---

## 📐 4. The Binary Radius & Zero-Shadow Elevation Rules

The core design philosophy is adapted from the Refero Private Banking and Institutional style guide:

```text
┌────────────────────────────────────────────────────────────────────────┐
│                       THE BINARY RADIUS SYSTEM                         │
├───────────────────────────────────┬────────────────────────────────────┤
│         9999px (Capsule)          │            4px (Subtle)            │
├───────────────────────────────────┼────────────────────────────────────┤
│ • Navigation Bar Pill Container   │ • Content Cards & Bento Panels     │
│ • Primary & Secondary Buttons     │ • Telemetry & Code Previews        │
│ • Category Badges & Filter Chips  │ • Data Tables & Grid Containers    │
│ • Search Inputs & Text Inputs     │ • Evaluation Bench Modules         │
│                                   │                                    │
│ ❌ Strictly Zero Intermediate Radii: No 8px, No 12px, No 16px cards     │
└───────────────────────────────────┴────────────────────────────────────┘
```

### 4.1 Strict Zero Drop Shadows
* **Rule**: `--elevation-none: none !important;`
* **Philosophy**: Shadows create visual muddiness and false skeuomorphic depth. In this light-blue institutional system, elevation is conveyed strictly through:
  1. **Tonal Contrast**: Stacking `#ffffff` cards on `#eaf4fb` canvas.
  2. **Hairline Boundaries**: 1px `#bae6fd` borders defining crisp geometric boundaries.
  3. **Inset Surfaces**: `#f0f7fe` recessed panels within white cards for secondary metadata.

---

## 🔤 5. Typography Hierarchy & Font Pairing

The design system marries classical academic gravitas with clean digital precision:

```text
DM Serif Display      ──► Monumental Headlines & Historical Numbers (1453, 50K+)
Plus Jakarta Sans     ──► High-Legibility Interface Copy, Forms & Buttons
JetBrains Mono        ──► REST Endpoints, Status Codes, Telemetry & JSON
```

### Typography Scale Matrix
| Level | Font Family | Weight | Size / Line-Height | Letter-Spacing | Usage Context |
| :--- | :--- | :---: | :--- | :---: | :--- |
| **Monumental Hero**| `DM Serif Display` | 400 | `3.65rem` / `1.10` | `-0.04em` | Main hero title on home page |
| **Statement Stat** | `DM Serif Display` | 400 | `3.25rem` / `1.05` | `-0.04em` | "By The Numbers" large metric digits |
| **H2 Section** | `DM Serif Display` | 400 | `2.25rem` / `1.20` | `-0.03em` | Module titles (Pillars, Feeds, Bench) |
| **H3 Card Title** | `Plus Jakarta Sans` | 700 | `1.15rem` / `1.35` | `-0.01em` | Bento grid cards, job titles |
| **Body Lead** | `Plus Jakarta Sans` | 400 | `1.15rem` / `1.65` | `0.00em` | Hero introductory deck |
| **Body Regular** | `Plus Jakarta Sans` | 500 | `0.95rem` / `1.60` | `0.00em` | Interface descriptions, table cells |
| **Micro Caption** | `Plus Jakarta Sans` | 700 | `0.75rem` / `1.20` | `+0.04em` | Status pills, badge labels (uppercase) |
| **Code / Route** | `JetBrains Mono` | 600 | `0.85rem` / `1.50` | `0.00em` | Endpoint paths, cURL parameters |

*Note: All numeric interface elements enable OpenType proportional and tabular figures via `font-feature-settings: 'lnum', 'tnum';`.*

---

## 🧩 6. UI Component Patterns

### 6.1 Floating Centered Navigation Pill
```html
<header class="nav-wrapper">
  <div class="nav-pill-container">
    <div class="nav-brand">...</div>
    <nav class="nav-links">...</nav>
    <div class="nav-actions">...</div>
  </div>
</header>
```
* **Radius**: `9999px`
* **Surface**: `rgba(255, 255, 255, 0.92)` with `backdrop-filter: blur(20px)`
* **Border**: `1px solid #bae6fd`
* **Behavior**: Pinned to viewport top with 16px margin on scroll.

### 6.2 Buttons & Capsule Triggers
* **Primary CTA (`.btn-primary`)**:
  * Background: `#0284c7`, Border: `1px solid #0284c7`, Color: `#ffffff`
  * Radius: `9999px`
  * Hover: `#0369a1`
* **Secondary Action (`.btn-secondary`)**:
  * Background: `#ffffff`, Border: `1px solid #bae6fd`, Color: `#0284c7`
  * Radius: `9999px`
  * Hover: Background `#f0f7fe`, Border `#93c5fd`
* **Tertiary Ghost (`.btn-ghost`)**:
  * Background: `transparent`, Color: `#1e3a52`
  * Hover: Background `rgba(2, 132, 199, 0.08)`

### 6.3 Bento Content Cards
* **Surface**: Solid `#ffffff`
* **Radius**: `4px` (strict minimal standard)
* **Border**: `1px solid #bae6fd`
* **Shadow**: `none`
* **Hover State**: Border shifts subtly to `#38bdf8` with `background: #fcfeff`.

### 6.4 Academic & Telemetry Badges
* **Live Database Health**: `#d1fae5` pill + `#059669` text + pulsing mint green indicator.
* **1453 Heritage**: `#fef3c7` pill + `#b45309` text.
* **404 Guardian Shield**: `#ffe4e6` pill + `#e11d48` text.
* **Academic Bench**: `#e0f2fe` pill + `#0284c7` text.

---

## 📢 7. Institutional Voice & Tone Guidelines

### 7.1 Tone Principles
1. **Dignified yet Contemporary**: Honor Istanbul University's status as Turkey's oldest and most prestigious university without bureaucratic dryness.
2. **Precision & Transparency**: Clearly convey system status, database health, and routing outcomes without vague marketing jargon.
3. **Inclusive Community Focus**: Inspire alumni to reconnect, mentor undergraduates, and collaborate on professional opportunities.

### 7.2 Standard Terminology
| Turkish (Canonical) | English (Canonical) | Deprecated |
| :--- | :--- | :--- |
| **Mezun Takip ve Kariyer Ağı** | Alumni Tracking & Career Network | Eski Mezunlar Sitesi |
| **Mezun Rehberi** | Alumni Directory | Mezun Listesi |
| **Kariyer Havuzu & İlan Panosu** | Career & Job Board | İlan Sayfası |
| **Akademik Değerlendirme Tezgahı**| Academic Evaluation Bench | Test Konsolu |
| **Hata Koruyucu Kalkanı** | Guardian Shield | Error Handler |

---

## 🔗 8. Cross-Documentation & Anchors

* 📐 [AI Design System Specification (`DESIGN.MD`)](DESIGN.MD)
* 📖 [Master Architecture & System Contract (`README.md`)](README.md)
* 🗄️ [Data Design Documentation & Schema Proofs (`ddd.md`)](ddd.md)
* 🌐 [Live Homepage Template (`src/views/home.html`)](src/views/home.html)
* 🛡️ [Generic 404 Guardian Error Page (`src/views/404.html`)](src/views/404.html)
* 🧪 [Postman Automated Test Collection (`postman_alumni_invalid_requests.json`)](postman_alumni_invalid_requests.json)
* 🎨 [Refero AngelList Style Benchmark](https://styles.refero.design/style/c75603c7-492d-4c26-9744-9acc22fe6225)
* 🌊 [Refero Cord.com Style Benchmark](https://styles.refero.design/style/485ae5fb-8f25-4aa3-a4e4-1deb1590d7d6)
* 📄 [Refero Resume.io Style Benchmark](https://styles.refero.design/style/41e65213-1c4a-41c4-b715-50427fa8926e)
