using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LABB3_Bordsbokning_Neliz
{
    internal interface IBokning
    {
        public string Datum { get; set; }
        public string BordsTyp { get; set; }
        public string Tid { get; set; }
    }
}
