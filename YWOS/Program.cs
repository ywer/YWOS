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
            string MMenus = "Info|Warning|SystemStatus|Settings[LEER]|Reset";
            string SystemStatusM = "Energy|Weapons|Fuel|Inventory";
            string ResetMenu = "Reset Warnings|Reset Info|Reset All";

            Channellist.Add(new Channels { Name = "MainMenu", Menus = MMenus, Type = "Menu", MenuCount = 0});
            Channellist.Add(new Channels { Name = "Info", Menus = "", Type = "Info", MenuCount = 0 });
            Channellist.Add(new Channels { Name = "Warning", Menus = "", Type = "Info", MenuCount = 0 });
            Channellist.Add(new Channels { Name = "SystemStatus", Menus = SystemStatusM, Type = "Menu", MenuCount = 0 });
            Channellist.Add(new Channels { Name = "Energy", Menus = "", Type = "Info", MenuCount = 0 });
            Channellist.Add(new Channels { Name = "Weapons", Menus = "", Type = "Info", MenuCount = 0 });
            Channellist.Add(new Channels { Name = "Fuel", Menus = "", Type = "Info", MenuCount = 0 });
            Channellist.Add(new Channels { Name = "Inventory", Menus = "", Type = "Info", MenuCount = 0 });

            Channellist.Add(new Channels { Name = "Settings", Menus = "LEER", Type = "Menu", MenuCount = 0 });
            Channellist.Add(new Channels { Name = "Reset", Menus = ResetMenu, Type = "Menu", MenuCount = 3 });
            Channellist.Add(new Channels { Name = "Reset Warnings", Menus = "", Type = "Setting", MenuCount = 1 });
            Channellist.Add(new Channels { Name = "Reset Info", Menus = "", Type = "Setting", MenuCount = 1 });
            Channellist.Add(new Channels { Name = "Reset All", Menus = "", Type = "Setting", MenuCount = 1 });
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

            RegisterData("Info", "1Bla blaojiguhjgkhgpüok oijhdgo okiofjgoijdgü ijufgih", "Testscript1", 3, 2);
            RegisterData("Info", "2Bla blaojiguhjgkhgpüok oijhdgo okiofjgoijdgü ijufgih", "Testscript2", 2, 2);
            RegisterData("Info", "3Bla blaojiguhjgkhgpüok oijhdgo okiofjgoijdgü ijufgih", "Testscript3", 1, 4);
            RegisterData("Warn", "4Bla blaojiguhjgkhgpüok oijhdgo okiofjgoijdgü ijufgih", "Testscript4", 4, 5);
            RegisterData("Warn", "5Bla blaojiguhjgkhgpüok oijhdgo okiofjgoijdgü ijufgih", "Testscript5", 5, 5);
            RegisterData("Warn", "6Bla blaojiguhjgkhgpüok oijhdgo okiofjgoijdgü ijufgih", "Testscript6", 6, 5);
            RegisterData("Warn", "7Bla blaojiguhjgkhgpüok oijhdgo okiofjgoijdgü ijufgih", "Testscrip7", 7, 5);
            RegisterData("Warn", "7uff", "Testscrip7", 7, 5);

            /*
            WarnMenu.Add(new Warnings { ID = 1, Message = "1Testfuck", Prio = 2, ScriptName = "test3" });
            WarnMenu.Add(new Warnings { ID = 2, Message = "2Testfuck2", Prio = 2, ScriptName = "teest4" });
            InfoMenu.Add(new Info { ID = 3, Message = "1Bla blaojiguhjgkhgpüok oijhdgo okiofjgoijdgü ijufgih ", Prio = 6, ScriptName = "test1" });
            InfoMenu.Add(new Info { ID = 4, Message = "2Bla blaojiguhjgkhgpüok oijhdgo okiofjgoijdgü ijufgih ", Prio = 6, ScriptName = "test2" });
            WarnMenu.Add(new Warnings { ID = 5, Message = "2Testfuck", Prio = 2, ScriptName = "" });
            WarnMenu.Add(new Warnings { ID = 6, Message = "3Testfuck2", Prio = 2, ScriptName = "" });
            InfoMenu.Add(new Info { ID = 7, Message = "3Bla blaojiguhjgkhgpüok oijhdgo okiofjgoijdgü ijufgih ", Prio = 6, ScriptName = "test5" });
            InfoMenu.Add(new Info { ID = 8, Message = "4Bla blaojiguhjgkhgpüok oijhdgo okiofjgoijdgü ijufgih ", Prio = 6, ScriptName = "test 6" });
            */

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
        double Version = 0.21;
        int MyPos = 0;
        int deep = 0;
        int Deletecount = 0;
        string Site = "";
        int MaxWarnSites = 0;
        int MaxInfSites = 0;
        
        int ResetAll = 0;
        int ResetWarning = 0;
        int ResetInfo = 0;
        
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

            public int ID { get; set; }

            
        }

        class Warnings
        {
            public string Message { get; set; }

            public string ScriptName { get; set; }

            public int Prio { get; set; }

            public int ID { get; set; }

            
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
                int MenuCount = 0;
               string Type = Channellist[Index].Type;

                if (Site == "Warning")
                {
                    MenuCount = MaxWarnSites;
                }
                else if(Site == "Info")
                {
                    MenuCount = MaxInfSites;
                }
                else if(Type == "Menu")
                {
                    MenuCount = Channellist[Index].MenuCount;
                }


                if (Direct == "UP")
                {

                    if (MyPos > 0)
                    {
                        MyPos--;
                        Echo(MyPos.ToString());
                        ShowMenu();
                        return;
                    }




                    return;
                }
                else if (Direct == "Down")
                {
                    Echo("Menu COunt " + MenuCount);
                    if (MyPos < MenuCount - 1)
                    {
                        MyPos++;
                        Echo(MyPos.ToString());
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

                            //Echo("MYPOS: " + MyPos.ToString());
                            deep++;
                            string Temp = Channellist[Index2].Menus;
                            //Echo("Menus: " + Channellist[Index2].Menus);
                            string[] MyMenu = Temp.Split('|');
                            Site = MyMenu[MyPos];
                          // Echo("Site: " + Site);
                            MyPos = 0;
                            ShowMenu();
                            return;
                        }
                    }
                    else if (Type == "Info")
                    {
                        /*
                        if (Deletecount == 0)
                        {
                            Deletecount = 1;
                            ShowMenu();
                            return;
                        }
                        else
                        {
                            if (MyPos < InfoMenu.Count)
                            {
                                Deletecount = 0;

                                    InfoMenu.RemoveAt(MyPos);
                                    MaxInfSites--;
                                    ShowMenu();
                                    return;
                                

                            }
                        }


                    }else if(Type == "Warning")
                    {
                        if (Deletecount == 0)
                        {
                            Deletecount = 1;
                            ShowMenu();
                            return;
                        }
                        else
                        {
                            if (WarnMenu[MyPos]  != null)
                            {
                                Deletecount = 0;
                                WarnMenu.RemoveAt(MyPos);
                                MaxWarnSites--;
                                ShowMenu();
                                return;
                            }
                        }
                     */   
                    }else if(Type == "Setting")
                    {
                        if(ResetWarning == 1)
                        {
                            WarnMenu.Clear();
                            ResetWarning = 0;
                            
                            ChangePos("Back");
                            return;
                        }else if(ResetInfo == 1)
                        {
                            InfoMenu.Clear();
                            ResetInfo = 0;
                            ChangePos("Back");
                            return;
                        }
                        else if(ResetAll == 1)
                        {
                            InfoMenu.Clear();
                            WarnMenu.Clear();
                            ResetAll = 0;
                            ChangePos("Back");

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
                    if (Site == "Info")
                    {
                       List<string> InfoOut2 = GetData("Info");
                        MaxInfSites = InfoOut2.Count;

                        if (InfoOut2.Count != 0)
                        {
                            int MyPosmath = MyPos + 1;
                                Out = "Infos: " + Environment.NewLine + InfoOut2[MyPos] + Environment.NewLine + Environment.NewLine + "Site:[" + MyPosmath  + "/" + MaxInfSites + "]" + Environment.NewLine;
                            
                            
                        }
                        else
                        {
                            Out = "No Info" + Environment.NewLine + Environment.NewLine ;
                        }

                        
                        DirectShow(Out);
                        return;

                    }
                    else if (Site == "Warning")
                    {
                        List<string> WarnOut2 = GetData("Warning");
                        MaxWarnSites = WarnOut2.Count;

                        if (WarnMenu.Count != 0)
                        {
                            int MyPosmath = MyPos + 1;
                            Out = "Warnings: " + Environment.NewLine + WarnOut2[MyPos] + Environment.NewLine + Environment.NewLine + "Site:[" + MyPosmath + "/" + MaxWarnSites + "]" + Environment.NewLine;
                        }
                        else
                        {
                            Out = "No Warnings" + Environment.NewLine + Environment.NewLine;
                        }

                        DirectShow(Out);
                        return;
                    }


                }
                else if (Type == "Menu")
                {
                    
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
                    
                    
                }else if(Type == "Setting")
                {
                    Out = "";
                    if(Site == "Reset All")
                    {
                        Out = "Enter to Delete all Infos/Warnings, Back to go back to Menu";
                        ResetAll = 1;
                        DirectShow(Out);

                    }else if(Site == "Reset Warnings")
                    {
                        Out = "Enter to Delete all Warnings, Back to go back to Menu";
                        ResetWarning = 1;
                        DirectShow(Out);

                    }
                    else if(Site == "Reset Info")
                    {
                        ResetInfo = 1;
                        Out = "Enter to Delete all Infos, Back to go back to Menu";
                        DirectShow(Out);
                       

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
                string Type = Channellist[Index].Type;
                if (Type != "Info")
                {
                    Max = Channellist[Index].MenuCount;
                    Show = Show + Environment.NewLine + "Position[" + Math + "/" + Max + "]" + Environment.NewLine;
                }
            }

            if (WarnMenu.Count > 0)
            {
                Show = Show + "Warnungen: " + WarnMenu.Count;
            }
            else
            {
                Show = Show + "Keine Warnungen" ;
            }


            if (MLCD != null)
            {
                MLCD.WriteText(Show, false);
                return;
            }
            return;
        }


        public List<string> GetData(string Type)
        {
            //-------------------Warnings-------------------------
            
            
            if (Type == "Warning")
            {
                MaxWarnSites = 0;
                List<string> List = new List<string>();
                string[] WarnOut = new string[99];
                int WMax = WarnMenu.Count;
               // Echo("Wmax " + WMax.ToString());
                int WCount = 0;

                int WmaxMath = WMax - 1;
                if (WMax > 0)
                {

                    
                    int WCount2 = 0;
                    int WCount3 = 0;
                    int WCount4 = 0;
                    do
                    {
                        List.Add("ScriptName: " + WarnMenu[WCount2].ScriptName + Environment.NewLine + " Prio: " + WarnMenu[WCount2].Prio + Environment.NewLine + " Message: " + Environment.NewLine + WarnMenu[WCount].Message + Environment.NewLine);
                        //WarnOut[WCount3] =  "ScriptName: " + WarnMenu[WCount2].ScriptName + Environment.NewLine +  " Prio: " + WarnMenu[WCount2].Prio + Environment.NewLine + " Message: " + Environment.NewLine + WarnMenu[WCount].Message + Environment.NewLine;
                        WCount2++;
                        WCount4++;
                       // Echo("Wcount2 = " + WCount2);
                        if (WCount4 == 11)
                        {
                            MaxWarnSites++;
                            WCount3++;
                            continue;
                        }
                        if (WCount2 == WMax)
                        {
                            break;

                        }


                    } while (WCount4 < 15);


                    return List;

                }
            }

            // Echo("Going to Info");

            //----------------------INFOS------------

            if (Type == "Info")
            {
                List<string> List2 = new List<string>();
                MaxWarnSites = 0;
               
                int IMax = InfoMenu.Count;
                int ICount = 0;
                int ImaxMath = IMax - 1;

                if (IMax > 0)
                {

                    int ICount2 = 0;
                    int ICount3 = 0;
                    int ICount4 = 0;
                    do
                    {
                        List2.Add("ScriptName: " + InfoMenu[ICount2].ScriptName + Environment.NewLine + " Prio: " + InfoMenu[ICount2].Prio + Environment.NewLine + " M: " + Environment.NewLine + InfoMenu[ICount].Message + Environment.NewLine);
                        //InfoOut[ICount3] =  "ScriptName: " + InfoMenu[ICount2].ScriptName + Environment.NewLine + " Prio: " + InfoMenu[ICount2].Prio + Environment.NewLine + " M: " + Environment.NewLine + InfoMenu[ICount].Message + Environment.NewLine;
                        ICount2++;
                        ICount4++;

                        if (ICount4 == 11)
                        {
                            MaxInfSites++;
                            ICount3++;
                            continue;
                        }
                        if (ICount2 == IMax)
                        {
                            break;

                        }


                    } while (ICount4 < 15);

                    return List2;
                }

            }
            

            return null;
        }


        public void RegisterData(string Type , string Data, string Source, int ID, int Prio)
        {
            string Ext = "";
            if(Source != "")
            {
                Ext = Source;
            }

            if(Type == "Warn")
            {
                int Index = WarnMenu.FindIndex(a => a.ID == ID);
                int Index2 = WarnMenu.FindIndex(a => a.ScriptName == Source);
                    if(Index != -1 & Index2 != -1)
                {
                    WarnMenu.RemoveAt(Index);
                    WarnMenu.Add(new Warnings() { ID = ID, Message = Data, Prio = Prio, ScriptName = Ext });
                    return;

                }
                else
                {
                    WarnMenu.Add(new Warnings() { ID = ID, Message = Data, Prio = Prio, ScriptName = Ext });
                    MaxWarnSites++;
                    return;

                }

            }
            else if (Type == "Info")
            {
                int Index3 = InfoMenu.FindIndex(a => a.ID == ID);
                int Index4 = InfoMenu.FindIndex(a => a.ScriptName == Source);
                if (Index3 != -1 & Index4 != -1)
                {
                    InfoMenu.RemoveAt(Index3);
                    InfoMenu.Add(new Info() { ID = ID, Message = Data, Prio = Prio, ScriptName = Ext });
                    return;

                }
                else
                {
                    InfoMenu.Add(new Info() { ID = ID, Message = Data, Prio = Prio, ScriptName = Ext });
                    MaxInfSites++;
                    return;

                }



            }



        }


        #endregion


    }
}
