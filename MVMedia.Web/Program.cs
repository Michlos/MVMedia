var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços necessários para autenticação via token
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<BearerTokenHandler>();

// Add services to the container.
builder.Services.AddRazorPages();

// Permitir certificados SSL não confiáveis apenas em desenvolvimento
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpClient("Default", client => { })
        .ConfigurePrimaryHttpMessageHandler(() =>
            new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            })
        .AddHttpMessageHandler<BearerTokenHandler>();
}
else
{
    builder.Services.AddHttpClient("Default", client => { })
        .AddHttpMessageHandler<BearerTokenHandler>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
