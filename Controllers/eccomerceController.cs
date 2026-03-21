using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecommerce_Business;
using static Ecommerce_DataAccess.clsUserData;
using Microsoft.IdentityModel.Tokens;

namespace ecommerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class eccomerceController : ControllerBase
    {



        [HttpGet("All", Name = "GetAllUser")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        //here we used UserDTO
        public ActionResult<IEnumerable<UserDTO>> GetAllUser() // Define a method to get all students.
        {
            //if (StudentDataSimulation.StudentsList.Count == 0) 
            //{
            //    return NotFound("No Students Found!");
            //}
            //return Ok(StudentDataSimulation.StudentsList); // Returns the list of students.

            List<UserDTO> UserList = Ecommerce_Business.clsUser.GetAllUsers();
            if (UserList.Count == 0)
            {
                return NotFound("No user Found!");
            }
            return Ok(UserList); // Returns the list of students.

        }



        [HttpGet("{id}", Name = "GetUserById")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<UserDTO> GetUserById(int id)
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
            Ecommerce_Business.clsUser clsUser = Ecommerce_Business.clsUser.FindByUserID(id);

            if (clsUser == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            //here we get only the DTO object to send it back.
            UserDTO UDTO = clsUser.UDTO;

            //we return the DTO not the student object.
            return Ok(UDTO);

        }
        //login 
        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("login", Name = "LoginUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<UserDTO> GetUserByemailandpassword([FromBody]LoginRequest loginRequest)
        {
            

            //var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            //if (student == null)
            //{
            //    return NotFound($"Student with ID {id} not found.");
            //}
            Ecommerce_Business.clsUser clsUser = Ecommerce_Business.clsUser.FindByEmailandPassword(loginRequest.Email, loginRequest.Password);

            if (clsUser == null)
            {
                return NotFound($"User with  not found.");
            }

            //here we get only the DTO object to send it back.
            UserDTO UDTO = clsUser.UDTO;

            //we return the DTO not the student object.
            return Ok(UDTO);

        }

       // for add new we use Http Post

        [HttpPost("Add", Name = "AddUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserDTO> AddUser([FromBody] UserDTO newUserDTO)
        {
            //we validate the data here
            if (newUserDTO == null || string.IsNullOrEmpty(newUserDTO.name) || string.IsNullOrEmpty(newUserDTO.email) || string.IsNullOrEmpty(newUserDTO.phone) || string.IsNullOrEmpty(newUserDTO.password))
            {
                return BadRequest("Invalid user data.");
            }

            //newStudent.Id = StudentDataSimulation.StudentsList.Count > 0 ? StudentDataSimulation.StudentsList.Max(s => s.Id) + 1 : 1;
            //if (Ecommerce_Business.clsUser.IsUserExistByEmail(newUserDTO.email,newUserDTO.password))
            //{
            //    return BadRequest($"User with email {newUserDTO.email} already exists.");
            //} 

            Ecommerce_Business.clsUser user = new Ecommerce_Business.clsUser(new UserDTO(newUserDTO.UserID, newUserDTO.name, newUserDTO.email, newUserDTO.phone, newUserDTO.verfiycode, newUserDTO.approve, newUserDTO.user_created, newUserDTO.password));
            user.Save();

            newUserDTO.UserID = user.UserID;

            //we return the DTO only not the full student object
            //we dont return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetUserById", new { id = newUserDTO.UserID }, newUserDTO);

        }


        //here we use http put method for update
        [HttpPut("{id}", Name = "Updateuser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> Updateuser(int id, [FromBody] UserDTO updatedUser)
        {
            if (id < 1 || updatedUser == null || string.IsNullOrEmpty(updatedUser.name) || string.IsNullOrEmpty(updatedUser.name) || string.IsNullOrEmpty(updatedUser.email) || string.IsNullOrEmpty(updatedUser.phone) || string.IsNullOrEmpty(updatedUser.password))
            {
                return BadRequest("Invalid user data.");
            }

            //var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);

            Ecommerce_Business.clsUser clsUser = Ecommerce_Business.clsUser.FindByUserID(id);

            if (clsUser == null)
            {
                return NotFound($"8ser with ID {id} not found.");
            }


            clsUser.name = updatedUser.name;
            clsUser.email = updatedUser.email;
            clsUser.phone = updatedUser.phone;
            clsUser.verfiycode = updatedUser.verfiycode;
            clsUser.approve = updatedUser.approve;
            clsUser.user_created = updatedUser.user_created;
            clsUser.Password = updatedUser.password;

            clsUser.Save();

            //we return the DTO not the full student object.
            return Ok(clsUser.UDTO);

        }


        //here we use HttpDelete method


        [HttpDelete("{id}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteUser(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            // var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            // StudentDataSimulation.StudentsList.Remove(student);

            if (Ecommerce_Business.clsUser.Delete(id))

                return Ok($"user  with ID {id} has been deleted.");
            else
                return NotFound($"user with ID {id} not found. no rows deleted!");
        }


        //[HttpPost("check-user-exists")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public ActionResult<LoginDTO> IsUserExist( string email)
        //{
        //    if (string.IsNullOrEmpty(email) )
        //    {
        //        return BadRequest("Email and Phone are required.");
        //    }
        //    Ecommerce_Business.clsUser login = Ecommerce_Business.clsUser.isUserExistByEmail(email);

        //    if (login == null)
        //    {
        //        return NotFound($"User with  not found.");
        //    }

        //    clsUser UDTO = login.UDTO;

        //    return Ok(new UDTO);
        //}




        //reset password


        //here we use http put method for update
        [HttpPut("ResetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<resetpasswordDTO> ResetPassword(  [FromBody] resetpasswordDTO updatedUser)
        {
            if (updatedUser == null ||  string.IsNullOrEmpty(updatedUser.Password))
            {
                return BadRequest("Invalid user data.");
            }

            //var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);

            Ecommerce_Business.clsUser clsUser = Ecommerce_Business.clsUser.FindByEmail(updatedUser);

          //  bool isexit = Ecommerce_Business.clsUser.IsEmailExit(Email);
            if (clsUser == null)
            {
                return NotFound($"user with Email not found.");
            }
             

           clsUser.Password = updatedUser.Password;

            clsUser.savenewpassword();

            //we return the DTO not the full student object.
            return Ok(clsUser.UDTO);

        }

    }
}
