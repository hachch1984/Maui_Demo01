using BackendApi;

var builder = WebApplication.CreateBuilder(args);



//01 - Configurar servicios
Program_Service.Configure(builder);

var app = builder.Build();

//02 - Configurar middleware 
Program_Middleware.Configure(builder, app);

 
//app.UseHttpsRedirection(); // Redirección HTTPS si es necesario

//03 - Configurar enpoints 
Program_EndPoint.Configure(builder, app);


await app.RunAsync();
