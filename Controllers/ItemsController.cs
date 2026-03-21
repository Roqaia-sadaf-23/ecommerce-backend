using Ecommerce_Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Ecommerce_DataAccess.clsItemsData;

namespace ecommerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {




        [HttpGet("AllItemsWithDescount")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        
        public ActionResult<IEnumerable<ItemsWithDescountDTO>> GetAllItems_Withdescount() // Define a method to get all students.
        {
            //if (StudentDataSimulation.StudentsList.Count == 0) 
            //{
            //    return NotFound("No Students Found!");
            //}
            //return Ok(StudentDataSimulation.StudentsList); // Returns the list of students.

            List<ItemsWithDescountDTO> ItemsList = Ecommerce_Business.clsItems.GetAllItemsWithDescount();
            if (ItemsList.Count == 0)
            {
                return NotFound("No user Found!");
            }
            return Ok(ItemsList); // Returns the list of students.

        }




        [HttpGet("FelterItemName/{name}", Name = "SearchItemsName")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        public ActionResult<IEnumerable<ItemViewDTO >> GetAllItems(string name) // Define a method to get all students.
        {
            //if (StudentDataSimulation.StudentsList.Count == 0) 
            //{
            //    return NotFound("No Students Found!");
            //}
            //return Ok(StudentDataSimulation.StudentsList); // Returns the list of students.

            List<ItemViewDTO> ItemsList = Ecommerce_Business.clsItems.GetAllItems(name);
            if (ItemsList.Count == 0)
            {
                return NotFound("No user Found!");
            }
            return Ok(ItemsList); // Returns the list of students.

        }


        //FindBycategoryID

        //[HttpGet("category/{id}", Name = "FindBycategoryID")]

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]

        //public ActionResult<IEnumerable<ItemsDTO>> FindBycategoryID(int id)
        //{

        //    if (id < 1)
        //    {
        //        return BadRequest($"Not accepted ID {id}");
        //    }

        //    //var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
        //    //if (student == null)
        //    //{
        //    //    return NotFound($"Student with ID {id} not found.");
        //    //}
        //    Ecommerce_Business.clsItems Items = Ecommerce_Business.clsItems.FindBycategoryID(id);

        //    if (Items == null)
        //    {
        //        return NotFound($"User with ID {id} not found.");
        //    }

        //    //here we get only the DTO object to send it back.
        //    ItemsDTO UDTO = Items.IDTO;

        //    //we return the DTO not the student object.
        //    return Ok(UDTO);

        //}
        //===================================HttpGet("category/{id}/{userid}")

        [HttpGet("category/{categoryid}/{userid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<ItemswithfavoriteDTO>> FindBycategoryID(int categoryid,int userid)
        {
            if (userid < 1)
            {
                return BadRequest($"Not accepted ID {userid}");
            }

            // جلب كل المنتجات حسب التصنيف
            List<ItemswithfavoriteDTO> items = Ecommerce_Business.clsItems.FindBycategoryID(categoryid, userid);

            if (items == null || items.Count == 0)
            {
                return NotFound($"No items found for category ID {userid}");
            }

            return Ok(items);
        }



        [HttpGet("Item/{id}", Name = "GetItemsById")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<ItemsDTO> GetItemsById(int id)
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
            Ecommerce_Business.clsItems Items = Ecommerce_Business.clsItems.FindByID(id);

            if (Items == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            //here we get only the DTO object to send it back.
            ItemsDTO UDTO = Items.IDTO;

            //we return the DTO not the student object.
            return Ok(UDTO);

        }



        [HttpPost("AddItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]


        public ActionResult<ItemsDTO> AddItems([FromBody] ItemsDTO CDTO)
        {
            if (CDTO == null)
            {
                return BadRequest("Invalid Items data.");
            }
            // Create a new clsItems object with the provided DTO
            Ecommerce_Business.clsItems newItems = new Ecommerce_Business.clsItems(new ItemsDTO(CDTO.Items_ID, CDTO.Items_Name, CDTO.Items_desc, CDTO.Items_count, CDTO.Items_Active, CDTO.Items_Price, CDTO.Items_Descount, CDTO.Items_ImagePath, CDTO.Items_date, CDTO.Categorles_ID));
            // Here you would typically call a method to save the new Items to the database
            // For example: clsItemsData.AddNewItems(newItems);
            // Return the created DTO
            newItems.Save();
            CDTO.Items_ID = newItems.Items_ID;


            return CreatedAtAction(nameof(GetItemsById), new { id = CDTO.Items_ID }, CDTO);
        }



        //here we use http put method for update
        [HttpPut("{id}", Name = "UpdateItems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ItemsDTO> UpdateItems(int id, [FromBody] ItemsDTO updatedC)
        {
            if (string.IsNullOrEmpty(updatedC.Items_Name) || string.IsNullOrEmpty(updatedC.Items_ImagePath))
            {
                return BadRequest("Invalid Items data.");
            }

            //var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);

            Ecommerce_Business.clsItems Items = Ecommerce_Business.clsItems.FindByID(id);

            if (Items == null)
            {
                return NotFound($"8ser with ID {id} not found.");
            }


            Items.Items_Name = updatedC.Items_Name;
            Items.Items_desc = updatedC.Items_desc;
            Items.Items_count = updatedC.Items_count;
            Items.Items_Active = updatedC.Items_Active;
            Items.Items_Price = updatedC.Items_Price;
            Items.Items_Descount = updatedC.Items_Descount;

            Items.Items_ImagePath = updatedC.Items_ImagePath;
            Items.Items_date = updatedC.Items_date;
            Items.Categorles_ID = updatedC.Categorles_ID;


            Items.Save();

            //we return the DTO not the full student object.
            return Ok(Items.IDTO);

        }


        //here we use HttpDelete method


        [HttpDelete("{id}", Name = "DeleteItems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteItems(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            // var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            // StudentDataSimulation.StudentsList.Remove(student);

            if (Ecommerce_Business.clsItems.Delete(id))

                return Ok($"user  with ID {id} has been deleted.");
            else
                return NotFound($"user with ID {id} not found. no rows deleted!");
        }












    }



}

