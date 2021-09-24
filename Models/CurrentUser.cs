using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP_OOP.Models
{
    public static class CurrentUser
    {
        public static int Id { get; set; }
        public static string Name { get; set; }
        public static string TypeName { get; set; }
        public static string Login { get; set; }
        public static string Password { get; set; }
        public static decimal Balance { get; set; }
        public static Nullable<short> Level { get; set; }
        public static Nullable<int> GuildId { get; set; }

        public static void SetUser(USERS user)
        {
            Id = user.US_ID;
            Name = user.US_NAME;
            TypeName = user.US_TYPE_NAME;
            Login = user.US_LOGIN;
            Password = user.US_PASSWORD;
            Balance = user.US_BALANCE;
            Level = user.US_LEVEL;
            GuildId = user.US_GUILD_ID;
        }
    }
}
