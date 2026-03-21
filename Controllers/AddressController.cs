using Ecommerce_DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Ecommerce_DataAccess.clsAddress;
using static Ecommerce_DataAccess.clsItemsData;
using static Ecommerce_DataAccess.clsUserData;

namespace ecommerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {


        [HttpGet("All", Name = "GetAllAddress")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        //here we used UserDTO
        public ActionResult<IEnumerable<AddressDTO>> GetAllAddress() // Define a method to get all students.
        {
            //if (StudentDataSimulation.StudentsList.Count == 0) 
            //{
            //    return NotFound("No Students Found!");
            //}
            //return Ok(StudentDataSimulation.StudentsList); // Returns the list of students.

            List<AddressDTO> List = Ecommerce_Business.Address.GetAll();
            if (List.Count == 0)
            {
                return NotFound("No Address Found!");
            }
            return Ok(List); // Returns the list of students.

        }



        [HttpGet("GetAddress/{id}", Name = "GetAddressById")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<AddressDTO> GetAddressById(int id)
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
            Ecommerce_Business.Address clsAddress = Ecommerce_Business.Address.FindByID(id);

            if (clsAddress == null)
            {
                return NotFound($"Address with ID {id} not found.");
            }

            //here we get only the DTO object to send it back.
            AddressDTO ADTO = clsAddress.ADTO;

            //we return the DTO not the student object.
            return Ok(ADTO);

        }


        [HttpPost("AddAddress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]


        public ActionResult<AddressDTO> Addaddress([FromBody] AddressDTO ADTO)
        {
            if (ADTO == null)
            {
                return BadRequest("Invalid Address data.");
            }
            // Create a new clsItems object with the provided DTO
            Ecommerce_Business.Address newAddress = new Ecommerce_Business.Address(new AddressDTO(ADTO.Addrees_ID, ADTO.Address_Userid, ADTO.Address_City, ADTO.Address_Street, ADTO.Address_Lat, ADTO.Address_Log));
            // Here you would typically call a method to save the new Items to the database
            // For example: clsItemsData.AddNewItems(newItems);
            // Return the created DTO
            newAddress.Save();
            ADTO.Addrees_ID = newAddress.Addrees_ID;


            return CreatedAtAction(nameof(GetAddressById), new { id = ADTO.Addrees_ID }, ADTO);
        }



        //here we use http put method for update
        [HttpPut("Update/{id}", Name = "UpdateAddress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<AddressDTO> Address(int id, [FromBody] AddressDTO ADTO)
        {
            if (ADTO.Addrees_ID <= 0 || ADTO.Address_Userid<=0)
            {
                return BadRequest("Invalid Address data.");
            }

            //var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);

            Ecommerce_Business.Address Address = Ecommerce_Business.Address.FindByID(id);

            if (Address == null)
            {
                return NotFound($"Address with ID {id} not found.");
            }



            Address.Save();

            //we return the DTO not the full student object.
            return Ok(Address.ADTO);

        }


        //here we use HttpDelete method


        [HttpDelete("Delete/{id}", Name = "DeleteAddress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteAddress(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            // var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            // StudentDataSimulation.StudentsList.Remove(student);

            if (Ecommerce_Business.Address.Delete(id))

                return Ok($"Address  with ID {id} has been deleted.");
            else
                return NotFound($"Address with ID {id} not found. no rows deleted!");
        }






    }
}
