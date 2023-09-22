using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AoCDiscord
{
    public class Person
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int LocalScore { get; set; }
        public int Stars { get; set; }
        public int LastStarTime { get; set; }
    }
}
