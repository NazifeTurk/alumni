# 🎨 Brand Identity & Design System (`brand.md`)

> **Course**: Web Programming (Web Programlama) — Istanbul University  
> **Instructor**: Assoc. Prof. Dr. Emre Akadal  
> **Student**: Nazife Türk (`nazife.turk@ogr.iu.edu.tr`)  
> **Repository**: [https://github.com/NazifeTurk/alumni](https://github.com/NazifeTurk/alumni)  
> **Reference Links**: ⬅️ [Return to Main Documentation (README.md)](README.md) &bull; 🗄️ [Data Design Documentation (ddd.md)](ddd.md)

---

## 📌 1. Brand Overview & Institutional Philosophy

The **Istanbul University Alumni Tracking System (`alumni`)** bridges over five centuries of academic excellence (established in 1453) with modern, enterprise-grade cloud software engineering.

```text
┌────────────────────────────────────────────────────────────────────────┐
│                        ISTANBUL UNIVERSITY                             │
│                  Alumni Tracking & Career Network                      │
│                  "Gelenekten Geleceğe Güçlü Köprü"                     │
└───────────────────────────────────┬────────────────────────────────────┘
                                    │
       ┌────────────────────────────┼────────────────────────────┐
       ▼                            ▼                            ▼
 🏛️ 1453 Legacy              🚀 Modern Tech              🤝 Alumni Network
 Academic Heritage,          C# .NET 7, PostgreSQL 16,   Mentorship, Careers,
 Preeminence & Trust         Docker, Clean Architecture   Homecoming & Unity
```

### Brand Personality & Core Values
* **Prestigious & Trustworthy**: Reflecting the historic prestige of Istanbul University.
* **Modern & Accessible**: Responsive, clean dark-mode UI with high contrast and readable typography.
* **Connected & Collaborative**: Centered around mentorship, referral pipelines, and institutional solidarity.
* **Engineering Rigor**: Professional software craftsmanship backed by strict architectural design tokens.

---

## 🎓 2. Logomark, Crest & Visual Identity

### 2.1 Primary Crest Anatomy
The primary mark combines the academic graduation cap (mortarboard), open knowledge ledger, and the historic gateway motif:

```text
       ┌───────────────┐
       │     🎓        │  ◄── Graduation Cap: Academic Achievement & Milestones
       │  İSTANBUL     │  ◄── University Title: Deep Academic Authority
       │  ÜNİVERSİTESİ │  ◄── Institutional Subtitle: Continuity (1453)
       └───────────────┘
```

* **Icon Symbol**: `🎓` (Academic Cap) wrapped in an Indigo-Emerald gradient badge with subtle glassmorphic stroke.
* **Badge Dimensions**: 44px $\times$ 44px on desktop navigation, 36px on mobile viewports.
* **Gradient Fill**: `linear-gradient(135deg, #4f46e5 0%, #10b981 100%)`.
* **Border Token**: `1px solid rgba(255, 255, 255, 0.15)`.

### 2.2 Clear Space & Safe Margin
To maintain legibility and visual authority:
* Keep a minimum margin of **`16px`** around the logomark in all web viewports.
* Never place high-contrast distracting photographic textures directly behind the primary emblem.

### 2.3 Logo Misuse Rules
| ❌ Do Not | ✅ Correct Approach |
| :--- | :--- |
| Do not stretch or distort the aspect ratio. | Scale proportionally maintaining 1:1 square icon bounds. |
| Do not change gradient colors to unapproved tones. | Always use the canonical Indigo $\rightarrow$ Emerald palette. |
| Do not use illegible low-contrast text colors. | Pair only with `--text-main (#f8fafc)` or `--text-muted (#94a3b8)`. |

---

## 🎨 3. Color Palette & Design Tokens

The palette is engineered specifically for deep-mode contrast, eye comfort, and clear visual hierarchy in compliance with **WCAG 2.1 AA** standards:

```text
┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐
│   IU Midnight    │  │ Academic Indigo  │  │  Imperial Gold   │  │  Emerald Active  │
│     #0a0f1d      │  │     #4f46e5      │  │     #f59e0b      │  │     #10b981      │
│  Base Background │  │   Primary Action │  │ Heritage / Bench │  │ PostgreSQL Live  │
└──────────────────┘  └──────────────────┘  └──────────────────┘  └──────────────────┘
```

### 3.1 Primary Brand Palette
| Color Name | Hex Token | RGB | HSL | Semantic Role |
| :--- | :--- | :--- | :--- | :--- |
| **IU Midnight Navy** | `#0a0f1d` | `10, 15, 29` | `224°, 49%, 8%` | Primary canvas & body background |
| **Deep Card Surface** | `#11192d` | `17, 25, 45` | `223°, 45%, 12%` | Bento cards, panels, and module containers |
| **Surface Card Hover** | `#162039` | `22, 32, 57` | `223°, 44%, 15%` | Interactive hover state for cards |
| **Academic Indigo** | `#4f46e5` | `79, 70, 229` | `243°, 75%, 59%` | Primary buttons, brand accents, key CTAs |
| **Indigo Hover** | `#4338ca` | `67, 56, 202` | `245°, 58%, 51%` | Hover and active interaction states |
| **Imperial Gold** | `#f59e0b` | `245, 158, 11` | `38°, 92%, 50%` | 1453 Heritage, Bench pill, VIP highlights |

### 3.2 Functional & Status Palette
| Semantic Role | Hex Token | RGB | Purpose / Association |
| :--- | :--- | :--- | :--- |
| **Emerald Active** | `#10b981` | `16, 185, 129` | Live database health, successful 200/201 responses, verified alumni |
| **Sky Cyan** | `#38bdf8` | `56, 189, 248` | REST API routes, Swagger documentation, technical highlights |
| **Coral Rose** | `#f43f5e` | `244, 63, 94` | 404 Guardian badge, 5xx crash shield, validation rejection notices |
| **Orchid Purple** | `#a855f7` | `168, 85, 247` | Mentorship programs, annual Homecoming Gala, cultural events |

### 3.3 Typography & Surface Contrast
| Token Name | Hex Token | Opacity / Level | Application |
| :--- | :--- | :--- | :--- |
| `--text-main` | `#f8fafc` | 100% | Primary headings, button labels, high-emphasis text |
| `--text-muted` | `#94a3b8` | 100% | Secondary descriptions, subheadings, metadata |
| `--text-dim` | `#64748b` | 100% | Footers, disabled states, copyright notices |
| `--border` | `#23314d` | 100% | Structural borders, card boundaries, table separators |
| `--border-focus`| `#38bdf8` | 100% | Input focus rings, interactive card focus states |

---

## 🔤 4. Typography Scale & Hierarchy

The visual identity employs **Plus Jakarta Sans** for display and interface text, paired with **JetBrains Mono** for endpoints, status codes, and code snippets:

```text
Display Heading (H1)  ──► Plus Jakarta Sans 800 (2.85rem / 45px)
Section Heading (H2)  ──► Plus Jakarta Sans 800 (1.85rem / 30px)
Card Heading (H3)     ──► Plus Jakarta Sans 700 (1.25rem / 20px)
Body Lead Paragraph   ──► Plus Jakarta Sans 400 (1.18rem / 19px)
Interface Body        ──► Plus Jakarta Sans 500 (0.95rem / 15px)
Monospace Telemetry   ──► JetBrains Mono     500 (0.88rem / 14px)
```

### Typography Scale Matrix
| Level | Font Family | Weight | Size / Line-Height | Tracking | Usage Context |
| :--- | :--- | :---: | :--- | :---: | :--- |
| **H1 Display** | `Plus Jakarta Sans` | 800 | `2.85rem` / `1.15` | `-0.03em` | Hero section main banner headline |
| **H2 Section** | `Plus Jakarta Sans` | 800 | `1.85rem` / `1.25` | `-0.02em` | Major module headings (Pillars, Feeds) |
| **H3 Card** | `Plus Jakarta Sans` | 700 | `1.25rem` / `1.35` | `-0.01em` | Pillar cards, showcase container titles |
| **H4 Subtitle**| `Plus Jakarta Sans` | 700 | `1.05rem` / `1.40` | `0.00em` | Job titles, announcement headers |
| **Body Lead** | `Plus Jakarta Sans` | 400 | `1.18rem` / `1.65` | `0.00em` | Hero introductory paragraph |
| **Body Regular**| `Plus Jakarta Sans` | 500 | `0.95rem` / `1.60` | `0.00em` | General paragraph text, descriptions |
| **Caption/Tag**| `Plus Jakarta Sans` | 700 | `0.75rem` / `1.20` | `+0.05em` | Status pills, category badges, dates |
| **Monospace** | `JetBrains Mono` | 500 | `0.88rem` / `1.50` | `0.00em` | URL endpoints, JSON keys, cURL snippets |

---

## 🧩 5. UI Component Design Tokens

### 5.1 Buttons & Interactive Triggers
* **Primary CTA (`.btn-primary`)**:
  * Background: `linear-gradient(135deg, #4f46e5 0%, #6366f1 100%)`
  * Text: `#ffffff` (Bold 700)
  * Shadow: `0 4px 16px rgba(79, 70, 229, 0.45)`
  * Hover: `transform: translateY(-2px)` with expanded shadow `0 6px 22px rgba(79, 70, 229, 0.6)`.
* **Secondary Surface (`.btn-secondary`)**:
  * Background: `#11192d`, Border: `1px solid #23314d`, Text: `#f8fafc`.
* **Status Action (`.btn-outline-emerald`)**:
  * Background: `rgba(16, 185, 129, 0.1)`, Border: `rgba(16, 185, 129, 0.3)`, Text: `#34d399`.

### 5.2 Badges & Status Pills
* **Live Stack Pill**:
  * Pulsing emerald dot (`animation: pulse-glow 2s infinite`) + `PostgreSQL 16 • EF Core 7 • Docker Live`.
* **Lab Evaluation Bench Pill**:
  * Amber border + gold text (`🧪 Bench: temporary-bench`).
* **404 Guardian Badge**:
  * Rose background + coral text (`🛡️ 404 Guardian Error Handler`).

### 5.3 Surface Glassmorphism & Elevation
* **Navigation Header**:
  * `backdrop-filter: blur(16px); -webkit-backdrop-filter: blur(16px);`
  * `background: rgba(10, 15, 29, 0.85);`
  * `border-bottom: 1px solid #23314d;`
* **Card Radius**:
  * Standard: `16px` (`--radius-lg`)
  * Buttons / Inputs: `12px` (`--radius-md`)
  * Tags / Pills: `9999px` (Full capsule)

---

## 📢 6. Voice, Tone & Communication Guidelines

### 6.1 Voice Attributes
* **Academic yet Accessible**: Avoid bureaucratic, stiff language while preserving the dignity of Istanbul University.
* **Empowering & Forward-Looking**: Emphasize career growth, student mentorship, and industry impact.
* **Transparent & Precise**: In API error responses (404 Guardian, 5xx Guardian), explain exact failure causes with actionable developer hints.

### 6.2 Standardized Vocabulary Table
| Preferred Term (TR) | Preferred Term (EN) | Deprecated / Avoid |
| :--- | :--- | :--- |
| **Mezun Takip Sistemi** | Alumni Tracking System | Eski Öğrenci Portalı |
| **Mezun Rehberi** | Alumni Directory | İsim Listesi |
| **Kariyer Havuzu & İlan Panosu**| Career & Job Board | İş İlanları Tablosu |
| **Mentorluk Ağı** | Mentorship Network | Danışmanlık Grubu |
| **Homecoming Galası** | Annual Homecoming Gala | Mezunlar Yemeği |
| **Değerlendirme Tezgahı** | Evaluation Bench | Test Sayfası |

---

## 🔗 7. Cross-Documentation & Anchors

* 📖 [Master Architecture & System Contract (`README.md`)](README.md)
* 🗄️ [Data Design Documentation & Persistence Proofs (`ddd.md`)](ddd.md)
* 🧪 [Postman Automated Test Suite (`postman_alumni_invalid_requests.json`)](postman_alumni_invalid_requests.json)
* 🌐 [Live Homepage Template (`src/views/home.html`)](src/views/home.html)
* 🛡️ [Generic 404 Error Page (`src/views/404.html`)](src/views/404.html)
