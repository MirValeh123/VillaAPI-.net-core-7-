using VillaApi.Models;

namespace VillaApi.Services.Interface
{
    public interface IDriverService
    {
        public Task<IEnumerable<Driver>> GetDrivers();
        public Task<Driver?> GetDriverById(int id);
        public Task<Driver> AddDriver(Driver Driver);
        public Task<Driver> UpdateDriver(Driver Driver);
        public Task<bool> DeleteDriver(int Id);
    }
}
