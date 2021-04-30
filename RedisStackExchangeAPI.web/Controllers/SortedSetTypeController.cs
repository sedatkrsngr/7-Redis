using Microsoft.AspNetCore.Mvc;
using RedisStackExchangeAPI.web.Service;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisStackExchangeAPI.web.Controllers
{
    public class SortedSetTypeController : Controller//setten farkı eklerken score veririz. Cache dolduğunda önceliğe göre siler
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;

        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDb(3);//Bu controllerda db3 da çalışıyoruz
        }

        public IActionResult Index()
        {
           

            _database.SortedSetAdd("name", "sedat", 1);
            _database.KeyExpire("name", DateTime.Now.AddDays(1));//ilgili değer belirtilen süreden sonra silincek

            _database.SortedSetScan("name");//listeyi redisteki sıralamaya göre çekeriz

            _database.SortedSetRemove("name", "sedat");//silme işlemmi

            _database.SortedSetRangeByRank("name", order: Order.Ascending);//scora göre küçükten büyüğe sıralar
            _database.SortedSetRangeByRank("name",0,5, order: Order.Ascending);// 0 ve 5 arasındakileri scora göre küçükten büyüğe sıralar
            return View();
        }
    }
}
