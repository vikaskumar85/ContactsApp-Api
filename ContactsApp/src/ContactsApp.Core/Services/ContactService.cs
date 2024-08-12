using ContactsApp.Core.Interfaces;
using ContactsApp.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ContactsApp.Core.Services
{
    /// <summary>
    /// Contacts Service class
    /// </summary>
    public class ContactService : IContactService
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        #region Ctor

        public ContactService(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Gets all contacts
        /// </summary>
        /// <returns></returns>
        public async Task<ResultModel> GetAll()
        {
            try
            {
                var items = await LoadData();

                var result = new ResultModel
                {
                    TotalNoOfContacts = items.Count(),
                    Contacts = items
                };

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Inserts contact
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Insert(ContactModel model)
        {
            try
            {
                var contacts = await LoadData();
                if (contacts is not null && contacts.Any())
                {
                    model.Id = contacts.LastOrDefault().Id + 1;
                    var contactsToSave  = contacts.Append(model);
                    return await SaveData(contactsToSave);
                }
                else
                {
                    // first insertion
                    model.Id = 1;
                    contacts = new List<ContactModel>() { model };
                    return await SaveData(contacts);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates contact
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Update(ContactModel model)
        {
            try
            {
                var contacts = await LoadData();
                if (contacts is not null && contacts.Any())
                {
                    var itemToUpdate = contacts.FirstOrDefault(c => c.Id == model.Id);
                    if (itemToUpdate != null)
                    {
                        itemToUpdate.FirstName = model.FirstName;
                        itemToUpdate.LastName = model.LastName;
                        itemToUpdate.Email = model.Email;

                        var updatedItems = contacts.Select(x => x.Id == itemToUpdate.Id ? itemToUpdate : x);
                        return await SaveData(updatedItems);
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes contact
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Delete(int contactId)
        {
            try
            {
                var contacts = await LoadData();
                if (contacts is not null && contacts.Any())
                {
                    var contactsToSave = contacts.Where(c => c.Id != contactId);
                    var flag = await SaveData(contactsToSave);
                    return flag;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Loads data from json file
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ContactModel>> LoadData()
        {
            try
            {
                IEnumerable<ContactModel> contactsList;
                var rootPath = _hostingEnvironment.ContentRootPath; //get the root path
                var fullPath = Path.Combine(rootPath, _configuration["DataFileName"]); //combine the root path with that of our json file

                using (StreamReader r = new StreamReader(fullPath))
                {
                    string json = r.ReadToEnd();
                    if (!string.IsNullOrWhiteSpace(json))
                        contactsList = JsonConvert.DeserializeObject<IEnumerable<ContactModel>>(json);
                    else
                        contactsList = new List<ContactModel>();
                }

                return contactsList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Saves data to json file
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveData(IEnumerable<ContactModel> contacts)
        {
            try
            {
                var rootPath = _hostingEnvironment.ContentRootPath; //get the root path
                var fullPath = Path.Combine(rootPath, _configuration["DataFileName"]); //combine the root path with that of our json file

                using (StreamWriter sw = new StreamWriter(fullPath))
                {
                    sw.Write(JsonConvert.SerializeObject(contacts));
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
