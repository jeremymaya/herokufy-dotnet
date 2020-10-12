using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Herokufy.Models;
using Herokufy.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Herokufy.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryManager _inventory;

        public InventoryController(IInventoryManager inventory)
        {
            _inventory = inventory;
        }

        /// <summary>
        /// Gets a list of amenities
        /// </summary>
        /// <returns>A list of data transfer object containing amenity information</returns>
        [HttpGet, Route("GetAmenities")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetAmenities()
        {
            return await _inventory.GetProducts();
        }
    }
}
