using Asp.Versioning;

namespace VtuHost.WebApi.Extensions;

public static class ApiVersioningExtension
{
    public static void ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;       
            options.DefaultApiVersion = new ApiVersion(1, 0);   
            options.ReportApiVersions = true;        
            options.ApiVersionReader = ApiVersionReader.Combine(                
                new UrlSegmentApiVersionReader(),                   
                new QueryStringApiVersionReader("api-version"),     
                new HeaderApiVersionReader("X-Version"),               
                new MediaTypeApiVersionReader("X-Version"));          
        })                                                                      
        .AddMvc(options => { })                                                 
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";             
            options.SubstituteApiVersionInUrl = true;       
        });
    }
}
