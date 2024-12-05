using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinderApp.Model
{
    public class Match
    {

        public int MatchId { get; set; }
        public int Usuario1Id { get; set; }
        public int Usuario2Id { get; set;}
        public DateTime FechaMatch {  get; set; }

    }
}
