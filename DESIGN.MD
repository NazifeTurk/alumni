# 📐 DESIGN.MD — Design System Specification

> **Course**: Web Programming (Web Programlama) — Istanbul University  
> **Instructor**: Assoc. Prof. Dr. Emre Akadal  
> **Student**: Nazife Turk (`nazife.turk@ogr.iu.edu.tr`)  
> **Repository**: [https://github.com/NazifeTurk/alumni](https://github.com/NazifeTurk/alumni)  
> **Standard**: VoltAgent `awesome-design-md` Plain-Text Specification for AI Agents & Human Engineers  
> **Design Benchmarks**:
> 1. [AngelList Private Banking (`c75603c7`)](https://styles.refero.design/style/c75603c7-492d-4c26-9744-9acc22fe6225) — Binary Radii (`9999px` / `4px`), Zero Drop Shadows, `DM Serif Display` Monumental Scale.
> 2. [Cord.com Deep Ocean Signal (`485ae5fb`)](https://styles.refero.design/style/485ae5fb-8f25-4aa3-a4e4-1deb1590d7d6) — Midnight Harbor (`#0b3658`), Signal Blue (`#4e9ad9`), Ice Tint (`#e6f1fa`), Sea Fog (`#dde7ee`) hairlines.
> 3. [Resume.io Fresh Worksheet Precision (`41e65213`)](https://styles.refero.design/style/41e65213-1c4a-41c4-b715-50427fa8926e) — Document-First Utility, High-Contrast Carbon (`#0f141e`) & Ink (`#1e2532`), minimal chrome.

---

## 1. Visual Theme & Emotional Atmosphere

The **Istanbul University Alumni Tracking System (`alumni`)** visual identity is an **Atmospheric Light Blue, Minimalist, and Deeply Trustworthy Institutional Interface**. It fuses Istanbul University's 500-year academic legacy (established 1453) with modern private-banking-grade digital craftsmanship.

```text
┌────────────────────────────────────────────────────────────────────────┐
│                        ATHMOSPHERIC LIGHT BLUE                         │
│                    "Signal Harbor & Academic Ledger"                   │
├───────────────────────────────────┬────────────────────────────────────┤
│ 🌊 Deep Maritime Authority        │ 📄 Document Worksheet Precision    │
│ • Midnight Harbor (#0b3658) text  │ • Fresh white cards (#ffffff)      │
│ • Calming Sea-Fog canvas (#eaf4fb)│ • Crisp 1px hairlines (#bae6fd)    │
│ • Signal Blue (#0284c7) flare     │ • High data-density & tabular copy │
├───────────────────────────────────┴────────────────────────────────────┤
│ 🏛️ Binary Geometry & Zero Shadows                                     │
│ • Full Capsule Pills (9999px) for triggers, search, and navigation     │
│ • Subtle 4px Radius for content panels and bento data containers       │
│ • Strictly Zero Drop Shadows: Depth is engineered solely via tonal     │
│   stepping (#eaf4fb → #ffffff → #f0f7fe) and razor-sharp hairlines.    │
└────────────────────────────────────────────────────────────────────────┘
```

### Core Design Principles
1. **Light Blue Intensive**: An airy, open canvas wash (`#eaf4fb` to `#f2f8fd`) replaces harsh blank whites or tiring dark modes, creating a serene, focused atmosphere.
2. **Deep Trustworthiness**: High-contrast abyssal navy inks (`#04263e` / `#0b3658`) carry every word with solemn institutional gravity, passing **WCAG 2.1 AAA** contrast standards.
3. **Radical Minimalism**: No skeuomorphism, no blurry drop shadows, no colorful decorative noise. Every pixel is purposeful, legible, and structurally grounded.
4. **Binary Corner Radii**: Exactly two corner radii exist in the entire system: **`9999px`** (full pill) for interactive/navigational elements, and **`4px`** for content containers. No intermediate 8px, 12px, or 16px radii are allowed.
5. **Serif & Geometric Sans Duality**: Classical academic authority is proclaimed by monumental `DM Serif Display` headlines, while lightning-fast scanning is delivered by `Plus Jakarta Sans` body copy with tabular figures (`tnum`).

---

## 2. Color Palette & Semantic Tokens

```text
┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐
│   Sea-Fog Canvas │  │   Card Surface   │  │  Midnight Harbor │  │    Signal Blue   │
│     #eaf4fb      │  │     #ffffff      │  │     #0b3658      │  │     #0284c7      │
│   Base Canvas    │  │  Flat Paper Card │  │  Abyssal Navy In │  │  Primary Action  │
└──────────────────┘  └──────────────────┘  └──────────────────┘  └──────────────────┘
```

### 2.1 Surfaces & Layout Canvas
| Token Name | Hex Code | RGB | HSL | Semantic Role |
| :--- | :--- | :--- | :--- | :--- |
| `--color-canvas` | `#eaf4fb` | `234, 244, 251` | `205°, 65%, 95%` | Primary atmospheric sea-fog page canvas |
| `--color-canvas-subtle` | `#f2f8fd` | `242, 248, 253` | `207°, 69%, 97%` | Secondary background wash for section grouping |
| `--color-surface-card` | `#ffffff` | `255, 255, 255` | `0°, 0%, 100%` | Pure white paper surface for bento cards & tables |
| `--color-surface-inset`| `#f0f7fe` | `240, 247, 254` | `210°, 78%, 97%` | Recessed wells, metrics containers, table headers |
| `--color-surface-nav` | `rgba(255, 255, 255, 0.92)` | — | — | Floating glass pill backdrop (`backdrop-filter: blur(20px)`) |

### 2.2 Hairlines & Structural Dividers
| Token Name | Hex Code | Purpose |
| :--- | :--- | :--- |
| `--color-hairline` | `#bae6fd` | Crisp 1px sky-blue border for cards, pills, and outer boundaries |
| `--color-hairline-subtle` | `#e0f2fe` | Subtle 1px divider for table rows, feed separators, and meta blocks |
| `--color-hairline-hover` | `#93c5fd` | Interactive card and button border hover state |

### 2.3 Typography Inks (High-Contrast Hierarchy)
| Token Name | Hex Code | Contrast on `#ffffff` | Contrast on `#eaf4fb` | Usage |
| :--- | :--- | :---: | :---: | :--- |
| `--color-text-display` | `#04263e` | **15.4:1 (AAA)** | **13.5:1 (AAA)** | Monumental serif headlines, stat numbers |
| `--color-text-navy` | `#0b3658` | **12.2:1 (AAA)** | **10.7:1 (AAA)** | Card titles, navigation logo, section headings |
| `--color-text-body` | `#1e3a52` | **10.8:1 (AAA)** | **9.5:1 (AAA)** | Default interface body copy, descriptions |
| `--color-text-muted` | `#486f8a` | **5.4:1 (AA)** | **4.7:1 (AA)** | Secondary helper text, timestamps, labels |
| `--color-text-light` | `#6a92ad` | **3.8:1 (UI)** | **3.3:1 (UI)** | Footers, copyright notices, disabled items |

### 2.4 Semantic Actions & Status Flares
| Semantic Token | Hex Code | Background Tint | Border Tint | Application |
| :--- | :--- | :--- | :--- | :--- |
| `--color-primary-action`| `#0284c7` | `#e0f2fe` | `#bae6fd` | Primary CTA buttons, active links, filter tabs |
| `--color-primary-hover` | `#0369a1` | `#bae6fd` | `#7dd3fc` | Hover & pressed states for primary triggers |
| `--color-bio-mint` | `#059669` | `#d1fae5` | `#a7f3d0` | PostgreSQL live database, 200 OK status |
| `--color-signal-teal` | `#42b3b1` | `#e0f7f6` | `#b2ebe9` | Online presence indicator, verified alumni |
| `--color-lavender-dawn` | `#6366f1` | `#e0e7ff` | `#c7d2fe` | Mentorship badge, academic networking |
| `--color-heritage-gold` | `#b45309` | `#fef3c7` | `#fde68a` | 1453 University Heritage, Bench pill |
| `--color-rose-shield` | `#e11d48` | `#ffe4e6` | `#fecdd3` | 404 Guardian Shield, 5xx Guardian alert |

---

## 3. Spatial System & Corner Radii

### 3.1 The Binary Radius System (Strict Standard)
To avoid the chaotic, arbitrary corner radii found in un-curated interfaces, every single UI element must use one of two radius tokens:

```css
:root {
  --radius-pills: 9999px; /* Pill radius: buttons, chips, badges, inputs, nav container */
  --radius-cards: 4px;    /* Subtle card radius: bento cards, data tables, code wells */
}
```

```text
┌────────────────────────────────────────────────────────────────────────┐
│                        CORNER RADIUS TAXONOMY                          │
├───────────────────────────────────┬────────────────────────────────────┤
│      9999px — Full Capsule        │         4px — Subtle Card          │
├───────────────────────────────────┼────────────────────────────────────┤
│ • Floating Header Navigation Pill │ • Product Bento Grid Cards         │
│ • Primary, Secondary & Ghost Btns │ • Academic Lab Evaluation Bench    │
│ • Live Status Indicators & Badges │ • Data Table Containers            │
│ • Search & Input Form Fields      │ • Telemetry & Code Previews        │
│ • Category Filter Chips           │ • 404 / 5xx Guardian Error Boxes   │
│                                   │                                    │
│ ❌ STRICT BAN: No 8px, No 12px, No 16px, No 24px intermediate corners. │
└───────────────────────────────────┴────────────────────────────────────┘
```

### 3.2 Spacing Metric (Base-4 Scale)
```css
--space-4:  4px;  /* Micro padding, badge gap */
--space-8:  8px;  /* Compact chip padding, inner icon gaps */
--space-12: 12px; /* Input vertical padding */
--space-16: 16px; /* Standard component padding, inline gaps */
--space-20: 20px; /* Card compact gutter */
--space-24: 24px; /* Standard card inner padding */
--space-32: 32px; /* Section sub-grid spacing */
--space-48: 48px; /* Block separation margin */
--space-64: 64px; /* Major page module vertical margin */
--space-80: 80px; /* Hero section top & bottom breathing room */
```

### 3.3 Strict Zero Drop Shadows
```css
--elevation-none: none !important;
```
* **No Drop Shadows**: All `box-shadow` values are set to `none`.
* **Depth Technique**: Elevation is communicated solely by placing `#ffffff` surfaces on `#eaf4fb` canvas, bounded by crisp `1px solid #bae6fd` hairlines, and nesting recessed `#f0f7fe` wells.

---

## 4. Typography Hierarchy & Font Stacks

```css
--font-display: 'DM Serif Display', Georgia, serif;
--font-sans:    'Plus Jakarta Sans', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
--font-mono:    'JetBrains Mono', ui-monospace, Menlo, Consolas, monospace;
```

### 4.1 Typography Scale Matrix
| Level | Font Stack | Weight | Size | Line Height | Tracking | OpenType Features |
| :--- | :--- | :---: | :---: | :---: | :---: | :--- |
| **Hero Display** | `DM Serif Display` | 400 | `3.65rem` (58px) | `1.10` | `-0.04em` | — |
| **Statement Stat** | `DM Serif Display` | 400 | `3.25rem` (52px) | `1.05` | `-0.04em` | `'lnum', 'tnum'` |
| **H2 Section** | `DM Serif Display` | 400 | `2.25rem` (36px) | `1.20` | `-0.03em` | — |
| **H3 Card Title** | `Plus Jakarta Sans`| 700 | `1.15rem` (18px) | `1.35` | `-0.01em` | — |
| **Body Lead** | `Plus Jakarta Sans`| 400 | `1.15rem` (18px) | `1.65` | `0.00em` | — |
| **Body Regular** | `Plus Jakarta Sans`| 500 | `0.95rem` (15px) | `1.60` | `0.00em` | `'lnum', 'tnum'` |
| **UI Control/Btn**| `Plus Jakarta Sans`| 600 | `0.92rem` (14.5px)| `1.20` | `+0.01em` | — |
| **Micro Tag/Chip**| `Plus Jakarta Sans`| 700 | `0.75rem` (12px) | `1.20` | `+0.04em` | `uppercase` |
| **Code/Telemetry**| `JetBrains Mono` | 600 | `0.85rem` (13.5px)| `1.50` | `0.00em` | `'tnum'` |

---

## 5. Component Blueprints & Patterns

### 5.1 Centered Floating Navigation Pill
```html
<header class="nav-wrapper">
  <div class="nav-pill-container">
    <div class="nav-brand">
      <span class="brand-crest">🎓</span>
      <div class="brand-title">
        <span class="title-main">İSTANBUL ÜNİVERSİTESİ</span>
        <span class="title-sub">Mezun Takip Sistemi</span>
      </div>
    </div>
    <nav class="nav-links">
      <a href="/users" class="nav-link">Mezun Rehberi</a>
      <a href="/jobs" class="nav-link">Kariyer Havuzu</a>
      <a href="/announcements" class="nav-link">Duyurular</a>
      <a href="/bench" class="nav-link">Akademik Tezgah</a>
    </nav>
    <div class="nav-actions">
      <a href="/swagger" class="btn btn-secondary">Swagger API</a>
      <a href="/login" class="btn btn-primary">Giriş Yap</a>
    </div>
  </div>
</header>
```
* **CSS Specs**:
  * `border-radius: 9999px;`
  * `background: rgba(255, 255, 255, 0.92);`
  * `backdrop-filter: blur(20px);`
  * `border: 1px solid #bae6fd;`
  * `box-shadow: none;`
  * Pinned top with 16px offset.

### 5.2 Hero Statement & Monumental Metrics
```html
<section class="hero-section">
  <div class="announcement-pill">
    <span class="pill-dot"></span>
    <span class="pill-text">1453'ten Günümüze &bull; Akademik Mükemmeliyet ve Dayanışma</span>
  </div>
  <h1 class="hero-headline">
    Gelenekten Geleceğe<br>
    <em>Güçlü Mezun Köprüsü</em>
  </h1>
  <p class="hero-deck">
    İstanbul Üniversitesi mezunlarını küresel kariyer fırsatları, akademik mentorluk ağları ve kurumsal işbirlikleriyle buluşturan yeni nesil takip platformu.
  </p>
  <div class="hero-actions">
    <a href="/users" class="btn btn-primary">Mezun Rehberini İncele</a>
    <a href="/bench" class="btn btn-secondary">Akademik Tezgah</a>
  </div>
</section>
```

### 5.3 Monumental "By The Numbers" Block
* **Cards**: `4px` radius, `#ffffff` surface, `1px solid #bae6fd` border, zero shadow.
* **Digits**: `DM Serif Display` 400, `color: #04263e`, size `3.25rem`, tracking `-0.04em`.
* **Metrics**:
  * `1453` — Kuruluş Yılı (Akademik Miras)
  * `50K+` — Aktif Kayıtlı Mezun Ağı
  * `100%` — Doğrulanmış Akademik Sicil
  * `3-Tier` — C# .NET 7 + PostgreSQL 16 + Docker

### 5.4 Bento Product Grid (4 Pillars)
* 4-column desktop layout (`grid-template-columns: repeat(4, 1fr)`).
* `border-radius: 4px;`
* `background: #ffffff;`
* `border: 1px solid #bae6fd;`
* Hover state: subtle background `#fcfeff`, border `#93c5fd`.
* Direct arrow links: `→` with `color: #0284c7`.

### 5.5 404 Guardian Error Shield
* Minimalist institutional card:
  * `border-radius: 4px;`
  * `border: 1px solid #bae6fd;`
  * `background: #ffffff;`
  * Huge `404` in `DM Serif Display` (`color: #04263e`).
  * Capsule error badge (`#ffe4e6` bg, `#e11d48` text, `9999px` radius).
  * Recessed path box (`#f0f7fe` bg, `4px` radius, `#bae6fd` border).

---

## 6. AI Agent Implementation Guardrails

When generating or editing HTML, CSS, React, Razor, or Blazor views for this project, AI coding agents MUST enforce the following constraints:

```text
┌────────────────────────────────────────────────────────────────────────┐
│                   AI CODING AGENT MANDATORY RULES                      │
├────────────────────────────────────────────────────────────────────────┤
│ 1. CANVASES MUST BE LIGHT BLUE                                         │
│    Use #eaf4fb for body background. NEVER use pure #ffffff or dark     │
│    #0f172a for page background.                                        │
│                                                                        │
│ 2. STRICT BINARY RADIUS (9999px vs 4px)                                │
│    Use 9999px for buttons, chips, badges, and the nav pill.            │
│    Use 4px for cards, tables, wells, and inputs.                       │
│    NEVER use rounded-md (6px), rounded-lg (8px), or rounded-xl (12px). │
│                                                                        │
│ 3. ZERO DROP SHADOWS                                                   │
│    Set box-shadow: none !important. Depth must be conveyed strictly    │
│    via tonal stepping (#eaf4fb → #ffffff → #f0f7fe) and 1px borders.  │
│                                                                        │
│ 4. TYPOGRAPHY PAIRING                                                  │
│    Use 'DM Serif Display' for H1, H2, and monumental stat digits with  │
│    tight tracking (-0.03em to -0.05em).                                │
│    Use 'Plus Jakarta Sans' for all UI copy and controls.               │
│                                                                        │
│ 5. TEXT CONTRAST INKS                                                  │
│    Headings: #04263e (Abyssal Navy)                                    │
│    Body: #1e3a52 (Deep Steel Blue)                                     │
│    Muted: #486f8a (Slate Blue)                                         │
│    Primary Button CTA: #0284c7 with hover #0369a1                      │
└────────────────────────────────────────────────────────────────────────┘
```

---

## 7. Tailwind CSS v4 Configuration Snippet

For agents generating Tailwind v4 utility classes:

```css
@theme {
  --color-canvas: #eaf4fb;
  --color-canvas-subtle: #f2f8fd;
  --color-surface-card: #ffffff;
  --color-surface-elevated: #f0f7fe;
  --color-hairline: #bae6fd;
  --color-hairline-subtle: #e0f2fe;

  --color-text-display: #04263e;
  --color-text-navy: #0b3658;
  --color-text-body: #1e3a52;
  --color-text-muted: #486f8a;
  --color-text-light: #6a92ad;

  --color-primary: #0284c7;
  --color-primary-hover: #0369a1;
  --color-mint: #059669;
  --color-teal: #42b3b1;
  --color-gold: #b45309;
  --color-rose: #e11d48;

  --font-display: "DM Serif Display", Georgia, serif;
  --font-sans: "Plus Jakarta Sans", sans-serif;
  --font-mono: "JetBrains Mono", monospace;

  --radius-card: 4px;
  --radius-pill: 9999px;
}
```

---

## 8. Cross-Documentation & Anchors

* 📖 [Master Architecture & System Contract (`README.md`)](README.md)
* 🎨 [Brand Identity & Design System (`brand.md`)](brand.md)
* 🗄️ [Data Design Documentation (`ddd.md`)](ddd.md)
* 🌐 [Production Homepage (`src/views/home.html`)](src/views/home.html)
* 🛡️ [Generic 404 Guardian Error Page (`src/views/404.html`)](src/views/404.html)
* 🧪 [Postman Automated Evaluation Suite (`postman_alumni_invalid_requests.json`)](postman_alumni_invalid_requests.json)
