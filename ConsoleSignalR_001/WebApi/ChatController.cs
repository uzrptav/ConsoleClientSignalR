using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleSignalR_001.WebApi
{
    class ChatController
    {
        private static string Url
        {
            get
            {   //TODO:перенести в конфиг
                return "http://intensfitapi.azurewebsites.net/api/Chat/";
                //return "http://localhost:61889/api/Chat/";
            }
        }

        /// <summary>
        /// Retrieves messages corresponding to appropriate group 
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <returns>List of messages corresponding to appropriate group</returns>
        internal List<Message> GetGroupMessages(int groupId)
        {
            return Utils.GetAsync<List<Message>>(Url, "GetGroupMessages?groupId=" + groupId).Result;
        }

        /// <summary>
        /// Retrieves list of user groups
        /// </summary>
        /// <param name="login">Login (phone number)</param>
        /// <returns>List of groups corresponding to appropriate user</returns>
        internal List<Group> GetUserGroups(string login)
        {
            return Utils.GetAsync<List<Group>>(Url, "GetUserGroups?login=" + login).Result;
        }

        /// <summary>
        /// Retrieves user entity by id or login (phone number)
        /// </summary>
        /// <param name="userId">User id (optional)</param>
        /// <param name="login">Login (phone number) (optional)</param>
        /// <returns>User entity (object)</returns>
        internal User GetUserEntity(int userId = 0, string login = "")
        {
            return Utils.GetAsync<User>(Url, "GetUserEntity?" + (userId > 0 ? "userId=" + userId : "login=" + login)).Result;
        }

        /// <summary>
        /// Retrieves list of group users
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>List of users corresponding to appropriate group</returns>
        internal List<GroupUser> GetGroupUsers(int groupId)
        {
            return Utils.GetAsync<List<GroupUser>>(Url, "GetGroupUsers?groupId=" + groupId).Result;
        }

        /// <summary>
        /// Retrieves content data in byte array
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>Byte array of content data</returns>
        internal byte[] GetContentDataBytes(Guid messageId)
        {
            return Utils.GetAsync<byte[]>(Url, "GetContentDataBytes?messageId=" + messageId).Result;
        }

        /// <summary>
        /// Add new group
        /// </summary>
        /// <param name="group"></param>
        /// <returns>Group id of created group</returns>
        internal int AddGroup(Group group)
        {
            return Utils.PostAsync<int, Group>(Url, "AddGroup", group).Result;
        }

        /// <summary>
        /// Add new group user
        /// </summary>
        /// <param name="group"></param>
        /// <returns>Group user id of created link</returns>
        internal int AddGroupUser(GroupUser groupUser)
        {
            return Utils.PostAsync<int, GroupUser>(Url, "AddGroupUser", groupUser).Result;
        }
    }
}
