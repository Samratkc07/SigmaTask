using Microsoft.EntityFrameworkCore;
using SigmaTask;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();
builder.Services.AddDbContext<CandidateDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("myConnection")));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();