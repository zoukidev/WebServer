using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=app.db"));

var key = Encoding.ASCII.GetBytes("MA_CLE_SUPER_SECRETE_32_CHARS_LONGUE!!");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// Création automatique de la base de données et des tables si elles n'existent pas
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

// Servir le build React depuis "wwwroot"
app.UseDefaultFiles();
app.UseStaticFiles();

// Auth: Inscription
app.MapPost("/register", async (AppDbContext db, UserDto dto) =>
{
    var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
    var user = new User { Username = dto.Username, Name = dto.Name, PasswordHash = hash };
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Ok("User created");
});

// Auth: Connexion
app.MapPost("/login", async (AppDbContext db, UserDto dto) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
    if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        return Results.Unauthorized();

    var token = JwtTokenHelper.GenerateToken(user.Username, key);
    return Results.Ok(new { token, user });
});
app.MapGet("/check-token", async (HttpContext http, AppDbContext db) =>
{
    // Vérifie que l'utilisateur est authentifié
    if (!http.User.Identity?.IsAuthenticated ?? true)
        return Results.Unauthorized();

    // Récupère le username depuis le token JWT
    var username = http.User.Identity?.Name ?? http.User.Claims.FirstOrDefault(c => c.Type == "unique_name" || c.Type == "name")?.Value;
    if (string.IsNullOrEmpty(username))
        return Results.Unauthorized();

    // Récupère l'utilisateur en base
    var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username);
    if (user == null)
        return Results.Unauthorized();

    // Génère un nouveau token
    var token = JwtTokenHelper.GenerateToken(user.Username, key);

    return Results.Ok(new { token, user });
}).RequireAuthorization();

app.MapPost("/logout", (HttpContext http) =>
{
    http.SignOutAsync();
    return Results.Ok("Logged out");
}).RequireAuthorization();

// Utilisateurs
app.MapGet("/users", async (AppDbContext db) =>
    await db.Users.Select(u => new { u.Id, u.Username, u.Name }).ToListAsync())
    .RequireAuthorization();

app.MapGet("/", Routes.Home);
    
app.Run();

record UserDto(string Username, string Name, string Password);
record TokenDto(string token);
