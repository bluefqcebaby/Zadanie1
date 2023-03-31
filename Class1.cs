using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie1
{
    class SortModel
    {
        public SortModel(int id, string name)
        {

            this.id = id;
            this.name = name;
        }

        public int id { get; set; }

        public string name { get; set; }

    }

    class FilterModel
    {
        public int ID_Type_agent { get; set;}
        public string Name { get; set;}
    }
}
