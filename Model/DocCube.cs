using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DocCube
    {
        public int ID { private set; get; }
        public string name { set; get; }
        public int nof_dims { set; get; }
        public int nof_look;
        public int nof_fed;
        public List<DocDimension> dims { set; get; }
        public List<DocCube> look_up_cubes { set; get; }
        public List<DocCube> fed_cubes { set; get; }

        public DocCube(int id, string name, int nof_dims, List<DocDimension> dims)
        {
            this.ID = id;
            this.name = name;
            this.nof_dims = nof_dims;
            this.dims = dims;
            look_up_cubes = new List<DocCube>();
            fed_cubes = new List<DocCube>();
        }

        public override string ToString()
        {
            return this.name.ToString();
        }
    }
}
