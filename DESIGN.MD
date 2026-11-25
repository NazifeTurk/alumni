# 📐 DESIGN.MD — AI Design System Specification

> **Course**: Web Programming (Web Programlama) — Istanbul University  
> **Student / System Architect**: Nazife Türk (`nazife.turk@ogr.iu.edu.tr`)  
> **Repository**: [https://github.com/NazifeTurk/alumni](https://github.com/NazifeTurk/alumni)  
> **Standard**: VoltAgent `awesome-design-md` Plain-Text Specification for AI Agents & Human Engineers  
> **Aesthetic Philosophy**: *Sade, Zarif ve Modern* (Minimal, Elegant & Modern)  
> **Color Hierarchy**:  
> &bull; **Primary Intensive Background**: `#FAF9F6` (Alabaster Warm Ivory Paper)  
> &bull; **Buttons, Badges & Interactive Details**: `#004225` (Deep Imperial Forest Green)  
> **Visual Anchors**:  
> &bull; **Official Logo**: Istanbul University Crest (`/assets/iu-logo.svg`)  
> &bull; **Background Watermark**: Historic Beyazıt Main Gate Silhouette (`/assets/iu-gate-silhouette.svg`)  
> **Cross-Doc Navigation**: 🎨 [Brand Guidelines (brand.md)](brand.md) &bull; ⬅️ [README.md](README.md) &bull; 🗄️ [ddd.md](ddd.md)

---

## 1. Visual Theme & Emotional Atmosphere

The **Istanbul University Alumni Tracking System (`alumni`)** visual identity is an **Alabaster Intensive (`#FAF9F6`), Deep Imperial Forest Green (`#004225`), Minimalist, and Deeply Trustworthy Institutional Interface**. It fuses Istanbul University's 500-year academic legacy (established 1453) with modern, editorial, private-banking-grade digital craftsmanship.

```text
┌────────────────────────────────────────────────────────────────────────┐
│                   ALABASTER & IMPERIAL FOREST GREEN                    │
│                 "Alabaster Stationery & Beyazıt Gate"                  │
├───────────────────────────────────┬────────────────────────────────────┤
│ 🌲 Deep Institutional Details     │ 🏛️ Historic Gate Watermark         │
│ • Deep Imperial Green (#004225)   │ • Beyazıt Gate Silhouette in bg    │
│ • Warm Alabaster canvas (#FAF9F6) │ • Crisp white cards (#FFFFFF)      │
│ • Soft Forest tint (#E8F0EB)      │ • Crisp 1px hairlines (#E2DFD6)    │
├───────────────────────────────────┴────────────────────────────────────┤
│ 📐 Binary Geometry & Zero Shadows                                     │
│ • Full Capsule Pills (9999px) for buttons, badges, and navigation      │
│ • Subtle 4px Radius for content panels and bento data containers       │
│ • Strictly Zero Drop Shadows: Depth is engineered solely via tonal     │
│   stepping (#FAF9F6 → #FFFFFF → #F5F4EE) and razor-sharp hairlines.    │
└────────────────────────────────────────────────────────────────────────┘
```

### Core Design Principles
1. **Sade ve Zarif (Minimal & Elegant)**:
   * Absolute freedom from clutter, unnecessary ornamentation, or distracting drop shadows.
   * Every component is purposeful, balanced, and visually restful.
2. **Modern & Crisp**:
   * Inspired by contemporary editorial and fintech interfaces (Linear, Stripe Press, AngelList).
   * Sharp 1px hairlines paired with micro-radii and generous whitespace create a cutting-edge feel.
3. **Alabaster Canvas Intensive (`#FAF9F6`)**:
   * An authentic, warm alabaster canvas dominates the viewport background, replacing stark blinding whites or synthetic dark modes.
4. **Imperial Green Details & Buttons (`#004225`)**:
   * All primary buttons, active tabs, pills, badges, and key typographic focal points use `#004225`, passing **WCAG 2.1 AAA** contrast standards.
5. **Historic Gate Silhouette Backdrop**:
   * The silhouette of Istanbul University's iconic Beyazıt Main Gate (`iu-gate-silhouette.svg`) anchors the background at 4.5% opacity, providing unmistakable institutional gravitas without interfering with legibility.

---

## 2. Official Istanbul University Logo & Gate Silhouette

### 2.1 Official Istanbul University Logo
* **Vector SVG Path**: Served via `GET /assets/iu-logo.svg` (stored at `src/views/assets/iu-logo.svg`).
* **Dual-Resilience Fallback Pattern**:
  ```html
  <img src="/assets/iu-logo.svg" 
       onerror="this.onerror=null; this.src='https://upload.wikimedia.org/wikipedia/commons/1/1e/%C4%B0stanbul_%C3%9Cniversitesi_Logo.svg';" 
       alt="İstanbul Üniversitesi Logosu">
  ```
* **Standard Dimensions**:
  * **Navigation Header Pill**: `32px × 32px`
  * **Hero Badge Container**: `44px × 44px`
  * **Footer Brand Stamp**: `36px × 36px`
  * **404 Guardian Shield**: `48px × 48px`

### 2.2 Istanbul University Entrance Gate Silhouette (Background Watermark)
The iconic **Tarihi Beyazıt Giriş Kapısı (1865-1866)** architectural vector silhouette is embedded as a subtle, fixed background layer across all web views.

* **Vector SVG Path**: Served via `GET /assets/iu-gate-silhouette.svg` (stored at `src/views/assets/iu-gate-silhouette.svg`).
* **CSS Background Specification**:
  ```css
  body {
      background-color: #FAF9F6;
      background-image: url('/assets/iu-gate-silhouette.svg');
      background-repeat: no-repeat;
      background-position: center 40px;
      background-size: 1160px auto;
      background-attachment: fixed;
  }
  ```
* **HTML Watermark Layer Specification**:
  ```html
  <div class="bg-gate-watermark" aria-hidden="true">
      <img src="/assets/iu-gate-silhouette.svg" alt="" onerror="this.style.display='none';">
  </div>
  ```
* **Layering & Opacity Rule**:
  * Opacity inside SVG: `0.045` fill, `0.09` hairline stroke in `#004225`.
  * `pointer-events: none; z-index: 0;` — strictly behind all `.page-container` (`z-index: 1`) content.

---

## 3. Color Palette & Semantic Tokens

```text
┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐
│ Alabaster Canvas │  │   Card Surface   │  │ Deep Forest Grn  │  │   Abyssal Ink    │
│     #FAF9F6      │  │     #FFFFFF      │  │     #004225      │  │     #0F2A1D      │
│ Intensive Bg     │  │  Flat Paper Card │  │ Buttons & Detail │  │  High Contrast   │
└──────────────────┘  └──────────────────┘  └──────────────────┘  └──────────────────┘
```

### 3.1 Surfaces & Layout Canvas
| Token Name | Hex Code | RGB | HSL | Semantic Role |
| :--- | :--- | :--- | :--- | :--- |
| `--color-canvas` | `#FAF9F6` | `250, 249, 246` | `45°, 25%, 97%` | Primary intensive warm alabaster page canvas |
| `--color-canvas-subtle` | `#F3F2EC` | `243, 242, 236` | `51°, 22%, 94%` | Secondary background wash for section grouping |
| `--color-surface-card` | `#FFFFFF` | `255, 255, 255` | `0°, 0%, 100%` | Pure white paper surface for bento cards & tables |
| `--color-surface-inset` | `#F5F4EE` | `245, 244, 238` | `51°, 23%, 95%` | Recessed wells, metrics containers, table headers |
| `--color-surface-nav` | `rgba(250, 249, 246, 0.94)` | — | — | Floating glass pill backdrop (`backdrop-filter: blur(20px)`) |

### 3.2 Hairlines & Structural Dividers
| Token Name | Hex Code | Purpose |
| :--- | :--- | :--- |
| `--color-hairline` | `#E2DFD6` | Crisp 1px warm stone border for cards, pills, and outer boundaries |
| `--color-hairline-subtle` | `#EBE8E0` | Subtle 1px divider for table rows, feed separators, and meta blocks |
| `--color-hairline-hover` | `#C8DBCF` | Interactive card and button border hover state |

### 3.3 Typography Inks (High-Contrast Hierarchy)
| Token Name | Hex Code | Contrast on `#FFFFFF` | Contrast on `#FAF9F6` | Usage |
| :--- | :--- | :---: | :---: | :--- |
| `--color-text-display` | `#004225` | **11.2:1 (AAA)** | **10.6:1 (AAA)** | Monumental serif headlines, stat numbers |
| `--color-text-title` | `#0F2A1D` | **14.8:1 (AAA)** | **14.0:1 (AAA)** | Card titles, navigation logo, section headings |
| `--color-text-body` | `#1C2920` | **12.5:1 (AAA)** | **11.8:1 (AAA)** | Default interface body copy, descriptions |
| `--color-text-muted` | `#55685C` | **5.4:1 (AA)** | **5.1:1 (AA)** | Secondary helper text, timestamps, labels |
| `--color-text-light` | `#7E9084` | **3.8:1 (UI)** | **3.6:1 (UI)** | Footers, copyright notices, disabled items |

### 3.4 Buttons, Actions & Semantic Flares
| Semantic Token | Hex Code | Background Tint | Border Tint | Application |
| :--- | :--- | :--- | :--- | :--- |
| `--color-primary-action`| `#004225` | `#E8F0EB` | `#C8DBCF` | Primary CTA buttons, active links, filter tabs |
| `--color-primary-hover` | `#065A35` | `#C8DBCF` | `#A3C2AD` | Hover & pressed states for primary triggers |
| `--color-bio-mint` | `#059669` | `#D1FAE5` | `#A7F3D0` | PostgreSQL live database, 200 OK status |
| `--color-heritage-gold` | `#B45309` | `#FEF3C7` | `#FDE68A` | 1453 University Heritage, Bench pill |
| `--color-signal-blue` | `#0284C7` | `#E0F2FE` | `#BAE6FD` | Documentation chip, telemetry endpoint |
| `--color-rose-shield` | `#E11D48` | `#FFE4E6` | `#FECDD3` | 404 Guardian Shield, 5xx Guardian alert |

---

## 4. Spatial System & Corner Radii

### 4.1 The Binary Radius System (Strict Standard)
To eliminate arbitrary, chaotic visual decisions across screens, every single UI element conforms to the **Binary Radius Rule**:

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

### 4.2 Spacing Metric (Base-4 Scale)
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

### 4.3 Strict Zero Drop Shadows
```css
--elevation-none: none !important;
```
* **No Drop Shadows**: All `box-shadow` values are set to `none`.
* **Depth Technique**: Elevation is communicated solely by placing `#FFFFFF` surfaces on `#FAF9F6` canvas, bounded by crisp `1px solid #E2DFD6` hairlines, and nesting recessed `#F5F4EE` wells.

---

## 5. Typography Hierarchy & Font Stacks

```css
--font-display: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
--font-sans:    'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
--font-mono:    'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
```

```text
Inter (700/800) ──► Monumental Headlines & Historical Numbers (1453, 50K+)
Inter (400/600) ──► High-Legibility Interface Copy, Forms & Buttons
Inter (500)     ──► REST Endpoints, Status Codes, Telemetry & JSON
```

*Note: All numeric interface elements enable OpenType proportional and tabular figures via `font-feature-settings: 'lnum', 'tnum';`.*

---

## 6. Page-by-Page Architectural Implementation Checklist

Every page of the application has been designed according to this specification:
1. **Home / Evaluation Workbench (`/`, `/home`, `/bench`, `/temporary-bench`)**:
   * Historic Beyazıt Gate silhouette watermark in `#FAF9F6` background.
   * Floating centered capsule navigation pill (`9999px`, `backdrop-filter: blur(20px)`).
   * Istanbul University logo in header, hero, evaluation bench, and footer.
   * 4px bento cards with zero drop shadows.
   * `#004225` buttons, badges, and interactive details.
   * Author & System Architect credits: Nazife Türk (`nazife.turk@ogr.iu.edu.tr`).
2. **Mezun Rehberi (`/users`, `/users/create`, `/users/{id}`)**:
   * Gate silhouette backdrop, high-density tabular worksheet, searchable directory, verified alumni badges.
3. **Kariyer Panosu (`/jobs`, `/jobs/create`, `/jobs/{id}`)**:
   * Gate silhouette backdrop, enterprise job postings, `#004225` action buttons.
4. **Duyurular (`/announcements`)**:
   * Gate silhouette backdrop, institutional communications feed, categories.
5. **Generic 404 Guardian (`/404`)**:
   * Gate silhouette backdrop, alabaster canvas, deep forest green error code, official IU logo, and auto-recovery links.
6. **Hakkında (`/about`)**:
   * Gate silhouette backdrop, architectural specifications, HTTP routing fundamentals, REST principles.
