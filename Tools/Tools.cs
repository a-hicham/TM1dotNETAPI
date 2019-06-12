using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM1dotNETAPI;
using Model;
using System.Xml.Linq;

namespace Tools
{
    /// <summary>
    /// The main methods to create pools, User handle, server connection etc
    /// are located in this class
    /// </summary>
    public class Tools
    {
        private static string adminHost;
        private static Tools instance; 
        public static User hUser { private set; get; }
        public static Server server { private set; get; }
        public static Cube docCube { private set; get; }
        public static DocumentationServer docServer { private set; get; }
        public static Pool mainPool { private set; get; }
        private static bool isConnected;
        public static bool IsConnected
        {
            get
            {
                return isConnected;
            }
            set
            {
                isConnected = value;
            }
        }
        //private string doc_cube_name = "}gmc2_server_documentation";

        /// <summary>
        /// Tools as a singleton
        /// </summary>
        private Tools()
        {
            TM1API.TM1DLLPath = Properties.Settings.Default.TM1APIPath; // change this for the dll Path
        }

        /// <summary>
        /// initializes the TM1API and sets the Tools object if not present
        /// by default the Admin Host is localhost
        /// </summary>
        /// <remarks>sets the hUser handle</remarks>
        /// <returns>an instance of Tools</returns>
        public static Tools Init(string host) 
        {
            if (instance == null || !adminHost.Equals(host))
            {
                if (isConnected)
                    Kill();

                instance = new Tools();
                adminHost = host;
                isConnected = false;
                TM1API.TM1APIInitialize();
                hUser = new User(TM1API.TM1SystemOpen());
                TM1API.TM1SystemAdminHostSet(hUser.handle, adminHost);
                mainPool = CreatePool();
            }
            
            return instance;
        }

        public static bool IsOK(Int32 handle)
        {
            return !TM1API.IsError(hUser.handle, handle);
        }

        public DocumentationServer createDocServer(string path, string xmlFile, int timestampPos, int tm1ServerPos)
        {
            DocumentationServer docServer = CreateDocServer(timestampPos,  tm1ServerPos);
            SaveServerToXML(docServer, path + xmlFile);
            return docServer;
        }

        public DocumentationServer CreateDocServer(int timestampPos, int tm1ServerPos)
        {
            // hier Parameter setzen
            docServer = new DocumentationServer(docCube, timestampPos, tm1ServerPos);

            return docServer;
        }

        public void SetDocCube(string docCubeName)
        {
            docCube = server.GetCubeByName(docCubeName);
            if (docCube == null)
                throw new Cube.NoSuchCubeException();
        }

        private void SaveServerToXML(DocumentationServer docServer, string path)
        {
            XElement elements = ServerToXML(docServer);
            //elements.Save(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\" + filename + ".xml");
            elements.Save(path + ".xml");
        }

        /// <summary>
        /// Creates the XML export File
        /// </summary>
        /// <param name="server">DocumentationServer to be exported</param>
        /// <returns></returns>
        private XElement ServerToXML(DocumentationServer server)
        {
            return new XElement("server", new XAttribute("name", server.name), 
                new XElement("cubes", server.cubes.Select(x => 
                    new XElement("cube", new XAttribute("name", x.name), 
                        new XElement("dimensions", x.dims.Select(y => 
                            new XElement("dimension", y.name))), 
                        new XElement("look_up_cubes", x.look_up_cubes.Select(l => 
                            new XElement("look_up_cube", l.name))), 
                        new XElement("feeder_cubes", x.fed_cubes.Select(f => 
                            new XElement("feeder_cube", f.name)))))));
        }

        /// <summary>
        /// best practice shut down: disco-->destroy-->close-->finalize
        /// </summary>
        public static void Kill()
        {
            if (instance != null)
            {
                TM1API.TM1SystemServerDisconnect(mainPool.handle, server.handle);
                TM1API.TM1ValPoolDestroy(mainPool.handle);
                TM1API.TM1SystemClose(hUser.handle);
                TM1API.TM1APIFinalize();

                //adminHost = null;
                //hUser = null;
                //server = null;
                //docCube = null;
                //docServer = null;
                //mainPool = null;
                //isConnected = false;
                instance = null;
            }
        }

        /// <summary>
        /// Sets the Admin Host Server for this hUser handle
        /// </summary>
        /// <param name="host">name of the Admin Host Server</param>
        public void SetAdminHost(string host)
        {
            TM1API.TM1SystemAdminHostSet(hUser.handle, host);
        }

        /// <summary>
        /// Creates a Handle Pool for this hUser handle
        /// </summary>
        /// <returns>new Pool</returns>
        public static Pool CreatePool() 
        {
            return new Pool(hUser);
        }

        /// <summary>
        /// This method creates a new server object and tries to connect
        /// </summary>
        /// <param name="pool">Connection Pool</param>
        /// <param name="name">Server Name</param>
        /// <param name="client">Login</param>
        /// <param name="password">Password</param>
        /// <returns>True if connected</returns>
        public bool ServerConnect(string name, string client, string password) 
        {
            server = new Server(mainPool, name, client, password);
            try
            {
                server.Connect();
                isConnected = IsOK(server.handle);
            }
            catch (Server.ServerConnectionException e)
            {
                System.Windows.Forms.MessageBox.Show("Connection failed");
            }
            return isConnected;
        }

        public void ServerSetDimensions()
        {
            if (isConnected)
                server.SetDimensions();
        }

        public void ServerSetCubes()
        {
            if (isConnected)
                server.SetCubes();
        }

        public string ServerToString()
        {
            return server.ToString();
        }

        public int GetNumberOfCubesByServer()
        {
            if (!isConnected)
                throw new NotConnectedException();

            return server.GetNumberOfCubes();
        }

        public int GetNumberOfDimensionsByServer()
        {
            if (!isConnected)
                throw new NotConnectedException();

            return server.GetNumberOfDimensions();
        }

        public Cube GetCubeByName(string cubeName)
        {
            if (!isConnected)
                throw new NotConnectedException();

            return server.GetCubeByName(cubeName);
        }

        public Dimension GetDimensionByName(string dimName)
        {
            if (!isConnected)
                throw new NotConnectedException();

            return server.GetDimensionByName(dimName);
        }

        public int GetNumberOfDimensionsByCube(Cube cube)
        {
            if (!isConnected)
                throw new NotConnectedException();

            return cube.getNumberOfDimensions();
        }

        public List<string> GetTimeStamps()
        {
            List<string> timestamps = new List<string>();

            //if (docCube == null)
            //    SetDocCube();

            docCube.dimensions[0].elements.ForEach(x => timestamps.Add(x.ToString()));

            return timestamps;
        }

        public List<string> GetTM1Servers()
        {
            List<string> servers = new List<string>();

            //if (docCube == null)
            //    SetDocCube();

            docCube.dimensions[1].elements.ForEach(x => servers.Add(x.ToString()));

            return servers;
        }

        public List<string> GetDimensions()
        {
            List<string> dimensions = new List<string>();

            server.dimensions.ForEach(x => dimensions.Add(x.ToString()));

            return dimensions;
        }

        public List<string> GetCubes()
        {
            List<string> cubes = new List<string>();

            server.cubes.ForEach(x => cubes.Add(x.name));

            return cubes;
        }
    }
}
