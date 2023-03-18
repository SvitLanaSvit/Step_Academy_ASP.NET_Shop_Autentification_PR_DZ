
using ASP_Meeting_18.AutoMapperProfiles;
using ASP_Meeting_18.Data;
using ASP_Meeting_18.Infrostructure.ModelBinderProviders;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var conficuration = builder.Configuration;
builder.Services.AddAutoMapper(typeof(CategoryProfile));
builder.Services.AddAutoMapper(typeof(ProductProfile));
//builder.Services.AddAutoMapper(typeof(PhotoProfile));

builder.Services.AddControllersWithViews(optins =>
optins.ModelBinderProviders.Insert(0, new CartModelBinderProvider()));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ShopDbContext>();

string connStr = builder.Configuration.GetConnectionString("shopDb");
builder.Services.AddDbContext<ShopDbContext>(options =>
options.UseSqlServer(connStr));

builder.Services.AddAuthentication().AddGoogle(options =>
{
    IConfigurationSection googleSection = conficuration.GetSection("Authentication:Google");
    options.ClientId = googleSection.GetValue<string>("ClientId");
    options.ClientSecret = googleSection["ClientSecret"];
});
builder.Services.AddAuthentication().AddFacebook(fbOptions =>
{
    IConfigurationSection facebookSection = conficuration.GetSection("Authentication:Facebook");
    fbOptions.AppId = facebookSection["AppId"];
    fbOptions.AppSecret = facebookSection["AppSecret"];
});
builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("FrameworkPolicy", policy =>
    {
        policy.RequireClaim("PrefferedFramework", new[] { "ASP.NET Core" });
        policy.RequireRole("admin", "manager");
    });
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

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
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();


//app.MapControllerRoute(
//    name: "categoryProducts",
//    pattern: "{category}/{page:int?}",
//    defaults: new { controller = "Home", action = "Index" });

//app.MapControllerRoute(
//    name: "paged",
//    pattern: "Page{page:int}",
//    defaults: new { controller = "Home", action = "Index" });
app.MapControllerRoute(
    name: "root",
 pattern: "/",
 defaults: new { controller = "Home", action = "Index", page=1, category = (string)null }
 );
app.MapControllerRoute(
     name: "Default",
     pattern: "Page{page}",
     defaults: new { controller = "Home", action = "Index", category = (string)null },
     constraints: new { page = @"\d+" });
app.MapControllerRoute(
    name: "cart",
    pattern: "cart/{action=Index}",
    defaults: new { controller = "Cart"}
 );
app.MapControllerRoute(
     name: "Category",
     pattern: "{category:alpha}",
     defaults: new { controller = "Home", action = "Index", page = 1 },

     constraints: new { category = @"^(?!Admin$|Products$|Categories$|Photos$|Cart$|Account$|Claims$|Roles$|User$).*" }
     );
app.MapControllerRoute(
    name: "CategoryPaging",
 pattern: "{category:alpha}/Page{page}",
 defaults: new { controller = "Home", action = "Index" },
 constraints: new { page = @"\d+" });






app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();