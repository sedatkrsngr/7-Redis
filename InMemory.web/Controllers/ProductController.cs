using InMemory.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemory.web.Controllers
{
    public class ProductController : Controller
    {

        private readonly IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            

            if (!_memoryCache.TryGetValue("zaman", out string zamanCache))//Cachete zaman key li veri var mı kontrol eder varsa zamanCachete göster
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);//Cache verilen süre kadar duracak sonra silinecek
                options.SlidingExpiration = TimeSpan.FromSeconds(10);//verilen süre içerisinde bu dataya erişilirse data cachete verilen süre kadar daha kalacak eğer erişilmezse silinecek

                //Sliding ve Absolute birlikte kullanılması verinin sürekli taze kalması açısından iyidir.Slidingle cachete kalma süresi  absolute yüzünden maks 1 dk olacak şekilde olur ve eğer veriye 10 saniye içinde erişilmezse veri yenilenir. Ve herseferinde erişilmesine rağmen 1 dk olunca veri yine yeniden eklenmek zorundadır. Bu sayede veri güncel kalıyor
                options.Priority = CacheItemPriority.Low;//Memory dolduğundan buradaki değere göre silme işlemi yapmalı NeverRemove hariç hepsini ihtiyaca göre silebilir.


                options.RegisterPostEvictionCallback((key,value,reason,state) =>//key silinirse neden silindiğini tutabiliyoruz.
                {
                    _memoryCache.Set("callback", $"key:{key}->value:{value}->sebep:{reason}");// Burada cachete tuttuk istersek veritabanına da ekleyebiliriz. bu method içerisinde
                });

                _memoryCache.Set<string>("zaman", DateTime.Now.ToString(),options);//Tarih bilgisini zaman key ile cache kaydettik.
                 //_memoryCache.Set("zaman", DateTime.Now);//Yukarıdaki ile farkı orada value değerimizin string olacağını belirtiyoruz.

            }


            //compleks verilerde cache örneği
            Product product = new Product { 
            Id=1,Name="Kalem",Price=200
            };
            _memoryCache.Set<Product>("product", product);//cachete oluşturma

            _memoryCache.Get<Product>("product");//cacheden cekme
            return View();
        }

        public IActionResult GetTime()
        {

            _memoryCache.TryGetValue("zaman", out string zamanCache);//zaman keyinde veri varsa bool 1 döner ve veriyi basar

            ViewBag.zaman = zamanCache;
            //ViewBag.zaman = _memoryCache.Get<string>("zaman");//cache'te zaman keyine ait value değerini getiririrz. <string> dememizin sebebi value değeri string olduğunu biliyoruz

            //_memoryCache.Remove("zaman");//cachete olan bilgiyi siler.

            //_memoryCache.GetOrCreate<string>("zaman", x =>//örneğin key ile veri çektik yoksa veriyi biz üretiriz.
            //{
            //    return DateTime.Now.ToString();
            //});

            return View();
        }
    }
}
