using Microsoft.AspNetCore.Mvc;
using RedisStackExchangeAPI.web.Service;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisStackExchangeAPI.web.Controllers
{
    public class SetTypeController : Controller // set list normal listten farkı ekleme rastgele yapılır
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;

        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDb(2);//Bu controllerda db2 da çalışıyoruz
        }
        public IActionResult Index()
        {
            _database.KeyExpire("name", DateTime.Now.AddMinutes(5));//ilgili key için her istek yapıldığında timeout sıfırlanıp 5 dk verecek eğer timeout sıfırlanmasın istiyorsak.

            if (!_database.KeyExists("name"))//eğer bu keye ait set listimiz varsa  timeout sıfırlayıp yeniden başlatma
            {
                _database.KeyExpire("name", DateTime.Now.AddMinutes(5));
            }


            _database.SetMembers("name");//burada listenin hepsini döner
            
            _database.SetAdd("name", "sedat");

            _database.SetRemove("name", "sedat");//sedat listeden silinir
            return View();
        }
    }
}
