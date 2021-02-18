using System.Collections.Generic;
using MicroServiceModel;

namespace MicroServiceDAL
{
    public class UserServer : IUserServer
    {
        public static List<UserModel> InMemoryList = new List<UserModel>
        {
            new UserModel()
            {
                Id = 1,
                UserName = "Ken",
                Port = 00,
            },
            new UserModel()
            {
                Id = 2,
                UserName = "Ann",
                Port = 12,
            },new UserModel()
            {
                Id = 2,
                UserName = "Ann",
                Port = 12,
            },
        };


        public List<UserModel> GetList()
        {
            return
        }

        public UserModel Find(int id)
        {
            return
        }
    }
}