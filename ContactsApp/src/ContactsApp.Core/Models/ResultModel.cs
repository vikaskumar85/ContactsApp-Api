using System.Diagnostics.CodeAnalysis;

namespace ContactsApp.Core.Models
{
    /// <summary>
    /// Result model
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ResultModel
    {
        // Contacts list
        public IEnumerable<ContactModel> Contacts { get; set; }

        /// <summary>
        /// Total no of records found
        /// </summary>
        public int TotalNoOfContacts { get; set; }
    }
}
