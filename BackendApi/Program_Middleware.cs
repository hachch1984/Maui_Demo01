namespace BackendApi
{
    public static class Program_Middleware
    {


        public static void Configure(WebApplicationBuilder builder, WebApplication app)
        {

            if (builder.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(
                 c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiPeliculas v1")
                    );

            }

            app.UseOutputCache();

            app.UseAuthentication();

            app.UseAuthorization();

        }
    }
}
