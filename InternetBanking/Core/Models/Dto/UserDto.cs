using System;

namespace Core.Models.Dto
{
    public class UserDto
    {

        /// <summary>
        /// Represents unique id by user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents name of user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents last name of user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Respresents email of user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Represents password of user
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Represents creation date of user
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Represents fullname user
        /// </summary>
        public string FullName { get; set; }
    }
}
