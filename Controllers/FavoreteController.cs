using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Ecommerce_DataAccess.clsCategorlesData;
using static Ecommerce_DataAccess.clsItemsData;
using static Ecommerce_DataAccess.FavoriteData;

namespace ecommerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoreteController : ControllerBase
    {  



        //[HttpGet("All")] // Marks this method to respond to HTTP GET requests.
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]


        //public ActionResult<IEnumerable<FavoriteDTO>> GetAllIFavorite() // Define a method to get all students.
        //{
        //    //if (StudentDataSimulation.StudentsList.Count == 0) 
        //    //{
        //    //    return NotFound("No Students Found!");
        //    //}
        //    //return Ok(StudentDataSimulation.StudentsList); // Returns the list of students.

        //    List<FavoriteDTO> Favoritelist = Ecommerce_Business.Favorite.GetAllFavorites();
        //    if (Favoritelist.Count == 0)
        //    {
        //        return NotFound("No  Found!");
        //    }
        //    return Ok(Favoritelist); // Returns the list of students.

        //}


[HttpGet("userid/{id}", Name = "GetFavoriteInfoByUserID")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public ActionResult<List<ItemsFavoriteDTO>> GetFavoriteInfoByUserID(int id)
{
    if (id < 1)
    {
        return BadRequest($"Not accepted ID {id}");
    }

       List<ItemsFavoriteDTO> Data  =Ecommerce_Business.Favorite.GetFavoritesByUserID(id);

    if (Data == null)
    {
        return NotFound($"User with ID {id} not found.");
    }



    // نرجع قائمة فيها عنصر واحد
    return Ok(Data );
}




        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]


        public ActionResult<FavoriteDTO> AddItems([FromBody] FavoriteDTO FDTO)
        {
            if (FDTO == null)
            {
                return BadRequest("Invalid Items data.");
            }
            if (FDTO.Favorite_ItemsID < 1 || FDTO.Favorite_UserID < 1)
            {
                return BadRequest($"Not accepted ID {FDTO.Favorite_ItemsID} or {FDTO.Favorite_UserID}");
            }
            // Create a new clsItems object with the provided DTO
            Ecommerce_Business.Favorite newFavorite = new Ecommerce_Business.Favorite(new FavoriteDTO(FDTO.Favorite_ID,FDTO.Favorite_ItemsID,FDTO.Favorite_UserID));
            // Here you would typically call a method to save the new Items to the database
            // For example: clsItemsData.AddNewItems(newItems);
            // Return the created DTO
            newFavorite.Save();
            FDTO.Favorite_ID = newFavorite.favoriteID;


            return CreatedAtAction(nameof(GetfavoriteInfoByID), new { id = FDTO.Favorite_ID }, FDTO);
        }

        // "Deletfavorites"
        [HttpDelete("delete/item/{items_ID}/user/{Users_ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
 
        public ActionResult Deletfavorite(int items_ID, int Users_ID)
        {
            if (items_ID < 1|| Users_ID<1)
            {
                return BadRequest($"Not accepted ID {items_ID}or {Users_ID}");
            }

            // var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            // StudentDataSimulation.StudentsList.Remove(student);

            if (Ecommerce_Business.Favorite.Delete(items_ID, Users_ID))

                return Ok($"user  with ID {items_ID} or {Users_ID} has been deleted.");
            else
                return NotFound($"user with ID {items_ID} or {Users_ID} not found. no rows deleted!");
        }




        [HttpGet("{id}", Name = "GetfavoriteInfoByID")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<FavoriteDTO> GetfavoriteInfoByID(int id)
        {

            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            //var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            //if (student == null)
            //{
            //    return NotFound($"Student with ID {id} not found.");
            //}
            Ecommerce_Business.Favorite clsfavorite = Ecommerce_Business.Favorite.FindByID(id);

            if (clsfavorite == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            //here we get only the DTO object to send it back.
            FavoriteDTO UDTO = clsfavorite.FDTO;

            //we return the DTO not the student object.
            return Ok(UDTO);

        }






        [HttpDelete("delete/favoriteid/{Favorite_ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeletfavoriteByFavoriteID(int Favorite_ID)
        {
            if (Favorite_ID < 1 || Favorite_ID < 1)
            {
                return BadRequest($"Not accepted ID {Favorite_ID} ");
            }

            // var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            // StudentDataSimulation.StudentsList.Remove(student);

            if (Ecommerce_Business.Favorite.DeleteByFavoriteID(Favorite_ID))

                return Ok($"user  with ID {Favorite_ID}  has been deleted.");
            else
                return NotFound($"user with ID {Favorite_ID} not found. no rows deleted!");
        }
    }
}

