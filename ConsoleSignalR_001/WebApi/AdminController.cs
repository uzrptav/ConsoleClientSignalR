using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSignalR_001.WebApi
{
    class AdminController
    {
        private static string Url
        {
            get
            {   //TODO:перенести в конфиг
                return "http://intensfitapi.azurewebsites.net/api/Admin/";
                //return "http://localhost:61889/api/Admin/";
            }
        }

        internal List<Client> GetClients()
        {
            return Utils.GetAsync<List<Client>>(Url, "GetClients").Result;
        }
    }
}
