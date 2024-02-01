using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;

namespace OTTER
{
    class Player : Sprite
    {
 
        private string pogled;
        public string Pogled
        {
            get { return pogled; }
            set { pogled = value; }
        }


        public virtual int PozX
        {
            get { return pozicijaumatriciX; }
            set 
            {
               if (value>= 0&&value<=8)
                { pozicijaumatriciX = value; }
                else
                {
                    if (this.Pogled == "gore")
                    {
                        pozicijaumatriciX = value + 1;
                    }
                    if (this.Pogled == "dolje")
                    {
                        pozicijaumatriciX = value - 1;
                    }
                    else { pozicijaumatriciX = 0; }
                   
                }
            }
        }       
        protected int pozicijaumatriciX;


        private int pozicijaumatriciY;
        public int PozY
        {
            get { return pozicijaumatriciY; }
            set 
            {    if (value >= 0 && value<=12)
                { pozicijaumatriciY = value; }
                else
                {
                    if (this.Pogled == "lijevo")
                    { pozicijaumatriciY = value + 1; }
                    if(this.Pogled=="desno")
                    {
                        pozicijaumatriciY = value - 1;
                    }
                    else
                    {
                        pozicijaumatriciY = 0;
                    }
                }
            }
        }

        protected bool ziva;
        public virtual bool Ziv
        {
            get { return ziva; }
            set { ziva = value; }
        }

        private bool playable;
        public bool Playable
        {
            get { return playable; }
            set { playable = value; }
        }

        public Player(string slika, int xp, int yp) : base(slika, xp, xp)
        {

        }

        public void ChangeCostume(int i)
        {
            this.CurrentCostume = this.Costumes[i];
        }



    }

    class Enemy : Player
    {
        public Enemy(string slika, int xp, int yp) : base(slika, xp, xp)
        {

        }

        public override int PozX
        {
            get { return pozicijaumatriciX; }
            set
            {
                if (value >= 0 && value <= 8)
                { pozicijaumatriciX = value; }
                else
                {
                    pozicijaumatriciX = -1;

                }
            }
        }

        public override bool Ziv
        {
            get { return ziva; }
            set
            {
                if (value != null)
                { ziva = value; }
                else
                {
                    ziva = true;
                }
            
            }
        }

    }
}

