using FiapCloudGamesAPI.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserService.Data;
using UserService.Interfaces;
using UserService.Repositories;
//Adicionado do monolito FIAPCloudGamesProject, para reaproveitar o código
//precisa analisar o que realmente é necessário
//modificar o que precisa ser modificado
//e remover o que não for necessário para microservices

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls("http://0.0.0.0:80");
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//Com erro comentar por enquanto

#region Banco de dados
//builder.Services.AddDbContext<UserDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//Com erro comentar por enquanto
#endregion

#region Injeção de dependência
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthService>();
#endregion

#region JWT Auth
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
//        };
//    });
//Com erro comentar por enquanto
#endregion

//para o Erro de Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
//para aplicar as migrations na primeira vez que subir o container do Docker
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
//    dbContext.Database.Migrate();
//}
//migrate com erro comentar por enquanto
app.UseCors();

#region Swagger
app.UseSwagger();
//app.UseSwaggerUI();
//Com erro comentar por enquanto
#endregion

#region Middleware customizado
//app.UseMiddleware<ErrorHandlingMiddleware>();
//Com erro comentar por enquanto
#endregion

#region Pipeline de autenticação e autorização
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
#endregion

app.Run();
