using System;

namespace Core.Models.Entities
{
    /// <summary>
    /// Represents entity of Account Log
    /// </summary>
    public sealed class AccountLog
    {
        /// <summary>
        /// Represents Account's Log unique identification
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents Account performing the transaction
        /// </summary>
        public string AccountPrimary { get; set; }

        /// <summary>
        /// Represents Account that receives the transaction
        /// </summary>
        public string AccountSecundary { get; set; }

        /// <summary>
        /// Represents amount to be transferred
        /// </summary>
        public decimal AmountAccountPrimary { get; set; }

        /// <summary>
        /// Represents amount received in the transfer
        /// </summary>
        public decimal AmountAccountSecundary{ get; set; }

        /// <summary>
        /// Represents creation date of account log
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
