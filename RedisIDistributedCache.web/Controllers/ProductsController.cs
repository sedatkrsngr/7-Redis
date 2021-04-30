using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisIDistributedCache.web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisIDistributedCache.web.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public IActionResult Index()
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);//In memory ile aynı verilen süreden sonra silinecek
            options.SlidingExpiration = TimeSpan.FromSeconds(10);//In memory ile aynı verilen sürede erişilirse verilen süre kadar süresi artacak        

            _distributedCache.SetString("name", "sedat",options);//string veriyi oluştururuz

            string name = _distributedCache.GetString("name");//string veriyi ilgili key ile sileriz

            _distributedCache.Remove("name");// veriyi key ile silebiliriz

            return View();
        }

        public async Task<IActionResult> Index2()// asenkron kullanım şekli
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);//In memory ile aynı verilen süreden sonra silinecek
            options.SlidingExpiration = TimeSpan.FromSeconds(10);//In memory ile aynı verilen sürede erişilirse verilen süre kadar süresi artacak        

            await _distributedCache.SetStringAsync("name", "sedat", options);//string veriyi oluştururuz

            string name = await _distributedCache.GetStringAsync("name");//string veriyi ilgili key ile sileriz

           await _distributedCache.RemoveAsync("name");// veriyi key ile silebiliriz

            return View();
        }

        public async Task<IActionResult> Index3()// Model gibi kompleks yapıları kaydetme
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);//In memory ile aynı verilen süreden sonra silinecek
            options.SlidingExpiration = TimeSpan.FromSeconds(10);//In memory ile aynı verilen sürede erişilirse verilen süre kadar süresi artacak        

            Product product = new Product { 
            Id=1,
            Name="Kalem",
            Price=200
            };

            string jsonProduct = JsonConvert.SerializeObject(product);

            //1.yol json formatında kaydetmek bunun için setstring ve getirmek için ise getstring kullanılır asenkron olup olmaması bize bağlı
           await _distributedCache.SetStringAsync("product:1", jsonProduct, options); //elimizdeki compleks yapıyı jsone çevirerek kaydettik


            //getirmek için ise

            string getJsonProduct = await _distributedCache.GetStringAsync("product:1");

            Product productNesnesi = JsonConvert.DeserializeObject<Product>(getJsonProduct);

            //2.yol ise binary olarak kaydetmek set ile getirmek ise get ile olur  asenkron olup olmaması bize bağlı

            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);

            await _distributedCache.SetAsync("product:1", byteProduct);

            //getirmek için ise

            Byte[] getByteProduct =  await _distributedCache.GetAsync("product:1");

            string jsonByteProduct = Encoding.UTF8.GetString(getByteProduct);

            Product productByteNesnesi = JsonConvert.DeserializeObject<Product>(jsonByteProduct);




            return View();
        }

        public async Task<IActionResult> Index4()// Resim,pdf gibi dosyaları cacheleme
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);//In memory ile aynı verilen süreden sonra silinecek
            options.SlidingExpiration = TimeSpan.FromSeconds(10);//In memory ile aynı verilen sürede erişilirse verilen süre kadar süresi artacak        

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/kurye.png");

            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            await _distributedCache.SetAsync("resim", imageByte,options);//binarye çevirerek kayıt işlemlerini gerçekleştirebiliriz

            return View();
        }


    }
}
