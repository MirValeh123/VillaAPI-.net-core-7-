using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VillaApi.Models;
using VillaApi.Data;
using Microsoft.AspNetCore.JsonPatch;
using VillaApi.Logging;

namespace VillaApi.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]

    
    public class VillaAPIController : ControllerBase
    {
        //private readonly ILogger _logger;
        //public VillaAPIController(ILogger<VillaAPIController> logger)
        //{
        //    _logger = logger;
        //}

        //public VillaAPIController(ApplicationDbContext context)
        //{
        //}
        private readonly ApplicationDbContext _context;

        private readonly ILogging _logger;
        public VillaAPIController(ILogging logger,ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult<IEnumerable<Villa>> GetVillas()
        {
            //_logger.LogInformation("Getting All Villas");
            _logger.Log("Getting All Villas", "INFo");
            
            return Ok(_context.Villas.ToList());
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] 
        public ActionResult<Villa> GetVilla(int id)
        {

            if (id == 0)
            {
                //_logger.LogError("Get Villa Error with Id" + "=" + id);
                _logger.Log("Get Villa Error with Id" + "=" + id,"error");
                return BadRequest();
            }
            var villa = _context.Villas.FirstOrDefault(x => x.Id == id);
            if (villa == null)
            {
                return NotFound();

            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Villa> AddVilla([FromBody] Villa villa)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);

            //}

            if (_context.Villas.FirstOrDefault(u => u.Name.ToLower() == villa.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa Already Exists!");
                return BadRequest(ModelState);
            }

            if (villa == null)
            {
                return BadRequest(villa);

            }
            if (villa.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Villa model = new Villa
            {
                Name = villa.Name,
                Price= villa.Price,
                Area= villa.Area
            };
            _context.Villas.Add(villa);
            _context.SaveChanges();
            return Ok(villa);

        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Villa> DeleteVilla(int id)
        {

            if (id == 0)
            {
                return BadRequest();
            }
            var villa = _context.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            _context.Villas.Remove(villa);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Villa> UpdateVilla(int id, [FromBody] Villa villa)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var existingVilla = _context.Villas.FirstOrDefault(u => u.Id == id);

            if (existingVilla == null)
            {
                return NotFound();
            }

            // Update existing villa with the data from the request
            existingVilla.Name = villa.Name;
            existingVilla.Price = villa.Price;
            existingVilla.Area = villa.Area;

            // Add a new villa with the data from the request
           

            // Add the new villa to the context

            // Save changes to the database
            _context.SaveChanges();

            return NoContent();
        }


        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult UpdateVirtualVilla(int id,JsonPatchDocument<Villa> patchDTO) 
        {
            if (patchDTO == null || id ==0)
            {
                
                return BadRequest();
            }
            var villa = _context.Villas.FirstOrDefault(u=>u.Id == id);
            if (villa == null)
            {
                return NotFound();

            }
            patchDTO.ApplyTo(villa,ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent(); 

        }


    }

}
