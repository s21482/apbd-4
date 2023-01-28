using AnimalsApi.Models;
using AnimalsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnimalsApi.Controllers
{
    [ApiController]
    [Route("api/animals")]
    public class AnimalsController : ControllerBase
    {
        private readonly IDatabaseService _dbService;

        public AnimalsController(IDatabaseService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetAnimals(string? orderBy = "name")
        {
            return Ok(_dbService.GetAnimals(orderBy));
        }


        [HttpPost]
        public IActionResult PostAnimal(Animal animal)
        {

            return Ok(_dbService.AddAnimal(animal));
        }

        [HttpPut("{indexNumber}")]
        public IActionResult ModifyAnimal(int indexNumber, Animal animal)
        {
            if (_dbService.ModifyAnimal(indexNumber, animal) == null)
            {
                return NotFound("Animal not found");
            }
            return Ok(_dbService.ModifyAnimal(indexNumber, animal));
        }


        [HttpDelete("{indexNumber}")]
        public IActionResult DeleteAnimal(int indexNumber)
        {
            if (!_dbService.DeleteAnimal(indexNumber))
            {
                return NotFound("Animal not found");
            }
            return (Ok("Successfully deleted animal"));
        }


    }
}
