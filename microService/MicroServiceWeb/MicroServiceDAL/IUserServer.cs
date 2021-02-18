using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using MicroServiceModel;

namespace MicroServiceDAL
{
    public interface IUserServer
    {
        List<UserModel> GetList();

        UserModel Find(int id);
    }
}
