using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VillaApi.Models;
using VillaApi.Data;
using Microsoft.AspNetCore.JsonPatch;

namespace VillaApi.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]

    
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger _logger;
        public VillaAPIController(ILogger<VillaAPIController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult<IEnumerable<Villa>> GetVillas()
        {
            _logger.LogInformation("Getting All Villas");
            return Ok(VillaStore.GetVillaList);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] 
        public ActionResult<Villa> GetVilla(int id)
        {

            if (id == 0)
            {
                _logger.LogError("Get Villa Error with Id" + "=" + id);
                return BadRequest();
            }
            var villa = VillaStore.GetVillaList.FirstOrDefault(x => x.Id == id);
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

            if (VillaStore.GetVillaList.FirstOrDefault(u => u.Name.ToLower() == villa.Name.ToLower()) != null)
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
            villa.Id = VillaStore.GetVillaList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            VillaStore.GetVillaList.Add(villa);
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
            var villa = VillaStore.GetVillaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            VillaStore.GetVillaList.Remove(villa);
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
            var villas = VillaStore.GetVillaList.FirstOrDefault(u => u.Id == id);
            if (villas == null)
            {
                return NotFound();
            }
            villas.Name = villa.Name;
            villas.Area = villa.Area;
            villas.Price = villa.Price;
            villas.CreatedDate = villa.CreatedDate;

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
            var villa = VillaStore.GetVillaList.FirstOrDefault(u=>u.Id == id);
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
