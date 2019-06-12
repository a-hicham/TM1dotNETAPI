using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM1dotNETAPI;

namespace Model
{
    public class Cube : ServerObject
    {
        public class NoSuchCubeException : Exception { }

        public Int32 handle {private set; get; }
        public string name { private set; get; }
        private Pool pool;
        public Server server {private set; get; }
        public List<Dimension> dimensions {private set; get; }

        public Cube(Pool pool, Server server, Int32 handle)
        {
            this.pool = pool;
            this.server = server;
            this.handle = handle;
            int nameProp = TM1API.TM1ObjectPropertyGet(pool.handle, handle, TM1API.TM1ObjectName());  // WORKS!!!!!!
            this.name = TM1API.intPtrToString(server.user.handle, nameProp);

            dimensions = new List<Dimension>();
            setDimensions();
        }

        /// <summary>
        /// Calculates the number of Dimensions in the cube
        /// </summary>
        /// <returns>Number of dimensions in this cube</returns>
        public int getNumberOfDimensions()
        {
            int viDimCount = TM1API.TM1ObjectListCountGet(this.pool.handle, this.handle, TM1API.TM1CubeDimensions());
            int number = TM1API.TM1ValIndexGet(server.user.handle, viDimCount);
            return number;
        }

        /// <summary>
        /// Iterates over all dimensions in this cube
        /// and stores them in the dimensions List
        /// </summary>
        public void setDimensions()
        {
            int nOfDims = getNumberOfDimensions();
            for (int i = 1; i <= nOfDims; i++)
            {
                int hDim = TM1API.TM1ObjectListHandleByIndexGet(this.pool.handle, this.handle, TM1API.TM1CubeDimensions(), TM1API.TM1ValIndex(this.pool.handle, i));

                //if (TM1API.IsError(this.server.user.handle, hDim))
                //    throw new Exception();

                //int nameProp = TM1API.TM1ObjectPropertyGet(pool.handle, hDim, TM1API.TM1ObjectName());  // WORKS!!!!!!
                //string dimName = TM1API.intPtrToString(server.user.handle, nameProp);

                dimensions.Add(new Dimension(pool, server, hDim));
            }
        }

        /// <summary>
        /// Overrides the default ToString method and returns this cube's name
        /// </summary>
        /// <returns>The name of this cube</returns>
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("------------------------------");
            s.AppendLine(this.name);

            for (int j = 0; j < dimensions.Count; j++)
                s.AppendLine(String.Format("\t{0}", dimensions[j].ToString()));
            
            s.AppendLine("------------------------------");

            return s.ToString();
        }
    }
}
