using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM1dotNETAPI;

namespace Model
{
    public class Dimension : ServerObject
    {
        public class NoSuchElementException : Exception { }

        public Int32 handle { private set; get; }
        public string name { private set; get; }
        private Pool pool;
        private Server server;
        public List<Element> elements { private set; get; }

        public Dimension(Pool pool, Server server, Int32 handle)
        {
            this.pool = pool;
            this.server = server;
            this.handle = handle;
            int nameProp = TM1API.TM1ObjectPropertyGet(pool.handle, handle, TM1API.TM1ObjectName());  // WORKS!!!!!!
            this.name = TM1API.intPtrToString(server.user.handle, nameProp);

            elements = new List<Element>();
            setElements(); 
        }

        public int getNumberOfElements()
        {
            int hNOfElements = TM1API.TM1ObjectListCountGet(pool.handle, this.handle, TM1API.TM1DimensionElements());
            return TM1API.TM1ValIndexGet(server.user.handle, hNOfElements);
        }

        public void setElements()
        {
            int nOfElements = getNumberOfElements();

            for (int i = 1; i <= nOfElements; i++)
            {
                int hElement = TM1API.TM1ObjectListHandleByIndexGet(this.pool.handle, this.handle, TM1API.TM1DimensionElements(), TM1API.TM1ValIndex(this.pool.handle, i));

                if (TM1API.IsError(this.server.user.handle, hElement))
                    throw new NoSuchElementException();

                int nameProp = TM1API.TM1ObjectPropertyGet(pool.handle, hElement, TM1API.TM1ObjectName());  // WORKS!!!!!!
                string elementName = TM1API.intPtrToString(server.user.handle, nameProp);

                Element element = new Element(this.pool, this.server, hElement);
                elements.Add(element);
            }
        }


        public string showElements()
        {
            {
                StringBuilder s = new StringBuilder();

                for (int j = 0; j < elements.Count; j++)
                    s.AppendLine(String.Format("\t\t{0}", elements[j].ToString()));

                return s.ToString();
            }
        }

        /// <summary>
        /// Overrides the default ToString method and returns this dimension's name
        /// </summary>
        /// <returns>The name of this dimension</returns>
        public override string ToString()
        {
            return this.name;
        }
    }
}
