using EduRateApi.Dtos;
using EduRateApi.Dtos.ShopDTO;
using EduRateApi.Interfaces;
using EduRateApi.Models;
using FirebaseAdmin.Auth;
using FireSharp.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Reflection;
using UrbanLvivProjectAPI.Models;

namespace EduRateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;
        public ShopController(IShopService shop)
        {
            _shopService= shop;
        }

        [HttpGet("GetShopItemById/{itemId}")]
        public async Task<ActionResult<ShopResponse>> GetShopItemById(string itemId)
        {
            var response = await _shopService.GetShopItemById(itemId);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpGet("GetActiveShopItems")]
        public async Task<ActionResult<AllShopItemResponse>> GetActiveShopItems()
        {
            var response = await _shopService.GetActiveShopItems();
            return Ok(response);
        }
        

        [HttpPost("UploadShopItem")]
        public async Task<ActionResult<ServerResponse>> UploadShopItem([FromBody] ShopItemDto itemDto)
        {
            var response = await _shopService.UploadItemInShop(itemDto);
            return StatusCode((int)response.statusCode, response);
        }
    }
}
