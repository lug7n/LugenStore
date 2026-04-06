h1 style="display:flex;align-items:center;gap:12px;font-family:Segoe UI, Roboto, Arial, sans-serif;">
  <img src="https://starwars-visualguide.com/assets/img/vehicles/14.jpg" alt="X-Wing" style="width:56px;height:56px;border-radius:6px;object-fit:cover;box-shadow:0 6px 18px rgba(2,6,23,.25);"/>
  <div>
    <div style="font-size:28px;margin-bottom:2px;">LugenStore.Api</div>
    <div style="font-size:12px;color:#6b7280;">A .NET 8 REST API for a digital games store — under development</div>
  </div>
  <span style="margin-left:auto;background:#fff3bf;color:#92400e;padding:6px 10px;border-radius:8px;font-weight:700;border:1px solid rgba(0,0,0,.06);">WIP</span>
</h1>

<hr/>

<section style="display:flex;gap:1.5rem;align-items:flex-start;flex-wrap:wrap;">
  <div style="flex:1;min-width:320px;max-width:780px;">
    <h2 style="margin:8px 0 6px 0">What is this?</h2>
    <p style="color:#374151;line-height:1.45;max-width:70ch;">LugenStore.Api is a layered RESTful backend for an online games store built with <strong>ASP.NET Core (.NET 8)</strong>, <strong>EF Core</strong> and <strong>PostgreSQL</strong>. It uses a Controllers → Services → Repositories pattern, DTO ↔ Model mapping inside services, and is prepared to run in Docker.</p>

    <h3 style="margin-top:14px">Quick start</h3>
    <ol style="color:#374151;">
      <li>Clone the repo: <code>git clone &lt;repo-url&gt;</code></li>
      <li>Run containers: <code>docker compose up -d</code></li>
      <li>Run locally: <code>cd LugenStore.Api &amp;&amp; dotnet run</code></li>
    </ol>

    <h3 style="margin-top:10px">Common endpoints</h3>
    <ul style="color:#374151;">
      <li><code>GET /api/games</code> — list games</li>
      <li><code>GET /api/games/{id}</code> — get game</li>
      <li><code>POST /api/games</code> — create game (body: <code>CreateGameDto</code>)</li>
    </ul>

    <h3 style="margin-top:12px">Guiding rules</h3>
    <ul style="color:#374151;">
      <li>Repositories return Models only — no DTOs inside the repository layer.</li>
      <li>Services handle validation and Model &lt;=&gt; DTO transformations.</li>
      <li>Navigation properties must be eager-loaded with <code>Include()</code> when needed.</li>
    </ul>

  </div>

  <aside style="width:360px;background:linear-gradient(180deg,#0b1220,#0f1724);border-radius:10px;padding:14px;color:#e6f0ff;box-shadow:0 10px 30px rgba(2,6,23,.5);">
    <h3 style="margin-top:0">Star Wars gallery</h3>
    <p style="font-size:13px;margin:.25rem 0 10px 0;color:#bcd7ff;">Real character images from <a href="https://starwars-visualguide.com/" style="color:#cfe8ff">Star Wars Visual Guide</a>. GIFs are external (Giphy) — replace with your licensed assets if needed.</p>

    <div style="display:grid;grid-template-columns:repeat(3,1fr);gap:8px;margin-bottom:10px;">
      <img src="https://starwars-visualguide.com/assets/img/characters/1.jpg" alt="Luke Skywalker" style="width:100%;height:88px;object-fit:cover;border-radius:8px;border:2px solid rgba(255,255,255,.04);"/>
      <img src="https://starwars-visualguide.com/assets/img/characters/4.jpg" alt="Darth Vader" style="width:100%;height:88px;object-fit:cover;border-radius:8px;border:2px solid rgba(255,255,255,.04);"/>
      <img src="https://starwars-visualguide.com/assets/img/characters/3.jpg" alt="R2-D2" style="width:100%;height:88px;object-fit:cover;border-radius:8px;border:2px solid rgba(255,255,255,.04);"/>
    </div>

    <h4 style="margin:6px 0">GIF highlights</h4>
    <div style="display:grid;grid-template-columns:1fr 1fr;gap:8px;">
      <!-- External GIFs — you may replace these links with your own hosted GIFs in /assets/gifs -->
      <img src="https://media.giphy.com/media/3o6Zt6ML6BklcajjsA/giphy.gif" alt="lightsaber" style="width:100%;height:84px;object-fit:cover;border-radius:8px;"/>
      <img src="https://media.giphy.com/media/l0MYt5jPR6QX5pnqM/giphy.gif" alt="millennium falcon" style="width:100%;height:84px;object-fit:cover;border-radius:8px;"/>
    </div>

    <p style="font-size:12px;color:#acc9ff;margin-top:10px;">Tip: to keep GIFs in the repo, add them to <code>assets/gifs/</code> and use relative paths (recommended for offline or CI environments).</p>
  </aside>
</section>

<hr/>

<section>
  <h3>Visual flourish — opening crawl</h3>
  <div style="background:#000;color:#ffd54f;padding:12px;border-radius:8px;max-width:760px;overflow:hidden;">
    <div style="perspective:400px;">
      <div style="transform-origin:50% 100%;animation:crawl 22s linear infinite;max-width:640px;margin:0 auto;text-align:center;">
        <p style="font-weight:700;letter-spacing:2px;margin:0;padding-top:8px;">LUGENSTORE.API</p>
        <p style="font-size:12px;opacity:.95;max-width:520px;margin:10px auto 0 auto;">Under development — a learning-oriented project that demonstrates domain layering with ASP.NET Core, EF Core and Docker. Expect changes as features are added.</p>
      </div>
    </div>
  </div>

  <style>
    @keyframes crawl {
      0% { transform: translateY(80px) scale(.5) rotateX(30deg); opacity:0 }
      10% { opacity:1 }
      100% { transform: translateY(-420px) scale(.6) rotateX(30deg); opacity:0 }
    }
  </style>
</section>

<hr/>

<section style="display:flex;gap:1rem;flex-wrap:wrap;align-items:center;">
  <div style="flex:1;min-width:260px;">
    <h4 style="margin:0 0 6px 0">Contribute</h4>
    <p style="margin:0;color:#374151;">Fork the repo, create a branch, add focused changes and open a PR. Add tests where applicable. Use GitHub Issues to propose larger changes.</p>
  </div>
  <div style="min-width:260px;color:#6b7280;font-size:13px;">Status: <strong>Under development</strong> — APIs and internals may change frequently.</div>
</section>

<footer style="margin-top:16px;color:#6b7280;font-size:13px;">Tech: .NET 8 · EF Core · PostgreSQL · Docker · Layered architecture</footer>
