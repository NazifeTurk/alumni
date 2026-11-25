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

            // Return main page (HTML view)
            return Results.Content(GetMainHtmlPage(), "text/html; charset=utf-8");
        });

        // 1 (Dedicated text endpoint). GET /ok -> "ok"
        app.MapGet("/ok", () => Results.Text("ok", "text/plain"));

        // 2. GET /hello -> "Hello, World!"
        app.MapGet("/hello", () => Results.Text("Hello, World!", "text/plain"));

        // 3. GET /hello/{name} -> "Hello, {name}!" (e.g., /hello/nazife -> "Hello, Nazife!")
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

        // 5. GET /home -> main page (HTML view)
        app.MapGet("/home", () => Results.Content(GetMainHtmlPage(), "text/html; charset=utf-8"));

        // 5b. GET /bench & /temporary-bench -> lab evaluation workbench
        app.MapGet("/bench", () => Results.Content(GetMainHtmlPage(), "text/html; charset=utf-8"));
        app.MapGet("/temporary-bench", () => Results.Content(GetMainHtmlPage(), "text/html; charset=utf-8"));

        // 6. GET /about -> temporary about page (HTML view)
        app.MapGet("/about", () => Results.Content(GetAboutHtmlPage(), "text/html; charset=utf-8"));

        // 7. GET /404 -> Generic 404 Page (HTML view with HTTP 404 Not Found)
        app.MapGet("/404", (HttpContext context) =>
        {
            var requestedPath = context.Request.Query["requestedPath"].ToString();
            if (string.IsNullOrWhiteSpace(requestedPath))
            {
                requestedPath = context.Request.Path.Value ?? "/";
            }

            var html = GetGeneric404HtmlPage(requestedPath);
            return Results.Content(html, "text/html; charset=utf-8", statusCode: StatusCodes.Status404NotFound);
        });

        // 8. GET /assets/iu-logo.svg -> Serves official Istanbul University logo
        app.MapGet("/assets/iu-logo.svg", () =>
        {
            var possiblePaths = new[]
            {
                Path.Combine(Directory.GetCurrentDirectory(), "views", "assets", "iu-logo.svg"),
                Path.Combine(Directory.GetCurrentDirectory(), "src", "views", "assets", "iu-logo.svg"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "views", "assets", "iu-logo.svg"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src", "views", "assets", "iu-logo.svg")
            };

            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    return Results.File(path, "image/svg+xml");
                }
            }

            return Results.Redirect("https://upload.wikimedia.org/wikipedia/commons/1/1e/%C4%B0stanbul_%C3%9Cniversitesi_Logo.svg");
        });

        // 9. GET /assets/iu-gate-silhouette.svg -> Serves Istanbul University gate silhouette
        app.MapGet("/assets/iu-gate-silhouette.svg", () =>
        {
            var possiblePaths = new[]
            {
                Path.Combine(Directory.GetCurrentDirectory(), "views", "assets", "iu-gate-silhouette.svg"),
                Path.Combine(Directory.GetCurrentDirectory(), "src", "views", "assets", "iu-gate-silhouette.svg"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "views", "assets", "iu-gate-silhouette.svg"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src", "views", "assets", "iu-gate-silhouette.svg")
            };

            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    return Results.File(path, "image/svg+xml");
                }
            }

            return Results.NotFound();
        });
    }

    private static string GetGeneric404HtmlPage(string requestedPath)
    {
        var possiblePaths = new[]
        {
            Path.Combine(Directory.GetCurrentDirectory(), "views", "404.html"),
            Path.Combine(Directory.GetCurrentDirectory(), "src", "views", "404.html"),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "views", "404.html"),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src", "views", "404.html")
        };

        foreach (var path in possiblePaths)
        {
            if (File.Exists(path))
            {
                try
                {
                    var template = File.ReadAllText(path);
                    return template.Replace("{{REQUESTED_PATH}}", System.Net.WebUtility.HtmlEncode(requestedPath));
                }
                catch
                {
                    // Fall through to fallback HTML if file read fails
                }
            }
        }

        // Inline fallback if 404.html template cannot be located
        return """
        <!DOCTYPE html>
        <html lang="tr">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>404 — Sayfa Bulunamadı</title>
            <style>
                body { font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif; background: #FAF9F6; color: #1C2920; text-align: center; padding: 4rem 1rem; }
                .box { max-width: 500px; margin: 0 auto; background: #FFFFFF; padding: 2.5rem; border-radius: 4px; border: 1px solid #E2DFD6; }
                .logo { width: 44px; height: 44px; margin-bottom: 1rem; }
                h1 { font-size: 3.5rem; color: #004225; margin-bottom: 0.5rem; font-family: Georgia, serif; }
                code { background: #F5F4EE; color: #004225; padding: 0.4rem 0.8rem; border-radius: 4px; border: 1px solid #E2DFD6; font-family: monospace; }
                a { display: inline-block; margin-top: 1.5rem; color: #fff; background: #004225; padding: 0.65rem 1.4rem; border-radius: 9999px; text-decoration: none; font-weight: 600; }
                a:hover { background: #065A35; }
            </style>
        </head>
        <body>
            <div class="box">
                <img src="/assets/iu-logo.svg" onerror="this.src='https://upload.wikimedia.org/wikipedia/commons/1/1e/%C4%B0stanbul_%C3%9Cniversitesi_Logo.svg';" class="logo" alt="IU">
                <h1>404</h1>
                <h2 style="color:#0F2A1D;margin-bottom:0.75rem;">Sayfa Bulunamadı</h2>
                <p style="margin: 1rem 0; color: #55685C;">Ulaşmaya çalıştığınız adres bulunamadı:</p>
                <code>{{REQUESTED_PATH}}</code>
                <div><a href="/">Ana Sayfaya Dön</a></div>
                <p style="margin-top: 1.5rem; font-size: 0.8rem; color: #7E9084;">İstanbul Üniversitesi &bull; Nazife Türk</p>
            </div>
        </body>
        </html>
        """.Replace("{{REQUESTED_PATH}}", System.Net.WebUtility.HtmlEncode(requestedPath));
    }

    private static string GetMainHtmlPage()
    {
        var possiblePaths = new[]
        {
            Path.Combine(Directory.GetCurrentDirectory(), "views", "home.html"),
            Path.Combine(Directory.GetCurrentDirectory(), "src", "views", "home.html"),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "views", "home.html"),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src", "views", "home.html")
        };

        foreach (var path in possiblePaths)
        {
            if (File.Exists(path))
            {
                try
                {
                    return File.ReadAllText(path);
                }
                catch
                {
                    // Fall through to inline fallback
                }
            }
        }

        return """
        <!DOCTYPE html>
        <html lang="tr">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>İstanbul Üniversitesi — Mezun Takip Sistemi</title>
            <link rel="preconnect" href="https://fonts.googleapis.com">
            <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
            <link href="https://fonts.googleapis.com/css2?family=DM+Serif+Display&family=Plus+Jakarta+Sans:wght@400;500;600;700&family=JetBrains+Mono:wght@400;500;600&display=swap" rel="stylesheet">
            <style>
                :root {
                    --color-canvas: #FAF9F6;
                    --color-surface-card: #FFFFFF;
                    --color-surface-inset: #F5F4EE;
                    --color-hairline: #E2DFD6;
                    --color-primary-action: #004225;
                    --color-primary-hover: #065A35;
                    --color-pill-tint: #E8F0EB;
                    --color-pill-border: #C8DBCF;
                    --color-text-display: #004225;
                    --color-text-title: #0F2A1D;
                    --color-text-body: #1C2920;
                    --color-text-muted: #55685C;
                    --color-text-light: #7E9084;
                    --radius-pills: 9999px;
                    --radius-cards: 4px;
                }
                * { box-sizing: border-box; margin: 0; padding: 0; }
                body {
                    font-family: 'Plus Jakarta Sans', -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
                    background-color: var(--color-canvas);
                    background-image: url('/assets/iu-gate-silhouette.svg');
                    background-repeat: no-repeat;
                    background-position: center 30px;
                    background-size: 1100px auto;
                    background-attachment: fixed;
                    color: var(--color-text-body);
                    line-height: 1.6;
                    padding: 2.5rem 1.5rem;
                }
                .container { max-width: 1040px; margin: 0 auto; }
                .nav-pill {
                    display: flex;
                    align-items: center;
                    justify-content: space-between;
                    background: rgba(255, 255, 255, 0.94);
                    backdrop-filter: blur(20px);
                    border: 1px solid var(--color-hairline);
                    border-radius: var(--radius-pills);
                    padding: 0.6rem 1.4rem;
                    margin-bottom: 2.5rem;
                }
                .nav-brand {
                    display: inline-flex;
                    align-items: center;
                    gap: 0.75rem;
                    text-decoration: none;
                    color: var(--color-text-title);
                    font-weight: 700;
                }
                .nav-logo { width: 32px; height: 32px; }
                .nav-logo img { width: 100%; height: 100%; object-fit: contain; }
                .nav-links { display: flex; align-items: center; gap: 0.5rem; }
                .nav-links a {
                    color: var(--color-text-muted);
                    text-decoration: none;
                    font-size: 0.88rem;
                    font-weight: 500;
                    padding: 0.4rem 0.9rem;
                    border-radius: var(--radius-pills);
                }
                .nav-links a:hover { color: var(--color-primary-action); background: var(--color-pill-tint); }
                .nav-links a.active { color: #fff; background: var(--color-primary-action); font-weight: 600; }
                .hero-card {
                    background: var(--color-surface-card);
                    border: 1px solid var(--color-hairline);
                    border-radius: var(--radius-cards);
                    padding: 3rem 2.5rem;
                    margin-bottom: 2rem;
                }
                .badge-pill {
                    display: inline-flex;
                    align-items: center;
                    gap: 0.45rem;
                    background: var(--color-pill-tint);
                    color: var(--color-primary-action);
                    border: 1px solid var(--color-pill-border);
                    padding: 0.35rem 0.85rem;
                    border-radius: var(--radius-pills);
                    font-size: 0.75rem;
                    font-weight: 600;
                    text-transform: uppercase;
                    margin-bottom: 1.25rem;
                }
                .hero-card h1 {
                    font-family: 'DM Serif Display', Georgia, serif;
                    font-size: 3rem;
                    color: var(--color-text-display);
                    line-height: 1.1;
                    margin-bottom: 1rem;
                    letter-spacing: -0.03em;
                }
                .hero-card p {
                    color: var(--color-text-muted);
                    font-size: 1.15rem;
                    max-width: 780px;
                    margin-bottom: 1.75rem;
                }
                .btn {
                    display: inline-flex;
                    align-items: center;
                    gap: 0.5rem;
                    padding: 0.7rem 1.4rem;
                    border-radius: var(--radius-pills);
                    font-weight: 600;
                    font-size: 0.92rem;
                    text-decoration: none;
                }
                .btn-primary { background: var(--color-primary-action); color: #fff; }
                .btn-secondary { background: var(--color-surface-card); color: var(--color-text-title); border: 1px solid var(--color-hairline); }
                .grid {
                    display: grid;
                    grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
                    gap: 1.5rem;
                    margin-bottom: 2.5rem;
                }
                .card {
                    background: var(--color-surface-card);
                    border: 1px solid var(--color-hairline);
                    border-radius: var(--radius-cards);
                    padding: 1.75rem;
                }
                .card h3 {
                    font-family: 'DM Serif Display', Georgia, serif;
                    font-size: 1.35rem;
                    color: var(--color-text-title);
                    margin-bottom: 0.6rem;
                }
                .card p {
                    color: var(--color-text-muted);
                    font-size: 0.92rem;
                    line-height: 1.6;
                    margin-bottom: 1rem;
                }
                .card a {
                    color: var(--color-primary-action);
                    text-decoration: none;
                    font-weight: 600;
                    font-size: 0.88rem;
                }
                footer {
                    text-align: center;
                    padding-top: 2rem;
                    border-top: 1px solid var(--color-hairline);
                    color: var(--color-text-light);
                    font-size: 0.85rem;
                }
                footer a { color: var(--color-primary-action); text-decoration: none; font-weight: 600; }
            </style>
        </head>
        <body>
            <div class="container">
                <nav class="nav-pill">
                    <a href="/" class="nav-brand">
                        <div class="nav-logo">
                            <img src="/assets/iu-logo.svg" onerror="this.src='https://upload.wikimedia.org/wikipedia/commons/1/1e/%C4%B0stanbul_%C3%9Cniversitesi_Logo.svg';" alt="İstanbul Üniversitesi">
                        </div>
                        <span>İstanbul Üniversitesi</span>
                    </a>
                    <div class="nav-links">
                        <a href="/" class="active">Ana Sayfa</a>
                        <a href="/users">Mezunlar</a>
                        <a href="/jobs">İş İlanları</a>
                        <a href="/announcements">Duyurular</a>
                        <a href="/about">Hakkında</a>
                        <a href="/swagger">Swagger</a>
                    </div>
                </nav>

                <div class="hero-card">
                    <span class="badge-pill">Kurumsal Mezun ve Kariyer Ağı &bull; 1453</span>
                    <h1>Gelenekten Geleceğe Güçlü Köprü</h1>
                    <p>
                        İstanbul Üniversitesi mezunlarını, akademisyenlerini ve öğrencilerini bir araya getiren kurumsal kariyer ve mentörlük platformu.
                    </p>
                    <div style="display: flex; gap: 0.75rem; flex-wrap: wrap;">
                        <a href="/users" class="btn btn-primary">Mezun Rehberini Keşfet</a>
                        <a href="/jobs" class="btn btn-secondary">Kariyer Fırsatları</a>
                    </div>
                </div>

                <div class="grid">
                    <div class="card">
                        <h3>🎓 Mezun Rehberi</h3>
                        <p>Kayıtlı mezunlarımızın profillerini inceleyin, fakülte ve bölüm bazlı arama yapın, iletişim kurun.</p>
                        <a href="/users">Rehbere Git &rarr;</a>
                    </div>
                    <div class="card">
                        <h3>💼 Kariyer Panosu</h3>
                        <p>Mezunlarımızın çalıştığı öncü kurumlardan iş ve staj ilanlarını takip edin, doğrudan başvurun.</p>
                        <a href="/jobs">İlanları Görüntüle &rarr;</a>
                    </div>
                    <div class="card">
                        <h3>📢 Duyuru &amp; Etkinlikler</h3>
                        <p>Mezun buluşmaları, homecoming günleri ve akademik etkinliklerden ilk siz haberdar olun.</p>
                        <a href="/announcements">Duyuruları İncele &rarr;</a>
                    </div>
                </div>

                <footer>
                    <p>&copy; 1453 &ndash; 2026 İstanbul Üniversitesi &bull; Sistem Mimarı: <strong>Nazife Türk</strong> (<a href="mailto:nazife.turk@ogr.iu.edu.tr">nazife.turk@ogr.iu.edu.tr</a>)</p>
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
        <html lang="tr">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Hakkında — İstanbul Üniversitesi Mezun Takip Sistemi</title>
            <link rel="preconnect" href="https://fonts.googleapis.com">
            <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
            <link href="https://fonts.googleapis.com/css2?family=DM+Serif+Display&family=Plus+Jakarta+Sans:wght@400;500;600;700&family=JetBrains+Mono:wght@400;500;600&display=swap" rel="stylesheet">
            <style>
                :root {
                    --color-canvas: #FAF9F6;
                    --color-surface-card: #FFFFFF;
                    --color-surface-inset: #F5F4EE;
                    --color-hairline: #E2DFD6;
                    --color-hairline-subtle: #EBE8E0;
                    --color-primary-action: #004225;
                    --color-primary-hover: #065A35;
                    --color-pill-tint: #E8F0EB;
                    --color-pill-border: #C8DBCF;
                    --color-text-display: #004225;
                    --color-text-title: #0F2A1D;
                    --color-text-body: #1C2920;
                    --color-text-muted: #55685C;
                    --color-text-light: #7E9084;
                    --radius-pills: 9999px;
                    --radius-cards: 4px;
                }
                * { box-sizing: border-box; margin: 0; padding: 0; }
                body {
                    font-family: 'Plus Jakarta Sans', -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
                    background-color: var(--color-canvas);
                    background-image: url('/assets/iu-gate-silhouette.svg');
                    background-repeat: no-repeat;
                    background-position: center 30px;
                    background-size: 1100px auto;
                    background-attachment: fixed;
                    color: var(--color-text-body);
                    line-height: 1.6;
                    padding: 2.5rem 1.5rem;
                    -webkit-font-smoothing: antialiased;
                }
                .container { max-width: 960px; margin: 0 auto; }
                .nav-pill {
                    display: flex;
                    align-items: center;
                    justify-content: space-between;
                    background: rgba(255, 255, 255, 0.94);
                    backdrop-filter: blur(20px);
                    border: 1px solid var(--color-hairline);
                    border-radius: var(--radius-pills);
                    padding: 0.6rem 1.4rem;
                    margin-bottom: 2.5rem;
                }
                .nav-brand {
                    display: inline-flex;
                    align-items: center;
                    gap: 0.75rem;
                    text-decoration: none;
                    color: var(--color-text-title);
                    font-weight: 700;
                }
                .nav-logo { width: 32px; height: 32px; }
                .nav-logo img { width: 100%; height: 100%; object-fit: contain; }
                .nav-links { display: flex; align-items: center; gap: 0.5rem; }
                .nav-links a {
                    color: var(--color-text-muted);
                    text-decoration: none;
                    font-size: 0.88rem;
                    font-weight: 500;
                    padding: 0.4rem 0.9rem;
                    border-radius: var(--radius-pills);
                    transition: all 0.15s ease;
                }
                .nav-links a:hover { color: var(--color-primary-action); background: var(--color-pill-tint); }
                .nav-links a.active { color: #fff; background: var(--color-primary-action); font-weight: 600; }
                
                .header-card {
                    background: var(--color-surface-card);
                    border: 1px solid var(--color-hairline);
                    border-radius: var(--radius-cards);
                    padding: 2.5rem;
                    margin-bottom: 2rem;
                }
                .badge-pill {
                    display: inline-flex;
                    align-items: center;
                    gap: 0.45rem;
                    background: var(--color-pill-tint);
                    color: var(--color-primary-action);
                    border: 1px solid var(--color-pill-border);
                    padding: 0.35rem 0.85rem;
                    border-radius: var(--radius-pills);
                    font-size: 0.75rem;
                    font-weight: 600;
                    text-transform: uppercase;
                    letter-spacing: 0.05em;
                    margin-bottom: 1rem;
                }
                .header-card h1 {
                    font-family: 'DM Serif Display', Georgia, serif;
                    font-size: 2.4rem;
                    color: var(--color-text-display);
                    line-height: 1.15;
                    margin-bottom: 0.75rem;
                }
                .header-card p {
                    color: var(--color-text-muted);
                    font-size: 1.05rem;
                    max-width: 720px;
                }
                .content-card {
                    background: var(--color-surface-card);
                    border: 1px solid var(--color-hairline);
                    border-radius: var(--radius-cards);
                    padding: 2.25rem;
                    margin-bottom: 1.75rem;
                }
                .content-card h2 {
                    font-family: 'DM Serif Display', Georgia, serif;
                    font-size: 1.5rem;
                    color: var(--color-text-title);
                    margin-bottom: 1rem;
                    display: flex;
                    align-items: center;
                    gap: 0.6rem;
                }
                .content-card p {
                    color: var(--color-text-body);
                    font-size: 0.95rem;
                    margin-bottom: 1rem;
                    line-height: 1.7;
                }
                .content-card ul {
                    margin-left: 1.5rem;
                    margin-bottom: 1.25rem;
                    color: var(--color-text-body);
                }
                .content-card li { margin-bottom: 0.5rem; }
                code {
                    font-family: 'JetBrains Mono', monospace;
                    background: var(--color-surface-inset);
                    color: var(--color-primary-action);
                    padding: 0.2rem 0.45rem;
                    border-radius: var(--radius-cards);
                    border: 1px solid var(--color-hairline-subtle);
                    font-size: 0.88em;
                }
                .footer-box {
                    text-align: center;
                    padding-top: 2rem;
                    margin-top: 2rem;
                    border-top: 1px solid var(--color-hairline);
                    color: var(--color-text-light);
                    font-size: 0.85rem;
                }
                .footer-box a { color: var(--color-primary-action); text-decoration: none; font-weight: 600; }
            </style>
        </head>
        <body>
            <div class="container">
                <nav class="nav-pill">
                    <a href="/" class="nav-brand">
                        <div class="nav-logo">
                            <img src="/assets/iu-logo.svg" onerror="this.src='https://upload.wikimedia.org/wikipedia/commons/1/1e/%C4%B0stanbul_%C3%9Cniversitesi_Logo.svg';" alt="İstanbul Üniversitesi">
                        </div>
                        <span>İstanbul Üniversitesi</span>
                    </a>
                    <div class="nav-links">
                        <a href="/">Ana Sayfa</a>
                        <a href="/users">Mezunlar</a>
                        <a href="/jobs">İş İlanları</a>
                        <a href="/announcements">Duyurular</a>
                        <a href="/about" class="active">Hakkında</a>
                        <a href="/swagger">Swagger</a>
                    </div>
                </nav>

                <div class="header-card">
                    <span class="badge-pill">Kurumsal Mimari &bull; 1453</span>
                    <h1>Mezun Takip Sistemi Hakkında</h1>
                    <p>
                        İstanbul Üniversitesi Bilgisayar Bilimleri bünyesinde geliştirilen kurumsal mezun ilişkileri, kariyer yönlendirme ve akademik ağ platformu.
                    </p>
                </div>

                <div class="content-card">
                    <h2>📖 Proje Hakkında ve Misyon</h2>
                    <p>
                        <strong>İstanbul Üniversitesi Mezun Takip Sistemi</strong>, köklü geçmişe sahip üniversitemizin mezunları ile aktif öğrencilerini ve akademik kadroyu birbirine bağlayan; kariyer seyrini izleyen ve mentörlük ağını kurumsallaştıran güvenli bir web platformudur.
                    </p>

                    <h2>🚪 HTTP Yönlendirme ve URL Anatomisi</h2>
                    <p>
                        Proje mimarisi temiz REST prensiplerine ve katmanlı mimariye dayanmaktadır:
                    </p>
                    <ul>
                        <li><strong>URL Anatomisi:</strong> Şema (HTTP/HTTPS), Host, Port (8000), Yol (<code>/users/42</code>), Parametreler (<code>?format=json</code>) ve Parçalar.</li>
                        <li><strong>Modern RESTful Kaynaklar:</strong> Statik dosya eşlemelerinden bağımsız, standart HTTP durum kodları ile çalışan modern API uçları.</li>
                        <li><strong>Güvenli ve İdempotent İstekler:</strong> <code>GET</code> işlemlerinde güvenli okuma, <code>POST</code> işlemlerinde durum değişikliği ve validasyon.</li>
                        <li><strong>HTTP Durum Yelpazesi:</strong> <code>200 OK</code>, <code>201 Created</code>, <code>400 Bad Request</code>, <code>404 Not Found</code> ve <code>500 Server Error</code> tam denetimi.</li>
                    </ul>

                    <h2>💻 Teknoloji Yığını &amp; Altyapı</h2>
                    <p>
                        Uygulama <strong>C# ASP.NET Core (.NET 7)</strong> ve <strong>PostgreSQL 16</strong> veritabanı motoru ile geliştirilmiş olup <strong>Docker Compose</strong> üzerinde tek komutla ayağa kaldırılacak şekilde konteynerleştirilmiştir.
                    </p>
                </div>

                <div class="footer-box">
                    <p>&copy; 1453 &ndash; 2026 İstanbul Üniversitesi &bull; Sistem Mimarı: <strong>Nazife Türk</strong> (<a href="mailto:nazife.turk@ogr.iu.edu.tr">nazife.turk@ogr.iu.edu.tr</a>)</p>
                </div>
            </div>
        </body>
        </html>
        """;
    }
}
