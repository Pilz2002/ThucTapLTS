using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ThucTapLTSedu.DataContext;
using ThucTapLTSedu.Payloads.Converter.CinemaConverter;
using ThucTapLTSedu.Payloads.Converter.UserConverter;
using ThucTapLTSedu.Payloads.DataResponses.AdminResponses;
using ThucTapLTSedu.Payloads.DataResponses.AuthResponses;
using ThucTapLTSedu.Payloads.DataResponses.UserResponses;
using ThucTapLTSedu.Payloads.Responses;
using ThucTapLTSedu.Services.Implements;
using ThucTapLTSedu.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Connect to database
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"));
});


//Config authencation
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
	x.RequireHttpsMetadata = false;
	x.SaveToken = true;
	x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		ValidateAudience = false,
		ValidateIssuer = false,
		IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:SecretKey").Value))
	};
});

//Add Cors
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: MyAllowSpecificOrigins,
					  policy =>
					  {
						  policy.WithOrigins("http://localhost:8080",
											  "http://www.contoso.com").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
					  });
});

//Add scopped
builder.Services.AddScoped<IAuthServices,AuthServices>();
builder.Services.AddScoped<IAdminServices, AdminServices>();
builder.Services.AddScoped<IUserServices, UserServices>();

builder.Services.AddScoped<ResponseObject<List<DataResponse_Seat>>>();
builder.Services.AddScoped<ResponseObject<List<DataResponse_Room>>>();
builder.Services.AddScoped<ResponseObject<DataResponses_Cinema>>();
builder.Services.AddScoped<ResponseObject<DataResponse_Token>>();
builder.Services.AddScoped<ResponseObject<DataResponse_User>>();
builder.Services.AddScoped<ResponseObject<DataResponse_Food>>();
builder.Services.AddScoped<ResponseObject<string>>();
builder.Services.AddScoped<ResponseObject<DataResponse_Room>>();
builder.Services.AddScoped<ResponseObject<DataResponse_Seat>>();
builder.Services.AddScoped<ResponseObject<DataResponse_Movie>>();
builder.Services.AddScoped<ResponseObject<DataResponse_Schedule>>();
builder.Services.AddScoped<ResponseObject<DataResponse_Bill>>();

builder.Services.AddScoped<Food_Converter>();
builder.Services.AddScoped<User_Converter>();
builder.Services.AddScoped<Room_Converter>();
builder.Services.AddScoped<Cinema_Converter>();
builder.Services.AddScoped<Seat_Converter>();
builder.Services.AddScoped<Movie_Converter>();
builder.Services.AddScoped<Schedule_Converter>();
builder.Services.AddScoped<Ticket_Converter>();
builder.Services.AddScoped<BillFood_Converter>();
builder.Services.AddScoped<BIll_Converter>();

builder.Services.AddScoped<AppDbContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
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
