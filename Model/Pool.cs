using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM1dotNETAPI;

namespace Model
{
    public class Pool
    {
        public Int32 handle { private set; get; }
        public User hUser { private set; get; }

        public Pool(User hUser)
        {
            this.hUser = hUser;
            this.handle = TM1API.TM1ValPoolCreate(hUser.handle);
        }

    }
}
