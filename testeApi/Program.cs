var builder = WebApplication.CreateBuilder(args);

// 1) Policy de CORS em DEV (j� configurada antes)
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

// 3) Middleware de arquivos est�ticos: 
//    - UseDefaultFiles ir� �mapear� requisi��es a �/� para wwwroot/index.html
//    - UseStaticFiles ir� servir tudo em wwwroot/** diretamente como arquivo est�tico.
app.UseDefaultFiles();
app.UseStaticFiles();

// 4) Se voc� tiver redirecionamento HTTPS, pode deixar ou remover conforme sua configura��o de certificado
app.UseHttpsRedirection();

// 5) Middleware de autoriza��o (caso utilize autentica��o)
app.UseAuthorization();

// 6) Finalmente, mapeia os controllers (ex.: /WeatherForecast)
app.MapControllers();

app.Run();
