using System.Text;
using ABB.Catalogo.AccesoDatos.Interfaces;
using ABB.Catalogo.AccesoDatos.Repositorios;
using ABB.Catalogo.LogicaNegocio;
using ABB.Catalogo.LogicaNegocio.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// 1. INYECCIÓN DE DEPENDENCIAS (DI)

// Repositorios (Acceso a Datos)
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

// Lógica de Negocio
builder.Services.AddScoped<IUsuarioLN, UsuarioLN>();
builder.Services.AddScoped<IProductoLN, ProductoLN>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. CONFIGURACIÓN DE SEGURIDAD NATIVA (JWT BEARER)
var jwtKey = builder.Configuration["Jwt:Key"]
             ?? throw new ArgumentNullException("La clave JWT no está configurada.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false, // En desarrollo, React es la única audiencia
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// 3. CONFIGURACIÓN DE CORS (Cross-Origin Resource Sharing)

/* Esto es VITAL para que nuestro Frontend en React no sea bloqueado
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirReact", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000") // Puertos comunes de React
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});*/
//Para angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirAngular", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();




// 4. PIPELINE DE MIDDLEWARES

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 1. CORS: Primero permitimos que Angular entre
app.UseCors("PermitirAngular");

// 2. Autenticación: Validamos quién es (Si trae su JWT)
app.UseAuthentication();

// 3. Autorización: Revisamos si tiene permiso para la URL que pide
app.UseAuthorization();

app.MapControllers();
app.Run();