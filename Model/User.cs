using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class User
    {
        public Int32 handle { private set; get; }

        public User(Int32 handle)
        {
            this.handle = handle;
        }
    }
}
