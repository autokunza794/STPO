using Microsoft.EntityFrameworkCore;
using StudentWeb.Data;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5300"); // use 5300 to avoid clashes

builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=students.db"));

var app = builder.Build();

// Ensure DB & seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    if (!db.Students.Any())
    {
        db.Students.AddRange(
            new StudentWeb.Models.Student { StudentCode="66010001", FirstName="สมชาย", LastName="ใจดี", Faculty="วิศวกรรมศาสตร์", Major="คอมพิวเตอร์", Year=2, Email="66010001@u.ac.th", Phone="0811111111", GPA=3.45 },
            new StudentWeb.Models.Student { StudentCode="66010002", FirstName="อรพรรณ", LastName="พัฒน์", Faculty="วิทยาศาสตร์", Major="คณิตศาสตร์", Year=1, Email="66010002@u.ac.th", Phone="0822222222", GPA=3.78 }
        );
        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

// Redirect root to students list
app.MapGet("/", () => Results.Redirect("/Students"));

app.Run();
