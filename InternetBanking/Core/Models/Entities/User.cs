using System;

namespace Core.Models.Entities
{
    /// <summary>
    /// Represents entity of User
    /// </summary>
    public sealed class User
    {
        /// <summary>
        /// Represents user's unique identification
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents account number of user
        /// </summary>
        public string AccountNumber { get; set; }

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
        /// Represents instance of by User Account
        /// </summary>
        public UserAccount UserAccount { get; set; }
    }
}
