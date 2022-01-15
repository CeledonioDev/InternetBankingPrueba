using System;

namespace Core.Models.Entities
{
    /// <summary>
    /// Represents entity of User Account
    /// </summary>
    public sealed class UserAccount
    {
        /// <summary>
        /// Represents User Account's unique identification
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Represents User Id  unique identification
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Represents amount account of account
        /// </summary>
        public decimal AmountAccount { get; set; }

        /// <summary>
        /// Represents instance of User
        /// </summary>
        public User User { get; set; }


        /// <summary>
        /// Represents creation date of user account
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
