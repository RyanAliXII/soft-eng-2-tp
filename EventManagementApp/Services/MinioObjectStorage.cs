using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Response;

namespace EventManagementApp.Services;
class MinioObjectStorage : IMinioObjectStorage{
    private readonly IMinioClient _minio;
    private readonly IConfiguration _config;
    public MinioObjectStorage(IMinioClient minio, IConfiguration config){
        _minio = minio;
        _config = config;
    }
    public IMinioClient GetClient(){
        return _minio;
    }
    public async Task<PutObjectResponse> Upload(Stream? file, string contentType, string? bucket = null , string folder = "" ){
        var extension = MimeTypes.MimeTypeMap.GetExtension(contentType);
        var filename = $"{Guid.NewGuid()}{extension}";
        var objectName = Path.Combine(folder, filename).Replace("\\", "/");
        var putObjectArgs = new PutObjectArgs();
        var b = bucket ?? _config.GetSection("Minio").GetValue("DefaultBucket", "");
        putObjectArgs.WithBucket(b);
        putObjectArgs.WithObject(objectName);
        putObjectArgs.WithStreamData(file);
        putObjectArgs.WithObjectSize(file?.Length ?? -1);
        putObjectArgs.WithContentType(contentType);
        return await _minio.PutObjectAsync(putObjectArgs);
    }
    public async void Delete(string objectName, string ? bucket = null){
        var b =  bucket ?? _config.GetSection("Minio").GetValue("DefaultBucket", "");
        var roa = new RemoveObjectArgs();
        roa.WithObject(objectName);
        roa.WithBucket(b);
        await _minio.RemoveObjectAsync(roa);
    }
    public async Task<string>GetPresignedUrl(string objectName, string? bucket = null, int expiration = 86400 ) {
        var pgoa = new PresignedGetObjectArgs();
        var b = bucket ?? _config.GetSection("Minio").GetValue("DefaultBucket", "");
        pgoa.WithBucket(b);
        pgoa.WithObject(objectName);
        pgoa.WithExpiry(expiration);
        var result = await _minio.PresignedGetObjectAsync(pgoa);
        return result;
    }
    
}

public interface IMinioObjectStorage {
     public Task<PutObjectResponse> Upload(Stream? file, string contentType, string? bucket = null , string folder = "" );
     public void Delete(string objectName, string ? bucket = null);
     public Task<string> GetPresignedUrl(string objectName, string? bucket = null, int expiration = 86400 ); 
}
