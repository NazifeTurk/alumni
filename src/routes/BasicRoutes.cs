using System.Globalization;

namespace Alumni.Routes;

public static class BasicRoutes
{
    public static void MapBasicRoutes(this WebApplication app)
    {
        // 1 & 5. GET / -> "ok" for API/curl clients, or temporary main page for browsers
        app.MapGet("/", (HttpContext context) =>
        {
            var acceptHeader = context.Request.Headers.Accept.ToString();
            var formatQuery = context.Request.Query["format"].ToString();

            // If explicitly requested as text/json or via curl without text/html
            if (formatQuery.Equals("text", StringComparison.OrdinalIgnoreCase) ||
                formatQuery.Equals("json", StringComparison.OrdinalIgnoreCase) ||
                (!acceptHeader.Contains("text/html") && !string.IsNullOrEmpty(acceptHeader) && acceptHeader != "*/*"))
            {
                return Results.Text("ok", "text/plain");
            }

            // Return temporary main page (HTML view)
            return Results.Content(GetMainHtmlPage(), "text/html");
        });

        // 1 (Dedicated text endpoint). GET /ok -> "ok"
        app.MapGet("/ok", () => Results.Text("ok", "text/plain"));

        // 2. GET /hello -> "Hello, World!"
        app.MapGet("/hello", () => Results.Text("Hello, World!", "text/plain"));

        // 3. GET /hello/{name} -> "Hello, {name}!" (e.g., /hello/emre -> "Hello, Emre!")
        app.MapGet("/hello/{name}", (string name) =>
        {
            // Capitalize first letter for clean formatting if desired, while preserving exact name
            string formattedName = string.IsNullOrWhiteSpace(name) ? "World" : name;
            return Results.Text($"Hello, {formattedName}!", "text/plain");
        });

        // 4. GET /sum/{number1}/{number2} -> performs addition with validation (400 Bad Request if non-numeric)
        app.MapGet("/sum/{number1}/{number2}", (string number1, string number2) =>
        {
            bool isNum1Valid = double.TryParse(number1, NumberStyles.Any, CultureInfo.InvariantCulture, out double n1);
            bool isNum2Valid = double.TryParse(number2, NumberStyles.Any, CultureInfo.InvariantCulture, out double n2);

            if (!isNum1Valid || !isNum2Valid)
            {
                return Results.BadRequest(new
                {
                    status = "error",
                    code = 400,
                    message = "Both parameters must be valid numeric values.",
                    invalidParams = new
                    {
                        number1 = isNum1Valid ? null : number1,
                        number2 = isNum2Valid ? null : number2
                    }
                });
            }

            double result = n1 + n2;
            return Results.Ok(new
            {
                status = "success",
                operation = "sum",
                number1 = n1,
                number2 = n2,
                result = result,
                expression = $"{n1} + {n2} = {result}"
            });
        });

        // 5. GET /home -> temporary one main page (HTML view)
        app.MapGet("/home", () => Results.Content(GetMainHtmlPage(), "text/html"));

        // 6. GET /about -> temporary about page (HTML view)
        app.MapGet("/about", () => Results.Content(GetAboutHtmlPage(), "text/html"));
    }

    private static string GetMainHtmlPage()
    {
        return """
        <!DOCTYPE html>
        <html lang="tr">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>İstanbul Üniversitesi Mezun Takip Sistemi</title>
            <style>
                :root {
                    --bg: #0f172a;
                    --card-bg: #1e293b;
                    --border: #334155;
                    --text: #f8fafc;
                    --text-muted: #94a3b8;
                    --primary: #4f46e5;
                    --primary-hover: #4338ca;
                    --accent: #10b981;
                    --accent-blue: #38bdf8;
                }
                * { box-sizing: border-box; margin: 0; padding: 0; }
                body {
                    font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif;
                    background-color: var(--bg);
                    color: var(--text);
                    line-height: 1.6;
                    padding: 2rem 1rem;
                }
                .container { max-width: 1040px; margin: 0 auto; }
                header {
                    border-bottom: 1px solid var(--border);
                    padding-bottom: 1.5rem;
                    margin-bottom: 2.5rem;
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    flex-wrap: wrap;
                    gap: 1rem;
                }
                .logo-area { display: flex; align-items: center; gap: 0.75rem; }
                .logo-icon { font-size: 2rem; }
                h1 { font-size: 1.5rem; font-weight: 700; color: #fff; line-height: 1.2; }
                .logo-sub { color: var(--text-muted); font-size: 0.85rem; }
                nav { display: flex; align-items: center; gap: 1.2rem; flex-wrap: wrap; }
                nav a {
                    color: var(--text-muted);
                    text-decoration: none;
                    font-weight: 500;
                    font-size: 0.95rem;
                    transition: color 0.2s;
                }
                nav a:hover, nav a.active { color: #fff; }
                nav a.btn-nav {
                    background: rgba(16, 185, 129, 0.15);
                    color: #34d399;
                    border: 1px solid rgba(16, 185, 129, 0.3);
                    padding: 0.4rem 0.9rem;
                    border-radius: 6px;
                    font-weight: 600;
                }
                nav a.btn-nav:hover { background: rgba(16, 185, 129, 0.25); color: #fff; }

                /* Hero Section */
                .hero {
                    background: linear-gradient(135deg, rgba(79, 70, 229, 0.2), rgba(16, 185, 129, 0.15));
                    border: 1px solid var(--border);
                    border-radius: 16px;
                    padding: 3rem 2.5rem;
                    margin-bottom: 2.5rem;
                    box-shadow: 0 10px 30px rgba(0,0,0,0.3);
                }
                .badge-live {
                    display: inline-flex;
                    align-items: center;
                    gap: 0.5rem;
                    background-color: rgba(16, 185, 129, 0.15);
                    color: var(--accent);
                    padding: 0.35rem 0.9rem;
                    border-radius: 9999px;
                    font-size: 0.85rem;
                    font-weight: 600;
                    border: 1px solid rgba(16, 185, 129, 0.3);
                    margin-bottom: 1.25rem;
                }
                .pulse-dot {
                    width: 8px;
                    height: 8px;
                    background-color: #10b981;
                    border-radius: 50%;
                    box-shadow: 0 0 8px #10b981;
                }
                .hero h2 { font-size: 2.1rem; font-weight: 800; line-height: 1.25; margin-bottom: 1rem; color: #fff; }
                .hero p { color: #cbd5e1; font-size: 1.15rem; max-width: 800px; margin-bottom: 1.75rem; }
                .hero-actions { display: flex; gap: 1rem; flex-wrap: wrap; }
                .btn {
                    display: inline-flex;
                    align-items: center;
                    gap: 0.5rem;
                    padding: 0.75rem 1.4rem;
                    border-radius: 8px;
                    font-weight: 600;
                    text-decoration: none;
                    transition: transform 0.15s, background 0.2s;
                }
                .btn-primary { background: var(--primary); color: #fff; }
                .btn-primary:hover { background: var(--primary-hover); transform: translateY(-1px); }
                .btn-secondary { background: var(--card-bg); color: var(--text); border: 1px solid var(--border); }
                .btn-secondary:hover { background: #334155; transform: translateY(-1px); }

                /* Stats Bar */
                .stats-grid {
                    display: grid;
                    grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
                    gap: 1rem;
                    margin-bottom: 2.5rem;
                }
                .stat-box {
                    background: var(--card-bg);
                    border: 1px solid var(--border);
                    border-radius: 12px;
                    padding: 1.25rem;
                    text-align: center;
                }
                .stat-value { font-size: 1.8rem; font-weight: 800; color: var(--accent-blue); margin-bottom: 0.25rem; }
                .stat-label { color: var(--text-muted); font-size: 0.9rem; font-weight: 500; }

                /* Pillars (3 Cards) */
                .section-title { font-size: 1.5rem; font-weight: 700; margin-bottom: 1.25rem; color: #fff; }
                .pillars-grid {
                    display: grid;
                    grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
                    gap: 1.5rem;
                    margin-bottom: 2.5rem;
                }
                .pillar-card {
                    background: var(--card-bg);
                    border: 1px solid var(--border);
                    border-radius: 14px;
                    padding: 1.75rem;
                    display: flex;
                    flex-direction: column;
                    justify-content: space-between;
                    transition: transform 0.2s, border-color 0.2s;
                }
                .pillar-card:hover { transform: translateY(-3px); border-color: var(--primary); }
                .pillar-icon { font-size: 2.2rem; margin-bottom: 1rem; }
                .pillar-card h3 { font-size: 1.25rem; font-weight: 700; color: #fff; margin-bottom: 0.75rem; }
                .pillar-card p { color: var(--text-muted); font-size: 0.95rem; margin-bottom: 1.25rem; line-height: 1.5; }
                .pillar-link { color: var(--accent-blue); font-weight: 600; text-decoration: none; font-size: 0.95rem; }
                .pillar-link:hover { text-decoration: underline; }

                /* Announcements Showcase */
                .showcase {
                    background: var(--card-bg);
                    border: 1px solid var(--border);
                    border-radius: 14px;
                    padding: 1.75rem;
                    margin-bottom: 2.5rem;
                }
                .showcase-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 1.25rem; }
                .news-item {
                    border-top: 1px solid var(--border);
                    padding: 1rem 0;
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    gap: 1rem;
                }
                .news-title { font-weight: 600; color: #fff; }
                .news-meta { color: var(--text-muted); font-size: 0.85rem; margin-top: 0.25rem; }
                .category-badge {
                    display: inline-block;
                    padding: 0.2rem 0.6rem;
                    border-radius: 9999px;
                    font-size: 0.75rem;
                    font-weight: 700;
                    text-transform: uppercase;
                }
                .cat-reunion { background: rgba(59, 130, 246, 0.2); color: #60a5fa; }
                .cat-mentorship { background: rgba(16, 185, 129, 0.2); color: #34d399; }

                /* Lab / Grading details panel */
                details.grading-panel {
                    background: rgba(15, 23, 42, 0.6);
                    border: 1px dashed var(--border);
                    border-radius: 12px;
                    padding: 1.25rem;
                    margin-top: 2rem;
                }
                details.grading-panel summary {
                    font-weight: 700;
                    color: var(--accent-blue);
                    cursor: pointer;
                    font-size: 1.05rem;
                    outline: none;
                }
                .grading-content { margin-top: 1.25rem; }
                table { width: 100%; border-collapse: collapse; margin-top: 1rem; }
                th, td {
                    text-align: left;
                    padding: 0.65rem 0.85rem;
                    border-bottom: 1px solid var(--border);
                    font-size: 0.9rem;
                }
                th { color: var(--text-muted); font-weight: 600; background: rgba(0,0,0,0.2); }
                .method {
                    display: inline-block;
                    padding: 0.2rem 0.5rem;
                    border-radius: 4px;
                    font-family: monospace;
                    font-size: 0.8rem;
                    font-weight: 700;
                }
                .method-get { background: rgba(16, 185, 129, 0.2); color: #34d399; }
                a.endpoint { color: #818cf8; text-decoration: none; font-family: monospace; }
                a.endpoint:hover { text-decoration: underline; }

                footer {
                    margin-top: 3rem;
                    border-top: 1px solid var(--border);
                    padding-top: 1.5rem;
                    text-align: center;
                    color: var(--text-muted);
                    font-size: 0.9rem;
                }
            </style>
        </head>
        <body>
            <div class="container">
                <header>
                    <div class="logo-area">
                        <span class="logo-icon">🎓</span>
                        <div>
                            <h1>İstanbul Üniversitesi</h1>
                            <p class="logo-sub">Alumni Tracking &amp; Career Network Portal</p>
                        </div>
                    </div>
                    <nav>
                        <a href="/" class="active">Ana Sayfa</a>
                        <a href="/users">Mezunlar</a>
                        <a href="/announcements">Duyurular</a>
                        <a href="/about">Hakkında</a>
                        <a href="/api/swagger" class="btn-nav">Swagger API ⚡</a>
                    </nav>
                </header>

                <div class="hero">
                    <div class="badge-live">
                        <span class="pulse-dot"></span>
                        PostgreSQL 16 Engine • Entity Framework Core • Docker Stack Live
                    </div>
                    <h2>Mezunlarımızla Bağlantıda Kalın, Geleceği Birlikte Şekillendirin</h2>
                    <p>
                        İstanbul Üniversitesi mezunları ve öğrencileri arasında güçlü bir köprü: 
                        Profesyonel ağınızı genişletin, sektörde tecrübeli mezunlardan mentorluk alın ve kariyer fırsatlarını yakalayın.
                    </p>
                    <div class="hero-actions">
                        <a href="/users" class="btn btn-primary">👥 Mezunlar Ağını Keşfet</a>
                        <a href="/announcements" class="btn btn-secondary">📢 Duyurular ve Etkinlikler</a>
                        <a href="/api/swagger" class="btn btn-secondary">⚡ API Test Paneli</a>
                    </div>
                </div>

                <div class="stats-grid">
                    <div class="stat-box">
                        <div class="stat-value">IU</div>
                        <div class="stat-label">Köklü Üniversite Mirası</div>
                    </div>
                    <div class="stat-box">
                        <div class="stat-value">PostgreSQL 16</div>
                        <div class="stat-label">Kalıcı Veritabanı</div>
                    </div>
                    <div class="stat-box">
                        <div class="stat-value">MVC + API</div>
                        <div class="stat-label">Çift Katmanlı Mimari</div>
                    </div>
                    <div class="stat-box">
                        <div class="stat-value">Docker</div>
                        <div class="stat-label">Tek Komutla Canlı</div>
                    </div>
                </div>

                <h3 class="section-title">🌟 Temel Platform Modülleri</h3>
                <div class="pillars-grid">
                    <div class="pillar-card">
                        <div>
                            <div class="pillar-icon">🤝</div>
                            <h3>Networking &amp; Profesyonel Topluluk</h3>
                            <p>
                                Fakülte, bölüm ve mezuniyet yılına göre mezunları filtreleyin. Mezunlar arası doğrudan bağlantılar kurun, sektör deneyimlerini paylaşın ve profesyonel çevrenizi büyütün.
                            </p>
                        </div>
                        <a href="/users" class="pillar-link">Mezun Rehberini Görüntüle &rarr;</a>
                    </div>

                    <div class="pillar-card">
                        <div>
                            <div class="pillar-icon">🎯</div>
                            <h3>Mentorship &amp; Kariyer Danışmanlığı</h3>
                            <p>
                                Sektörde deneyim kazanmış kıdemli mezunlarımız, son sınıf öğrencilerine ve yeni mezunlara birebir kariyer rehberliği, mülakat hazırlığı ve teknik mentorluk sağlar.
                            </p>
                        </div>
                        <a href="/announcements" class="pillar-link">Mentorluk Başvurularını İncele &rarr;</a>
                    </div>

                    <div class="pillar-card">
                        <div>
                            <div class="pillar-icon">💼</div>
                            <h3>Kariyer Havuzu &amp; Şirket Referansları</h3>
                            <p>
                                Mezunlarımızın çalıştığı şirketlerdeki açık iş ve staj pozisyonları, şirket içi yönlendirme (referral) imkanları ve üniversitemize özel kariyer etkinlikleri.
                            </p>
                        </div>
                        <a href="/announcements" class="pillar-link">Fırsat Panosuna Git &rarr;</a>
                    </div>
                </div>

                <div class="showcase">
                    <div class="showcase-header">
                        <h3 style="font-size: 1.25rem; font-weight: 700; color: #fff;">📌 Güncel Duyurular ve Etkinlikler</h3>
                        <a href="/announcements" class="pillar-link" style="font-size: 0.9rem;">Tümünü Gör &rarr;</a>
                    </div>
                    <div class="news-item">
                        <div>
                            <div class="news-title">2026 Annual Alumni Homecoming Gala</div>
                            <div class="news-meta">Alumni Association &bull; 1 Mayıs 2026 &bull; Merkez Kampüs</div>
                        </div>
                        <span class="category-badge cat-reunion">Reunion</span>
                    </div>
                    <div class="news-item">
                        <div>
                            <div class="news-title">Tech Mentorship Cohort 2026 Applications Open</div>
                            <div class="news-meta">Career Center &bull; 10 Mayıs 2026 &bull; Mühendislik Fakültesi</div>
                        </div>
                        <span class="category-badge cat-mentorship">Mentorship</span>
                    </div>
                </div>

                <!-- Akademik Değerlendirme & Test Paneli (Emre Hoca ve Notlandırma İçin) -->
                <details class="grading-panel">
                    <summary>🛠️ Akademik Laboratuvar &amp; Değerlendirme Rotaları (Week 02 &ndash; Week 05)</summary>
                    <div class="grading-content">
                        <p style="color: var(--text-muted); font-size: 0.9rem; margin-bottom: 1rem;">
                            Ders kapsamında (Doç. Dr. Emre Akadal) tahtada atanan temel URL anatomi, parametrik yönlendirme ve API test linkleri:
                        </p>
                        <table>
                            <thead>
                                <tr>
                                    <th>Method</th>
                                    <th>Rota</th>
                                    <th>Açıklama</th>
                                    <th>Test Linki</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td><span class="method method-get">GET</span></td>
                                    <td><code>/ok</code></td>
                                    <td>Sağlık ve Başlangıç Durumu</td>
                                    <td><a class="endpoint" href="/ok">/ok</a></td>
                                </tr>
                                <tr>
                                    <td><span class="method method-get">GET</span></td>
                                    <td><code>/hello</code></td>
                                    <td>Statik Selamlama</td>
                                    <td><a class="endpoint" href="/hello">/hello</a></td>
                                </tr>
                                <tr>
                                    <td><span class="method method-get">GET</span></td>
                                    <td><code>/hello/{name}</code></td>
                                    <td>Parametrik Selamlama</td>
                                    <td><a class="endpoint" href="/hello/emre">/hello/emre</a></td>
                                </tr>
                                <tr>
                                    <td><span class="method method-get">GET</span></td>
                                    <td><code>/sum/{n1}/{n2}</code></td>
                                    <td>Matematiksel Toplama &amp; 400 Doğrulama</td>
                                    <td><a class="endpoint" href="/sum/15/27">/sum/15/27</a></td>
                                </tr>
                                <tr>
                                    <td><span class="method method-get">GET</span></td>
                                    <td><code>/api/users</code></td>
                                    <td>Kalıcı PostgreSQL Mezun Listesi (JSON)</td>
                                    <td><a class="endpoint" href="/api/users">/api/users</a></td>
                                </tr>
                                <tr>
                                    <td><span class="method method-get">GET</span></td>
                                    <td><code>/api/announcements</code></td>
                                    <td>Kalıcı Duyurular Listesi (JSON)</td>
                                    <td><a class="endpoint" href="/api/announcements">/api/announcements</a></td>
                                </tr>
                                <tr>
                                    <td><span class="method method-get">GET</span></td>
                                    <td><code>/api/swagger</code></td>
                                    <td>Etkileşimli Swagger API Arayüzü</td>
                                    <td><a class="endpoint" href="/api/swagger">/api/swagger</a></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </details>

                <footer>
                    <p>İstanbul Üniversitesi &bull; Web Programlama &bull; Nazife Türk (nazife.turk@ogr.iu.edu.tr)</p>
                </footer>
            </div>
        </body>
        </html>
        """;
    }

    private static string GetAboutHtmlPage()
    {
        return """
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>About - Alumni Tracking System</title>
            <style>
                :root {
                    --primary: #4f46e5;
                    --bg: #0f172a;
                    --card-bg: #1e293b;
                    --text: #f8fafc;
                    --text-muted: #94a3b8;
                    --border: #334155;
                }
                * { box-sizing: border-box; margin: 0; padding: 0; }
                body {
                    font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Helvetica, Arial, sans-serif;
                    background-color: var(--bg);
                    color: var(--text);
                    line-height: 1.6;
                    padding: 2rem;
                }
                .container { max-width: 900px; margin: 0 auto; }
                header {
                    border-bottom: 1px solid var(--border);
                    padding-bottom: 1.5rem;
                    margin-bottom: 2rem;
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                }
                h1 { font-size: 1.8rem; font-weight: 700; color: #fff; }
                nav a {
                    color: var(--text-muted);
                    text-decoration: none;
                    margin-left: 1.5rem;
                    font-weight: 500;
                    transition: color 0.2s;
                }
                nav a:hover, nav a.active { color: #fff; }
                .card {
                    background-color: var(--card-bg);
                    border: 1px solid var(--border);
                    border-radius: 12px;
                    padding: 2rem;
                    margin-bottom: 1.5rem;
                }
                h2 { font-size: 1.4rem; margin-bottom: 1rem; color: #fff; }
                p { color: var(--text-muted); margin-bottom: 1rem; font-size: 1.05rem; }
                ul { margin-left: 1.5rem; margin-bottom: 1.5rem; color: var(--text-muted); }
                li { margin-bottom: 0.5rem; }
                code { background: #0f172a; padding: 0.2rem 0.4rem; border-radius: 4px; font-family: monospace; color: #a5b4fc; }
                footer {
                    margin-top: 3rem;
                    border-top: 1px solid var(--border);
                    padding-top: 1.5rem;
                    text-align: center;
                    color: var(--text-muted);
                    font-size: 0.9rem;
                }
            </style>
        </head>
        <body>
            <div class="container">
                <header>
                    <div>
                        <h1>Alumni Tracking System</h1>
                        <p style="color: var(--text-muted); font-size: 0.9rem;">About &amp; Architectural Specifications</p>
                    </div>
                    <nav>
                        <a href="/">Home</a>
                        <a href="/about" class="active">About</a>
                        <a href="/api/alumni">Alumni API</a>
                    </nav>
                </header>

                <div class="card">
                    <h2>📖 About the Project</h2>
                    <p>The <strong>Alumni Tracking System</strong> is an enterprise-oriented web application designed to track graduates, analyze career milestones, facilitate student-alumni mentorship networks, and organize university events.</p>
                    
                    <h2>🚪 Week 02: Routing Fundamentals</h2>
                    <p>In Week 02, the application demonstrates the core mechanics of HTTP routing as outlined in the lecture:</p>
                    <ul>
                        <li><strong>URL Anatomy:</strong> Scheme, Host, Port (443/8000), Path (<code>/api/alumni/42</code>), Query Parameters (<code>?sort=year</code>), and Fragment identifiers.</li>
                        <li><strong>Generations of the Web:</strong> Transition from physical file mapping (1995: <code>/list.php</code>) and RPC commands (2005: <code>?action=getAlumni</code>) to modern RESTful resources (Today: <code>GET /api/alumni/42</code>).</li>
                        <li><strong>HTTP Method Resolution:</strong> Safe vs non-safe, idempotent vs non-idempotent semantics.</li>
                        <li><strong>Status Spectrum:</strong> Explicit status codes including <code>200 OK</code>, <code>201 Created</code>, <code>400 Bad Request</code>, <code>404 Not Found</code>, and <code>405 Method Not Allowed</code>.</li>
                    </ul>

                    <h2>💻 Technology Stack</h2>
                    <p>Constructed using <strong>C# with ASP.NET Core (.NET 7)</strong> and <strong>PostgreSQL 16</strong> inside a reproducible Docker Compose environment.</p>
                </div>

                <footer>
                    <p>Istanbul University • Web Programming • Nazife Türk</p>
                </footer>
            </div>
        </body>
        </html>
        """;
    }
}
