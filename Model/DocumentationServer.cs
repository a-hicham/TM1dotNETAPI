using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM1dotNETAPI;

namespace Model
{
    public class DocumentationServer
    {
        Cube docCube;
        string cube_name;
        string dim_name;
        int timestamp;
        int tm1server;
        int TM1OBJECT_CUBE = 0;
        int hDim_timestamp;
        int vDim_timestamp;
        int hDim_tm1server;
        int vDim_tm1server;
        int hDim_tm1object;
        int vDim_tm1object;
        int hDim_list;
        int vDim_list;
        int hDim_item_name;
        int vDim_item_name;
        int hDim_item_nof_dims;
        int vDim_item_nof_dims;
        int hDim_measure;
        int vDim_measure;
        int vArrayOfCells;
        int value_name;
        int value_nof_dims;
        int nof_dims;
        int value_nof_look;
        int nof_look;
        int value_nof_fed;
        int nof_fed;
        int hDim_item_nof_look;
        int vDim_item_nof_look;
        int hDim_item_list_of_look;
        int vDim_item_list_of_look;
        int hDim_item_nof_fed;
        int vDim_item_nof_fed;
        int hDim_item_list_of_feed;
        int vDim_item_list_of_feed;
        int hDim_item_dim_name;
        int vDim_item_dim_name;
        int value_dim_name;
        public string name { private set; get; }
        public List<DocCube> cubes { private set; get; }
        public ISet<DocDimension> dimensions { private set; get; }
        public IDictionary<string, List<string>> look_ups;
        public IDictionary<string, List<string>> feeder;

        /// <summary>
        /// Reads the given documentation cube on the server and creates a model for it
        /// </summary>
        /// <param name="docCube">The documentation cube, should be "}gmc2_server_documentation", change in the caller method/class if not</param>
        /// <param name="timestampPos">Position of the desired timestamp in the "}gmc2_server_documentation_timestamp" dimension, 0 = last timestamp</param>
        /// <param name="tm1server">Position of the desired tm1server in the "}tm1server" dimension, 0 = last tm1server</param>
        public DocumentationServer(Cube docCube, int timestampPos, int tm1server)
        {
            cubes = new List<DocCube>();
            dimensions = new HashSet<DocDimension>();
            look_ups = new Dictionary<string, List<string>>();
            feeder = new Dictionary<string, List<string>>();

            this.name = docCube.dimensions[1].elements[tm1server].ToString();
            this.docCube = docCube;
            this.timestamp = timestampPos;
            this.tm1server = tm1server;

            // timestamp Dim...
            hDim_timestamp = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, docCube.server.handle, TM1API.TM1ServerDimensions(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[0].ToString(), 100));
            vDim_timestamp = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, hDim_timestamp, TM1API.TM1DimensionElements(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[0].elements[timestamp].ToString(), 100));
            // ... and server Dim are given
            hDim_tm1server = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, docCube.server.handle, TM1API.TM1ServerDimensions(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[1].ToString(), 100));
            vDim_tm1server = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, hDim_tm1server, TM1API.TM1DimensionElements(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[1].elements[tm1server].ToString(), 100));

            SetCubes();
            SetLookFeed();
        }

        private void SetCubes() 
        {
            // Dimension object auf Cube gesetzt
            hDim_tm1object = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, docCube.server.handle, TM1API.TM1ServerDimensions(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[2].ToString(), 100));
            vDim_tm1object = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, hDim_tm1object, TM1API.TM1DimensionElements(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[2].elements[TM1OBJECT_CUBE].ToString(), 100));

            for (int i = 0; i < docCube.dimensions[3].elements.Count; i++)
            {
                hDim_item_name = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, docCube.server.handle, TM1API.TM1ServerDimensions(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[4].ToString(), 100));
                vDim_item_name = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, hDim_item_name, TM1API.TM1DimensionElements(), TM1API.TM1ValString(docCube.server.pool.handle, "cube_name", 100));

                hDim_item_nof_dims = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, docCube.server.handle, TM1API.TM1ServerDimensions(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[4].ToString(), 100));
                vDim_item_nof_dims = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, hDim_item_nof_dims, TM1API.TM1DimensionElements(), TM1API.TM1ValString(docCube.server.pool.handle, "cube_number_of_dimensions", 100));

                hDim_item_nof_look = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, docCube.server.handle, TM1API.TM1ServerDimensions(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[4].ToString(), 100));
                vDim_item_nof_look = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, hDim_item_nof_look, TM1API.TM1DimensionElements(), TM1API.TM1ValString(docCube.server.pool.handle, "cube_number_of_lookup_cubes", 100));

                hDim_item_list_of_look = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, docCube.server.handle, TM1API.TM1ServerDimensions(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[4].ToString(), 100));
                vDim_item_list_of_look = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, hDim_item_nof_look, TM1API.TM1DimensionElements(), TM1API.TM1ValString(docCube.server.pool.handle, "cube_list_of_lookup_cubes", 100));

                hDim_item_nof_fed = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, docCube.server.handle, TM1API.TM1ServerDimensions(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[4].ToString(), 100));
                vDim_item_nof_fed = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, hDim_item_nof_fed, TM1API.TM1DimensionElements(), TM1API.TM1ValString(docCube.server.pool.handle, "cube_number_of_fed_cubes", 100));

                hDim_item_list_of_feed = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, docCube.server.handle, TM1API.TM1ServerDimensions(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[4].ToString(), 100));
                vDim_item_list_of_feed = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, hDim_item_nof_look, TM1API.TM1DimensionElements(), TM1API.TM1ValString(docCube.server.pool.handle, "cube_list_of_fed_cubes", 100));

                hDim_measure = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, docCube.server.handle, TM1API.TM1ServerDimensions(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[5].ToString(), 100));
                vDim_measure = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, hDim_measure, TM1API.TM1DimensionElements(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[5].elements[0].ToString(), 100));

                hDim_list = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, docCube.server.handle, TM1API.TM1ServerDimensions(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[3].ToString(), 100));
                vDim_list = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, hDim_list, TM1API.TM1DimensionElements(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[3].elements[i].ToString(), 100));

                int[] elements_name = { vDim_timestamp, vDim_tm1server, vDim_tm1object, vDim_list, vDim_item_name, vDim_measure };

                vArrayOfCells = TM1API.TM1ValArray(docCube.server.pool.handle, ref elements_name, 6);

                TM1API.TM1ValArraySet(vArrayOfCells, vDim_timestamp, 1);
                TM1API.TM1ValArraySet(vArrayOfCells, vDim_tm1server, 2);
                TM1API.TM1ValArraySet(vArrayOfCells, vDim_tm1object, 3);
                TM1API.TM1ValArraySet(vArrayOfCells, vDim_list, 4);
                TM1API.TM1ValArraySet(vArrayOfCells, vDim_item_name, 5);
                TM1API.TM1ValArraySet(vArrayOfCells, vDim_measure, 6);

                value_name = TM1API.TM1CubeCellValueGet(docCube.server.pool.handle, docCube.handle, vArrayOfCells);
                cube_name = TM1API.intPtrToString(docCube.server.user.handle, value_name);

                // am Ende der Cubes angekommen
                if (cube_name.Equals(""))
                    break;

                // ---------------------------------- List Dimensions ---------------------------------- //
                TM1API.TM1ValArraySet(vArrayOfCells, vDim_item_nof_dims, 5);
                value_nof_dims = TM1API.TM1CubeCellValueGet(docCube.server.pool.handle, docCube.handle, vArrayOfCells);
                nof_dims = Int32.Parse(TM1API.intPtrToString(docCube.server.user.handle, value_nof_dims));

                List<DocDimension> list_dims = new List<DocDimension>();

                hDim_item_dim_name = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, docCube.server.handle, TM1API.TM1ServerDimensions(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[4].ToString(), 100));
                vDim_item_dim_name = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, hDim_item_nof_dims, TM1API.TM1DimensionElements(), TM1API.TM1ValString(docCube.server.pool.handle, "cube_list_of_dimensions", 100));

                hDim_measure = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, docCube.server.handle, TM1API.TM1ServerDimensions(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[5].ToString(), 100));

                // get the dims
                for (int k = 1; k <= nof_dims; k++)
                {
                    vDim_measure = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, hDim_measure, TM1API.TM1DimensionElements(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[5].elements[k].ToString(), 100));

                    TM1API.TM1ValArraySet(vArrayOfCells, vDim_item_dim_name, 5);
                    TM1API.TM1ValArraySet(vArrayOfCells, vDim_measure, 6);

                    value_dim_name = TM1API.TM1CubeCellValueGet(docCube.server.pool.handle, docCube.handle, vArrayOfCells);
                    dim_name = TM1API.intPtrToString(docCube.server.user.handle, value_dim_name);

                    DocDimension dim = new DocDimension(dim_name);

                    dimensions.Add(dim);
                    list_dims.Add(dimensions.First(x => x.name.Equals(dim_name)));
                }


                // ---------------------------------- List Look Up ---------------------------------- //
                vDim_measure = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, hDim_measure, TM1API.TM1DimensionElements(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[5].elements[0].ToString(), 100));

                TM1API.TM1ValArraySet(vArrayOfCells, vDim_item_nof_look, 5);
                TM1API.TM1ValArraySet(vArrayOfCells, vDim_measure, 6);

                value_nof_look = TM1API.TM1CubeCellValueGet(docCube.server.pool.handle, docCube.handle, vArrayOfCells);

                // check if look_up cubes exist
                try
                {
                    nof_look = Int32.Parse(TM1API.intPtrToString(docCube.server.user.handle, value_nof_look));
                    List<string> list_look_up_cubes = new List<string>();

                    for (int k = 1; k <= nof_look; k++)
                    {
                        int value_look_up_dim_name;
                        string look_up_dim_name;
                      
                        vDim_measure = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, hDim_measure, TM1API.TM1DimensionElements(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[5].elements[k].ToString(), 100));

                        TM1API.TM1ValArraySet(vArrayOfCells, vDim_item_list_of_look, 5);
                        TM1API.TM1ValArraySet(vArrayOfCells, vDim_measure, 6);

                        value_look_up_dim_name = TM1API.TM1CubeCellValueGet(docCube.server.pool.handle, docCube.handle, vArrayOfCells);
                        look_up_dim_name = TM1API.intPtrToString(docCube.server.user.handle, value_look_up_dim_name);

                        list_look_up_cubes.Add(look_up_dim_name);
                    }
                    look_ups.Add(cube_name, list_look_up_cubes);
                }
                // if not set nof look_up cubes to -1
                catch (System.FormatException)
                {
                    nof_look = -1;
                }


                // ---------------------------------- List Feeder ---------------------------------- //
                vDim_measure = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, hDim_measure, TM1API.TM1DimensionElements(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[5].elements[0].ToString(), 100));

                TM1API.TM1ValArraySet(vArrayOfCells, vDim_item_nof_fed, 5);
                TM1API.TM1ValArraySet(vArrayOfCells, vDim_measure, 6);

                value_nof_fed = TM1API.TM1CubeCellValueGet(docCube.server.pool.handle, docCube.handle, vArrayOfCells);

                // check if feeder cubes exist
                try
                {
                    nof_fed = Int32.Parse(TM1API.intPtrToString(docCube.server.user.handle, value_nof_fed));
                    List<string> list_feeder_cubes = new List<string>();

                    for (int k = 1; k <= nof_fed; k++)
                    {
                        int value_feeder_dim_name;
                        string feeder_dim_name;

                        vDim_measure = TM1API.TM1ObjectListHandleByNameGet(docCube.server.pool.handle, hDim_measure, TM1API.TM1DimensionElements(), TM1API.TM1ValString(docCube.server.pool.handle, docCube.dimensions[5].elements[k].ToString(), 100));

                        TM1API.TM1ValArraySet(vArrayOfCells, vDim_item_list_of_feed, 5);
                        TM1API.TM1ValArraySet(vArrayOfCells, vDim_measure, 6);

                        value_feeder_dim_name = TM1API.TM1CubeCellValueGet(docCube.server.pool.handle, docCube.handle, vArrayOfCells);
                        feeder_dim_name = TM1API.intPtrToString(docCube.server.user.handle, value_feeder_dim_name);

                        list_feeder_cubes.Add(feeder_dim_name);
                    }
                    feeder.Add(cube_name, list_feeder_cubes);
                }
                // if not set nof look_up cubes to -1
                catch (System.FormatException)
                {
                    nof_fed = -1;
                }

                // -------------------------------------------------------------- //
                DocCube cube = new DocCube(i+1, cube_name, nof_dims, list_dims);
                cube.nof_look = nof_look;
                cube.nof_fed = nof_fed;
                cubes.Add(cube);
            } 
        }

        private void SetLookFeed()
        {
            foreach (string key in look_ups.Keys)
            {
                DocCube myCube = cubes.Find(x => x.name.Equals(key));

                foreach (string lookUpCube in look_ups[key])
                {
                    myCube.look_up_cubes.Add(cubes.Find(x => x.name.Equals(lookUpCube)));
                }

            }

            foreach (string key in feeder.Keys)
            {
                DocCube myCube = cubes.Find(x => x.name.Equals(key));
                    
                foreach (string feederCube in feeder[key])
                {
                    myCube.fed_cubes.Add(cubes.Find(x => x.name.Equals(feederCube)));
                }
            }
        }

        public List<Element> GetTimeStamps()
        {
            return docCube.dimensions[0].elements;
        }

        public List<Element> GetTM1Servers()
        {
            return docCube.dimensions[1].elements;
        }
    }
}
