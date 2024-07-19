using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;

namespace Application.Obilasci
{
    public class ObilazakParams : PagingParams
    {
        public bool IsGoing { get; set; }
        public bool IsHost {get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;//po defaultu da krece da prikazuje samo buduce obilaske od trenutnog dana
    }
}