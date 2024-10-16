
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using EduRateApi.Dtos.ShopDTO;
using EduRateApi.Interfaces;
using EduRateApi.Models;
using UrbanLvivProjectAPI.Interfaces;
using UrbanLvivProjectAPI.Models;

namespace EduRateApi.Implementation
{
    public class ShopService : IShopService
    {

        private readonly IFirebaseConnectingService _firebaseConnectingService;
        public ShopService(IFirebaseConnectingService firebaseConnectingService)
        {
            _firebaseConnectingService = firebaseConnectingService;
        }
        
        
        public async Task<AllShopItemResponse> GetActiveShopItems()
        {
            try
            {
                using (var client = _firebaseConnectingService.GetFirebaseClient())
                {
                    if (client != null)
                    {
                        var response = await client.GetAsync("Shop");
                        if (response.Body != "null") 
                        {
                            var data = response.ResultAs<Dictionary<string, ShopItem>>();
                            var activeShopItems = new List<ShopItem>();

                            foreach (var pair in data)
                            {
                                if (!pair.Value.disabled)
                                {
                                    activeShopItems.Add(pair.Value);
                                }
                            }

                            return new AllShopItemResponse(shopItem: activeShopItems, message: "OK", statusCode: 200);
                        }
                        else
                        {
                            return new AllShopItemResponse(shopItem: new List<ShopItem>(), message: "OK, but zero items", statusCode: 200);
                        }
                    }
                    else
                    {
                        return new AllShopItemResponse(shopItem: new List<ShopItem>(), message: "BAD", statusCode: 400);
                    }
                }
            }
            catch (Exception ex)
            {
                return new AllShopItemResponse(shopItem: new List<ShopItem>(), message: "BAD", statusCode: 400);
            }
        }

        public async Task<ShopResponse> GetShopItemById(string itemId)
        {
            try
            {
                using (var client = _firebaseConnectingService.GetFirebaseClient())
                {
                    if (client != null)
                    {
                        var response = await client.GetAsync($"Shop/{itemId}");
                        if (response.Body != "null")
                        {
                            var shopItem = response.ResultAs<ShopItem>();
                            return new ShopResponse(shopItem, "Post retrieved successfully", 200);
                        }
                        else
                        {
                            return new ShopResponse(new ShopItem(), "None items", 200);
                        }
                    }
                    else
                    {
                        return new ShopResponse(new ShopItem(), "", 500);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ShopResponse(new ShopItem(), "Firebase connection failed", 400);
            }
        }

        public async Task<ServerResponse> UpdateShopItem(ShopItem shopItem)
        {
            try
            {
                using (var client = _firebaseConnectingService.GetFirebaseClient())
                {
                    if (client != null)
                    {
                        var response = await client.UpdateAsync($"Shop/{shopItem.itemId}", shopItem);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            return new ServerResponse($"Shop item {shopItem.itemId} updated successfully", 200);
                        }
                        else
                        {
                            return new ServerResponse($"Failed to update shop item {shopItem.itemId}", (int)response.StatusCode);
                        }
                    }
                    else
                    {
                        return new ServerResponse("Firebase connection failed", 400);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ServerResponse("An error occurred while updating shop item in Firebase: " + ex.Message, 500);
            }
        }

        public async Task<ServerResponse> UploadItemInShop(ShopItemDto item)
        {
            try
            {
                using (var client = _firebaseConnectingService.GetFirebaseClient())
                {
                    if (client != null)
                    {

                        var newShopItem = Guid.NewGuid().ToString();
                        var setUserPost = await client.SetAsync($"Shop/{newShopItem}/", new ShopItem
                        {
                            itemId = newShopItem,
                            price = item.price,
                            description = item.description,
                            title = item.title,
                            itemCount = item.itemCount,
                            itemImage = item.itemImage,
                            disabled = false
                        });
                        return new ServerResponse(message: "Item Succesfully Uploaded", statusCode: 200);

                    }
                    else
                    {
                        return new ServerResponse(message: "Firebase connection failed", statusCode: 500);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ServerResponse(message: "An error occurred while uploading items to Firebase:", statusCode: 400);
            }
        }
    }
}
