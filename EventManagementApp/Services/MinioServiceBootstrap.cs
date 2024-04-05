using Microsoft.IdentityModel.Tokens;
using Minio;
using Minio.DataModel.Args;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace EventManagementApp.Services;
public static class MinioServiceBootstrap {
    public async static void CreateDefaultBucketAndPolicy(IMinioClient minio, IConfiguration config){
       
       var minioConfig =  config.GetSection("Minio");
       var DefaultBucket = minioConfig.GetValue<string>("DefaultBucket", "");
       if(DefaultBucket.IsNullOrEmpty()){
            throw new Exception("Invalid bucket name");
       }
       var bea = new BucketExistsArgs();
       bea.WithBucket(DefaultBucket);
       var isBucketExists =  await minio.BucketExistsAsync(bea);
       if(!isBucketExists){
            var mba = new MakeBucketArgs();
            mba.WithBucket(DefaultBucket);
            await minio.MakeBucketAsync(mba);
       }
       CreateBucketPolicy(minio, DefaultBucket ?? "");
    }
    private static async void CreateBucketPolicy(IMinioClient minio, string bucket){
            var spa = new SetPolicyArgs();
              Policy policy = new Policy
                {
                    Version = "2012-10-17",
                    Statement = new List<Statement>
                    {
                        new Statement
                        {
                            Effect = "Allow",
                            Principal = new Principal
                            {
                                AWS = new List<string> { "*" }
                            },
                            Action = new List<string> { "s3:GetObject" },
                            Resource = []
                        }
                    }
                };
            string jsonPolicy = JsonSerializer.Serialize(policy, new JsonSerializerOptions(){
                WriteIndented = true
            });    
            spa.WithBucket(bucket);
            spa.WithPolicy(jsonPolicy);
            await minio.SetPolicyAsync(spa);
    }
    public static IMinioClient BuildDefaultMinioClient(IMinioClient client, IConfiguration config){

        var minioConfig =  config.GetSection("Minio");
        var minioAccessKey = minioConfig.GetValue<string>("AccessKey", "");
        var minioSecretKey = minioConfig.GetValue<string>("SecretKey", "");
        var endpoint = minioConfig.GetValue<string>("Endpoint", "");
        if(minioAccessKey.IsNullOrEmpty()){
            throw new Exception("Minio access key is required.");
        }
        if(minioSecretKey.IsNullOrEmpty()){
            throw new Exception("Minio secret key is required.");
        }
        if(endpoint.IsNullOrEmpty()){
            throw new Exception("Minio endpoint is required.");
        }
        client.WithCredentials(minioAccessKey, minioSecretKey).WithEndpoint(endpoint).WithSSL(false);
        return client;
    }
}

public class Principal
{
    [JsonPropertyName("AWS")]
    public List<string> AWS { get; set; } = [];
}

public class Statement
{
    [JsonPropertyName("Effect")]
    public string Effect { get; set; } = "";

    [JsonPropertyName("Principal")]
    public Principal Principal { get; set; } = new Principal();

    [JsonPropertyName("Action")]
    public List<string> Action { get; set; } = [];

    [JsonPropertyName("Resource")]
    public List<string> Resource { get; set; } = [];
}

public class Policy
{
    [JsonPropertyName("Version")]
    public string Version { get; set; } = "";

    [JsonPropertyName("Statement")]
    public List<Statement> Statement { get; set; } = [];
}
