using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OTTER
{
    public abstract class Item : Sprite  
    {


        public Item(string s, int xcor, int ycor)
           : base(s, xcor, ycor)
        {
           
        }


        protected int pozicijaX;

       

        protected int pozicijaY;

        


        public abstract int PozicijaX {get; set;}

        public abstract int PozicijaY { get; set; }


    }

    public class Bomba : Item
    {


        public Bomba(string slika, int xp, int yp) :base(slika,xp,yp)
        {

        }


        private bool koristise;

        public bool Koristise
        {
            get { return koristise; }
            set { koristise = value; }
        }

        public override int PozicijaX
        {
            get { return pozicijaX; }
            set { pozicijaX = value; }
        }


        public override int PozicijaY
        {
            get { return pozicijaY; }
            set { pozicijaY = value; }
        }

      





    }

    class Poklon : Item
    {
        public Poklon(string slika, int xp, int yp) : base(slika, xp, yp)
        {

        }

        public override int PozicijaX
        {
            get { return pozicijaX; }
            set { pozicijaX = value; }
        }


        public override int PozicijaY
        {
            get { return pozicijaY; }
            set { pozicijaY = value; }
        }

        private bool pokupljen;

        public bool Pokupljen
        {
            get { return pokupljen; }
            set { pokupljen = value; }
        }

    }
}
