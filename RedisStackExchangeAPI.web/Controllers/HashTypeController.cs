using Microsoft.AspNetCore.Mvc;
using RedisStackExchangeAPI.web.Service;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisStackExchangeAPI.web.Controllers
{
    public class HashTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;

        public HashTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDb(1);//Bu controllerda db1 da çalışıyoruz
        }
        public IActionResult Index()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            _database.HashSet("isimler", "name", "sedat");//isimler hashlistesinde name keyine ait sedat adı var

            if (_database.KeyExists("isimler"))
            {
                _database.HashGetAll("isimler").ToList().ForEach(x=> {//veriinin hepsini çektik

                    list.Add(x.Name, x.Value);
                
                });

            }
            _database.HashDelete("isimler", "sedat");//silme işlemi

            _database.HashGet("isimler", "name");//name keyine ait datayı verir

            _database.HashExists("isimler","name");//isimleri hash üzerinde name key var mı

            return View();
        }
    }
}
