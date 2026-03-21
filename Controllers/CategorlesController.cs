using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Ecommerce_DataAccess.clsCategorlesData;
using static Ecommerce_DataAccess.clsUserData;

namespace ecommerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorlesController : ControllerBase
    {



        [HttpGet("AllCategorles")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        public ActionResult<IEnumerable<CategorleesDTO>> GetAllCategorlees() // Define a method to get all students.
        {
            //if (StudentDataSimulation.StudentsList.Count == 0) 
            //{
            //    return NotFound("No Students Found!");
            //}
            //return Ok(StudentDataSimulation.StudentsList); // Returns the list of students.

            List<CategorleesDTO> CategorleesList = Ecommerce_Business.clsCategorles.GetAllCategorles();
            if (CategorleesList.Count == 0)
            {
                return NotFound("No user Found!");
            }
            return Ok(CategorleesList); // Returns the list of students.

        }





        [HttpGet("{id}", Name = "GetCategorleesById")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<CategorleesDTO> GetCategorleesById(int id)
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
            Ecommerce_Business.clsCategorles Categorles = Ecommerce_Business.clsCategorles.FindByID(id);

            if (Categorles == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            //here we get only the DTO object to send it back.
            CategorleesDTO UDTO = Categorles.CDTO;

            //we return the DTO not the student object.
            return Ok(UDTO);

        }



        [HttpPost("Addcatorges")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]


        public ActionResult<CategorleesDTO> AddCategorlees([FromBody] CategorleesDTO CDTO)
        {
            if (CDTO == null)
            {
                return BadRequest("Invalid Categorlees data.");
            }
            // Create a new clsCategorles object with the provided DTO
            Ecommerce_Business.clsCategorles newCategorles =
                new Ecommerce_Business.clsCategorles(new CategorleesDTO(CDTO.Categorles_ID, CDTO.Categorles_name,
                CDTO.Categorles_ImagePath, CDTO.Categorles_date));
            // Here you would typically call a method to save the new categorles to the database
            // For example: clsCategorlesData.AddNewCategorlees(newCategorles);
            // Return the created DTO
            newCategorles.Save();
            CDTO.Categorles_ID = newCategorles.Categorles_ID;


            return CreatedAtAction(nameof(GetCategorleesById), new { id = CDTO.Categorles_ID }, CDTO);
        }



        //here we use http put method for update
        [HttpPut("{id}", Name = "UpdateCategorlees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CategorleesDTO> UpdateCategorlees(int id, [FromBody] CategorleesDTO updatedC)
        {
            if (string.IsNullOrEmpty(updatedC.Categorles_name))
            {
                return BadRequest("Invalid Categorlees data.");
            }

            //var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);

            Ecommerce_Business.clsCategorles Categorles = Ecommerce_Business.clsCategorles.FindByID(id);

            if (Categorles == null)
            {
                return NotFound($"8ser with ID {id} not found.");
            }


            Categorles.Categorles_name = updatedC.Categorles_name;
            Categorles.Categorles_ImagePath = updatedC.Categorles_ImagePath;


            Categorles.Save();

            //we return the DTO not the full student object.
            return Ok(Categorles.CDTO);

        }


        ////here we use HttpDelete method


        [HttpDelete("{id}", Name = "DeleteCategorles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteCategorles(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            // var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            // StudentDataSimulation.StudentsList.Remove(student);

            if (Ecommerce_Business.clsCategorles.Delete(id))

                return Ok($"user  with ID {id} has been deleted.");
            else
                return NotFound($"user with ID {id} not found. no rows deleted!");
        }


    }

}
