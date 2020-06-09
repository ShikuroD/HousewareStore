using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MvcClient.Infrastructure;
using MvcClient.Models;
using MvcClient.ViewModels;

namespace MvcClient.Services
{
    public class ItemService : IItemService
    {
        private readonly string _baseUrl;
        private readonly IHttpClient _httpClient;

        public ItemService(IHttpClient httpClient, IOptions<AppSettings> appSettings)
        {
            _httpClient = httpClient;
            _baseUrl = appSettings.Value.ItemUrl;
        }

        public async Task<IndexViewModel> GetCatalog(string category, string searchString, string sortOrder)
        {
            var uri = _baseUrl + $"/catalog?searchString={searchString}&category={category}&sortOrder={sortOrder}";

            return await _httpClient.GetAsync<IndexViewModel>(uri);
        }
        
        
        public async Task<CreateItemViewModel> GetCreateItem()
        {
            var uri = _baseUrl + $"/create";

            return await _httpClient.GetAsync<CreateItemViewModel>(uri);
        }

        public async Task<IList<Category>> GetCategories()

        {
            var uri = _baseUrl + $"/categories";

            return await _httpClient.GetAsync<List<Category>>(uri);
        }

        public async Task<Item> GetItem(int id)
        {
            var uri = _baseUrl + $"/{id}";

            return await _httpClient.GetAsync<Item>(uri);
        }

        public async Task CreateItem(Item item)
        {
            var uri = _baseUrl;

            await _httpClient.PostAsync<Item>(uri, item);
        }

        public async Task UpdateItem(int id, Item item)
        {
            var uri = _baseUrl + $"/{id}";

            await _httpClient.PutAsync<Item>(uri, item);
        }

        public async Task DeleteItem(int id)
        {
            var uri = _baseUrl + $"/{id}";

            await _httpClient.DeleteAsync(uri);
        }

        public async Task ChangeItemStatus(int id, ItemStatus status)
        {
            var uri = _baseUrl + $"/{id}";

            await _httpClient.DeleteAsync(uri);
        }

    }
}