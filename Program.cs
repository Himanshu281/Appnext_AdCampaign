using Appnext_AdCampaign.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactDev", policy =>
    {
        policy.WithOrigins("http://localhost:3000")   // React dev server origin
              .AllowAnyMethod()
              .AllowAnyHeader()                       // or .WithHeaders("X-API-KEY", "Content-Type")
              .AllowCredentials();
    });
});

builder.Services.AddSingleton<ICampaignService, CampaignService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Middlewares
app.UseCors("AllowReactDev");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
