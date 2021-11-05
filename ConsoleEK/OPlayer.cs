using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    public class OPlayer
    {
        public int Position { get; set; }
        public int Turn { get; set; }
        public int NumOfCard { get; set; }
        public bool Explode { get; set; }
        public bool GetBoom { get; set; }
        public OPlayer()
        {
            Explode = false;
            GetBoom = false;
        }
    }
}
