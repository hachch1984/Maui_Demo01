using BackendApi;

var builder = WebApplication.CreateBuilder(args);



// Configurar servicios
Program_Service.Configure(builder);

var app = builder.Build();

// Configurar middleware 
Program_Middleware.Configure(builder, app);

 
//app.UseHttpsRedirection(); // Redirección HTTPS si es necesario

// Configurar enpoints 
Program_EndPoint.Configure(builder, app);


await app.RunAsync();
