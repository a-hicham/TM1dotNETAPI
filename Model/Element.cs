using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TM1dotNETAPI;

namespace Model
{
    public class Element
    {
        public Int32 handle { private set; get; }
        public string name { private set; get; }
        private Pool pool;
        private Server server;
        public List<Model.Attribute> attributes { private set; get; }
        //public TM1Type type { private set; get; }

        public Element(Pool pool, Server server, Int32 handle)
        {
            this.pool = pool;
            this.server = server;
            this.handle = handle;
            //this.TYPECONSOLIDATED = TM1API.TM1TypeElementConsolidated();
            int nameProp = TM1API.TM1ObjectPropertyGet(this.pool.handle, this.handle, TM1API.TM1ObjectName());  // WORKS!!!!!!
            this.name = TM1API.intPtrToString(server.user.handle, nameProp);
            this.attributes = new List<Model.Attribute>();
            setAttributes();
        }

        public int getNumberOfAttributes()
        {
            int hNOfAttributes = TM1API.TM1ObjectListCountGet(pool.handle, this.handle, TM1API.TM1ElementComponents());
            return TM1API.TM1ValIndexGet(server.user.handle, hNOfAttributes);
        }

        private void setAttributes()
        {
            int hNof = getNumberOfAttributes();
            if  (this.name.Equals(""))  
                throw new NotImplementedException();
        }

        // FUNZT NICHT
        //private void setType()
        //{
        //    int hType = TM1API.TM1ObjectPropertyGet(this.pool.handle, this.handle, TM1API.TM1TypeElement());

        //    if (hType == TM1API.TM1TypeElementConsolidated() || hType == TM1API.TM1TypeElementSimple())
        //        this.type = TM1Type.TM1Real;
        //    else if (hType == TM1API.TM1TypeElementString())
        //        this.type = TM1Type.TM1String;
        //}

        public override string ToString()
        {
            return this.name;
        }
    }
}
