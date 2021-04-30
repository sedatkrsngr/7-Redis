using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisStackExchangeAPI.web.Service
{
    public class RedisService //service olarak kullanmak için startupta services olarak eklemeliyiz
    {
        private readonly string _redisHost;    //adresi burda manuel vermeyelim diye appsetting.json içerisinde tanımlandı
        private readonly string _redisPort;    //adresi burda manuel vermeyelim diye appsetting.json içerisinde tanımlandı
        private ConnectionMultiplexer _redis; //redisle haberleşecek ana sınıf

        public IDatabase database { get; set; }//database üzerindeki methodları barındırır
        public RedisService(IConfiguration configuration)
        {
            _redisHost = configuration["Redis:Host"];
            _redisPort = configuration["Redis:Port"];
        }

        public void Connect()//redis servere bağlanmak için. Uygulama ayağı kalkarken çalışması için startup
        {
            var configString = $"{_redisHost}:{_redisPort}";//localhost:6380

            _redis = ConnectionMultiplexer.Connect(configString);
        }

        public IDatabase GetDb(int dbId)//Redis üzerinde 16 db var db0,db1,db2,...db15 şeklinde olduğu içn hangi dbye kaydedileceğini belirtelim
        {

            return _redis.GetDatabase(dbId);//eğer boş bırakırsak db0 üzerine yazar
        }

    }
}
