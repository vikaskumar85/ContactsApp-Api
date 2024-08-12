using System.Diagnostics.CodeAnalysis;

namespace ContactsApp.Core.Models
{
    // Contact item model
    [ExcludeFromCodeCoverage]
    public class ContactModel
    {
        /// <summary>
        /// Contact item identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
    }
}
