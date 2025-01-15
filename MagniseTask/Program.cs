using System.Text.Json.Serialization;
using MagniseTask.Data;
using MagniseTask.Interfaces;
using MagniseTask.Services;
using MagniseTask.Services.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
	.AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "bearer",
		BearerFormat = "JWT",
		Description = "JWT Authorization header using the Bearer scheme."
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] {}
		}
	});
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<MarketDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<UserCredentials>(builder.Configuration.GetSection("FintachartsAPI:credentials"));

builder.Services.AddHttpClient("FintachartsClient",client =>
{
	client.BaseAddress = new Uri(builder.Configuration["FintachartsAPI:Uri"]!);
});

builder.Services.AddScoped<IFintachartsAuthService, FintachartsAuthService>();
builder.Services.AddScoped<IFintachartsDataService, FintachartsDataService>();
builder.Services.AddScoped<IAssetsRepository, AssetsRepository>();
builder.Services.AddTransient<IPriceUpdateService, PriceUpdateService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();