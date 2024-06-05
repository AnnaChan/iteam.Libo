using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using iteam.Libo.Common.Dto;
using System.Text.Json;

namespace iteam.Libo.WebApp.Services;

public class ItemService
{
    private readonly HttpClient _httpClient;

    public ItemService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Method to get a list of items
    public async Task<List<ItemDto>> GetItemsAsync()
    {
        // Use the HttpClient instance to make the GET request
        return await _httpClient.GetFromJsonAsync<List<ItemDto>>("/items");
    }

    // Method to borrow an item
    public async Task<HttpResponseMessage> BorrowItemAsync(int itemId, int borrowerId)
    {
        // Create an instance of BorrowItemDto with ItemId and BorrowerId
        var borrowItemDto = new BorrowItemDto(itemId, borrowerId);

        // Create JSON content from the borrowItemDto object
        var content = JsonContent.Create(borrowItemDto);

        // Make a POST request to the /items/borrow endpoint with the JSON content
        var response = await _httpClient.PostAsync("/items/borrow", content);

        return response; // Return the HttpResponseMessage to handle the response in the calling code
    }

}

