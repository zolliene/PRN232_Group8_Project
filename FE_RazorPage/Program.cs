using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddHttpClient("BackendApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7022/api/");
});

builder.Services.AddSingleton(new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
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
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapRazorPages();
//app.Use(async (context, next) =>
//{
//    // Kiểm tra nếu request là "/" hoặc "/Index"
//    if ((context.Request.Path == "/" || context.Request.Path == "/Index"))
//    {
//        // Kiểm tra session đã có jwtToken chưa
//        var hasToken = context.Session.GetString("jwtToken");
//        if (string.IsNullOrEmpty(hasToken))
//        {
//            // Nếu CHƯA đăng nhập thì redirect login
//            context.Response.Redirect("/Index");
//            return;
//        }
//    }
//    await next();
//});


app.Run();
