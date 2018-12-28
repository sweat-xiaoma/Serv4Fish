using System;
namespace Serv4Fish3.Model
{
    public class User
    {
        public int id;
        public string username;
        public string password;

        public User(int id, string username, string password)
        {
            this.id = id;
            this.username = username;
            this.password = password;
        }
    }
}
