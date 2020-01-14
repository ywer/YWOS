using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using System.Threading;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRage;
using VRageMath;
using Sandbox.Common.ObjectBuilders;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {


        public Program()
        {
            string MMenus = "Info|Warnings|SystemStatus|Settings[LEER]|ResetAll";
            string SystemStatusM = "Energy|Weapons|Fuel|Inventory";


            Channellist.Add(new Channels { Name = "MainMenu", Menus = MMenus, Type = "Menu", MenuCount = 0});
            Channellist.Add(new Channels { Name = "Info", Menus = "", Type = "Info", MenuCount = 0 });
            Channellist.Add(new Channels { Name = "Warning", Menus = "", Type = "Info", MenuCount = 0 });
            Channellist.Add(new Channels { Name = "SystemStatus", Menus = SystemStatusM, Type = "Menu", MenuCount = 0 });
            Channellist.Add(new Channels { Name = "Energy", Menus = "", Type = "Info", MenuCount = 0 });
            Channellist.Add(new Channels { Name = "Weapons", Menus = "", Type = "Info", MenuCount = 0 });
            Channellist.Add(new Channels { Name = "Fuel", Menus = "", Type = "Info", MenuCount = 0 });
            Channellist.Add(new Channels { Name = "Inventory", Menus = "", Type = "Info", MenuCount = 0 });

            Channellist.Add(new Channels { Name = "Settings", Menus = "LEER", Type = "Menu", MenuCount = 0 });
            Channellist.Add(new Channels { Name = "Reset", Menus = "LEER", Type = "Menu", MenuCount = 0 });

            int C2 = 0;

            do
            {
                string MType = Channellist[C2].Type;
                if(MType == "Menu")
                {
                    string MenusList = Channellist[C2].Menus;
                    string[] M1 = MenusList.Split('|');
                   // Echo(M1.Length.ToString());
                    Channellist[C2].MenuCount = M1.Length;


                }
               // Echo("C2: " + C2.ToString());

                C2++;
            } while (C2 < Channellist.Count);


        }

        public void Save()
        {

        }

        #region settings
        //settings
        string LCDName = "NachrichtenPanel"; //MSg LCD
        string WarningLCDName = "Warnings"; //Warning LCD
        string Controller = "ControllerPanel"; //Panel to Switch
        string MenuLCD = "MenuLCD";
        int ShowOnly = 0; //Only Show, dont Turn on Anything Mode!

        //Setting end ----> DONT EDIT BELOW
        #endregion

        #region Stuff
        double Version = 0.2;
        int MyPos = 0;
        int deep = 0;
        int Deletecount = 0;
        string Site = "";
        string LastMenu = "";
        IMyTextPanel MLCD;
        List<Channels> Channellist = new List<Channels>();
        List<Info> InfoMenu = new List<Info>();
        List<Warnings> WarnMenu = new List<Warnings>();
        List<IMyTextPanel> InfoLCDList = new List<IMyTextPanel>();
        List<IMyTextPanel> WarnLCDList = new List<IMyTextPanel>();
        List<IMyButtonPanel> ControllerList = new List<IMyButtonPanel>();
        List<IMyReactor> AllMyReactors = new List<IMyReactor>();
        List<IMyLargeMissileTurret> MyMissleTurrets = new List<IMyLargeMissileTurret>();
        List<IMyLargeGatlingTurret> MyGatlingTurrets = new List<IMyLargeGatlingTurret>();
        string[] Steps = new string[20];


        class Channels
        {
            public string Name { get; set; }

            public string Type { get; set; }

            public string Menus { get; set; }

            public int MenuCount { get; set; }

        }

        class Info
        {
            public string Message { get; set; }

            public string ScriptName { get; set; }

            public int Prio { get; set; }
        }

        class Warnings
        {
            public string Message { get; set; }

            public string ScriptName { get; set; }

            public int Prio { get; set; }
        }

        #endregion




        #region Menu/Main

        public void Main(string argument, UpdateType updateSource)
        {
            string Status = argument;

            if (Status == "Reset")
            {

            }
            else if (Status == "ShowOnly")
            {
                ShowOnly = 1;
            }
            else if (Status == "UP")
            {
                ChangePos("UP");

            }
            else if (Status == "Down")
            {
                ChangePos("Down");

            }
            else if (Status == "Enter")
            {
                ChangePos("Enter");

            }
            else if (Status == "Back")
            {
                ChangePos("Back");

            }
            else if (Status.Contains("Trans:"))
            {

                Echo("Nicht Implementiert");

            }
            List<IMyTerminalBlock> allBlocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocks(allBlocks);

            foreach (IMyTerminalBlock Block in allBlocks)
            {

                if (Block is IMyTextPanel)
                {

                    if (Block.CustomName.Contains(LCDName))
                    {


                        InfoLCDList.Add((IMyTextPanel)Block);

                    }

                    if (Block.CustomName.Contains(WarningLCDName))
                    {


                        WarnLCDList.Add((IMyTextPanel)Block);

                    }

                    if (Block.CustomName.Contains(Controller))
                    {
                        ControllerList.Add((IMyButtonPanel)Block);

                    }


                    if (Block.CustomName.Contains(MenuLCD))
                    {


                        MLCD = Block as IMyTextPanel;

                    }
                }

                if (Block is IMyReactor)
                {
                    AllMyReactors.Add((IMyReactor)Block);
                }
                /*
                if(Block is IMyOxygenGenerator)
                {
                    AllMyGasGen.Add((IMyOxygenGenerator)Block);
                }
                */

                if (Block is IMyLargeMissileTurret)
                {
                    MyMissleTurrets.Add((IMyLargeMissileTurret)Block);

                }

                if (Block is IMyLargeGatlingTurret)
                {
                    MyGatlingTurrets.Add((IMyLargeGatlingTurret)Block);
                }

                if (Block is IMyGasTank)
                {



                }





            }

            ShowMenu();



        }

        public void ChangePos(String Direction)
        {
            string Direct = Direction;
            int Index = Channellist.FindIndex(a => a.Name == Site);
            if (Index != -1)
            {
                string Type = Channellist[Index].Type;
                int MenuCount = Channellist[Index].MenuCount;

                if (Direct == "UP")
                {

                    if (MyPos > 0)
                    {
                        MyPos--;
                        ShowMenu();
                        return;
                    }




                    return;
                }
                else if (Direct == "Down")
                {
                    if (MyPos < MenuCount - 1)
                    {
                        MyPos++;
                        ShowMenu();
                        return;

                    }
                    return;
                }
                else if (Direct == "Back")
                {
                    if(deep > 0)
                    {
                        deep--;
                    }
                    
                    Site = Steps[deep];
                    MyPos = 0;
                    

                    ShowMenu();

                }
                else if (Direct == "Enter")
                {
                    if (Type == "Menu")
                    {
                        int Index2 = Channellist.FindIndex(a => a.Name == Site);

                        if (Index2 != -1)
                        {
                            Steps[deep] = Site;

                            Echo("MYPOS: " + MyPos.ToString());
                            deep++;
                            string Temp = Channellist[Index2].Menus;
                            Echo("Menus: " + Channellist[Index2].Menus);
                            string[] MyMenu = Temp.Split('|');
                            Site = MyMenu[MyPos];
                            Echo("Site: " + Site);
                            MyPos = 0;
                            ShowMenu();
                            return;
                        }
                    }
                    else if (Type == "Info")
                    {
                        if (Deletecount == 0)
                        {
                            Deletecount = 1;
                            ShowMenu();
                            return;
                        }
                        else
                        {
                            InfoMenu.RemoveAt(MyPos);
                            ShowMenu();
                            return;
                        }


                    }


                }



            }



        }



        public void ShowMenu()
        {

            string Out = "";
            if (Site == "")
            {
                Site = "MainMenu";
                ShowMenu();
                return;
            }
            int Index = Channellist.FindIndex(a => a.Name == Site);
            if (Index != -1)
            {
                string Type = Channellist[Index].Type;
                if (Type == "Info")
                {



                }
                else if (Type == "Menu")
                {
                    GetData();
                    string MenuFull = Channellist[Index].Menus;
                    string MenuName = Channellist[Index].Name;
                    //Echo(MenuFull);
                    string[] Menu = MenuFull.Split('|');
                    if (Menu.Length > 0)
                    {
                        Out = MenuName + ":" + Environment.NewLine;
                        int C1 = 0;
                        foreach (string Men in Menu)
                        {
                            if (C1 == MyPos)
                            {
                                string Uff = Menu[C1] + "<---";
                                Out = Out + Uff + Environment.NewLine;
                            }
                            else
                            {
                                Out = Out + Men + Environment.NewLine;

                            }


                            C1++;
                        }
                        DirectShow(Out);
                        return;

                    }


                }

            }
            else
            {
                Site = "MainMenu";
                ShowMenu();
                return;
            }



        }


        public void DirectShow(string Show)
        {
            int Max = 0;
            int Index = Channellist.FindIndex(a => a.Name == Site);
            int Math = MyPos + 1;
            if (Index != -1)
            {
                Max = Channellist[Index].MenuCount;
                Show = Show + Environment.NewLine + "Position[" + Math + "/" + Max + "]" + Environment.NewLine;

            }

            if (WarnMenu.Count > 0)
            {
                Show = Show + "Warnungen: " + WarnMenu.Count;
            }
            else
            {
                Show = Show + "Keine Warnungen";
            }


            if (MLCD != null)
            {
                MLCD.WriteText(Show, false);
                return;
            }
            return;
        }


        public void GetData()
        {
            hier
        }


        #endregion


    }
}
