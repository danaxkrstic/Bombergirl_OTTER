using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OTTER
{
    class ImeIgraca
    {

      
            private int bodovi;

            public int Bodovi
            {
                get { return bodovi; }
                set { bodovi = value; }
            }

            private string ime;

            public string Ime
            {
                get { return ime; }
                set { ime = value; }
            }

            public ImeIgraca(int i, string s)
            {
                this.Bodovi = i;
                this.Ime = s;
            }
        
    }
}
