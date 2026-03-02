using Microsoft.EntityFrameworkCore;
using PontoSaaS.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    
builder.Services.AddRazorPages();
// builder.Services.AddSession();  Remover Session e usar Cookie Auth
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!context.Empresas.Any())
    {
        var empresa = new Empresa
        {
            Nome = "Empresa Teste",
            Cnpj = "000000000001"
        };

        context.Empresas.Add(empresa);
        context.SaveChanges();

        // Admin
        context.Usuarios.Add(new Usuario
        {
            Nome = "Admin",
            Email = "admin@teste.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            Role = "Admin",
            EmpresaId = empresa.Id
        });

        // Funcionário
        context.Usuarios.Add(new Usuario
        {
            Nome = "User1",
            Email = "user1@teste.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            Role = "Funcionario",
            EmpresaId = empresa.Id
        });

        context.SaveChanges();
    }
}



app.Run();
