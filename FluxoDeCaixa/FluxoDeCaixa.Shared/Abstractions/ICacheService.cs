﻿namespace FluxoDeCaixa.Shared.Abstractions
{
    public interface ICacheService
    {
        Task<TItem> GetOrCreateAsync<TItem>(string cacheKey, Func<Task<TItem>> factory);
        Task RemoveAsync(string cacheKey);
    }
}
