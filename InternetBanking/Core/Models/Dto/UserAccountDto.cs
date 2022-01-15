namespace Core.Models.Dto
{
    public sealed class UserAccountDto
    {
        /// <summary>
        /// Represents amount account of account
        /// </summary>
        public decimal AmountAccount { get; set; }

        /// <summary>
        /// Represents User Id  unique identification
        /// </summary>
        public string AccountNumber { get; set; }
    }
}
