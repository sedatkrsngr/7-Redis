using Microsoft.AspNetCore.Mvc;
using RedisStackExchangeAPI.web.Service;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisStackExchangeAPI.web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDb(0);//Bu controllerda db0 da çalışıyoruz
        }

        public IActionResult Index()
        {

            _database.StringSet("name", "sedat");
            _database.StringSet("sayi", 100);

            Byte[] img = default(byte[]);
            _database.StringSet("resim", img);//istersej byte değer de verebiliriz
            return View();
        }

        public IActionResult show()
        {
            

            var name = _database.StringGet("name");
           _database.StringIncrement("sayi",10);//sayıyı verilen değer kadar arttırır
           _database.StringDecrement("sayi",10);//sayıyı verilen değer kadar azaltır
           var data =_database.StringDecrementAsync("sayi",10).Result;//asenkron istersek await, async kullanmadan direkt alabiliriz. R
            _database.StringDecrementAsync("sayi", 10).Wait();//asenkron metotda işlemi yapsın geriye birşey dönmesin istersek bu şekilde 

            var valName = _database.StringGetRange("name", 0, 3);//ilgili indeks aralığındaki harfleri basar
            var countName = _database.StringLength("name");//ilgili value değeri kaç karakterli


            if (name.HasValue)//rediste böyle bir kayıt varsa
            {
                ViewBag.name = name;
            }

            return View();
        }
    }
}
