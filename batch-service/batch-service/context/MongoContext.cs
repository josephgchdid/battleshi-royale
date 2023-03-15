using MongoDB.Driver; 

namespace batch_service.context;

public class MongoContext
{
    public readonly IMongoDatabase database;

    private IConfiguration configuration;

    private IWebHostEnvironment env { get; }

    public MongoContext(IConfiguration _configuration,IWebHostEnvironment _env)
    {
        env = _env;

        configuration = _configuration;

        var client = new MongoClient(
            configuration.GetValue<string>("DataBaseSettings:ConnectionString")
        );

        database = client.GetDatabase(
            configuration.GetValue<string>(
                env.IsDevelopment() ?
                    "DataBaseSettings:DatabaseName_Dev"
                    :
                    "DataBaseSettings:DatabaseName"
            )
        );
    }


}

