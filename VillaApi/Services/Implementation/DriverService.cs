using Microsoft.EntityFrameworkCore;
using System;
using VillaApi.Data;
using VillaApi.Models;
using VillaApi.Services.Interface;

namespace VillaApi.Services.Implementation
{
    public class DriverService : IDriverService
    {
        private readonly ApplicationDbContext _dbContext;
        public DriverService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Driver>> GetDrivers()
        {
           
            return await _dbContext.Drivers.ToListAsync();
        }

        public async  Task<Driver?> GetDriverById(int id)
        {
            return await _dbContext.Drivers.FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<Driver> AddDriver(Driver Driver)
        {
            var result = _dbContext.Drivers.Add(Driver);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<Driver> UpdateDriver(Driver Driver)
        {
            var result = _dbContext.Drivers.Update(Driver);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteDriver(int Id)
        {
            var filteredData = _dbContext.Drivers.FirstOrDefault(x => x.Id == Id);
            var result = _dbContext.Remove(filteredData);
            await _dbContext.SaveChangesAsync();
            return result != null ? true : false;
        }



        
    }
}
