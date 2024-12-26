var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<EnrollmentRepository>();
builder.Services.AddScoped<EnquiryRepository>();
builder.Services.AddScoped<FeedbackRepository>();
builder.Services.AddScoped<MaterialRepository>();
// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("MyConstr") ??
                throw new InvalidOperationException("Connection string 'MyConstr' not found");
 
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));
// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(50);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
//app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();
