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

                if (Block is IMyLargeMissileTurret)
                {
                    MyMissleTurrets.Add((IMyLargeMissileTurret)Block);

                }

                if (Block is IMyLargeGatlingTurret)
                {
                    MyGatlingTurrets.Add((IMyLargeGatlingTurret)Block);
                }

                if(Block is IMyLargeInteriorTurret)
                {
                    MyIntTurrets.Add((IMyLargeInteriorTurret)Block);
                }


                if (Block is IMyGasTank)
                {
                    MyFuelTanks.Add((IMyGasTank)Block);


                }
                if (Block is IMyBatteryBlock)
                {
                    MyBatteries.Add((IMyBatteryBlock)Block);
                }
                if (Block is IMySolarPanel)
                {
                    MySolar.Add((IMySolarPanel)Block);
                }
                if (Block is IMyPowerProducer)
                {
                    MyPowerProd.Add((IMyPowerProducer)Block);
                }




            }


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
                   
                    Channellist[C2].MenuCount = M1.Length;


                }
               

                C2++;
            } while (C2 < Channellist.Count);


            
            RegisterMessage(0, 1, "test4", "ywerInc4");
            RegisterMessage(0, 1, "test5", "ywerInc5");
            RegisterMessage(0, 1, "test6", "ywerInc6");
            RegisterMessage(1, 1, "test1", "ywerInc1");
            RegisterMessage(1, 1, "test2", "ywerInc2");
            RegisterMessage(1, 1, "test3", "ywerInc3");



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
        double Version = 0.222;
        int MyPos = 0;
        int deep = 0;
        int Deletecount = 0;
        string Site = "";


        

        int ResetAll = 0;
        int ResetWarning = 0;
        int ResetInfo = 0;
        
        IMyTextPanel MLCD;
        List<Channels> Channellist = new List<Channels>();
        List<IMyTextPanel> InfoLCDList = new List<IMyTextPanel>();
        List<IMyTextPanel> WarnLCDList = new List<IMyTextPanel>();
        List<IMyButtonPanel> ControllerList = new List<IMyButtonPanel>();
        List<IMyReactor> AllMyReactors = new List<IMyReactor>();
        List<IMyLargeMissileTurret> MyMissleTurrets = new List<IMyLargeMissileTurret>();
        List<IMyLargeGatlingTurret> MyGatlingTurrets = new List<IMyLargeGatlingTurret>();
        List<IMyBatteryBlock> MyBatteries = new List<IMyBatteryBlock>();
        List<IMySolarPanel> MySolar = new List<IMySolarPanel>();
        List<IMyPowerProducer> MyPowerProd = new List<IMyPowerProducer>();
        List<IMyLargeInteriorTurret> MyIntTurrets = new List<IMyLargeInteriorTurret>();
        List<IMyGasTank> MyFuelTanks = new List<IMyGasTank>();
        string[] Steps = new string[20];
        List<Inf> InfoM = new List<Inf>();
        List<Inf> InfoMTemp = new List<Inf>();
        List<Warn> WarnM = new List<Warn>();
        List<Warn> WarnMTemp = new List<Warn>();
        string LastWSource = "";
        string LastISource = "";
        int IID = 0;
        int WID = 0;

        class Channels
        {
            public string Name { get; set; }

            public string Type { get; set; }

            public string Menus { get; set; }

            public int MenuCount { get; set; }

        }


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

                            
                            deep++;
                            string Temp = Channellist[Index2].Menus;
                            
                            string[] MyMenu = Temp.Split('|');
                            Site = MyMenu[MyPos];
                          
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
                                
                                Deletecount = 0;
                                int Del2 = DeleteMessage(0,MyPos);
                                MyPos = 0;
                                    ShowMenu();
                                    return;
                                

                            }
                        }


                    }else if(Site == "Warning")
                    {
                        if (Deletecount == 0)
                        {
                            
                           
                            
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
                        int MaxI = ReturnMaxMessages(0);
                        
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
                        int MaxW = ReturnMaxMessages(1);
                        
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
                    else if(Site == "Energy")
                    {
                        float MaxPower = 0;
                        float PowerUsed = 0;
                        int ReacIsRunning = 0;
                        float MaxSolarPower = 0;
                        float OutputSolarPower = 0;
                        float BatMaxLoad = 0;
                        float BatCurrentLoad = 0;
                        int MaxReac = 0;
                        float BatOutput = 0;
                        float BatInput = 0;
                        int BatCount = 0;



                         MaxReac = AllMyReactors.Count;
                        foreach (IMyReactor Rea in AllMyReactors)
                        {
                            PowerUsed = PowerUsed + Rea.CurrentOutput;
                            MaxPower = MaxPower + Rea.MaxOutput;

                            if (Rea.Enabled)
                            {
                                ReacIsRunning++;
                            }

                        }


                        foreach (IMySolarPanel Sola in MySolar)
                        {
                            MaxSolarPower = MaxSolarPower + Sola.MaxOutput;
                            OutputSolarPower = OutputSolarPower + Sola.CurrentOutput;

                        }


                        BatCount = MyBatteries.Count;
                        foreach (IMyBatteryBlock Bat in MyBatteries)
                        {
                            BatMaxLoad = BatMaxLoad + Bat.MaxInput;
                            BatCurrentLoad = BatCurrentLoad + Bat.CurrentStoredPower;
                            BatInput = BatInput + Bat.CurrentInput;
                            BatOutput = BatOutput + Bat.CurrentOutput;

                        }

                        Out = "Energy Monitor" + Environment.NewLine + "Reactors: " + Environment.NewLine + "Currently Running of: " + ReacIsRunning + "/" + MaxReac + Environment.NewLine + "Output/Max Output: " + PowerUsed + "/" + MaxPower + Environment.NewLine +  Environment.NewLine + "Solar Power: " + Environment.NewLine + "Output/Max Output: " + OutputSolarPower + "/" + MaxSolarPower + Environment.NewLine +Environment.NewLine + "Batterys:" +Environment.NewLine + "Count: " + BatCount + Environment.NewLine + "Current/Max" + BatCurrentLoad + "/" + BatMaxLoad + Environment.NewLine + "Input/Output: " + BatInput + "/" + BatOutput + Environment.NewLine + Environment.NewLine;



                        DirectShow(Out);



                        return;

                    }
                    else if(Site == "Weapons")
                    {
                        int MissTurretsActive = 0;
                        int GatTurretsActive = 0;
                        int IntTurretsActive = 0;
                        int GatAIEnabled = 0;
                        int GatIsShooting = 0;
                        int IntIsShooting = 0;
                        int MissIsShooting = 0;
                        int IntAIEnabled = 0;
                        int MissAIEnabled = 0;
                        foreach (IMyLargeGatlingTurret Gat in MyGatlingTurrets)
                        {
                            if (Gat.Enabled)
                            {
                                GatTurretsActive++;
                             }
                            if(Gat.AIEnabled)
                            {
                                GatAIEnabled++;
                            }
                            if(Gat.IsShooting)
                            {
                                GatIsShooting++;
                            }


                        }

                        foreach (IMyLargeMissileTurret Miss in MyMissleTurrets)
                        {
                            if(Miss.Enabled)
                            {
                                MissTurretsActive++;
                            }
                            if (Miss.AIEnabled)
                            {
                                MissAIEnabled++;
                            }
                            if (Miss.IsShooting)
                            {
                                MissIsShooting++;
                            }

                        }

                        foreach(IMyLargeInteriorTurret Int in MyIntTurrets)
                        {
                            if(Int.Enabled)
                            {
                                IntTurretsActive++;
                            }
                            if (Int.AIEnabled)
                            {
                                IntAIEnabled++;
                            }
                            if (Int.IsShooting)
                            {
                                IntIsShooting++;
                            }

                        }

                        Out = "Weapons: " + Environment.NewLine + "Int. Turrets: " + Environment.NewLine + "Aktive/Max: " + IntTurretsActive + "/" + MyIntTurrets.Count + Environment.NewLine + "Ai Enabled: " + IntAIEnabled + "/" + MyIntTurrets.Count + Environment.NewLine + "Shooting: " + IntIsShooting + "/" + MyIntTurrets.Count + Environment.NewLine + Environment.NewLine + "Gatling Guns: " + Environment.NewLine + "Aktive/Max: " + GatTurretsActive + "/" + MyGatlingTurrets.Count + Environment.NewLine + "Ai Enabled: " + GatAIEnabled + "/" + MyGatlingTurrets.Count + Environment.NewLine + "Shooting: " + GatIsShooting + "/" + MyGatlingTurrets.Count + Environment.NewLine + Environment.NewLine + "Missle Turrets: " + Environment.NewLine + "Aktive/Max: " + MissTurretsActive + "/" + MyMissleTurrets.Count + Environment.NewLine + "Ai Enabled: " + MissAIEnabled + "/" + MyMissleTurrets.Count + Environment.NewLine + "Shooting: " + MissIsShooting + "/" + MyMissleTurrets.Count + Environment.NewLine + Environment.NewLine;

                        DirectShow(Out);
                        return;
                    }
                    if(Site == "Fuel")
                    {
                        float MaxFuel = 0;
                        double CurrentFuel = 0;


                        Out = "";
                        foreach(IMyGasTank Fuel in MyFuelTanks)
                        {
                            MaxFuel = MaxFuel + Fuel.Capacity;
                            CurrentFuel = CurrentFuel + Fuel.FilledRatio;
                                
                        }

                        Out = "Fuel: " + Environment.NewLine + "Tanks: " + MyFuelTanks.Count + Environment.NewLine + "Fuel " + CurrentFuel + Environment.NewLine + "Max Fuel: " + MaxFuel + Environment.NewLine; 

                        DirectShow(Out);
                        return;
                    }

                }
                else if (Type == "Menu")
                {
                    
                    string MenuFull = Channellist[Index].Menus;
                    string MenuName = Channellist[Index].Name;
                    
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

            }
            else if(MType == 1)
            {
                WarnM.RemoveAt(ID);
                
                WID--;
                WarnMTemp.AddRange(WarnM);
                WarnM.Clear();
                WarnM.Clear();
                WarnM.AddRange(WarnMTemp);
                WarnMTemp.Clear();
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
