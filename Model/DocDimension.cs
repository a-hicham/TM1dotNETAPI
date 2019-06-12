using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DocDimension
    {
        public string name;
        public IDictionary<int, Dictionary<string, string>> properties;

        public DocDimension(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return this.name.ToString();
        }
    }
}
