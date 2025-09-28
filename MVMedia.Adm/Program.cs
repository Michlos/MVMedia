using MVMedia.Adm.DTOs.Mapping;
using MVMedia.Adm.Services;
using MVMedia.Adm.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<BearerTokenHandler>();

//REGISTER HTTP CLIENT SERVICE WITH API BASE URL
builder.Services.AddHttpClient("MVMediaAPI", a =>
{
    a.BaseAddress = new Uri(builder.Configuration["ServiceUri:MVMediaAPI"]);
})
.AddHttpMessageHandler<BearerTokenHandler>();

//REGISTER SERVICES
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<ApiAuthService>();

//ADD SERVICES AUTOMAPPER
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<EntitiesToDTOMappingProfile>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
