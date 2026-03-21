using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Ecommerce_DataAccess.clsCartData;
using static Ecommerce_DataAccess.FavoriteData;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ecommerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        [HttpGet("GetTatalByuserID/{Users_ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TotaltDTO> GetTatalByuserID(int Users_ID)

        {
            if ( Users_ID < 1)
            {
                return BadRequest($"Not accepted ID  {Users_ID}");
            }

            TotaltDTO GetTatalByuserID = Ecommerce_Business.Cart.GetTatalByuserID( Users_ID);

            //if (count == 0)
            //{
            //    return NotFound("No Found!");
            //}
            var response = new
            {
                Totaprice = GetTatalByuserID
            };



            return Ok(response); // هنا الاسم المستعار
        }

        [HttpGet("{items_ID}/{Users_ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetCountItem(int items_ID, int Users_ID)
        { 
            if (items_ID < 1 || Users_ID < 1)
            {
                return BadRequest($"Not accepted ID {items_ID} or {Users_ID}");
            }

            int count = Ecommerce_Business.Cart.Getcountitems(items_ID, Users_ID);

            //if (count == 0)
            //{
            //    return NotFound("No Found!");
            //}

            return Ok(new { countItem = count }); // هنا الاسم المستعار
        }
 



        [HttpGet("All/{user_ID}")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        public ActionResult<IEnumerable<CartDTOview>> GetAllICart(int user_ID) // Define a method to get all students.
        {
            //if (StudentDataSimulation.StudentsList.Count == 0) 
            //{
            //    return NotFound("No Students Found!");
            //}
            //return Ok(StudentDataSimulation.StudentsList); // Returns the list of students.

            List<CartDTOview> cartlist = Ecommerce_Business.Cart.GetAllcartinfo(user_ID);

            if (cartlist.Count == 0)

            {
                return NotFound("No  Found!");
            }
            var response = new
            {
                data = cartlist
            };
            return Ok(cartlist); // Returns the list of students.

        }


        [HttpGet("Items_ID/{id}", Name = "GetCartInfoByItemiD")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<CartWithItemInfoDTO>> GetCartInfoByCardiD(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            List<CartWithItemInfoDTO> Data = Ecommerce_Business.Cart.GetinfoByCardID(id);

            if (Data == null)
            {
                return NotFound($"User with ID {id} not found.");
            }



            // نرجع قائمة فيها عنصر واحد
            return Ok(Data);
        }




        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]


        public ActionResult<CarBasictDTO> AddItems([FromBody] CarBasictDTO FDTO)
        {
            if (FDTO == null)
            {
                return BadRequest("Invalid Items data.");
            }
            if (FDTO.Card_ItemsID < 1 || FDTO.card_UserID < 1)
            {
                return BadRequest($"Not accepted ID {FDTO.Card_ItemsID} or {FDTO.card_UserID}");
            }
            // Create a new clsItems object with the provided DTO
            Ecommerce_Business.Cart newcart = new Ecommerce_Business.Cart(new CarBasictDTO(FDTO.Card_ID, FDTO.Card_ItemsID,FDTO.card_UserID));
            // Here you would typically call a method to save the new Items to the database
            // For example: clsItemsData.AddNewItems(newItems);
            // Return the created DTO
            newcart.Save();
            FDTO.Card_ID = newcart.CartID;


            return CreatedAtAction(nameof(GetCartInfoByCardiD), new { id = FDTO.Card_ID }, FDTO);
        }

       

        [HttpDelete("{items_ID}/{Users_ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Deletcart(int items_ID, int Users_ID)
        {
            if (items_ID < 1 || Users_ID < 1)
            {
                return BadRequest($"Not accepted ID {items_ID}or {Users_ID}");
            }

            // var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            // StudentDataSimulation.StudentsList.Remove(student);

            if (Ecommerce_Business.Cart.Delete(items_ID, Users_ID))

                return Ok($"user  with ID {items_ID} or {Users_ID} has been deleted.");
            else
                return NotFound($"user with ID {items_ID} or {Users_ID} not found. no rows deleted!");
        }









    }
}
