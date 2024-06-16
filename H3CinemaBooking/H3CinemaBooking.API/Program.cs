using H3CinemaBooking.Repository.Data;
using H3CinemaBooking.Repository.DTO;
using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using H3CinemaBooking.Repository.Repositories;
using H3CinemaBooking.Repository.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IUserDetailRepository, UserDetailRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<Roles>();

//Add Generic Repository to Services, And add services to the container
builder.Services.AddScoped<IGenericRepository<Cinema>, GenericRepository<Cinema>>();
builder.Services.AddScoped<IGenericRepository<CinemaHall>, GenericRepository<CinemaHall>>();
builder.Services.AddScoped<IGenericRepository<Area>, GenericRepository<Area>>();
builder.Services.AddScoped<IGenericRepository<Region>, GenericRepository<Region>>();
builder.Services.AddScoped<IGenericRepository<Roles>, GenericRepository<Roles>>();

//Add service to Services, And add services to the container
builder.Services.AddScoped<IUserDetailService, UserDetailService>();
builder.Services.AddScoped<IJWTokenService, JWTokenService>();
builder.Services.AddScoped<IPropertyValidationService, PropertyValidationService>();
//Add IShowService 
builder.Services.AddScoped<IShowService, ShowService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<HashingService>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("coffee",
                          policy =>
                          {
                              policy.AllowAnyOrigin()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod();
                          });
});

var conStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Dbcontext>(options => options.UseSqlServer(conStr, b => b.MigrationsAssembly("H3CinemaBooking.API")));

// Add JWT authentication and authorization
var key = Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("AdminOrCustomer", policy => policy.RequireRole("Admin", "Customer"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("coffee");
app.UseHttpsRedirection();

app.UseAuthentication();  // Add this line
app.UseAuthorization();

app.MapControllers();

app.Run();
