using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;

namespace Application.Biljke
{
    public class BiljkeParams : PagingParams
    {
        public bool Lekovita { get; set; }
        public bool NeLekovita {get; set;}
    }
}