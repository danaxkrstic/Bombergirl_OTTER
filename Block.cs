using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace OTTER
{
    class Block : Sprite
    {
        protected List<Block> kamenje = new List<Block>();
        public Block[,] matrica = new Block[4, 6];
        protected Block test;
       
        public Block(string slika, int xp, int yp) : base(slika, xp, yp) { }

        public void PostaviKamenje()
        {
            bool krug = false; //pamti kad se napravio 1. krug
            kamenje.Clear();
            kamenje.Add(this);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 6; j++)
                {
               
                    if (j == 0 && i == 0)
                    {

                        matrica[i, j] = this;
                      
                    }
                    else
                    {
                        if (j != 0 && krug == false) //tu nam treba pamcenje kruga jer j!=0 i j==0 nam tribaju zbog index out of range i kad je i>=1
                        {
                            test = new Block("sprites\\block.png", kamenje[j - 1].X + this.Width*2, kamenje[j - 1].Y);
                        }
                        else //ovo nam treba nakon 1. kruga jer sad za (1,0) imamo samo 0. element, pa bi bilo out of range da stavimo j-1, a ono krug nam treba jer za npr. (1,1) gdje j!=0 će 
                        {            //se bez kruga pozvati na if j!=0 i namistit će blok obzirom na kamenje[j-1]
                         
                            test = new Block("sprites\\block.png", kamenje[j].X + this.Width*2, kamenje[j].Y);
                        }
                        kamenje.Add(test);
                        matrica[i, j] = test;
            

                    }
                }
                if (i == 0) 
                    test = new Block("sprites\\block.png", kamenje[0].X - this.Width*2, kamenje[1].Y + this.Width*2);
                else
                    test = new Block("sprites\\block.png", kamenje[0].X, kamenje[1].Y + this.Width*2);


                if (i != 3)
                    matrica[i + 1, 0] = test;



                kamenje.Clear();

              
                kamenje.Add(test);
                krug = true;


            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Game.AddSprite(matrica[i, j]);
                }

            }


        }

        public Block DodirKamenja(Player p)
        {


            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (p.TouchingSprite(matrica[i, j]))
                    {
                        return matrica[i, j];
                    }
                }
            }
            return null;


        }

        


    }

    class RoseBlock : Block
    {
         
        public RoseBlock(string slika, int xp, int yp) : base(slika, xp, yp)
        {
          
        }

        private int koordinatamatriceX;

        public int KoomX
        {
            get { return koordinatamatriceX; }
            set 
            { 
          
                koordinatamatriceX = value;
               
            
            }
        }

        private int koordinatamatriceY;

        public int KoomY
        {
            get { return koordinatamatriceY; }
            set 
            {
               
                    koordinatamatriceY = value;
              
            }
        }




        private RoseBlock test;
        public List<RoseBlock> roseKamenje = new List<RoseBlock>();

        public void PostaviRoseBlocks(Block b)
        {
            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X, b.matrica[0, 0].Y - this.Heigth);
            test.KoomX = 0;
            test.KoomY = 1;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X*4, b.matrica[0, 0].Y - this.Heigth);
            test.KoomX = 0;
            test.KoomY = 4;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X*6, b.matrica[0, 0].Y - this.Heigth);
            test.KoomX = 0;
            test.KoomY = 6;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X*7, b.matrica[0, 0].Y - this.Heigth);
            test.KoomX = 0;
            test.KoomY = 7;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X*11, b.matrica[0, 0].Y - this.Heigth);
            test.KoomX = 0;
            test.KoomY = 11;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            //***//

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X*2, b.matrica[0, 0].Y);
            test.KoomX = 1;
            test.KoomY = 2;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X*4, b.matrica[0, 0].Y);
            test.KoomX = 1;
            test.KoomY = 4;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X*10, b.matrica[0, 0].Y);
            test.KoomX = 1;
            test.KoomY = 10;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            //***//

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X*2, b.matrica[0, 0].Y + this.Heigth);
            test.KoomX = 2;
            test.KoomY = 2;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X*4, b.matrica[0, 0].Y + this.Heigth);
            test.KoomX = 2;
            test.KoomY = 4;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X*6, b.matrica[0, 0].Y + this.Heigth);
            test.KoomX = 2;
            test.KoomY = 6;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X*7, b.matrica[0, 0].Y + this.Heigth);
            test.KoomX = 2;
            test.KoomY = 7;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X*8, b.matrica[0, 0].Y + this.Heigth);
            test.KoomX = 2;
            test.KoomY = 8;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            //**//

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X * 2, b.matrica[0, 0].Y * 3);
            test.KoomX = 3;
            test.KoomY = 2;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X * 8, b.matrica[0, 0].Y * 3);
            test.KoomX = 3;
            test.KoomY = 8;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            //**//

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X*0,b.matrica[0,0].Y*4);
            test.KoomX = 4;
            test.KoomY = 0;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            for (int i = 1; i < 6; i++)
            {
                test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X * i, b.matrica[0, 0].Y * 4);
                test.KoomX = 4;
                test.KoomY =i;
                roseKamenje.Add(test);
                Game.AddSprite(test);

            }

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X *8, b.matrica[0, 0].Y * 4);
            test.KoomX = 4;
            test.KoomY = 8;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            //**//

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X * 1, b.matrica[0, 0].Y * 6);
            test.KoomX = 6;
            test.KoomY = 1;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X * 3, b.matrica[0, 0].Y * 6);
            test.KoomX = 6;
            test.KoomY = 3;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            for (int i = 3; i < 14; i++)
            {
                if (i % 2 == 0)
                {
                    test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X * i, b.matrica[0, 0].Y * 6);
                    test.KoomX = 6;
                    test.KoomY = i;
                    roseKamenje.Add(test);
                    Game.AddSprite(test);
                }

            }

            //**//

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X * 2, b.matrica[0, 0].Y * 7);
            test.KoomX = 7;
            test.KoomY = 2;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X * 4, b.matrica[0, 0].Y * 7);
            test.KoomX = 7;
            test.KoomY = 4;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X * 8, b.matrica[0, 0].Y * 7);
            test.KoomX = 7;
            test.KoomY = 8;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            //**//

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X * 4, b.matrica[0, 0].Y * 8);
            test.KoomX = 8;
            test.KoomY = 4;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X * 5, b.matrica[0, 0].Y * 8);
            test.KoomX = 8;
            test.KoomY = 5;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X * 6, b.matrica[0, 0].Y * 8);
            test.KoomX = 8;
            test.KoomY = 6;
            roseKamenje.Add(test);
            Game.AddSprite(test);

            test = new RoseBlock("sprites\\rose_block.png", b.matrica[0, 0].X * 2, b.matrica[0, 0].Y * 8);
            test.KoomX = 8;
            test.KoomY = 2;
            roseKamenje.Add(test);
            Game.AddSprite(test);
        }


    }
    } 
