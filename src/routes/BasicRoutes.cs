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
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Alumni Tracking System - Week 02</title>
            <style>
                :root {
                    --primary: #4f46e5;
                    --primary-hover: #4338ca;
                    --bg: #0f172a;
                    --card-bg: #1e293b;
                    --text: #f8fafc;
                    --text-muted: #94a3b8;
                    --accent: #10b981;
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
                .badge {
                    display: inline-block;
                    background-color: rgba(16, 185, 129, 0.2);
                    color: var(--accent);
                    padding: 0.3rem 0.8rem;
                    border-radius: 9999px;
                    font-size: 0.85rem;
                    font-weight: 600;
                    border: 1px solid rgba(16, 185, 129, 0.3);
                }
                nav a {
                    color: var(--text-muted);
                    text-decoration: none;
                    margin-left: 1.5rem;
                    font-weight: 500;
                    transition: color 0.2s;
                }
                nav a:hover, nav a.active { color: #fff; }
                .hero {
                    background: linear-gradient(135deg, rgba(79, 70, 229, 0.15), rgba(16, 185, 129, 0.1));
                    border: 1px solid var(--border);
                    border-radius: 12px;
                    padding: 2rem;
                    margin-bottom: 2rem;
                }
                .hero h2 { margin-bottom: 0.75rem; font-size: 1.5rem; }
                .hero p { color: var(--text-muted); font-size: 1.05rem; }
                .card {
                    background-color: var(--card-bg);
                    border: 1px solid var(--border);
                    border-radius: 12px;
                    padding: 1.5rem;
                    margin-bottom: 1.5rem;
                }
                .card h3 {
                    font-size: 1.2rem;
                    margin-bottom: 1rem;
                    color: #fff;
                    display: flex;
                    align-items: center;
                    gap: 0.5rem;
                }
                table { width: 100%; border-collapse: collapse; margin-top: 1rem; }
                th, td {
                    text-align: left;
                    padding: 0.75rem 1rem;
                    border-bottom: 1px solid var(--border);
                    font-size: 0.95rem;
                }
                th { color: var(--text-muted); font-weight: 600; background: rgba(0,0,0,0.15); }
                .method {
                    display: inline-block;
                    padding: 0.2rem 0.5rem;
                    border-radius: 4px;
                    font-family: monospace;
                    font-size: 0.85rem;
                    font-weight: 700;
                }
                .method-get { background: rgba(16, 185, 129, 0.2); color: #34d399; }
                .method-post { background: rgba(59, 130, 246, 0.2); color: #60a5fa; }
                a.endpoint {
                    color: #818cf8;
                    text-decoration: none;
                    font-family: monospace;
                }
                a.endpoint:hover { text-decoration: underline; color: #a5b4fc; }
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
                        <p style="color: var(--text-muted); font-size: 0.9rem;">Week 02 — Routing &amp; The Doors of the System</p>
                    </div>
                    <nav>
                        <a href="/" class="active">Home</a>
                        <a href="/about">About</a>
                        <a href="/api/alumni">Alumni API</a>
                    </nav>
                </header>

                <div class="hero">
                    <span class="badge">Status: OK • System Live</span>
                    <h2 style="margin-top: 1rem;">Welcome to Alumni Tracking System</h2>
                    <p>Week 02 implements foundational URL anatomy, parameterized routing, HTTP method resolution, and status code mechanics.</p>
                </div>

                <div class="card">
                    <h3>🚪 Whiteboard Assigned Endpoints (Click to Test)</h3>
                    <table>
                        <thead>
                            <tr>
                                <th>Method</th>
                                <th>Route</th>
                                <th>Task Description</th>
                                <th>Sample Link</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td><span class="method method-get">GET</span></td>
                                <td><code>/</code> or <code>/ok</code></td>
                                <td>Health &amp; Inception Status</td>
                                <td><a class="endpoint" href="/ok">/ok</a></td>
                            </tr>
                            <tr>
                                <td><span class="method method-get">GET</span></td>
                                <td><code>/hello</code></td>
                                <td>Static Greeting</td>
                                <td><a class="endpoint" href="/hello">/hello</a></td>
                            </tr>
                            <tr>
                                <td><span class="method method-get">GET</span></td>
                                <td><code>/hello/{name}</code></td>
                                <td>Parameterized Path Greeting</td>
                                <td><a class="endpoint" href="/hello/emre">/hello/emre</a></td>
                            </tr>
                            <tr>
                                <td><span class="method method-get">GET</span></td>
                                <td><code>/sum/{n1}/{n2}</code></td>
                                <td>Mathematical Summation</td>
                                <td><a class="endpoint" href="/sum/15/27">/sum/15/27</a></td>
                            </tr>
                            <tr>
                                <td><span class="method method-get">GET</span></td>
                                <td><code>/home</code></td>
                                <td>Temporary Main Landing Page</td>
                                <td><a class="endpoint" href="/home">/home</a></td>
                            </tr>
                            <tr>
                                <td><span class="method method-get">GET</span></td>
                                <td><code>/about</code></td>
                                <td>Temporary About Page</td>
                                <td><a class="endpoint" href="/about">/about</a></td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <div class="card">
                    <h3>🏛️ Resource Routing Table (Slide Section D)</h3>
                    <table>
                        <thead>
                            <tr>
                                <th>Method</th>
                                <th>Route</th>
                                <th>Purpose</th>
                                <th>Sample Link</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td><span class="method method-get">GET</span></td>
                                <td><code>/api/alumni</code></td>
                                <td>List Alumni Resources</td>
                                <td><a class="endpoint" href="/api/alumni">/api/alumni</a></td>
                            </tr>
                            <tr>
                                <td><span class="method method-get">GET</span></td>
                                <td><code>/api/alumni/{id}</code></td>
                                <td>Dynamic Wildcard: Alumnus #42</td>
                                <td><a class="endpoint" href="/api/alumni/42">/api/alumni/42</a></td>
                            </tr>
                            <tr>
                                <td><span class="method method-get">GET</span></td>
                                <td><code>/api/alumni/{id}/jobs</code></td>
                                <td>Nested Wildcard Route: Jobs</td>
                                <td><a class="endpoint" href="/api/alumni/42/jobs">/api/alumni/42/jobs</a></td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <footer>
                    <p>Istanbul University • Web Programming • Nazife Türk</p>
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
