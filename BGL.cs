using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Drawing.Text;
using System.Xml;
using System.Security.Cryptography;
using System.IO;

namespace OTTER
{
    /// <summary>
    /// -
    /// </summary>
    public partial class BGL : Form
    {
        /* ------------------- */
        #region Environment Variables

        List<Func<int>> GreenFlagScripts = new List<Func<int>>();

        /// <summary>
        /// Uvjet izvršavanja igre. Ako je <c>START == true</c> igra će se izvršavati.
        /// </summary>
        /// <example><c>START</c> se često koristi za beskonačnu petlju. Primjer metode/skripte:
        /// <code>
        /// private int MojaMetoda()
        /// {
        ///     while(START)
        ///     {
        ///       //ovdje ide kod
        ///     }
        ///     return 0;
        /// }</code>
        /// </example>
        public static bool START = true;

        //sprites
        /// <summary>
        /// Broj likova.
        /// </summary>
        public static int spriteCount = 0, soundCount = 0;

        /// <summary>
        /// Lista svih likova.
        /// </summary>
        //public static List<Sprite> allSprites = new List<Sprite>();
        public static SpriteList<Sprite> allSprites = new SpriteList<Sprite>();

        //sensing
        int mouseX, mouseY;
        Sensing sensing = new Sensing();

        //background
        List<string> backgroundImages = new List<string>();
        int backgroundImageIndex = 0;
        string ISPIS = "";

        SoundPlayer[] sounds = new SoundPlayer[1000];
        TextReader[] readFiles = new StreamReader[1000];
        TextWriter[] writeFiles = new StreamWriter[1000];
        bool showSync = false;
        int loopcount;
        DateTime dt = new DateTime();
        String time;
        double lastTime, thisTime, diff;

        #endregion
        /* ------------------- */
        #region Events

        private void Draw(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            try
            {
                foreach (Sprite sprite in allSprites)
                {
                    if (sprite != null)
                        if (sprite.Show == true)
                        {
                            g.DrawImage(sprite.CurrentCostume, new Rectangle(sprite.X, sprite.Y, sprite.Width, sprite.Heigth));
                        }
                    if (allSprites.Change)
                        break;
                }
                if (allSprites.Change)
                    allSprites.Change = false;
            }
            catch
            {
                //ako se doda sprite dok crta onda se mijenja allSprites
                MessageBox.Show("Greška!");
            }
        }

        private void startTimer(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Start();
            Init();
        }

        private void updateFrameRate(object sender, EventArgs e)
        {
            updateSyncRate();
        }

        /// <summary>
        /// Crta tekst po pozornici.
        /// </summary>
        /// <param name="sender">-</param>
        /// <param name="e">-</param>
        public void DrawTextOnScreen(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            var brush = new SolidBrush(Color.WhiteSmoke);
            string text = ISPIS;

            SizeF stringSize = new SizeF();
            Font stringFont = new Font("Arial", 14);
            stringSize = e.Graphics.MeasureString(text, stringFont);

            using (Font font1 = stringFont)
            {
                RectangleF rectF1 = new RectangleF(0, 0, stringSize.Width, stringSize.Height);
                e.Graphics.FillRectangle(brush, Rectangle.Round(rectF1));
                e.Graphics.DrawString(text, font1, Brushes.Black, rectF1);
            }
        }

        private void mouseClicked(object sender, MouseEventArgs e)
        {
            //sensing.MouseDown = true;
            sensing.MouseDown = true;
        }

        private void mouseDown(object sender, MouseEventArgs e)
        {
            //sensing.MouseDown = true;
            sensing.MouseDown = true;
        }

        private void mouseUp(object sender, MouseEventArgs e)
        {
            //sensing.MouseDown = false;
            sensing.MouseDown = false;
        }

        private void mouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;

            //sensing.MouseX = e.X;
            //sensing.MouseY = e.Y;
            //Sensing.Mouse.x = e.X;
            //Sensing.Mouse.y = e.Y;
            sensing.Mouse.X = e.X;
            sensing.Mouse.Y = e.Y;

        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            sensing.Key = e.KeyCode.ToString();
            sensing.KeyPressedTest = true;
        }

        private void keyUp(object sender, KeyEventArgs e)
        {
            sensing.Key = "";
            sensing.KeyPressedTest = false;
        }

        private void Update(object sender, EventArgs e)
        {
            if (sensing.KeyPressed(Keys.Escape))
            {
                START = false;
            }

            if (START)
            {
                this.Refresh();
            }
        }

        #endregion
        /* ------------------- */
        #region Start of Game Methods

        //my
        #region my

        //private void StartScriptAndWait(Func<int> scriptName)
        //{
        //    Task t = Task.Factory.StartNew(scriptName);
        //    t.Wait();
        //}

        //private void StartScript(Func<int> scriptName)
        //{
        //    Task t;
        //    t = Task.Factory.StartNew(scriptName);
        //}

        private int AnimateBackground(int intervalMS)
        {
            while (START)
            {
                setBackgroundPicture(backgroundImages[backgroundImageIndex]);
                Game.WaitMS(intervalMS);
                backgroundImageIndex++;
                if (backgroundImageIndex == 3)
                    backgroundImageIndex = 0;
            }
            return 0;
        }

        private void KlikNaZastavicu()
        {
            foreach (Func<int> f in GreenFlagScripts)
            {
                Task.Factory.StartNew(f);
            }
        }

        #endregion

        /// <summary>
        /// BGL
        /// </summary>
        public BGL()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Pričekaj (pauza) u sekundama.
        /// </summary>
        /// <example>Pričekaj pola sekunde: <code>Wait(0.5);</code></example>
        /// <param name="sekunde">Realan broj.</param>
        public void Wait(double sekunde)
        {
            int ms = (int)(sekunde * 1000);
            Thread.Sleep(ms);
        }

        //private int SlucajanBroj(int min, int max)
        //{
        //    Random r = new Random();
        //    int br = r.Next(min, max + 1);
        //    return br;
        //}

        /// <summary>
        /// -
        /// </summary>
        public void Init()
        {
            if (dt == null) time = dt.TimeOfDay.ToString();
            loopcount++;
            //Load resources and level here
            this.Paint += new PaintEventHandler(DrawTextOnScreen);
            SetupGame();
        }

        /// <summary>
        /// -
        /// </summary>
        /// <param name="val">-</param>
        public void showSyncRate(bool val)
        {
            showSync = val;
            if (val == true) syncRate.Show();
            if (val == false) syncRate.Hide();
        }

        /// <summary>
        /// -
        /// </summary>
        public void updateSyncRate()
        {
            if (showSync == true)
            {
                thisTime = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
                diff = thisTime - lastTime;
                lastTime = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;

                double fr = (1000 / diff) / 1000;

                int fr2 = Convert.ToInt32(fr);

                syncRate.Text = fr2.ToString();
            }

        }

        //stage
        #region Stage

        /// <summary>
        /// Postavi naslov pozornice.
        /// </summary>
        /// <param name="title">tekst koji će se ispisati na vrhu (naslovnoj traci).</param>
        public void SetStageTitle(string title)
        {
            this.Text = title;
        }

        /// <summary>
        /// Postavi boju pozadine.
        /// </summary>
        /// <param name="r">r</param>
        /// <param name="g">g</param>
        /// <param name="b">b</param>
        public void setBackgroundColor(int r, int g, int b)
        {
            this.BackColor = Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Postavi boju pozornice. <c>Color</c> je ugrađeni tip.
        /// </summary>
        /// <param name="color"></param>
        public void setBackgroundColor(Color color)
        {
            this.BackColor = color;
        }

        /// <summary>
        /// Postavi sliku pozornice.
        /// </summary>
        /// <param name="backgroundImage">Naziv (putanja) slike.</param>
        public void setBackgroundPicture(string backgroundImage)
        {
            this.BackgroundImage = new Bitmap(backgroundImage);
        }

        /// <summary>
        /// Izgled slike.
        /// </summary>
        /// <param name="layout">none, tile, stretch, center, zoom</param>
        public void setPictureLayout(string layout)
        {
            if (layout.ToLower() == "none") this.BackgroundImageLayout = ImageLayout.None;
            if (layout.ToLower() == "tile") this.BackgroundImageLayout = ImageLayout.Tile;
            if (layout.ToLower() == "stretch") this.BackgroundImageLayout = ImageLayout.Stretch;
            if (layout.ToLower() == "center") this.BackgroundImageLayout = ImageLayout.Center;
            if (layout.ToLower() == "zoom") this.BackgroundImageLayout = ImageLayout.Zoom;
        }

        #endregion

        //sound
        #region sound methods

        /// <summary>
        /// Učitaj zvuk.
        /// </summary>
        /// <param name="soundNum">-</param>
        /// <param name="file">-</param>
        public void loadSound(int soundNum, string file)
        {
            soundCount++;
            sounds[soundNum] = new SoundPlayer(file);
        }

        /// <summary>
        /// Sviraj zvuk.
        /// </summary>
        /// <param name="soundNum">-</param>
        public void playSound(int soundNum)
        {
            sounds[soundNum].Play();
        }

        /// <summary>
        /// loopSound
        /// </summary>
        /// <param name="soundNum">-</param>
        public void loopSound(int soundNum)
        {
            sounds[soundNum].PlayLooping();
        }

        /// <summary>
        /// Zaustavi zvuk.
        /// </summary>
        /// <param name="soundNum">broj</param>
        public void stopSound(int soundNum)
        {
            sounds[soundNum].Stop();
        }

        #endregion

        //file
        #region file methods

        /// <summary>
        /// Otvori datoteku za čitanje.
        /// </summary>
        /// <param name="fileName">naziv datoteke</param>
        /// <param name="fileNum">broj</param>
        public void openFileToRead(string fileName, int fileNum)
        {
            readFiles[fileNum] = new StreamReader(fileName);
        }

        /// <summary>
        /// Zatvori datoteku.
        /// </summary>
        /// <param name="fileNum">broj</param>
        public void closeFileToRead(int fileNum)
        {
            readFiles[fileNum].Close();
        }

        /// <summary>
        /// Otvori datoteku za pisanje.
        /// </summary>
        /// <param name="fileName">naziv datoteke</param>
        /// <param name="fileNum">broj</param>
        public void openFileToWrite(string fileName, int fileNum)
        {
            writeFiles[fileNum] = new StreamWriter(fileName);
        }

        /// <summary>
        /// Zatvori datoteku.
        /// </summary>
        /// <param name="fileNum">broj</param>
        public void closeFileToWrite(int fileNum)
        {
            writeFiles[fileNum].Close();
        }

        /// <summary>
        /// Zapiši liniju u datoteku.
        /// </summary>
        /// <param name="fileNum">broj datoteke</param>
        /// <param name="line">linija</param>
        public void writeLine(int fileNum, string line)
        {
            writeFiles[fileNum].WriteLine(line);
        }

        /// <summary>
        /// Pročitaj liniju iz datoteke.
        /// </summary>
        /// <param name="fileNum">broj datoteke</param>
        /// <returns>vraća pročitanu liniju</returns>
        public string readLine(int fileNum)
        {
            return readFiles[fileNum].ReadLine();
        }

        /// <summary>
        /// Čita sadržaj datoteke.
        /// </summary>
        /// <param name="fileNum">broj datoteke</param>
        /// <returns>vraća sadržaj</returns>
        public string readFile(int fileNum)
        {
            return readFiles[fileNum].ReadToEnd();
        }

        #endregion

        //mouse & keys
        #region mouse methods

        /// <summary>
        /// Sakrij strelicu miša.
        /// </summary>
        public void hideMouse()
        {
            Cursor.Hide();
        }

        /// <summary>
        /// Pokaži strelicu miša.
        /// </summary>
        public void showMouse()
        {
            Cursor.Show();
        }

        /// <summary>
        /// Provjerava je li miš pritisnut.
        /// </summary>
        /// <returns>true/false</returns>
        public bool isMousePressed()
        {
            //return sensing.MouseDown;
            return sensing.MouseDown;
        }

        /// <summary>
        /// Provjerava je li tipka pritisnuta.
        /// </summary>
        /// <param name="key">naziv tipke</param>
        /// <returns></returns>
        public bool isKeyPressed(string key)
        {
            if (sensing.Key == key)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Provjerava je li tipka pritisnuta.
        /// </summary>
        /// <param name="key">tipka</param>
        /// <returns>true/false</returns>
        public bool isKeyPressed(Keys key)
        {
            if (sensing.Key == key.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion
        /* ------------------- */

        /* ------------ GAME CODE START ------------ */

        /* Game variables */

        //SPRITES
        Player bombergirl;
        Block kamen;
        RoseBlock roseBlock;
        Bomba bomba;
        Bomba novaBomba1, novaBomba2;
        Enemy monster1, monster2;
        Enemy spider1, spider2;
        Poklon poklon;
        Sprite eksplozija1,eksplozija2,eksplozija3,eksplozija4,eksplozija5;
        

        //SVAKAKVE VARIJABLE
        bool ubrzanje;
        int brojac = 0;
        public int[,] mapa = new int[9, 13];
        public int killCount;
        bool pisem;
        bool prviput = true;
        bool reset;


        //LISTE
        List<Sprite> kamenje;
        List<ImeIgraca> players;
        List<Enemy> cudovista = new List<Enemy>();
  
        //DELEGATI
        public delegate void Ispisivanje(int i,string s);
        Ispisivanje _pisi, _citaj;
        public delegate void ICanHandleBombsNow(Bomba b);
        public static event ICanHandleBombsNow _rusi, _ubijanjecudovista, _ubijanjebombergirl;       

        private void SetupGame()
        {
            
            //1. setup stage
            SetStageTitle("---bombergirl---");
            setBackgroundColor(Color.DarkKhaki);
            //setBackgroundPicture("backgrounds\\back.jpg");
            //none, tile, stretch, center, zoom
            //setPictureLayout("stretch");



            //2. add sprites
            kamen = new Block("sprites\\block.png", 0, 0);
            bombergirl = new Player("sprites\\bombergirl_front.png", 5, 100);
            bombergirl.AddCostumes("sprites\\bombergirl_back.png", "sprites\\bombergirl_left.png", "sprites\\bombergirl_right.png", "sprites\\bombergirl_gameover.png");
            monster1 = new Enemy("sprites\\mon1_front.png", 0, 0);
            spider1 = new Enemy("sprites\\spider_left.gif", 0, 0);
            monster2 = new Enemy("sprites\\mon1_front.png", 0, 0);
            spider2 = new Enemy("sprites\\spider_left.gif", 0, 0);
            poklon = new Poklon("sprites\\poklon.png", -100, -100);
            eksplozija1 = new Sprite("sprites\\eksplozija.png", -100, -100);
            eksplozija2 = new Sprite("sprites\\eksplozija.png", -100, -100);
            eksplozija3 = new Sprite("sprites\\eksplozija.png", -100, -100);
            eksplozija4 = new Sprite("sprites\\eksplozija.png", -100, -100);
            eksplozija5 = new Sprite("sprites\\eksplozija.png", -100, -100);
            roseBlock = new RoseBlock("sprites\\rose_block.png", 100, 100);
            bomba = new Bomba("sprites\\bomb.png", -100, -100);


            //SVOJSTVA ZA BOMBERGIRL
            bombergirl.Ziv = true;
            bombergirl.Playable = true; //triba za score


            //KOSTIMI I POZICIJE CUDOVISTA
            monster1.AddCostumes("sprites\\mon1_back.png");
            spider1.AddCostumes("sprites\\spider_right.gif");
            spider1.Width = kamen.Width;
            spider1.Heigth = kamen.Heigth;
            monster1.X = 12 * kamen.Width;
            spider1.X = 10 * kamen.Width;
            monster2.AddCostumes("sprites\\mon1_back.png");
            spider2.AddCostumes("sprites\\spider_right.gif");
            spider2.Width = kamen.Width;
            spider2.Heigth = kamen.Heigth;
            monster2.Y = 5 * kamen.Width;
            monster2.X = 1;
            spider2.X = 12 * kamen.Width;
            spider2.Y = 8 * kamen.Width;
            poklon.PozicijaX = -100;
            poklon.PozicijaY = -100;



            //OZIVI CUDOVISTA
            monster1.Ziv = true;
            monster2.Ziv = true;
            spider1.Ziv = true;
            spider2.Ziv = true;


            //POSTAVLJANJE CUDOVISTA NA MAPU
            monster1.PozX = 0;
            monster1.PozY = 12;
            monster2.PozX = 5;
            monster2.PozY = 0;
            spider1.PozX = 0;
            spider1.PozY = 10;
            spider2.PozX = 8;
            spider2.PozY = 12;

            //DODAVANJE CUDOVISTA NA LISTU
            cudovista.Add(monster1);
            cudovista.Add(monster2);
            cudovista.Add(spider1);
            cudovista.Add(spider2);

          



            //NAMISTANJE BOMBE
            bomba.Koristise = false;

           
            //POSTAVLJANJE KAMENJA
            kamen.X = kamen.Heigth;
            kamen.Y = kamen.Width;
            kamen.PostaviKamenje();
            roseBlock.PostaviRoseBlocks(kamen);  

            
            //OSTALA ZADAVANJA
            killCount = 0;
            poklon.Pokupljen = false;
            reset = false;
            pisem = false;
            ubrzanje = false;


            //POPUNJAVANJE MAPE MATRICE I STAVARANJE LISTE ZA IMENA
            PopuniMiniMapu();
            players = new List<ImeIgraca>();
            PocetniUnosScorea();

            //DELEGATI
            _pisi = new Ispisivanje(Pisi);
            _citaj = new Ispisivanje(Citaj);
            _pisi += _citaj;
            _rusi = new ICanHandleBombsNow(Rusenje);
            _ubijanjecudovista = new ICanHandleBombsNow(UbijCudoviste);
            _ubijanjebombergirl = new ICanHandleBombsNow(BumKrajBomberGirl);
            _ubijanjecudovista += _ubijanjebombergirl;
            _rusi += _ubijanjecudovista;


           


            //2.1 dodavanje sprite u igru
            Game.AddSprite(bombergirl);
            Game.AddSprite(bomba);
            Game.AddSprite(monster1);
            Game.AddSprite(spider1);
            Game.AddSprite(spider2);
            Game.AddSprite(monster2);
            Game.AddSprite(poklon);
            Game.AddSprite(eksplozija1);
            Game.AddSprite(eksplozija2);
            Game.AddSprite(eksplozija3);
            Game.AddSprite(eksplozija4);
            Game.AddSprite(eksplozija5);


            //SKRIPTE
            Game.StartScript(PokreniCudoviste1);
            Game.StartScript(PokreniPauka1);
            Game.StartScript(PokreniPauka2);
            Game.StartScript(PokreniCudoviste2);
            Game.StartScript(ScoreTimer);
            Game.StartScript(Resetiraj);
            Game.StartScript(ResetriajReset);
           Game.StartScript(Pokret);
            Game.StartScript(BaciBombu);
            Game.StartScript(Eksplozija);
           Game.StartScript(UdaracCudovista);
           Game.StartScript(PostaviPoklonIOstaleKatastrofe);

          
          






        }

        /* Scripts */

        public bool resetiraj = false;
        public bool resetReset2 = false;
       
        public int Resetiraj()
        {

          
            while (true)
            {
                if (pisem == false)
                {
                    if (sensing.KeyPressed("R"))
                    {
                        
                        resetiraj = true;
                        //pričekamo malo da se stigne zaustaviti
                        Wait(0.5);
                      
                      
                        break;
                    }

                    Wait(0.01);
                }
            }
            pisem = false;
            ISPIS = "Loading... Please wait...";
            Wait(3);
           

            reset = true;
            VratiNaZadano();
            
            
            resetiraj = false;
            Wait(0.1);
            Game.StartScript(PokreniCudoviste1);
            Game.StartScript(PokreniPauka1);
            Game.StartScript(PokreniPauka2);
            Game.StartScript(PokreniCudoviste2);
            Wait(5);
            ISPIS = "";
            resetReset2 = true;
            return 0;
        }
        private int ResetriajReset()
        {
            while (true)
            {
                if (resetReset2 == true)
                {
                    Game.StartScript(Resetiraj);
                    resetReset2 = false;
                }
            }
            return 0;
        }

      
        public int PokreniPauka2()
        {
           
                prviput = true;
                while (START&&spider2.Ziv&&resetiraj==false)
                {
                    
                    spider2.SetHeading(-90);


                if (prviput == false)
                {
                    spider2.NextCostume();
                }
                else
                {
                    spider2.ChangeCostume(0);
                }
                    prviput = false;
                 
                    int koji = -1;
                    for (int i = 6; i >= 0; i--)
                    {

                        if (mapa[8, i] == 1)
                        {
                            koji = i;
                            break;
                        }


                    }

                    while (spider2.PozY - 1 != koji&&resetiraj==false)
                    {
                        if (ubrzanje == false)
                            Wait(0.5);
                        else
                            Wait(0.3);
                        spider2.MoveSteps(kamen.Width);
                        spider2.PozY -= 1;
                    }

               
                    spider2.SetHeading(90);

                    spider2.NextCostume();

                    while (spider2.PozY + 1 <= 12&&resetiraj==false)
                    {
                        if (ubrzanje == false)
                            Wait(0.5);
                        else
                            Wait(0.3);
                        spider2.MoveSteps(kamen.Width);
                        spider2.PozY += 1;
                    }

                }
            
            return 0;
        }

        public int PokreniPauka1()
        {
            
                prviput = true;
                while (START&&spider1.Ziv&&resetiraj==false)
                {
                  
                   
                    spider1.SetHeading(-90);

                    if (prviput == false)
                    {
                        spider1.NextCostume();
                    }
                else
                {
                    spider1.ChangeCostume(1);
                }
                prviput = false;
                   
                    int koji = -1;
                    for (int i = 7; i >=0 ; i--)
                    {

                        if (mapa[0, i] == 1)
                        {
                            koji = i;
                            break;
                        }
                       
                     
                    }
                   
                        while (spider1.PozY - 1 != koji&&resetiraj==false)
                        {
                            if (ubrzanje == false)
                                Wait(0.5);
                            else
                                Wait(0.3);
                            spider1.MoveSteps(kamen.Width);
                            spider1.PozY -= 1;
                        }

                    
                    spider1.SetHeading(90);

                    spider1.NextCostume();

                    if (mapa[0, 11] == 1)
                    {
                        while (spider1.PozY + 1 != 11&&resetiraj==false)
                        {
                            if (ubrzanje == false)
                                Wait(0.5);
                            else
                                Wait(0.3);
                            spider1.MoveSteps(kamen.Width);
                            spider1.PozY += 1;

                        }
                    }
                    else
                    {
                        while (spider1.PozY + 1 <13&&resetiraj==false)
                        {
                            if (ubrzanje == false)
                                Wait(0.5);
                            else
                                Wait(0.3);
                            spider1.MoveSteps(kamen.Width);
                            spider1.PozY += 1;

                        }
                    }
                 
                }
            
            return 0;
        }

        public int PokreniCudoviste1()
        {
                while (START&&monster1.Ziv&&resetiraj==false)
                {

                    if (monster1.Ziv == true)
                    {
                        monster1.SetHeading(180);


                        if (prviput == false)
                        {
                            monster1.NextCostume();
                        }
                    else
                    {
                        monster1.ChangeCostume(0);
                    }
                    prviput = false;

                        foreach (RoseBlock rb in roseBlock.roseKamenje)
                        {
                            if (rb.KoomX == 6 && rb.KoomY == 12&&mapa[6,12]==1)
                            {
                                while (!monster1.TouchingSprite(rb)&&resetiraj == false)
                                {
                                    if (ubrzanje == false)
                                        Wait(0.5);
                                    else
                                        Wait(0.3);
                                    monster1.MoveSteps(kamen.Width);
                                    monster1.PozX += 1;



                                }
                            }
                            if(rb.KoomX==6&&rb.KoomY==12&&mapa[6,12]==0)
                            {
                                while (monster1.PozX + 1 < 9&&resetiraj == false)
                                {
                                    if (ubrzanje == false)
                                        Wait(0.5);
                                    else
                                        Wait(0.3);
                                    monster1.MoveSimple(kamen.Width);

                                    monster1.PozX += 1;
                                }
                            }

                        }
                       
                        monster1.SetHeading(0);
                        monster1.NextCostume();
                        while (monster1.PozX - 1 != -1&&resetiraj == false)
                        {
                            if (ubrzanje == false)
                                Wait(0.5);
                            else
                                Wait(0.3);
                            monster1.MoveSimple(kamen.Width);
                            if (monster1.PozX != -1)
                                monster1.PozX -= 1;
                            
                        }


                    }

                }

            
            return 0;
        }

        public int PokreniCudoviste2()
        {
          
         

                prviput = true;
                while (START&&monster2.Ziv&&resetiraj==false)
                {


                    monster2.SetHeading(180);



                    if (prviput == false)
                    {
                        monster2.NextCostume();
                    }
                else
                {
                    monster2.ChangeCostume(0);
                }
                prviput = false;

                    while (monster2.PozX + 1<9&&resetiraj==false)
                    {
                        if (ubrzanje == false)
                            Wait(0.5);
                        else
                            Wait(0.3);
                        monster2.MoveSimple(kamen.Width);
                       
                        monster2.PozX += 1;
                    }
                    Wait(0.1);
                    monster2.SetHeading(0);
                    monster2.NextCostume();
                  
                    if (mapa[4, 0] == 1)
                    {
                        while (monster2.PozX - 1 != 4)
                        {
                            if (ubrzanje == false)
                                Wait(0.5);
                            else
                                Wait(0.3);
                            monster2.MoveSteps(kamen.Width);
                            monster2.PozX -= 1;
                           
                        }
                     
                    }
                    else
                    {
                       
                        while (monster2.PozX-1!=-1&&resetiraj==false)
                        {
                            if (ubrzanje == false)
                                Wait(0.5);
                            else
                                Wait(0.3);
                            monster2.MoveSimple(kamen.Width);
                            if(monster2.PozX!=-1)
                            monster2.PozX -= 1;
                       
                        }
                     
                    }
                }
            
            return 0;
        }
       

       


        private int Pokret()
        {

            while (START) 
            {
                if (bombergirl.Ziv == true && killCount != 4)
                {
                    if (sensing.KeyPressed("A"))
                    {
                        bombergirl.SetHeading(-90);
                        bombergirl.ChangeCostume(2);
                        bombergirl.Pogled = "lijevo";
                        if (MogucnostProlaska() == true)
                        {
                            bombergirl.PozY -= 1;
                            bombergirl.MoveSimple(kamen.Width);
                            Wait(0.01);
                            mapa[bombergirl.PozX, bombergirl.PozY + 1] = 0;
                        }

                        Wait(0.1);

                        //IspisMape();
                    }
                    if (sensing.KeyPressed("D"))
                    {
                        bombergirl.SetHeading(90);
                        bombergirl.ChangeCostume(3);             
                        bombergirl.Pogled = "desno";
                        if (MogucnostProlaska() == true)
                        {
                            bombergirl.PozY += 1;
                            bombergirl.MoveSimple(kamen.Width);
                            Wait(0.01);
                            mapa[bombergirl.PozX, bombergirl.PozY - 1] = 0;

                        }

                        Wait(0.1);

                        //IspisMape();
                    }
                    if (sensing.KeyPressed("W"))
                    {

                        bombergirl.SetHeading(0);
                        bombergirl.ChangeCostume(1);
                        bombergirl.Pogled = "gore";
                        if (MogucnostProlaska() == true)
                        {
                            bombergirl.PozX -= 1;
                            bombergirl.MoveSimple(kamen.Width);
                            Wait(0.01);
                            mapa[bombergirl.PozX + 1, bombergirl.PozY] = 0;
                        }

                        Wait(0.1);

                        //IspisMape();
                    }
                    if (sensing.KeyPressed("S"))
                    {
                        bombergirl.SetHeading(180);
                        bombergirl.ChangeCostume(0);
                        bombergirl.Pogled = "dolje";
                        if (MogucnostProlaska() == true)
                        {
                            bombergirl.PozX += 1;
                            bombergirl.MoveSimple(kamen.Width);
                            Wait(0.01);
                            mapa[bombergirl.PozX - 1, bombergirl.PozY] = 0;
                        }

                        Wait(0.1);

                        //IspisMape();
                    }

                }
                Wait(0.01);

            }
            return 0;
        }

      
        private int BaciBombu()
        {


            while (START)
            {
                if (bombergirl.Ziv&&killCount!=4)
                {
                    if (sensing.KeyPressed(Keys.Space))
                    {
                        
                        if (bomba.Koristise == false)
                        {
                            Wait(0.01);

                            
                            bomba.Koristise = true;
                            bomba.PozicijaX = bombergirl.PozX;
                            bomba.PozicijaY = bombergirl.PozY;
                            bomba.X = bombergirl.X;
                            bomba.Y = bombergirl.Y;


                        }
                    }

                   
                }

            }
            return 0;
        }


        private int Eksplozija()
        {
            while (START)
            {
                if (bombergirl.Ziv == true&&killCount!=4)
                {
                    if (bomba.Koristise == true)
                    {

                        Wait(2);

                        eksplozija1.X = kamen.X * bomba.PozicijaY;
                        eksplozija1.Y = kamen.Y * bomba.PozicijaX;
                        if (bomba.PozicijaX + 1 < 9 && mapa[bomba.PozicijaX + 1, bomba.PozicijaY] == 0)
                        {
                            eksplozija2.X = kamen.X * bomba.PozicijaY;
                            eksplozija2.Y = kamen.Y * (bomba.PozicijaX + 1);
                        }
                        if (bomba.PozicijaX - 1 >= 0 && mapa[bomba.PozicijaX - 1, bomba.PozicijaY] == 0)
                        {
                            eksplozija3.X = kamen.X * bomba.PozicijaY;
                            eksplozija3.Y = kamen.Y * (bomba.PozicijaX - 1);
                        }
                        if (bomba.PozicijaY + 1 < 12 && mapa[bomba.PozicijaX, bomba.PozicijaY + 1] == 0)
                        {
                            eksplozija4.X = kamen.X * (bomba.PozicijaY + 1);
                            eksplozija4.Y = kamen.Y * bomba.PozicijaX;
                        }
                        if (bomba.PozicijaY - 1 >= 0 && mapa[bomba.PozicijaX, bomba.PozicijaY - 1] == 0)
                        {
                            eksplozija5.X = kamen.X * (bomba.PozicijaY - 1);
                            eksplozija5.Y = kamen.Y * bomba.PozicijaX;
                        }
                        bomba.X = -100;
                        bomba.Y = -100;

                        Wait(0.2);

                        _rusi.Invoke(bomba);
                     
                        eksplozija1.X = -100;
                        eksplozija2.X = -100;
                        eksplozija3.X = -100;
                        eksplozija4.X = -100;
                        eksplozija5.X = -100;
                       
                        bomba.Koristise = false;

                    }
                }
               
            }
                return 0;
        }
        public void MetodaIspisa(Ispisivanje isp,int i,string s)
        {
            isp(i, s);
        }
        public void PopuniMiniMapu()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    if (i % 2 != 0 && j % 2 != 0)
                    {
                        mapa[i, j] = 2;
                    }
                    else
                    {
                        mapa[i, j] = 0;
                    }
                }
            }

            foreach (RoseBlock rb in roseBlock.roseKamenje)
            {
                mapa[rb.KoomX, rb.KoomY] = 1;
            }

            bombergirl.PozX = 0;
            bombergirl.PozY = 0;

            mapa[bombergirl.PozX, bombergirl.PozY] = 6;


        }

            private bool MogucnostProlaska()
            {
                string gdje = bombergirl.Pogled;
                if (gdje == "gore")
                {
                    if ((bombergirl.PozX - 1) < 0)
                    {
                        return false;
                    }
                    if (mapa[bombergirl.PozX - 1, bombergirl.PozY] != 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
                if (gdje == "dolje")
                {
                    if (bombergirl.PozX + 1 >= 9)
                    {
                        return false;
                    }

                    if (mapa[bombergirl.PozX + 1, bombergirl.PozY] != 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
                if (gdje == "desno")
                {
                    if (bombergirl.PozY + 1 > 12)
                    {
                        return false;
                    }
                    if (mapa[bombergirl.PozX, bombergirl.PozY + 1] != 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
                else
                {
                    if ((bombergirl.PozY - 1) < 0)
                    {
                        return false;
                    }
                    if (mapa[bombergirl.PozX, bombergirl.PozY - 1] != 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
               

            }

            private void Rusenje(Bomba bum)
            {
                int gornji;
                int donji;
                int lijevi;
                int desni;


                if (bum.PozicijaX - 1 >= 0)
                    gornji = mapa[bum.PozicijaX - 1, bum.PozicijaY];
                else
                    gornji = -1;
                if (bum.PozicijaX + 1 <= 8)
                    donji = mapa[bum.PozicijaX + 1, bum.PozicijaY];
                else
                    donji = -1;
                if (bum.PozicijaY - 1 >= 0)
                    lijevi = mapa[bum.PozicijaX, bum.PozicijaY - 1];
                else
                    lijevi = -1;
                if (bum.PozicijaY + 1 <= 12)
                    desni = mapa[bum.PozicijaX, bum.PozicijaY + 1];
                else
                    desni = -1;


  
                if (gornji == 1)
                {
                    mapa[bum.PozicijaX - 1, bum.PozicijaY] = 0;
                    foreach (RoseBlock rb in roseBlock.roseKamenje)
                    {
                        if (rb.KoomX == bum.PozicijaX - 1 && rb.KoomY == bum.PozicijaY)
                        {
                            rb.SetVisible(false);

                        }
                    }
                }
                if (donji == 1)
                {
                    mapa[bum.PozicijaX + 1, bum.PozicijaY] = 0;
                    foreach (RoseBlock rb in roseBlock.roseKamenje)
                    {
                        if (rb.KoomX == bum.PozicijaX + 1 && rb.KoomY == bum.PozicijaY)
                        {
                            rb.SetVisible(false);

                        }
                    }
                }
                if (lijevi == 1)
                {
                    mapa[bum.PozicijaX, bum.PozicijaY - 1] = 0;
                    foreach (RoseBlock rb in roseBlock.roseKamenje)
                    {
                        if (rb.KoomX == bum.PozicijaX && rb.KoomY == bum.PozicijaY - 1)
                        {
                            rb.SetVisible(false);

                        }
                    }
                }
                if (desni == 1)
                {
                    mapa[bum.PozicijaX, bum.PozicijaY + 1] = 0;
                    foreach (RoseBlock rb in roseBlock.roseKamenje)
                    {
                        if (rb.KoomX == bum.PozicijaX && rb.KoomY == bum.PozicijaY + 1)
                        {
                            rb.SetVisible(false);

                        }
                    }
                }

                if (gornji == 0 && poklon.Pokupljen)
                {
                    if (bum.PozicijaX - 2 >= 0)
                    {
                        gornji = mapa[bum.PozicijaX - 2, bum.PozicijaY];
                        if (gornji == 1)
                        {
                            mapa[bum.PozicijaX - 2, bum.PozicijaY] = 0;
                            foreach (RoseBlock rb in roseBlock.roseKamenje)
                            {
                                if (rb.KoomX == bum.PozicijaX - 2 && rb.KoomY == bum.PozicijaY)
                                {
                                    rb.SetVisible(false);

                                }
                            }


                            poklon.Pokupljen = false;
                        }
                    }
                }
                if (donji == 0  && poklon.Pokupljen)
                {
                    if (bum.PozicijaX + 2 <= 8)
                    {
                        donji = mapa[bum.PozicijaX + 2, bum.PozicijaY];
                        if (donji == 1)
                        {
                            mapa[bum.PozicijaX + 2, bum.PozicijaY] = 0;
                            foreach (RoseBlock rb in roseBlock.roseKamenje)
                            {
                                if (rb.KoomX == bum.PozicijaX + 2 && rb.KoomY == bum.PozicijaY)
                                {
                                    rb.SetVisible(false);

                                }
                            }


                            poklon.Pokupljen = false;
                        }
                    }
                }
                if (lijevi == 0  && poklon.Pokupljen)
                {
                    if (bum.PozicijaY - 2 >= 0)
                    {
                        lijevi = mapa[bum.PozicijaX, bum.PozicijaY - 2];
                        if (lijevi == 1)
                        {
                            mapa[bum.PozicijaX, bum.PozicijaY - 2] = 0;
                            foreach (RoseBlock rb in roseBlock.roseKamenje)
                            {
                                if (rb.KoomX == bum.PozicijaX && rb.KoomY == bum.PozicijaY - 2)
                                {
                                    rb.SetVisible(false);

                                }
                            }


                            poklon.Pokupljen = false;
                        }
                    }
                }
                if (desni == 0 && poklon.Pokupljen)
                {
                    if (bum.PozicijaY + 2 <= 12)
                    {
                        desni = mapa[bum.PozicijaX, bum.PozicijaY + 2];
                        if (desni == 1)
                        {
                            mapa[bum.PozicijaX, bum.PozicijaY + 2] = 0;
                            foreach (RoseBlock rb in roseBlock.roseKamenje)
                            {
                                if (rb.KoomX == bum.PozicijaX - 1 && rb.KoomY == bum.PozicijaY + 2)
                                {
                                    rb.SetVisible(false);

                                }
                            }


                            poklon.Pokupljen = false;
                        }
                    }
                }
            }

            public void IspisMape()
            {
                ISPIS = "";
                mapa[bombergirl.PozX, bombergirl.PozY] = 6;
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        ISPIS += mapa[i, j].ToString() + " ";
                    }
                    ISPIS += "\n";
                }
                ISPIS += bombergirl.PozX.ToString() + " " + bombergirl.PozY.ToString();
            }

            private void UbijCudoviste(Bomba bum)
            {
                foreach (Enemy e in cudovista)
                {
                    if (e.Ziv == true)
                    {

                        if (bum.PozicijaX == e.PozX)
                        {
                            if (bum.PozicijaY == e.PozY)
                            {
                            
                                e.SetVisible(false);
                                e.Ziv = false;
                                killCount += 1;
                            

                            }
                            if (bum.PozicijaY - 1 >= 0)
                            {
                                if (bum.PozicijaY - 1 == e.PozY)
                                {
                               
                                e.SetVisible(false);
                                    e.Ziv = false;
                                    killCount += 1;
                                

                                }
                            }
                            if (bum.PozicijaY + 1 <= 12)
                            {
                                if (bum.PozicijaY + 1 == e.PozY)
                                {
                               
                                e.SetVisible(false);
                                    e.Ziv = false;
                                    killCount += 1;
                                
                                }
                            }
                        }
                        if (bum.PozicijaX + 1 <= 8)
                        {
                            if (bum.PozicijaX + 1 == e.PozX)
                            {
                                if (bum.PozicijaY == e.PozY)
                                {
                               
                                e.SetVisible(false);
                                    e.Ziv = false;
                                    killCount += 1;
                                
                                 }
                            }
                        }
                        if (bum.PozicijaX - 1 >= 0)
                        {
                            if (bum.PozicijaX - 1 == e.PozX)
                            {
                                if (bum.PozicijaY == e.PozY)
                                {
                            
                                e.SetVisible(false);
                                    e.Ziv = false;
                                    killCount += 1;
                              
                                }
                            }
                        }
                        if (poklon.Pokupljen)
                        {
                            if (bum.PozicijaX == e.PozX)
                            {

                                if (bum.PozicijaY - 2 >= 0)
                                {
                                    if (bum.PozicijaY - 2 == e.PozY)
                                    {
                                        e.SetVisible(false);
                                        e.Ziv = false;
                                        killCount += 1;
                                  

                                     }
                                }
                                if (bum.PozicijaY + 2 <= 12)
                                {
                                    if (bum.PozicijaY + 2 == e.PozY)
                                    {
                                        e.SetVisible(false);
                                        e.Ziv = false;
                                        killCount += 1;
                                   
                                     }
                                }
                            }
                            if (bum.PozicijaX + 2 <= 8)
                            {
                                if (bum.PozicijaX + 2 == e.PozX)
                                {
                                    if (bum.PozicijaY == e.PozY)
                                    {
                                        e.SetVisible(false);
                                        e.Ziv = false;
                                        killCount += 1;

                                    }
                                }
                            }
                            if (bum.PozicijaX - 2 >= 0)
                            {
                                if (bum.PozicijaX - 2 == e.PozX)
                                {
                                    if (bum.PozicijaY == e.PozY)
                                    {
                                        e.SetVisible(false);
                                        e.Ziv = false;
                                        killCount += 1;
                                  
                                    }
                                }
                            }
                        }

                    }
                }

            }

            public void VratiNaZadano()
            {
                        reset = true;
                        ISPIS = "";
                    
                        bomba.X = -100;
                        bomba.Y= -100;
                        for (int i = 0; i < 9; i++)
                        {
                            for (int j = 0; j < 13; j++)
                            {
                                mapa[i, j] = 0;
                            }
                        }
                        bombergirl.ChangeCostume(0);
                        bombergirl.Ziv = true;
                        bombergirl.Playable = true;
                        bombergirl.X = 0;
                        bombergirl.Y = 0;
                        killCount = 0;
                        poklon.X = -100;
                        poklon.Y = -100;
                        ubrzanje = false;
                        poklon.Pokupljen = false;

                monster1.X = 12 * kamen.Width;
                monster1.Y = 0;
                monster1.SetHeading(180);
                spider1.X = 10 * kamen.Width;
                spider1.Y = 0;
                spider1.SetHeading(-90);
                monster2.Y = 5 * kamen.Width;
                monster2.X = 1;
                monster2.SetHeading(180);
                spider2.X = 12 * kamen.Width;
                spider2.Y = 8 * kamen.Width;
                spider2.SetHeading(-90);
                monster1.PozX = 0;
                monster1.PozY = 12;
                monster2.PozX = 5;
                monster2.PozY = 0;
                spider1.PozX = 0;
                spider1.PozY = 10;
                spider2.PozX = 8;
                spider2.PozY = 12;
                foreach (Enemy e in cudovista)
                {
                            e.SetVisible(true);
                            e.Ziv = true;

                }
                        foreach (RoseBlock rb in roseBlock.roseKamenje)
                        {
                            rb.SetVisible(true);
                        }
                        PopuniMiniMapu();

                    
               
            }

            private void BumKrajBomberGirl(Bomba bum)
            {
                

                if (bombergirl.PozX == bum.PozicijaX + 1 && bombergirl.PozY == bum.PozicijaY)
                {
                    bombergirl.ChangeCostume(4);
                    bombergirl.Ziv = false;
                    ISPIS = "Pritisnite R za restart!";
                    Wait(0.1);
                 

                }

                if (bombergirl.PozX == bum.PozicijaX - 1 && bombergirl.PozY == bum.PozicijaY)
                {
                    bombergirl.ChangeCostume(4);
                    bombergirl.Ziv = false;
                    ISPIS = "Pritisnite R za restart!";
                    Wait(0.1);
                   

                }

                if (bombergirl.PozY == bum.PozicijaY + 1 && bombergirl.PozX == bum.PozicijaX)
                {
                    bombergirl.ChangeCostume(4);
                    bombergirl.Ziv = false;
                    ISPIS = "Pritisnite R za restart!";
                    Wait(0.1);
                 

                }

                if (bombergirl.PozY == bum.PozicijaY - 1 && bombergirl.PozX == bum.PozicijaX)
                {
                    bombergirl.ChangeCostume(4);
                    bombergirl.Ziv = false;
                    ISPIS = "Pritisnite R za restart!";
                    Wait(0.1);
                   

                }
                if (bombergirl.PozX == bum.PozicijaX && bombergirl.PozY == bum.PozicijaY)
                {
                    bombergirl.ChangeCostume(4);
                    bombergirl.Ziv = false;
                    Wait(0.1);
              

                }


            }

            private int UdaracCudovista()
            {
                while (START)
                {
                    foreach (Enemy e in cudovista)
                    {
                       
                        if (e.PozX == bombergirl.PozX && e.PozY == bombergirl.PozY && e.Ziv)
                        {
                            bombergirl.Ziv = false;
                            bombergirl.ChangeCostume(4);
                            ISPIS = "Pritisnite R za restart!";

                        }
                    }
                }
                return 0;
            }

            //public int GameOver()
            //{
            //    while (true)
            //    {
                  


            //        if (bombergirl.Ziv == false)
            //        {

            //            ISPIS = "Pritisnite R za restart";
            //            Wait(1);
                 
            //        }

            //    }
            //    return 0;
            //}

        public int PostaviPoklonIOstaleKatastrofe()
        {
            bool prviPut = true;

            while (true)
            {

                if (killCount == 3 && poklon.Pokupljen == false && prviPut == true)
                {
                    poklon.X = kamen.X * 5 + kamen.Width;
                    poklon.Y = kamen.Y * 4 + kamen.Width;
                    poklon.PozicijaX = 5;
                    poklon.PozicijaY = 6;



                }

                if (bombergirl.PozX == poklon.PozicijaX && bombergirl.PozY == poklon.PozicijaY)
                {

                    poklon.Pokupljen = true;
                    poklon.X = -100;
                    poklon.Y = -200;
                    prviPut = false;
                    ubrzanje = true;


                }
               


            }
                return 0;
        }
        

        public int ScoreTimer()
        {

            while (START)
            {
                int vrime = 0;
                
                while (bombergirl.Playable&&START)
                {
                    
                    if (reset == true)
                    {
                        reset = false;
                        vrime = 0;

                    }


                    Wait(1);
                    vrime++;
                    //ISPIS += vrime.ToString()+"\n";

                    if (killCount == 4&&bombergirl.Ziv)
                    {
                        pisem = true;
                        IspisListe(vrime);
                        
                        bombergirl.Playable= false;

                        ISPIS += "Stisnite R za restart!";
                        pisem = false;
                        break;                    
                    }

                    
                   
                }

            }
            return 0;
        }

        public void IspisListe(int vrime)
        {
            bool tp = true;
            if (killCount == 4)
            {
                Wait(0.1);
                ISPIS = "Unesite ime: ";
                string s = "";

                while (tp&&START)
                {
                    if (sensing.KeyPressed(Keys.Back)&&s!="")
                    {
                        string temp = "";
                        for (int i = 0; i < s.Length-1; i++)
                        {
                            temp += s[i];

                        }
                        s = temp;
                        Wait(0.1);
                        ISPIS ="Unesite ime: "+ s;
                    }
                    else
                    s += UnosImena();
                    Wait(0.01);

                    if (sensing.KeyPressed(Keys.Enter))
                    {

                       
                        MetodaIspisa(_pisi, vrime, s);
                        
                        tp=false;
                      
                        
                    }
                  
                }
                




            }
        }

        public void Citaj(int it,string st)
        {
           ISPIS="";
            using (StreamReader sr = new StreamReader("leaderboard.txt"))
            {

                string[] niz;
                ImeIgraca ii;
                List<ImeIgraca> ig1 = new List<ImeIgraca>();
                ImeIgraca[] igraci = new ImeIgraca[10];


                string line = sr.ReadLine();
                while (line != null)
                {
                    niz = line.Split('#');
                    int q = int.Parse(niz[0]);
                    ii = new ImeIgraca(q, niz[1]);


                    ig1.Add(ii);


                    line = sr.ReadLine();
                }



                for (int i = 0; i < 10; i++)
                {
                    if (i < ig1.Count)
                    {
                        igraci[i] = ig1[i];
                    }
                    else
                        igraci[i] = new ImeIgraca(1000, "");
                }






                for (int i = 0; i < 10; i++)
                {
                    for (int j = i + 1; j < 10; j++)
                    {

                        if (igraci[i].Bodovi > igraci[j].Bodovi)
                        {
                            ImeIgraca temp = igraci[i];
                            igraci[i] = igraci[j];
                            igraci[j] = temp;
                        }

                    }
                }

                for (int i = 0; i <10; i++)
                {
                    if (igraci[i].Bodovi != 1000)
                        ISPIS+=(i + 1).ToString() + ". " + igraci[i].Ime + " (" + igraci[i].Bodovi.ToString() + ")\n";
                    else
                    {
                       ISPIS+=(i+1).ToString() + ". ---"+"\n";
                    }
                }

                ISPIS += "Rezltat igrača " + st + " je: " + it.ToString() + "\n";
            }
        }

        public void PocetniUnosScorea()
        {
            using (StreamReader sr = new StreamReader("leaderboard.txt"))
            {

                string[] niz;
                ImeIgraca ii;
          


                string line = sr.ReadLine();
                while (line != null)
                {
                    niz = line.Split('#');
                    int q = int.Parse(niz[0]);
                    ii = new ImeIgraca(q, niz[1]);


                    players.Add(ii);


                    line = sr.ReadLine();
                }

            }

        }

        public void Pisi(int vr,string s)
        {

                ImeIgraca   ii = new ImeIgraca(vr, s);
            if (players.Count < 10)
            {
                players.Add(ii);
            }
            else
            {
                int br = 0;
                int temp = players[0].Bodovi;
                foreach (ImeIgraca i in players)
                {
                    
                    if (temp < i.Bodovi)
                    {
                        players.RemoveAt(br);
                        players.Add(ii);
                        break;
                    }
                    br++;
                }
            }
            using (StreamWriter sw = new StreamWriter("leaderboard.txt"))
            {

                foreach(ImeIgraca i in players)
                {
                    sw.WriteLine(i.Bodovi.ToString() + "#" + i.Ime);
                }
            }
        }

        private string UnosImena()
            {
                String[] znakovi = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "V", "Z","Q","W","Y","X" };
           
                foreach (string s in znakovi)
                {
               
                    if (sensing.KeyPressed(s))
                    {
                        ISPIS += s;
                        Wait(0.05);
                        return s;
                    }
                }
                return "";
            }
            /* ------------ GAME CODE END ------------ */


      

        }
    } 
