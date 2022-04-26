using assigntwo.Data;
using assigntwo.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace assigntwo.Controllers{

    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]

    public class MatchController : ControllerBase{

        private readonly IMatchRepository repository;
        private readonly LinkGenerator linkgenerator;

        public MatchController(IMatchRepository repository,LinkGenerator linkgenerator){

            this.repository = repository;
            this.linkgenerator = linkgenerator;
        }

        [HttpGet("keys/{key}")]

        public ActionResult<Match> Get(string key){

            //GET - "/keys/{key}" - Get key-value pair by key if found
            if (repository.Exist(key)){
                var match = repository.GetMatchbykey(key);
                return Ok(match);
            }
            // else Return 404 (Not Found) if the key does not exist
            else{
                return StatusCode(StatusCodes.Status404NotFound, "Specified key does not exist");
            }
        }

        [HttpPost("keys")]

        // { /api/[controller]/keys }
        public async Task<ActionResult<Match>> Post([FromBody] Match match){

            //Returning 409 (Conflict code) if the key already exists.
            if (repository.Exist(match.Key)){
                return StatusCode(StatusCodes.Status409Conflict, "Specified Key already exist");
            }

            //Adding Match element to the repository
            repository.Add(match);

            //Saving changes to the repository  
            if (await repository.SaveChanges()){
                var location = linkgenerator.GetPathByAction("Get", "Match", new { key = match.Key });
                return Created(location!, match);
            }
            else{
                return BadRequest("Unable to save changes");
            }
        }



        [HttpPatch("keys/{key}/{value}")]

        public async Task<IActionResult> Patch(string key, int value){

            //Checking the key element with specified key exists or not
            if (repository.Exist(key)){

                //Updating the value of the key
                repository.Update(key, value);

                //Saving the updated item
                if (await repository.SaveChanges()){
                    return Ok();
                }
            }
            //else returning 404 (Not Found) if the key does not exist.
            return StatusCode(StatusCodes.Status404NotFound, "Specified Key does not exist");
        }

        [HttpDelete("keys/{key}")]

        public async Task<IActionResult> Delete(string key){

            //Checking the key element with specified key exists or not
            if(repository.Exist(key)){   

                //Deleting the key-value pair by key
                repository.Delete(repository.GetMatchbykey(key));

                //Saving the updated item
                if (await repository.SaveChanges()){
                    return Ok();
                }

                //Else returning 404 (Not Found) if getting bad request.
                else{
                    return BadRequest($"Failed to delete {key} and save changes");
                }
            }
            //Else returning 404 (Not Found) if the key does not exist.
            else{
                return StatusCode(StatusCodes.Status404NotFound, " Key does not exist");
            }

        }
    }
}
