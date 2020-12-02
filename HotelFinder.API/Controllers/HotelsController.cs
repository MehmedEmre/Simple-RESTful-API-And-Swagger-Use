using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelFinder.Buisness.Abstract;
using HotelFinder.Buisness.Concrete;
using HotelFinder.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelFinder.API.Controllers
{
    [Authorize]// “Authorize” attribute’u ile işaretlenerek yetkisiz erişim engellenir bir hale getirilmiştir.
    [Route("api/[controller]")]
    [ApiController]//Eğer bu attribute kullanılırsa validation kontrolleri otomatik olarak yapılır.
                  //Örneğin post metoduna gelen verinin isim değeri required olsun ve o değeri null gönderelim.
                  //ModelState.Isvalid'e düşmeyiz çünkü [ApiController] bunu bizim için otomatik yapar
    public class HotelsController : ControllerBase
    {
        private IHotelService _hotelService;

       
        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        /// <summary>
        /// Get All Hotels
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllHotels()
        {
            List<Hotel> hotels = await _hotelService.GetAllHotels();

            return Ok(hotels);//Response olarak 200 döndür body kısmına hotels'i ekle
        }

        /// <summary>
        /// Get Hotel By Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]/{id}")]//api/hotels/GetHotelById/2
        //Action ismi ile metod ismi aynı olacaksa [action] kullanılabilir.
        public async Task<IActionResult> GetHotelById(int id)
        {
            Hotel hotel = await  _hotelService.GetHotelById(id);

            if (hotel != null)
            {
                return Ok(hotel);//200 + hotel data
            }
            return NotFound();

        }

        /// <summary>
        /// Get Hotel By Name
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]/{name}")]//api/hotels/GetHotelByName/name
        public async Task<IActionResult> GetHotelByName(string name)
        {

            Hotel hotel = await _hotelService.GetHotelByName(name);

            if (hotel != null)
            {
                return Ok(hotel);
            }
           
            return NotFound();//200 + hotel data
           
        }

        /*
        //http://localhost:51909/api/hotels/GetHotelByIdAndName/2/zorlu requestine cevap verir:

        [HttpGet]
        [Route("[action]/{id}/{name}")]//api/hotels/GetHotelByName/name
        public async Task<IActionResult> GetHotelByIdAndName(int id,string name)
        {
            Hotel hotel = await _hotelService.GetHotelByName(name);

            if(hotel.id == id)
            {
                if (hotel != null && id != 0)
                {
                    return Ok(hotel);
                }
            }

            return NotFound();//200 + hotel data

        }

        */

        //http://localhost:51909/api/hotels/GetHotelByIdAndName?id=2&name=zorlu requestine cevap verir:
        [HttpGet]
        [Route("[action]")]//api/hotels/GetHotelByName/name
        public async Task<IActionResult> GetHotelByIdAndName(int id, string name)
        {
            Hotel hotel = await _hotelService.GetHotelByName(name);

            if (hotel.id == id)
            {
                if (hotel != null && id != 0)
                {
                    return Ok(hotel);
                }
            }

            return NotFound();//200 + hotel data

        }


        /// <summary>
        /// Create Hotel
        /// </summary>
        /// <param name="hotel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]//api/hotels/CreateHotel
        public async Task<IActionResult> CreateHotel([FromBody]Hotel hotel)
        {

            if (ModelState.IsValid)//Buna gerek yok çünkü [ApiController] bunu otomatik yapıyor
            {
                Hotel newHotel = await _hotelService.CreateHotel(hotel);

                return CreatedAtAction("Get", new { id = newHotel.id }, newHotel);//CreatedAtAction kullandığımız için headerda location bilgisini görebiliriz. => http://localhost:51909/api/Hotels/7

                //201+data
            }

            return BadRequest();

        }

        /// <summary>
        /// Update Hotel
        /// </summary>
        /// <param name="hotel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]//api/hotels/UpdateHotel
        public async Task<IActionResult>  UpdateHotel([FromBody] Hotel hotel)
        {
            Hotel updateHotel = await _hotelService.GetHotelById(hotel.id);
            
            if(updateHotel != null)
            {
                return Ok(_hotelService.UpdateHotel(hotel));
            }

            return NotFound();
        }

        /// <summary>
        /// Delete Hotel
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Route("[action]/{id}")]//api/hotels/DeleteHotel
        public async Task<IActionResult> DeleteHotel(int id)
        {
            Hotel updateHotel = await _hotelService.GetHotelById(id);

            if (updateHotel != null)
            {
                await _hotelService.DeleteHotel(id);

                return Ok();//200 Ok
            }

            return NotFound();
        }
    }
}
