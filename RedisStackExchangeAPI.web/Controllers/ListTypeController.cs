using Microsoft.AspNetCore.Mvc;
using RedisStackExchangeAPI.web.Service;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisStackExchangeAPI.web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;

        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDb(1);//Bu controllerda db1 da çalışıyoruz
        }
        public IActionResult Index()
        {
            _database.ListLeftPush("name", "sedat");//listenin sonuna ekler

            if (_database.KeyExists("name"))//veritabanında böyle bir liste var mı
            {
                _database.ListRange("name",2);//index 2 den itibareb verileri getirir. "name",-1 veya sadece "name" koyarsak hepsini getirir
            }

            _database.ListRemove("name", "sedat");// verilen value değerini listeden siler

            _database.ListLeftPop("name");//listeden ilk veriyi çıkarır
            _database.ListRightPop("name");//listeden son veriyi çıkarır

            return View();
        }
    }
}
