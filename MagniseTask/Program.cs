using MagniseTask.Data;
using MagniseTask.Interfaces;
using MagniseTask.Services;
using MagniseTask.Services.Background;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MarketDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<UserCredentials>(builder.Configuration.GetSection("FintachartsAPI:credentials"));

builder.Services.AddHttpClient("FintachartsClient",client =>
{
	client.BaseAddress = new Uri("https://platform.fintacharts.com");
});

builder.Services.AddSingleton<AuthDataManager>();
builder.Services.AddScoped<IFintachartsAuthService, FintachartsAuthService>();
builder.Services.AddScoped<IAssetsRepository, AssetsRepository>();
builder.Services.AddTransient<IPriceUpdateService, PriceUpdateService>();

builder.Services.AddHostedService<TokenRefreshBgService>();

builder.Services.AddAuthentication("Bearer")
	.AddJwtBearer(options =>
	{
		options.Authority = $"{builder.Configuration["FintachartsAPI:Uri"]}/identity";
		options.Audience = $"{builder.Configuration["FintachartsAPI:client_id"]}";
	});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();