using ContactsApp.Core.Models;

namespace ContactsApp.Core.Interfaces
{
    /// <summary>
    /// Contact Service Interface
    /// </summary>
    public interface IContactService
    {
        /// <summary>
        /// Gets all contacts
        /// </summary>
        /// <returns></returns>
        Task<ResultModel> GetAll();

        /// <summary>
        /// Inserts contact
        /// </summary>
        /// <returns></returns>
        Task<bool> Insert(ContactModel model);

        /// <summary>
        /// Updates contact
        /// </summary>
        /// <returns></returns>
        Task<bool> Update(ContactModel model);

        /// <summary>
        /// Deletes contact
        /// </summary>
        /// <returns></returns>
        Task<bool> Delete(int contactId);

        /// <summary>
        /// Loads data from json file
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ContactModel>> LoadData();

        /// <summary>
        /// Saves data to json file
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveData(IEnumerable<ContactModel> contacts);
    }
}
