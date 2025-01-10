using StackExchange.Redis;
using System.Threading.Tasks;

namespace GarageQueueUpload.Services
{
    public class RedisService
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<string> GetFilePathAsync(string fileName)
        {
            var db = _redis.GetDatabase();
            var fileData = await db.HashGetAsync($"file:{fileName}", "FilePath");

            return fileData.HasValue ? fileData.ToString() : string.Empty;
        }
    }
}
