var builder = WebApplication.CreateBuilder(args);

// 1) Policy de CORS em DEV (já configurada antes)
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsParaDesenvolvimento", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2) Habilita CORS para todas as rotas (apenas em DEV)
//    Se quiser filtrar origens diferentes em PROD, ajuste aqui.
if (app.Environment.IsDevelopment())
{
    app.UseCors("CorsParaDesenvolvimento");
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 3) Middleware de arquivos estáticos: 
//    - UseDefaultFiles irá “mapear” requisições a “/” para wwwroot/index.html
//    - UseStaticFiles irá servir tudo em wwwroot/** diretamente como arquivo estático.
app.UseDefaultFiles();
app.UseStaticFiles();

// 4) Se você tiver redirecionamento HTTPS, pode deixar ou remover conforme sua configuração de certificado
app.UseHttpsRedirection();

// 5) Middleware de autorização (caso utilize autenticação)
app.UseAuthorization();

// 6) Finalmente, mapeia os controllers (ex.: /WeatherForecast)
app.MapControllers();

app.Run();
