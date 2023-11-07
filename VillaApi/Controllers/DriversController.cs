using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VillaApi.Exceptions;
using VillaApi.Models;
using VillaApi.Services.Interface;
using NotImplementedException = System.NotImplementedException;

namespace VillaApi.Controllers
{
    [Route("api/driver")]
    [ApiController]

    public class DriversController : ControllerBase
    {

        private readonly IDriverService _driverServices;
        public DriversController(
            IDriverService driverServices)
        {
            _driverServices = driverServices;
        }
        [HttpGet("driverlist")]
        public async Task<IEnumerable<Driver>> DriverList()
        {
            var driverList = await _driverServices.GetDrivers();
            throw new NotFoundException("data not found");
            return driverList;
        }
        [HttpGet("getdriverbyid")]
        public async Task<IActionResult> GetDriverById(int Id)
        {
           
            var driver = await _driverServices.GetDriverById(Id);
            if (driver == null)
            {
                //throw new Notfound($"Driver ID {Id} not found.");

                return NotFound();
            }
            return Ok(driver);
        }
        [HttpPost("adddriver")]
        public async Task<IActionResult> AddDriver(Driver driver)
        {
            var result = await _driverServices.AddDriver(driver);
            return Ok(result);
        }
        [HttpPut("updatedriver")]
        public async Task<IActionResult> UpdateDriver(Driver driver)
        {
            var result = await _driverServices.UpdateDriver(driver);
            return Ok(result);
        }
        [HttpDelete("deletedriver")]
        public async Task<bool> DeleteDriver(int Id)
        {
            return await _driverServices.DeleteDriver(Id);
        }

    }
}
