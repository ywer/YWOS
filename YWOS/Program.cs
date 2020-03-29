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
            //MessageHandling MES = new MessageHandling();
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


            
            RegisterMessage(0, 1, "test4", "ywerInc4");
            RegisterMessage(0, 1, "test5", "ywerInc5");
            RegisterMessage(0, 1, "test6", "ywerInc6");
            RegisterMessage(1, 1, "test1", "ywerInc1");
            RegisterMessage(1, 1, "test2", "ywerInc2");
            RegisterMessage(1, 1, "test3", "ywerInc3");

           
            /*
            RegisterData("Info", "1Bla blaojiguhjgkhgpüok oijhdgo okiofjgoijdgü ijufgih", "Testscript1", 3, 2);
            RegisterData("Info", "2Bla blaojiguhjgkhgpüok oijhdgo okiofjgoijdgü ijufgih", "Testscript2", 2, 2);
            RegisterData("Info", "3Bla blaojiguhjgkhgpüok oijhdgo okiofjgoijdgü ijufgih", "Testscript3", 1, 4);
            RegisterData("Warn", "4Bla blaojiguhjgkhgpüok oijhdgo okiofjgoijdgü ijufgih", "Testscript4", 4, 5);
            RegisterData("Warn", "5Bla blaojiguhjgkhgpüok oijhdgo okiofjgoijdgü ijufgih", "Testscript5", 5, 5);
            RegisterData("Warn", "6Bla blaojiguhjgkhgpüok oijhdgo okiofjgoijdgü ijufgih", "Testscript6", 6, 5);
            RegisterData("Warn", "7Bla blaojiguhjgkhgpüok oijhdgo okiofjgoijdgü ijufgih", "Testscrip7", 7, 5);
            RegisterData("Warn", "7uff", "Testscrip7", 7, 5);
            */
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
        double Version = 0.22;
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
        //List<Info> InfoMenu = new List<Info>();
        //List<Warnings> WarnMenu = new List<Warnings>();
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

        /*
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
        */
        #endregion




        #region Menu/Main

        public void Main(string argument, UpdateType updateSource)
        {
           // MessageHandling MES = new MessageHandling();


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
           // MessageHandling MES = new MessageHandling();
            string Direct = Direction;
            int Index = Channellist.FindIndex(a => a.Name == Site);
            if (Index != -1)
            {
                int MenuCount = 0;
               string Type = Channellist[Index].Type;

                if (Site == "Warning")
                {
                    MenuCount = ReturnMaxMessages(1);
                }
                else if(Site == "Info")
                {
                    MenuCount = ReturnMaxMessages(0);
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
                    else if (Site == "Info")
                    {
                        
                        if (Deletecount == 0)
                        {
                            Deletecount = 1;
                            ShowMenu();
                            return;
                        }
                        else
                        {
                            if (MyPos < ReturnMaxMessages(0))
                            {
                                Echo("InfoDelete");
                                Deletecount = 0;
                                int Del2 = DeleteMessage(0,MyPos);
                                Echo("Del2: " + Del2);
                                Echo("MyPos: " + MyPos);
                                // InfoMenu.RemoveAt(MyPos);
                                //MaxInfSites--;
                                MyPos = 0;
                                    ShowMenu();
                                    return;
                                

                            }
                        }


                    }else if(Site == "Warning")
                    {
                        if (Deletecount == 0)
                        {
                            Echo("WarnDelete");
                            Echo("MyPos: " + MyPos);
                            Echo("MAX: " + ReturnMaxMessages(1));
                            Deletecount = 1;
                            ShowMenu();
                            return;
                        }
                        else
                        {
                            if (MyPos < ReturnMaxMessages(1))
                            {
                                Deletecount = 0;
                                int Del = DeleteMessage(1,MyPos);
                                Echo("DelWarn: " + Del);
                                Echo("MyPosWarn: " + MyPos);
                                //WarnMenu.RemoveAt(MyPos);
                                //MaxWarnSites--;
                                MyPos = 0;
                                ShowMenu();
                                return;
                            }
                        }
                        
                    }else if(Type == "Setting")
                    {
                        if(ResetWarning == 1)
                        {
                            DeleteWarnings();
                            ResetWarning = 0;
                            
                            ChangePos("Back");
                            return;
                        }else if(ResetInfo == 1)
                        {
                            DeleteInfos();
                            ResetInfo = 0;
                            ChangePos("Back");
                            return;
                        }
                        else if(ResetAll == 1)
                        {
                            DeleteInfos();
                            DeleteWarnings();
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
            //MessageHandling MES = new MessageHandling();
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
                        //string[] InfoOut2 = GetData("Info");
                        //MaxInfSites = InfoOut2.Length;
                        int MaxI = ReturnMaxMessages(0);
                        Echo("Max I " + MaxI);
                        if (MaxI > 0)
                        {
                            string IMessage = ReturnMessage(0, MyPos);
                                int MyPosmath = MyPos + 1;

                                Out = "Infos: " + Environment.NewLine + IMessage + Environment.NewLine + Environment.NewLine + "Site:[" + MyPosmath + "/" + MaxI + "]" + Environment.NewLine;
                            

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

                        //string[] WarnOut2 = GetData("Warning");
                        //MaxWarnSites = WarnOut2.Length;
                        int MaxW = ReturnMaxMessages(1);
                        Echo("Max W " + MaxW);
                        if (MaxW > 0)
                        {

                                int MyPosmath = MyPos + 1;
                            string WMessage = ReturnMessage(1, MyPos);
                            Out = "Warnings: " + Environment.NewLine + WMessage + Environment.NewLine + Environment.NewLine + "Site:[" + MyPosmath + "/" + MaxW + "]" + Environment.NewLine;


                            
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
            //MessageHandling MES = new MessageHandling();
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

            if (WarnM.Count > 0)
            {
                Show = Show + "Warnungen: " + ReturnMaxMessages(1);
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

        /*
        public string[] GetData(string Type)
        {
            //-------------------Warnings-------------------------
            //fehler hier irgendwo index out of range
            
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
                    
                    string[] Out = List.ToArray();
                    return Out;
                    
                    string[] Out1 = new string[1];
                    Out1[0] = "LEER";
                    return Out1;
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
                    /*
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
                    
                    string[] Out2 = List2.ToArray();
                    return Out2;
                    
                    string[] Out2 = new string[1];
                    Out2[0] = "LEER";
                    return Out2;
                }

            }
            string[] Out3 = new string[1];
            Out3[0] = "LEER";

            return Out3;
        }
        */
        /*
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
        */

        //----------------------------------------------- NEU----------------------------------

        class Inf
        {
            public string Message { get; set; }

            public string ScriptName { get; set; }

            public int Prio { get; set; }

            public int ID { get; set; }


        }


        class Warn
        {
            public string Message { get; set; }

            public string ScriptName { get; set; }

            public int Prio { get; set; }

            public int ID { get; set; }


        }

        #region Stuff2
        List<Inf> InfoM = new List<Inf>();
        List<Inf> InfoMTemp = new List<Inf>();
        List<Warn> WarnM = new List<Warn>();
        List<Warn> WarnMTemp = new List<Warn>();
        string LastWSource = "";
        string LastISource = "";
        int IID = 0;
        int WID = 0;
        #endregion


        public void RegisterMessage(int MType, int Prio, string Message, string Source)
        {

            //0 - Info
            //1 - Warn

            if (MType == 0)
            {
                if (Source == "User")
                {
                    IID++;
                    InfoM.Add(new Inf() { ID = IID, Message = Message, Prio = Prio, ScriptName = Source });
                    return;
                }
                else
                {
                    if (Source == LastISource)
                    {
                        int Index2 = InfoM.FindIndex(a => a.ScriptName == Source);
                        if (Index2 != -1)
                        {
                            int temp = InfoM[Index2].ID;
                            InfoM.RemoveAt(Index2);
                            InfoM.Add(new Inf() { ID = temp, Message = Message, Prio = Prio, ScriptName = Source });
                            return;
                        }

                        return;
                    }
                    else
                    {
                        IID++;
                        InfoM.Add(new Inf() { ID = WID, Message = Message, Prio = Prio, ScriptName = Source });
                        return;
                    }
                }



            }
            else if (MType == 1)
            {
                if (Source == "User")
                {
                    WID++;
                    WarnM.Add(new Warn() { ID = WID, Message = Message, Prio = Prio, ScriptName = Source });
                    return;


                }
                else
                {

                    if (LastWSource == Source)
                    {
                        int Index3 = WarnM.FindIndex(a => a.ScriptName == Source);
                        if (Index3 != -1)
                        {
                            int temp2 = WarnM[Index3].ID;
                            InfoM.RemoveAt(Index3);
                            WarnM.Add(new Warn() { ID = temp2, Message = Message, Prio = Prio, ScriptName = Source });
                            return;
                        }
                    }
                    else
                    {
                        WID++;
                        WarnM.Add(new Warn() { ID = WID, Message = Message, Prio = Prio, ScriptName = Source });
                        return;

                    }

                }




                return;
            }
            else
            {

                return;
            }




        }



        public int DeleteMessage(int MType, int ID)
        {
            //mtype:
            //0 - Info
            //1 - Warn

            //meldungen:
            //0 - Erfolg
            //1 Fehler 

            Echo("Delete");
            Echo("Type: " + MType);

            if (MType == 0)
            {

                InfoM.RemoveAt(ID);
                IID--;
                InfoMTemp.AddRange(InfoM);
                InfoM.Clear();
                InfoM.Clear();
                InfoM.AddRange(InfoMTemp);
                InfoMTemp.Clear();
                return 0;
                //counter rückwärts, alles in nem array rein und neu ins die liste



            }
            else if(MType == 1)
            {
                WarnM.RemoveAt(ID);
                Echo("Deleted at: " + ID);
                WID--;
                WarnMTemp.AddRange(WarnM);
                WarnM.Clear();
                WarnM.Clear();
                WarnM.AddRange(WarnMTemp);
                WarnMTemp.Clear();
                //counter rückwärts, alles in nem array rein und neu ins die liste

                return 0;

            }
            else
            {

                return 1;
            }




        }

        public void DeleteWarnings()
        {
            WarnM.Clear();
            return;
        }

        public void DeleteInfos()
        {
            InfoM.Clear();
            return;
        }



        public string ReturnMessage(int MType, int ID)
        {
            //mtype:
            //0 - Info
            //1 - Warn

            if (MType == 0)
            {
                if (ID < InfoM.Count)
                {
                    string Out1 = "Source: " + InfoM[ID].ScriptName + Environment.NewLine + "Prio: " + InfoM[ID].Prio + Environment.NewLine + "Message: " + InfoM[ID].Message + Environment.NewLine;
                    return Out1;
                }
                else
                {
                    return "LEER";
                }
            }
            else if (MType == 1)
            {
                if (ID < WarnM.Count)
                {
                    string Out2 = "Source: " + WarnM[ID].ScriptName + Environment.NewLine + "Prio: " + WarnM[ID].Prio + Environment.NewLine + "Message: " + WarnM[ID].Message + Environment.NewLine;
                    return Out2;
                }
                else
                {
                    return "LEER";
                }
            }
            else
            {
                return null;
            }



        }

        public int ReturnMaxMessages(int MType)
        {
            //mtype:
            //0 - Info
            //1 - Warn
            int MAX = 0;

            if (MType == 0)
            {
                MAX = InfoM.Count;
            }
            else if (MType == 1)
            {
                MAX = WarnM.Count;
            }


            return MAX;
        }













        #endregion


    }
}
