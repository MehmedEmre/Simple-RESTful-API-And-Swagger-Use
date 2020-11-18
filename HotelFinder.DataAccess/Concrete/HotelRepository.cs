using HotelFinder.DataAccess.Abstract;
using HotelFinder.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelFinder.DataAccess.Concrete
{
    public class HotelRepository : IHotelRepository
    {
        public async Task<Hotel> CreateHotel(Hotel hotel)
        {
            using (var hotelDbContext = new HotelDbContext())
            {
                hotelDbContext.HotelTable.Add(hotel);
                await hotelDbContext.SaveChangesAsync();
                return hotel;
            }
        }

        public async Task DeleteHotel(int id)
        {
            using (var hotelDbContext = new HotelDbContext())
            {
                var deletedHotel = await GetHotelById(id);
                hotelDbContext.HotelTable.Remove(deletedHotel);
                await hotelDbContext.SaveChangesAsync();

            }
        }

        public async Task<List<Hotel>> GetAllHotels()
        {
            using(var hotelDbContext = new HotelDbContext())
            {
                return await hotelDbContext.HotelTable.ToListAsync();
            }
        }

        public async Task<Hotel> GetHotelById(int id)
        {
            using(var hotelDbContext = new HotelDbContext())
            {
                return await hotelDbContext.HotelTable.FindAsync(id);
            }
        }

        public async Task<Hotel> GetHotelByName(string name)
        {
            using (var hotelDbContext = new HotelDbContext())
            {
                return await hotelDbContext.HotelTable.FirstOrDefaultAsync(x => x.Name == name);
            }
        }


        public async Task<Hotel> UpdateHotel(Hotel hotel)
        {
            using (var hotelDbContext = new HotelDbContext())
            {
                hotelDbContext.HotelTable.Update(hotel);
                await hotelDbContext.SaveChangesAsync();
                return hotel;
            }
        }
    }
}
