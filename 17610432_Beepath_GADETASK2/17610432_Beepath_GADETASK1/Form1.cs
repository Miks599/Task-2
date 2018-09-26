using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace _17610432_Beepath_GADETASK1
{

    public partial class Form1 : Form
    {
        Timer tTimer = new Timer(); //https://stackoverflow.com/questions/12535722/what-is-the-best-way-to-implement-a-timer
        int iTimer = 0;

        Map Field;

        ResourceBuilding ResourceBuildings = new ResourceBuilding();
        FactoryBuilding FactoryBuildings;

        //public static string[,] arrNewMap = new string[20, 20];

            /*public TextBox TbInformation
            {
                get;
                set;
            }*/
            //https://www.dotnetperls.com/property
            //https://stackoverflow.com/questions/5096926/what-is-the-get-set-syntax-in-c
            /*public TableLayoutPanel TlpMap
            {
                get;
                set;
            }*/

            public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Field = new Map(tlpMap, this);
            FactoryBuildings = new FactoryBuilding(Field);

            tTimer.Interval = 1000; //1 second
            tTimer.Tick += new EventHandler(this.TimerTick);

            
            Field.GenerateField();

            //for (int a = 0; a < 20; a++)
            //{
            //    for (int b = 0; b < 20; b++)
            //    {
            //        arrNewMap[a, b] = Field.arrMap[a, b];
            //    }   //endfor
            //}   //endfor
        }

        public void NoUnitClick(object sender, EventArgs e)   //https://stackoverflow.com/questions/14479143/what-is-the-use-of-object-sender-and-eventargs-e-parameters
        {
            tbInformation.Text = "No Unit in the Block";
        }

        public void MeleeUnitClick(object sender, EventArgs e)
        {
            tbInformation.Text = "Melee Unit";
        }

        public void RangedUnitClick(object sender, EventArgs e)
        {
            tbInformation.Text = "Ranged Unit";
        }

        public void ResourceBuildingClick(object sender, EventArgs e)
        {
            tbInformation.Text = "Resource Building";
        }

        public void FactoryBuildingClick(object sender, EventArgs e)
        {
            tbInformation.Text = "Factory Building";
        }

        private void TimerTick(object sender, EventArgs e)
        {
            lblGameTime.Text = "";
            iTimer++;
            lblGameTime.Text += iTimer.ToString();

            ResourceBuildings.GenerateResources();
            lblResourceCount.Text = ResourceBuildings.IResourcesRemaining.ToString();

            if (iTimer == 20)
            {
                FactoryBuildings.SpawnNewUnits();
            }   //endif
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            tTimer.Start();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            tTimer.Stop();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //string Filename = @"D:\\BuildingArray.txt";

            //using (var sw = new StreamWriter(Filename))
            //{
            //    for (int i = 0; i < 4; i++)
            //    {
            //        for (int j = 0; j < 3; j++)
            //        {
            //            sw.Write(arrNewMap[i, j] + " ");
            //        }
            //        sw.Write("\n");
            //    }
            //}

            Field.Save();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            Field.Read();
        }
    }


    abstract class Building
    {
        protected int iXPosition;
        protected int iYPosition;
        protected int iBuildingHealth;
        protected string sTeam;
        protected string sSymbol;

        public Building()
        {

        }

        public abstract void Destruction(); //when building hp hits 0
        //public abstract bool Destruction(bool bTrueFalse);

        public abstract void ToString();    //returns all building info

        public abstract void Save();
    }

    class ResourceBuilding : Building   //Building that generates resources
    {
        private string sResourceType = "Stars";
        private int iResourcesPerGameTick = 2;
        private int iResourcesRemaining = 0;

        protected int iXPosition;
        protected int iYPosition;
        protected int iBuildingHealth = 1000;
        protected string sTeam;
        protected string sSymbol;
        
        private bool isDestroyed;

        public ResourceBuilding()
        {
            sSymbol = "*";
            iXPosition = 0;
            iYPosition = 0;
        }

        public override void Destruction()
        {
            if (iBuildingHealth <= 0)
            {
                isDestroyed = true;
            }   //endif
        }
        //public override bool Destruction(bool bTrueFalse)
        //{
        //    return bTrueFalse = false;
        //}

        public override void ToString()
        {

        }

        public void GenerateResources() //and remove resources from the remaining amount of resources
        {
            if (iBuildingHealth > 0)
            {
                iResourcesRemaining = iResourcesRemaining + iResourcesPerGameTick;
            }   //endif

            if (iResourcesRemaining == 20)
            {
                iResourcesRemaining = iResourcesRemaining - 20;
            }   //endif
        }

        public override void Save()
        {
            
        }

        //public string SResourceType
        //{
        //    get;
        //    set;
        //}
        //public int IResourcesPerGameTick
        //{
        //    get;
        //    set;
        //}
        public int IResourcesRemaining
        {
            get { return iResourcesRemaining; }
            set { iResourcesRemaining = value; }
        }
    }

    class FactoryBuilding : Building    //Building that generates units
    {
        protected Map Field;

        private string sUnitsToProduce;
        private int iGameTicksPerProduction = 0;
        private int iSpawnPoint;

        protected int iXPosition;
        protected int iYPosition;
        protected int iBuildingHealth = 1000;
        protected string sTeam;
        protected string sSymbol;
        
        private bool isDestroyed;

        public FactoryBuilding(Map Field)
        {
            sSymbol = "F";
            iXPosition = 1;
            iYPosition = 1;
            this.Field = Field;
        }

        public override void Destruction()
        {
            if (iBuildingHealth <= 0)
            {
                isDestroyed = true;
            }   //endif
        }
        //public override bool Destruction(bool bTrueFalse)
        //{
        //    return bTrueFalse = false;
        //}

        public override void ToString()
        {

        }

        public void SpawnNewUnits() //spawns units at the correct interval
        {
            iGameTicksPerProduction = iGameTicksPerProduction + 1;

            

            while (isDestroyed == false)
            {
                if (iGameTicksPerProduction % 20 == 0)
                {
                    Field.CreateNewUnit();
                }   //endif
            }   //endwhile
        }

        public override void Save()
        {

        }

        //public string SUnitsToProduce
        //{
        //    get;
        //    set;
        //}
        //public int IGameTicksPerProduction
        //{
        //    get;
        //    set;
        //}
        //public int ISpawnPoint
        //{
        //    get;
        //    set;
        //}
    }

    abstract class Unit
    {
        private int iXPos;
        private int iYPos;
        //check the index of where the symbol is in the array
        private int iHealth;
        private int iSpeed;
        private int iAttack;
        private int iAttackRange;

        private string sTeam;
        private string sSymbol;

        protected string sTypeName; //This will store the name of the type of unit
        public Unit()
        {
            //A constructor
        }

        public virtual void NewPostion()
        {
            //A method to move to a new position
        }

        public virtual void CombatHandling()
        {
            //A method to handle combat with another unit
        }

        public virtual void CheckAttackRange()
        {
            //A method to determine whether another unit is within attack range
        }

        public virtual void PosClosestUnit()
        {
            //A method to return position of the closest other unit to this unit
        }

        public virtual void Death()
        {
            //A method to handle the death/ destruction of this unit
        }

        public virtual void ToString()  //https://www.dotnetperls.com/virtual
        {
            //Returns a neatly formatted string showing all the unit information
        }
        public abstract void Save();
    }

    class MeleeUnit : Unit  //unit that can only attack other units directly next to it
    {
        Map map;

        private int iXPos;
        private int iYPos;
        private int iHealth = 100;
        private int iSpeed = 2;
        private int iAttack = 4;
        private int iAttackRange = 1;

        private string sTeam;
        private string sSymbol;

        bool isDead;

        public MeleeUnit(Map map)
        {
            this.map = map;
            sSymbol = "1";
        }

        public override void NewPostion()
        {

        }

        public override void CombatHandling()
        {
            
        }

        public override void CheckAttackRange()
        {

        }

        public override void PosClosestUnit()
        {

        }

        public override void Death()
        {
            if (iHealth <= 0)
            {
                isDead = true;
            }   //endif
        }

        public override void ToString()
        {

        }

        public override void Save()
        {

        }

        public int IXPos
        {
            get;
            set;
        }
        public int IYPos
        {
            get;
            set;
        }
        public int IHealth
        {
            get;
            set;
        }
        public int ISpeed
        {
            get;
            set;
        }
        public int IAttack
        {
            get;
            set;
        }
        public int IAttackRange
        {
            get;
            set;
        }
        public string STeam
        {
            get;
            set;
        }
        public string SSymbol
        {
            get;
            set;
        }
    }

    class RangedUnit : Unit  //unit that can only attack other units up to its attack range
    {
        Map map;

        private int iXPos;
        private int iYPos;
        private int iHealth = 60;
        private int iSpeed = 4;
        private int iAttack = 2;
        private int iAttackRange = 3;

        private string sTeam;
        private string sSymbol;

        bool isDead;

        public RangedUnit(Map map)
        {
            this.map = map;
            sSymbol = "2";
        }

        public override void NewPostion()
        {

        }

        public override void CombatHandling()
        {

        }

        public override void CheckAttackRange()
        {

        }

        public override void PosClosestUnit()
        {

        }

        public override void Death()
        {
            if (iHealth <= 0)
            {
                isDead = true;
            }   //endif
        }

        public override void ToString()
        {

        }

        public override void Save()
        {

        }

        public int IXPos
        {
            get;
            set;
        }
        public int IYPos
        {
            get;
            set;
        }
        public int IHealth
        {
            get;
            set;
        }
        public int ISpeed
        {
            get;
            set;
        }
        public int IAttack
        {
            get;
            set;
        }
        public int IAttackRange
        {
            get;
            set;
        }
        public string STeam
        {
            get;
            set;
        }
        public string SSymbol
        {
            get;
            set;
        }
    }

    class Map
    {
        public string[,] arrMap = new string[20, 20];
        private Form1 frm1;
        TableLayoutPanel tlpMap;

        /*public void NoUnitClick(object sender, EventArgs e)   //https://stackoverflow.com/questions/14479143/what-is-the-use-of-object-sender-and-eventargs-e-parameters
        {
            frm1.TbInformation.Text = "No Unit in the Block";
        }*/

        public Map(TableLayoutPanel tlpMap, Form1 frm1)
        {
            this.tlpMap = tlpMap;
            this.frm1 = frm1;
            for (int a = 0; a < 20; a++)
            {
                for (int b = 0; b < 20; b++)
                {
                    arrMap[a, b] = " ";
                }   //endfor
            }   //endfor
        }   //gives each item a default value in the array

        public void GenerateBattleField()
        {
            //MeleeUnit mUnit = new MeleeUnit(this);
            //RangedUnit rUnit = new RangedUnit(this);

            //ResourceBuilding StarMaker = new ResourceBuilding();
            arrMap[0, 0] = "*"; //'*' is a Resource Building
            arrMap[1, 1] = "F"; //'F' is a Factory Building
            //arrMap[1, 1] = "R"; //'R' is a Factory Building (ranged unit production)
            //arrMap[2, 2] = "M"; //'M' is a Factory Building (melee unit production)

            Random RandomNumberX = new Random(Guid.NewGuid().GetHashCode());
            Random RandomNumberY = new Random(Guid.NewGuid().GetHashCode());

            Random TrueFalse = new Random(Guid.NewGuid().GetHashCode());

            for (int a = 0; a < 20; a++)
            {
                int X = RandomNumberX.Next(3, 20);
                int Y = RandomNumberY.Next(3, 20);
                //units are not generated in 
                //arrMap[0, 0], arrMap[0, 1], arrMap[0, 2], arrMap[1, 0], arrMap[2, 0], arrMap[1, 1], and arrMap[2, 2]
                //to leave space for the Resource Building, Factory Building and the units the Factory Building produces

                int TF = TrueFalse.Next(1, 3);
                if (TF == 1)
                {
                    arrMap[X, Y] = "m";
                    //mUnit.IXPos = X;
                    //mUnit.IYPos = Y;
                }   //endif
                else if (TF == 2)
                {
                    arrMap[X, Y] = "r";
                    //rUnit.IXPos = X;
                    //rUnit.IYPos = Y;
                }   //endelseif
                //else
                //{

                //}
            }   //endfor        //randomly adds random units to the array
        }

        public void GenerateField()
        {
            tlpMap.Controls.Clear();

            //Map Board = new Map(this);
            GenerateBattleField();

            for (int a = 0; a < 20; a++)
            {
                for (int b = 0; b < 20; b++)
                {
                    Label lbl = new Label();                       //https://www.c-sharpcorner.com/uploadfile/mahesh/label-in-C-Sharp/

                    if (arrMap[a, b] == "*")
                    {
                        lbl.Name = (a + 1).ToString() + "|" + (b + 1).ToString();
                        lbl.Text = "*";     //Resource Building
                        lbl.Font = new Font(lbl.Font.Name, 12, FontStyle.Bold);

                        lbl.Width = 40;
                        lbl.Height = 40;
                        lbl.TextAlign = ContentAlignment.MiddleCenter;

                        lbl.Click += new EventHandler(frm1.ResourceBuildingClick);

                        tlpMap.Controls.Add(lbl);
                    }   //endif
                    if (arrMap[a, b] == "F")
                    {
                        lbl.Name = (a + 1).ToString() + "|" + (b + 1).ToString();
                        lbl.Text = "F";     //Factory Building
                        lbl.Font = new Font(lbl.Font.Name, 12, FontStyle.Bold);

                        lbl.Width = 40;
                        lbl.Height = 40;
                        lbl.TextAlign = ContentAlignment.MiddleCenter;

                        lbl.Click += new EventHandler(frm1.FactoryBuildingClick);

                        tlpMap.Controls.Add(lbl);
                    }   //endif
                    //if (arrMap[a, b] == "R")
                    //{
                    //    lbl.Name = (a + 1).ToString() + "|" + (b + 1).ToString();
                    //    lbl.Text = "R";     //Factory Building
                    //    lbl.Font = new Font(lbl.Font.Name, 12, FontStyle.Bold);

                    //    lbl.Width = 40;
                    //    lbl.Height = 40;
                    //    lbl.TextAlign = ContentAlignment.MiddleCenter;

                    //    lbl.Click += new EventHandler(frm1.FactoryBuildingClick);

                    //    tlpMap.Controls.Add(lbl);
                    //}   //endif
                    //if (arrMap[a, b] == "M")
                    //{
                    //    lbl.Name = (a + 1).ToString() + "|" + (b + 1).ToString();
                    //    lbl.Text = "M";     //Factory Building
                    //    lbl.Font = new Font(lbl.Font.Name, 12, FontStyle.Bold);

                    //    lbl.Width = 40;
                    //    lbl.Height = 40;
                    //    lbl.TextAlign = ContentAlignment.MiddleCenter;

                    //    lbl.Click += new EventHandler(frm1.FactoryBuildingClick);

                    //    tlpMap.Controls.Add(lbl);
                    //}   //endif
                    if (arrMap[a, b] == " ")
                    {
                        lbl.Name = (a + 1).ToString() + "|" + (b + 1).ToString();
                        lbl.Text = "0";     //no unit

                        lbl.Width = 40;
                        lbl.Height = 40;
                        lbl.TextAlign = ContentAlignment.MiddleCenter;

                        lbl.Click += new EventHandler(frm1.NoUnitClick);

                        tlpMap.Controls.Add(lbl);
                    }   //endif
                    if (arrMap[a, b] == "m")
                    {
                        lbl.Name = (a + 1).ToString() + "|" + (b + 1).ToString();
                        lbl.Text = "1";     //melee unit
                        lbl.Font = new Font(lbl.Font.Name, 12, FontStyle.Bold);

                        lbl.Width = 40;
                        lbl.Height = 40;
                        lbl.TextAlign = ContentAlignment.MiddleCenter;

                        lbl.Click += new EventHandler(frm1.MeleeUnitClick);

                        tlpMap.Controls.Add(lbl);
                    }   //endif
                    if (arrMap[a, b] == "r")
                    {
                        lbl.Name = (a + 1).ToString() + "|" + (b + 1).ToString();
                        lbl.Text = "2";     //ranged unit
                        lbl.Font = new Font(lbl.Font.Name, 12, FontStyle.Bold);

                        lbl.Width = 40;
                        lbl.Height = 40;
                        lbl.TextAlign = ContentAlignment.MiddleCenter;

                        lbl.Click += new EventHandler(frm1.RangedUnitClick);

                        tlpMap.Controls.Add(lbl);
                    }   //endif
                }   //endfor
            }   //endfor        //reads through the array and creates, and adds a label to the table layout panel component, for every item in the array
        }

        public void GenerateNewUnit()
        {
            MeleeUnit mUnit = new MeleeUnit(this);
            RangedUnit rUnit = new RangedUnit(this);

            Random TrueFalse = new Random(Guid.NewGuid().GetHashCode());

            //units spawn next to the factory building. arrMap locations are 0, 1 and 1, 0.
            //units will move away from the building to make space for new units to be produced.

            int TF = TrueFalse.Next(1, 3);
            if (TF == 1)
            {
                arrMap[0, 1] = "m";
                mUnit.IXPos = 0;
                mUnit.IYPos = 1;
            }   //endif
            else if (TF == 2)
            {
                arrMap[1, 0] = "r";
                rUnit.IXPos = 1;
                rUnit.IYPos = 0;
            }   //endelseif
        }

        public void CreateNewUnit()
        {
            tlpMap.Controls.Clear();

            GenerateBattleField();
            GenerateNewUnit();

            for (int a = 0; a < 20; a++)
            {
                for (int b = 0; b < 20; b++)
                {
                    Label lbl = new Label();

                    if (arrMap[a, b] == "m")
                    {
                        lbl.Name = (a + 1).ToString() + "|" + (b + 1).ToString();
                        lbl.Text = "1";     //melee unit
                        lbl.Font = new Font(lbl.Font.Name, 12, FontStyle.Bold);

                        lbl.Width = 40;
                        lbl.Height = 40;
                        lbl.TextAlign = ContentAlignment.MiddleCenter;

                        lbl.Click += new EventHandler(frm1.MeleeUnitClick);

                        tlpMap.Controls.Add(lbl);
                    }   //endif
                    if (arrMap[a, b] == "r")
                    {
                        lbl.Name = (a + 1).ToString() + "|" + (b + 1).ToString();
                        lbl.Text = "2";     //ranged unit
                        lbl.Font = new Font(lbl.Font.Name, 12, FontStyle.Bold);

                        lbl.Width = 40;
                        lbl.Height = 40;
                        lbl.TextAlign = ContentAlignment.MiddleCenter;

                        lbl.Click += new EventHandler(frm1.RangedUnitClick);

                        tlpMap.Controls.Add(lbl);
                    }   //endif
                }   //endfor
            }   //endfor        //reads through the array and creates, and adds a label to the table layout panel component, for every new item in the array
        }

        public void MoveUnit()
        {

        }

        public void UpdatePosition()
        {

        }

        public void Save()
        {
            string Filename = @"D:\\MapArray.txt";

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(Filename, FileMode.Create, FileAccess.Write);

            formatter.Serialize(stream, arrMap);
            stream.Close();
        }

        public void Read()  //takes 5-10mins to load depending on your pc' specs
        {
            string Filename = @"D:\\MapArray.txt";

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(Filename, FileMode.Open, FileAccess.Read);

            arrMap = (String[,])formatter.Deserialize(stream);
            stream.Close();
            GenerateField();
        }

    }

    class GameEngine
    {
        public GameEngine()
        {

        }
    }
}