var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

string CORSOpenPolicy = "AddCORSPolicy";
builder.Services.AddCors();
// Add services to the container.
builder.Services.AddCors(options =>
{
	options.AddPolicy(
	name: CORSOpenPolicy,
	builder => {
		builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
	});
});
app.UseCors(CORSOpenPolicy);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
