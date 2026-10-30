var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<CollegePortal.Services.CollegeStore>();
builder.Services.AddAuthentication("CollegePortal.Cookie")
    .AddCookie("CollegePortal.Cookie", options =>
    {
        options.LoginPath = "/Access/Login";
        options.AccessDeniedPath = "/Access/Denied";
        options.Cookie.Name = "CollegePortal.Auth";
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
