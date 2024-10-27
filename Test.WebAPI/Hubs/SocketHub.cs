using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace Test.WebAPI.Hubs
{
    public class SocketHub : Hub
    {
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SocketHub(IMemoryCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
        }
        public override async Task OnConnectedAsync()
        {
            var http_context = _httpContextAccessor.HttpContext;

            if (http_context?.Request.Headers.TryGetValue("UserId", out var userId) == true)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(8));

                _cache.Set(userId.ToString(), Context.ConnectionId, cacheEntryOptions);


            }

            await base.OnConnectedAsync();
        }



    }
}
