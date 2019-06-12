using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM1dotNETAPI;

namespace Model
{
    public class Server
    {
        public class ServerConnectionException : Exception { }
        public class NoSuchDimensionException : Exception { }

        public Int32 handle {private set; get; }
        public Pool pool { private set; get; }
        public User user { private set; get; }
        public string name {private set;  get; }
        public string client { private set; get; }
        public List<Cube> cubes {private set; get; }
        public List<Dimension> dimensions { private set; get; }
        private Int32 vServerName;
        private Int32 vClientName;
        private Int32 vPassword;

        public Server(Pool pool, string name, string client, string password)
        {
            this.pool = pool;
            this.user = pool.hUser;
            this.name = name;
            this.client = client;
            this.vServerName = TM1API.TM1ValString(pool.handle, name.Trim(), 0);
            this.vClientName = TM1API.TM1ValString(pool.handle, client.Trim(), 0);
            this.vPassword = TM1API.TM1ValString(pool.handle, password.Trim(), 0);
            cubes = new List<Cube>();
            dimensions = new List<Dimension>();
        }

        /// <summary>
        /// Connects the server to the database and sets the handle
        /// </summary>
        public int Connect()
        {
            this.handle = TM1API.TM1SystemServerConnect(pool.handle, vServerName, vClientName, vPassword);

            if (TM1API.IsError(this.user.handle, this.handle))
                throw new ServerConnectionException();

            /// if necessary cubes and dimensions can be stored ///
            SetDimensions();
            SetCubes();//
            /// ---------------------------------------------- ///

            return this.handle;
        }

        /// <summary>
        /// Iterates over the cubes and their dimensions and returns their names
        /// </summary>
        /// <returns>Cubes and dimensions names</returns>
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            for (int i = 0; i < cubes.Count; i++)
                s.Append(cubes[i].ToString());
            
            s.AppendLine(String.Format("\nNumber of Cubes = {0}\nNumber of Dimensions = {1}", cubes.Count, dimensions.Count));
            return s.ToString();
        }


        // --------------------------- Cube Section --------------------------- //

        /// <summary>
        /// Calculates the number of cubes on this server
        /// </summary>
        /// <returns>Number of cubes on this server</returns>
        public int GetNumberOfCubes()
        {
            int viCubeCount = TM1API.TM1ObjectListCountGet(pool.handle, this.handle, TM1API.TM1ServerCubes());
            int number = TM1API.TM1ValIndexGet(user.handle, viCubeCount);
            return number;
        }

        /// <summary>
        /// Iterates over all cubes on this server
        /// and stores them in the cubes List
        /// </summary>
        public void SetCubes()
        {
            for (int i = 1; i <= GetNumberOfCubes(); i++)
            {
                int hCube = TM1API.TM1ObjectListHandleByIndexGet(this.pool.handle, this.handle, TM1API.TM1ServerCubes(), TM1API.TM1ValIndex(this.pool.handle, i));

                if (TM1API.IsError(user.handle, hCube))
                    throw new Exception();

                Cube cube = new Cube(this.pool, this, hCube);
                cubes.Add(cube);
            }
        }

        /// <summary>
        /// Searches the servers cubes list for the cube with the given name
        /// </summary>
        /// <param name="name">Cube name to search for</param>
        /// <returns>The first found cube with the given name</returns>
        public Cube GetCubeByName(string name)
        {
            Cube cube = this.cubes.Find(x => x.name.Equals(name));

            if (cube == null)
            {
                int hCube = TM1API.TM1ObjectListHandleByNameGet(this.pool.handle, this.handle, TM1API.TM1ServerCubes(), TM1API.TM1ValString(this.pool.handle, name, 100));

                if (TM1API.IsError(user.handle, hCube))
                    throw new Cube.NoSuchCubeException();

                cube = new Cube(this.pool, this, hCube);
            }

            return cube;
        }


        // --------------------------- Dimension Section --------------------------- //

        /// <summary>
        /// Calculates the number of dimensions on this server
        /// </summary>
        /// <returns>Number of dimensions on this server</returns>
        public int GetNumberOfDimensions()
        {
            int viDimCount = TM1API.TM1ObjectListCountGet(pool.handle, this.handle, TM1API.TM1ServerDimensions());
            int number = TM1API.TM1ValIndexGet(user.handle, viDimCount);
            return number;
        }

        /// <summary>
        /// Iterates over all dimensions in this server
        /// and stores them in the dimensions List
        /// </summary>
        public void SetDimensions()
        {
            for (int i = 1; i <= GetNumberOfDimensions(); i++)
            {
                int hDim = TM1API.TM1ObjectListHandleByIndexGet(this.pool.handle, this.handle, TM1API.TM1ServerDimensions(), TM1API.TM1ValIndex(this.pool.handle, i));
                
                if(TM1API.IsError(user.handle, hDim))
                    throw new Exception();
                
                Dimension dim = new Dimension(this.pool, this, hDim);
                dimensions.Add(dim);
            }
        }

        /// <summary>
        /// Searches the servers dimensions list for the dimension with the given name
        /// </summary>
        /// <param name="name">Dimension name to search for</param>
        /// <returns>The first found dimension with the given name</returns>
        public Dimension GetDimensionByName(string name)
        {
            Dimension dim = this.dimensions.Find(x => x.name.Equals(name));

            if (dim == null)
                throw new NoSuchDimensionException();

            return dim;
        }
    }
}
