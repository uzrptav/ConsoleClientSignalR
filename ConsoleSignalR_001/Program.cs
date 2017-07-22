using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using ConsoleSignalR_001.WebApi;
using Microsoft.AspNet.SignalR.Client;

namespace ConsoleSignalR_001
{
    class Program
    {
        

        static void Main(string[] args)
        {

            #region

            AdminController adminController = new AdminController();
            List<Client> clients = adminController.GetClients();

            return;
            #endregion

            #region for ChatController

            ChatController chatController = new ChatController();

            //ищем юзера с ИД = 3 (Богдан)
            User user1 = chatController.GetUserEntity(userId: 3);
            Console.WriteLine("user1.firstName = " + user1.firstName);

            //ищем юзера с логином = 380632762112 (Виталыч)
            User user2 = chatController.GetUserEntity(login: "380632762112");
            Console.WriteLine("user2.firstName = " + user2.firstName);

            Console.WriteLine(Environment.NewLine + "--------------------" + Environment.NewLine);

            //ищем группы в которые входит юзер с логином 380501046052 (Серж)
            List<Group> groups = chatController.GetUserGroups(login: "380501046052");
            groups.ForEach(t => Console.WriteLine("groups = " + t.id+ " - " + t.name));

            Console.WriteLine(Environment.NewLine + "--------------------" + Environment.NewLine);

            List<GroupUser> groupUsers = chatController.GetGroupUsers(groupId: 4);
            groupUsers.ForEach(u => Console.WriteLine("groupUsers.User.firstName = " + u.User.firstName));

            Console.WriteLine(Environment.NewLine + "--------------------" + Environment.NewLine);

            //ищем месседжи группы с ИД 4 (Серж - Игорь)
            List<Message> messages = chatController.GetGroupMessages(groupId: 4);
            messages.ForEach(delegate(Message message)
            {
                //а тут достаем юзера для отображения ленты сообщений
                GroupUser groupUser = groupUsers.FirstOrDefault( gu => gu.id == message.groupUserId);
                //ну и сама лента месседжей
                Console.WriteLine("{0}->{1}->{2}", message.createdOn, groupUser.User.firstName, message.text);

            });

            Console.WriteLine(Environment.NewLine + "--------------------" + Environment.NewLine);

            //достаем полноценный контент конкретного сообщения (файл, картинку или длинный текст) - выполнять только когда пользователь кликает на вложение
            byte[] contentDataBytes = chatController.GetContentDataBytes(messageId: new Guid("C397138F-297A-4488-A1F1-01E8A4026038"));
            Console.WriteLine("contentDataBytes.Length = " + contentDataBytes.Length);

            Console.WriteLine(Environment.NewLine + "--------------------" + Environment.NewLine);

            //добавление новой группы
            Group group = new Group() {name = "New Group", createdOn = DateTime.UtcNow, createdById = 1};
            group.id = chatController.AddGroup(group);
            Console.WriteLine("Новая группа {0} с ИД {1} создана", group.name, group.id);

            Console.WriteLine(Environment.NewLine + "--------------------" + Environment.NewLine);

            //добавление нового юзера в группу
            GroupUser grUser = new GroupUser() {addedById = 1, addedOn = DateTime.UtcNow, groupId = 7, userId = 1};
            grUser.id = chatController.AddGroupUser(grUser);

            Console.ReadKey();

            #endregion;


            return;

            #region just for console client

            int groupId = 4;

            Console.WriteLine("Choose user (just type number):");
            Console.WriteLine("1 - Серж");
            Console.WriteLine("2 - Игорь");
            Console.WriteLine("3 - Богдан");
            Console.WriteLine("4 - Виталий");
            Console.WriteLine("5 - Анатолий");
            int userId = Convert.ToInt32(Console.ReadLine());
            string userLogin = string.Empty;
            string userName = string.Empty;
            switch (userId)
            {
                case 1 : userLogin = "380501046052";
                    userName = "Серж Паламарчук";
                    break;
                case 2 : userLogin = "380938025747";
                    userName = "Игорь Шведин";
                    break;
                case 3 : userLogin = "380937069902";
                    userName = "Богдан Чирка";
                    break;
                case 4 : userLogin = "380632762112";
                    userName = "Виталий Гутниченко";
                    break;
                case 5 : userLogin = "380974353881";
                    userName = "Анатолий Ткаченко";
                    break;
            }
            Console.WriteLine(Environment.NewLine + "Welcome to chat, {0} ({1})", userName, userLogin);
            Console.WriteLine(Environment.NewLine + "--------------------" + Environment.NewLine);
            Console.ReadKey();
            #endregion;
            string url = "http://intensfitapi.azurewebsites.net";
            //string url = "http://localhost:61889";


            var hubConnection = new HubConnection(url, new Dictionary<string, string>
            {
                { "UserLogin", userLogin }
            });

            //hubConnection.AddClientCertificate(X509Certificate.CreateFromCertFile("MyCert.cer"));
            //hubConnection.Headers.Add("HeaderKey", "HeaderValue");

            /** Создаем прокси для работы из сети банка **/
            //WebProxy proxy = new WebProxy("tmg.bank.lan", 8080)
            //{
            //    UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential("a.Tkachenko", "LBNdyu27$zxcv", "Bank")
            //};
            //hubConnection.Proxy = proxy;

            var chat = hubConnection.CreateHubProxy("ChatHub");
            chat.On<string, string>("addNewMessageToPage", (name, message) => { Console.Write(name + ": "); Console.WriteLine(message); });
            hubConnection.Start().Wait();
            chat.Invoke("Notify", "Console app", hubConnection.ConnectionId);
            string msg = null;
            

            while ((msg = Console.ReadLine()) != null)
            {
                //chat.Invoke("Send", "Console app", msg).Wait();
                //chat.Invoke("Remove", msgEntity).Wait();
                
                Message message = new Message();
                message.id = Guid.NewGuid();
                message.typeId = 1;
                message.text = msg;
                //message.participantId = 3;
                message.createdOn = DateTime.UtcNow;

                message.Content = new Content();
                message.Content.messageId = message.id;
                message.Content.typeId = 1;
                message.Content.data = Encoding.ASCII.GetBytes("content file or huge text: " + message.text);

                message.GroupUser = new GroupUser()
                {
                    User = new User() { cellphone = userLogin, firstName = userName },
                    groupId = groupId
                };
                chat.Invoke("AddMessage", message).Wait();
            }
        }
        
        public static string CallRestMethod(string url)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.Method = "GET";
            webrequest.ContentType = "application/x-www-form-urlencoded";
            webrequest.Headers.Add("Username", "xyz");
            webrequest.Headers.Add("Password", "abc");
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            string result = string.Empty;
            result = responseStream.ReadToEnd();
            webresponse.Close();
            return result;
        }

    }
}
