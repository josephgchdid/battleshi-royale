using AutoMapper;
using game_service.Mapper;
using game_service.Repository;
using game_service.Repository.Interface;
using game_service.Service;
using Newtonsoft.Json;

namespace game_service;

public class Startup
{
    private IConfiguration configuration;

    public Startup(IConfiguration _configuration)
    {
        configuration = _configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });

        services.AddScoped<MongoContext>();
        
        services.AddScoped<IGameRepository, GameRepository>();

        services.AddScoped<GameService>();
        
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        IMapper mapper = mapperConfig.CreateMapper();

        services.AddSingleton(mapper);


    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
