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
using System.Runtime.Remoting.Contexts;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Weapons;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        //Script By Ywer


        #region settings
        //settings
        string MainLCDName = "MainLCD"; //LCD
        string MainControllerName = "MainController"; //button Panel
        int MaxLoggerRows = 200;//Set this too high will cause lag
        //Setting end ----> DONT EDIT BELOW
        #endregion

        #region Private
        double Version = 3.002;
        double UVersion = 0.3;
        int Error = 0;
        string ErrorText = "";
        bool Setup = false;
        IMyTextPanel MainScreen;
        IMyButtonPanel MainControll;
        int Tick = 0;
        int Maxtick = 100;
        IMyCubeGrid MyGrid;
        int Maxrows = 11;//max rows per page
        List<WarningValue> Warnings = new List<WarningValue>();

        class WarningValue
        {
            public string Warning { get; set; }

            public string From { get; set; }


        }



        #endregion

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
            WriteToLog("First Startup");

            Startup();
        }

        public void Save()
        {

        }


        public void Main(string argument, UpdateType updateSource)
        {
            string Input = argument;
            /*
            Tick++;
            if(Tick >= Maxtick)
            {
                Tick = -1;
            }
            WriteToLog("Running on Tick " + Tick);
            */
            if(Input != null || Input != "")
            {

                switch (Input)
                {
                    case "UP":
                        MMove(Input);
                        break;
                    case "DOWN":
                        MMove(Input);
                        break;
                    case "ENTER":
                        MMove(Input);
                        break;
                    case "BACK":
                        MMove(Input);
                        break;
                    case "REFIND":
                        FindBlocks();
                        break;

                }




            }

           ShowMenu();
            return;
        }


        #region Startup/Findblocks
       

        public void Startup()
        {
            if (Setup == false)
            {
                Echo("Starting Startup....");
                WriteToLog("Starting Startup....");
                WriteToLog("Serching for Screen and Controller..");
                Echo("Serching for Screen and Controller..");
                MainScreen = (IMyTextPanel) GridTerminalSystem.GetBlockWithName(MainLCDName);
                MainControll  =(IMyButtonPanel) GridTerminalSystem.GetBlockWithName(MainControllerName);
                if (MainScreen == null|| MainControll == null)
                {
                    if(MainScreen == null)
                    {
                        ErrorHandler("No MainScreen Found");
                    }

                    if(MainControll == null)
                    {
                        ErrorHandler("No MainController Found");
                    }

                    return;
                }
                else
                {
                    int uff = 0;
                    string Out = "";
                    do
                    {
                        if (uff == 0)
                        {
                            Out = "Setting Up.... " + Environment.NewLine + "Please Wait";
                        }
                        if(uff == 30)
                        {
                            Out = "Setting Up.... " + Environment.NewLine + "Please Wait" + Environment.NewLine + "..";
                        }
                        if (uff == 40)
                        {
                            Out = "Setting Up.... " + Environment.NewLine + "Please Wait" + Environment.NewLine + "....";
                        }
                        if (uff == 50)
                        {
                            Out = "Setting Up.... " + Environment.NewLine + "Please Wait" + Environment.NewLine + "......";
                        }
                        if (uff == 60)
                        {
                            Out = "Setting Up.... " + Environment.NewLine + "Please Wait" + Environment.NewLine + "......";
                        }
                        if (uff == 70)
                        {
                            Out = "Setting Up.... " + Environment.NewLine + "Please Wait" + Environment.NewLine + "........";
                        }
                        if (uff == 70)
                        {
                            Out = "Setting Up.... " + Environment.NewLine + "Please Wait" + Environment.NewLine + "...........";
                        }
                        if (uff == 70)
                        {
                            Out = "Setting Up.... " + Environment.NewLine + "Please Wait" + Environment.NewLine + ".............";
                        }
                        if (uff == 80)
                        {
                            Out = "Setup Finished";
                        }

                        MainScreen.WriteText(Out, false);
                        uff++;              
                    } while (uff < 4);

                    MyGrid = Me.CubeGrid;
                    Setup = true;
                    FindBlocks();
                    AddMenus();
                    Echo("Setup Finished");
                    WriteToLog("Setup Finished");

                    //Out = "Setup Finished, Loading Menu";
                    //MainScreen.WriteText(Out, false);
                    ShowMenu();
                    return;
                }


            }
            return;
        }

        List<IMyThrust> AllThruster = new List<IMyThrust>();
        List<IMyTextPanel> AllLCD = new List<IMyTextPanel>();
        List<IMyReactor> AllReactors = new List<IMyReactor>();
        List<IMyLargeMissileTurret> AllMissleTurrets = new List<IMyLargeMissileTurret>();
        List<IMyLargeGatlingTurret> AllGattlingTurrets = new List<IMyLargeGatlingTurret>();
        List<IMyLargeInteriorTurret> AllInteriorTurrets = new List<IMyLargeInteriorTurret>();
        List<IMyGasTank> AllGasTanks = new List<IMyGasTank>();
        List<IMyBatteryBlock> AllBatterys = new List<IMyBatteryBlock>();
        List<IMySolarPanel> AllSolarPanels = new List<IMySolarPanel>();
        List<IMyCargoContainer> AllCargoContainers = new List<IMyCargoContainer>();
        List<IMyShipConnector> AllConnectors = new List<IMyShipConnector>();
        List<IMyLightingBlock> ALlLights = new List<IMyLightingBlock>();
        List<IMyDoor> AllDoors = new List<IMyDoor>();
        List<IMyAirtightHangarDoor> AllHangarDoors = new List<IMyAirtightHangarDoor>();
        List<IMyShipController> AllCockpits = new List<IMyShipController>();
        List<IMyGyro> AllGyros = new List<IMyGyro>();

        List<IMyTerminalBlock> allBlocks = new List<IMyTerminalBlock>();
        public void FindBlocks()
        {
            allBlocks.Clear();

            GridTerminalSystem.GetBlocks(allBlocks);

            foreach (IMyTerminalBlock Block in allBlocks)
                {

                if(Block is IMyThrust)
                {
                    IMyThrust Thruster = (IMyThrust)Block;

                    if (Block.IsSameConstructAs(Me))
                    {

                        AllThruster.Add(Thruster);
                    }

                }

                if(Block is IMyGyro)
                {
                    IMyGyro Gyro = (IMyGyro)Block;
                    if (Block.IsSameConstructAs(Me))
                    {

                        AllGyros.Add(Gyro);
                    }

                }

                if (Block is IMyShipController)
                {
                    IMyShipController Controller = (IMyShipController)Block;
                    if (Block.IsSameConstructAs(Me))
                    {

                        AllCockpits.Add(Controller);
                    }

                }






                if (Block is IMyTextPanel)
                {
                    IMyTextPanel LCD = (IMyTextPanel)Block;

                    if(Block.IsSameConstructAs(Me))
                    {

                        AllLCD.Add(LCD);
                    }

                }

                if (Block is IMyReactor)
                {
                    IMyReactor Reactor = (IMyReactor)Block;
                    if (Block.IsSameConstructAs(Me))
                    {

                        AllReactors.Add(Reactor);
                    }



                }

                if (Block is IMyLargeMissileTurret)
                {
                    IMyLargeMissileTurret MTurret = (IMyLargeMissileTurret)Block;
                    if (Block.IsSameConstructAs(Me))
                    {

                        AllMissleTurrets.Add(MTurret);
                    }
                }

                if (Block is IMyLargeGatlingTurret)
                {
                    IMyLargeGatlingTurret Gatlingt = (IMyLargeGatlingTurret)Block;
                    if (Block.IsSameConstructAs(Me))
                    {

                        AllGattlingTurrets.Add(Gatlingt);
                    }
                }

                if (Block is IMyLargeInteriorTurret)
                {
                    IMyLargeInteriorTurret IntTurret = (IMyLargeInteriorTurret)Block;
                    if (Block.IsSameConstructAs(Me))
                    {

                        AllInteriorTurrets.Add(IntTurret);
                    }
                }

                if (Block is IMyGasTank)
                {
                    IMyGasTank HydrogenTank = (IMyGasTank)Block;
                    if (Block.IsSameConstructAs(Me))
                    {

                        AllGasTanks.Add(HydrogenTank);
                    }
                }


                if (Block is IMyBatteryBlock)
                {
                    IMyBatteryBlock Battery = (IMyBatteryBlock)Block;
                    if (Block.IsSameConstructAs(Me))
                    {

                        AllBatterys.Add(Battery);
                    }
                }

                if (Block is IMySolarPanel)
                {
                    IMySolarPanel Solar = (IMySolarPanel)Block;
                    if (Block.IsSameConstructAs(Me))
                    {

                        AllSolarPanels.Add(Solar);
                    }
                }


                if (Block is IMyCargoContainer)
                {
                    IMyCargoContainer Cargo = (IMyCargoContainer)Block;
                    if (Block.IsSameConstructAs(Me))
                    {

                        AllCargoContainers.Add(Cargo);
                    }

                }

                if (Block is IMyShipConnector)
                {

                    IMyShipConnector Connector = (IMyShipConnector)Block;
                    if (Block.IsSameConstructAs(Me))
                    {

                        AllConnectors.Add(Connector);
                    }
                }


                if (Block is IMyLightingBlock)
                {
                    IMyLightingBlock Light = (IMyLightingBlock)Block;
                    if (Block.IsSameConstructAs(Me))
                    {

                        ALlLights.Add(Light);
                    }
                }

                if (Block is IMyDoor)
                {
                    IMyDoor Door = (IMyDoor)Block;
                    if (Block.IsSameConstructAs(Me))
                    {

                        AllDoors.Add(Door);
                    }
                }


                if (Block is IMyAirtightHangarDoor)
                {
                    IMyAirtightHangarDoor Gate = (IMyAirtightHangarDoor)Block;
                    if (Block.IsSameConstructAs(Me))
                    {

                        AllHangarDoors.Add(Gate);
                    }
                }



            }

            Echo("Blockfind Finished...");
            WriteToLog("Blockfind Finished...");
            WriteToLog("Blocks Found: " +allBlocks.Count);
            WriteToLog("List of Blocks:");
            WriteToLog("Thruster: " + AllThruster.Count);
            WriteToLog("LCD: " + AllLCD.Count);
            WriteToLog("Reactor: " + AllReactors.Count);
            WriteToLog("MissleTurrets: " + AllMissleTurrets.Count);
            WriteToLog("AllGattlingTurrets: " + AllGattlingTurrets.Count);
            WriteToLog("AllInteriorTurrets: " + AllInteriorTurrets.Count);
            WriteToLog("AllGasTanks: " + AllGasTanks.Count);
            WriteToLog("AllBatterys: " + AllBatterys.Count);
            WriteToLog("AllSolarPanels: " + AllSolarPanels.Count);
            WriteToLog("AllCargoContainers: " + AllCargoContainers.Count);
            WriteToLog("AllConnectors: " + AllConnectors.Count);
            WriteToLog("ALlLights: " + ALlLights.Count);
            WriteToLog("AllDoors: " + AllDoors.Count);
            WriteToLog("AllHangarDoors: " + AllHangarDoors.Count);
            WriteToLog("AllCockpits: " + AllCockpits.Count);
            WriteToLog("AllGyros: " + AllGyros.Count);


           // Menus.Add(new MenuStorage { MenuName = "Main", InfoType = "Menu", UpperChannel = "none", Subchannels = new List<Sub> { new Sub() { SubValue = "TEst1" }, new Sub() { SubValue = "TEst2" }, new Sub() { SubValue = "TEst3" }, new Sub() { SubValue = "TEst4" }, new Sub() { SubValue = "TEst4.1" }, new Sub() { SubValue = "TEst hidden " , Hidden = true }, new Sub() { SubValue = "TEst5" }, new Sub() { SubValue = "TEst6" }, new Sub() { SubValue = "TEst7" }, new Sub() { SubValue = "TEst8" } } });
           // MenuValueStorage.Add(new MValues { MenuName = "Main", Subchannels = new List<string> { new string(t) } })




            return;
        }



        #endregion

        #region ErrorHandling
        public void ErrorHandler(string ErrorText)
        {
            string Fault = "ERROR: " + ErrorText;
            Error = 1;
            Echo(Fault);
            if (MainScreen != null)
            {
                MainScreen.WriteText(ErrorText, false);
            }
            WriteToLog(ErrorText);
            CurrentMenu = "Main";
            CurrentPage = 0;
            CurrentPos = 0;
            CurrentStage = 0;
            ShowMenu();
            return;
        }

        #endregion

        #region Logger

        public void WriteToLog(string Text)
        {
            string Data = Me.CustomData;
            string[] Lines = Data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);




            string[] NewData = new string[Lines.Length +2];
            DateTime TimeNow = System.DateTime.Now;
            if (Lines.Length >= MaxLoggerRows -1)
            {
               
                if(Lines.Length > 0)
                {

                    Array.Copy(Lines, 1, NewData, 0, Lines.Length -1);
                }
                NewData[MaxLoggerRows] = TimeNow + ":" + Text + Environment.NewLine;

            }
            else
            {
                
                if (Lines.Length > 0)
                {
                    
                    Array.Copy(Lines, 0, NewData, 0, Lines.Length);
                }
                NewData[Lines.Length +1] = TimeNow + ":" + Text + Environment.NewLine;
            }

            //NewData[49] = TimeNow + ":" + Text + Environment.NewLine;
            
            Me.CustomData = string.Join(Environment.NewLine, NewData);
            return;
        }


        #endregion

        #region Menu

        string CurrentMenu = "Main";
        int CurrentStage = 0;
        int CurrentPos = 0;
        int CurrentPage = 0;

        List<MenuStorage> Menus= new List<MenuStorage>();
        List<MValues> MenuValueStorage = new List<MValues>();
        string[] Steps = new string[20];

        class MenuStorage
        {
            public string MenuName { get; set; }
            public string UpperChannel { get; set; }

            public string InfoType { get; set; }

            public List<Sub> Subchannels { get; set; } = new List<Sub>();

            public List<MSettings> Values { get; set; } = new List<MSettings>();

            public bool IsInfoPage { get; set; }

            public bool IsSetting { get; set; }

            public bool IsMenu { get; set; }

            public int MaxPages { get; set; }

            public int MaxMenus { get; set; }



        }

        class Sub
        {
            public string SubValue { get; set; }

            public bool Hidden { get; set; }
        }

        class MSettings
        {
            public string SetName { get; set; }

            public string SValue { get; set; }

            

            public int IValue { get; set; }

            public int IValueRange { get; set; }

            public bool BValue { get; set; }

            public bool CanBeDeleted { get; set; }

            public bool Hidden { get; set; }

        }




        class MValues
        {
            public string MenuName { get; set; }
            public List<Sub> Subchannels { get; set; } = new List<Sub>();

            public List<MSettings> Values { get; set; } = new List<MSettings>();

            public List<string> VSettings { get; set; } = new List<string>();


        }


        int Deletecounter = 0;
        int Deletecounter2 = 0;
        public void MMove(string Direction)
        {
            int Index = Menus.FindIndex(a => a.MenuName == CurrentMenu);
           // int MaxMenus = 0;
            //int MaxPages = 0;

            if (Index != -1)
            {

                int V1 = Menus[Index].Values[CurrentPos].IValue;
                bool V2 = Menus[Index].Values[CurrentPos].BValue;
                bool IsMenu = Menus[Index].IsMenu;
                string MenuPoint = Menus[Index].Values[CurrentPos].SetName;
                bool IsInfoSite = Menus[Index].IsInfoPage;
                bool IsSetting = Menus[Index].IsSetting;
                int MaxMenus = Menus[Index].MaxMenus;
                int MaxPages = Menus[Index].MaxPages;
                bool CanBeDeleted = Menus[Index].Values[CurrentPos].CanBeDeleted;
                /*
                if(Menus[Index].Values.Count != 0)
                {
                    MaxMenus = Menus[Index].Values.Count;

                }
                else if(Menus[Index].Subchannels.Count != 0)
                {
                    if (Menus[Index].Values.Count == 0)
                    {
                        MaxMenus = Menus[Index].Subchannels.Count;
                    }
                }
                Menus[Index].MaxMenus = MaxMenus;


                if (MaxMenus > Maxrows)
                {
                    MaxPages = MaxMenus / Maxrows;

                    Menus[Index].MaxPages = MaxPages;
                }
                else
                {
                    Menus[Index].MaxPages = 0;
                }
                */
                if(Deletecounter != 0)
                {
                    Deletecounter2 = 1;
                }

                switch (Direction)
                {
                    case "ENTER":
                        if(IsSetting == true)
                        {
                            int MaxRange = Menus[Index].Values[CurrentPos].IValueRange;
                            if (MaxRange != 0)
                            {
                                if (V1 != -1)
                                {

                                    if (V1 < MaxRange)
                                    {
                                        V1++;

                                    }
                                    else
                                    {
                                        V1 = 0;

                                    }
                                    Menus[Index].Values[CurrentPos].IValue = V1;
                                    break;
                                }
                                break;
                            }
                            else
                            {
                                
                                if (V2 == true)
                                {
                                    V2 = false;
                                }
                                else
                                {
                                    V2 = true;
                                }

                                Menus[Index].Values[CurrentPos].BValue = V2;
                                break;

                            }

                        }
                        else if(IsMenu == true)
                        {
                            Steps[CurrentStage] = CurrentMenu;
                            CurrentStage++;
                            CurrentPos = 0;
                            CurrentPage = 0;

                            CurrentMenu = MenuPoint;
                            break;
                        }
                        else if(CanBeDeleted == true)
                        {
                            Steps[CurrentStage] = CurrentMenu;
                            CurrentStage++;
                            Deletecounter++;
                            CurrentMenu = "Delete";
                            string Out = "Delete?" + Environment.NewLine + "Press ENTER to Delete" + Environment.NewLine + "Press BACK to go Back" + Environment.NewLine;
                            MainScreen.WriteText(Out, false);
                            break;
                        }
                        if (Deletecounter2 != 0)
                        {
                            if (CurrentMenu == "Delete")
                            {
                                if (Deletecounter != 0)
                                {
                                    Deletecounter = 0;
                                    Deletecounter2 = 0;
                                    Warnings.RemoveAt(CurrentPos);
                                    CurrentMenu = "Main";
                                    CurrentPage = 0;
                                    CurrentPos = 0;
                                    break;
                                }

                            }

                        }

                            break;
                    case "BACK":
                        if (CurrentStage > 0)
                        {
                            WriteToLog("DEBUG: BACK");
                            string Type = Menus[Index].InfoType;
                            if (Type != null)
                            {
                                WriteToLog("DEBUG: Deleted menu");
                                Menus[Index].Values.Clear();
                            }
                            CurrentStage--;
                            CurrentMenu = Steps[CurrentStage];
                            CurrentPos = 0;
                            CurrentPage = 0;
                            
                            break;

                        }

                        break;
                    case "UP":
                        if(CurrentPos > 0)
                        {
                            if(IsInfoSite == false)
                            {
                                CurrentPos--;
                            }


                            break;
                        }
                        else if(CurrentPage > 0)
                        {
                            if(CurrentPos == 0)
                            {
                                CurrentPage--;
                                CurrentPos = 0;

                            }

                            break;
                        }
                        break;

                    case "DOWN":
                        int MaxMenu = Menus[Index].Values.Count;

                        if (IsInfoSite == false)
                        {

                            if (CurrentPos < MaxMenus -1)
                            {

                                CurrentPos++;
                                break;
                            }
                            else
                            {
                                if(CurrentPage < MaxPages -1)
                                {
                                    CurrentPage++;
                                    CurrentPos = 0;
                                    break;

                                }
                                break;
                            }


                        }
                        else if(IsInfoSite == true)
                        {
                            if(CurrentPage < MaxPages)
                            {
                                CurrentPage++;
                                break;
                            }
                            break;
                        }

                       

                        break;

                }
                WriteToLog("DEBUG: CurrentPos: " + CurrentPos);
                WriteToLog("DEBUG: CurrentPage: " + CurrentPage);
                WriteToLog("DEBUG: MaxMenus:  " + MaxMenus);
                WriteToLog("DEBUG: MaxPages:  " + MaxPages);
                WriteToLog("DEBUG: CurrentMenu:  " + CurrentMenu);
                WriteToLog("DEBUG: Currentstep:  " + CurrentStage);
                WriteToLog("DEBUG: VALUES COUNT: " + Menus[Index].Values.Count);
                WriteToLog("DEBUG:  menu: " + Steps[CurrentStage]);
               // WriteToLog("DEBUG: forwards menu: " + Steps[CurrentStage -1]);
                //WriteToLog("DEBUG: back menu: " + Steps[CurrentStage + 1]);

                ShowMenu();

            }
            return;
        }

        public void ShowMenu()
        {
            int Index = Menus.FindIndex(a => a.MenuName == CurrentMenu);
            bool IsMenu = false;
            bool IsInfoPage = false;
            bool IsSetting = false;
            string Out = "";
            int MaxMenus = 0;
            int MaxPages = 0;

            if (Index != -1)
            {
                

                string Type = Menus[Index].InfoType;
                WriteToLog("DEBUG: Type: " + Type);

                if (Type == "Energy")
                {
                    GetEnergyData();
                }
                else if (Type == "Sotrage")
                {
                    GetStorageData();
                }
                else if (Type == "Connector")
                {
                    GetConnectorData();
                }
                else if (Type == "Hydrogen")
                {
                    GetHydrogenData();
                }
                else if (Type == "Warning")
                {
                    Menus[Index].Values.Clear();
                    foreach (WarningValue Warn in Warnings)
                    {
                        Menus[Index].Values.Add(new MSettings() { SetName = Warn.From, SValue = Warn.Warning, CanBeDeleted = true }); ;

                    }
                }

                if (Menus[Index].Values.Count != 0)
                {
                    MaxMenus = Menus[Index].Values.Count;

                }
                else if (Menus[Index].Subchannels.Count != 0)
                {
                    if (Menus[Index].Values.Count == 0)
                    {
                        MaxMenus = Menus[Index].Subchannels.Count;
                    }
                }
                Menus[Index].MaxMenus = MaxMenus;


                if (MaxMenus > Maxrows)
                {
                    MaxPages = MaxMenus / Maxrows;

                    Menus[Index].MaxPages = MaxPages;
                }
                else
                {
                    Menus[Index].MaxPages = 0;
                }

                IsMenu = Menus[Index].IsMenu;
                IsInfoPage = Menus[Index].IsInfoPage;
                IsSetting = Menus[Index].IsSetting;
                MaxMenus = Menus[Index].MaxMenus;
                MaxPages = Menus[Index].MaxPages;

                Out = CurrentMenu + "    " + DateTime.Now;
                int C1 = 0;
                if (CurrentPage == 0)
                {
                    C1 = 0;
                } 
                else
                {
                    C1 = CurrentPage * Maxrows - Maxrows;
                }


                if (Menus[Index].Values.Count > 0)
                {
                    do
                    {
                        MSettings Value = Menus[Index].Values[C1];
                        if (IsMenu == true)
                        {
                            if (C1 == CurrentPos)
                            {
                                if (Value.Hidden == false)
                                {
                                    Out = Out + Environment.NewLine + Value.SetName + "<-----";
                                }

                            }
                            else
                            {
                                if (Value.Hidden == false)
                                {
                                    Out = Out + Environment.NewLine + Value.SetName;
                                }

                            }
                        }
                        else if (IsInfoPage == true)
                        {
                            if (Value.Hidden == false)
                            {
                                Out = Out + Environment.NewLine + Value.SetName + Environment.NewLine + Value.SValue;
                            }

                        }
                        else if (IsSetting == true)
                        {
                            if (C1 == CurrentPos)
                            {
                                if (Value.Hidden == false)
                                {
                                    Out = Out + Environment.NewLine + Value.SetName + "   " + Value.BValue + "<----";
                                }
                            }
                            else
                            {
                                if(Value.Hidden == false)
                                {
                                    Out = Out + Environment.NewLine + Value.SetName + "   " + Value.SValue;
                                }
                            }

                        }


                        if (C1 == MaxMenus)
                        {
                            break;
                        }
                        C1++;
                    } while (C1 <= MaxMenus - 1);

                    MainScreen.WriteText(Out, false);

                    return;

                }
                else
                {
                    ErrorHandler("Menu is Empty!");
                }

            }
            else
            {
                ErrorHandler(" Menu not Found");
                MMove("BACK");
                return;
            }


        }


        public void AddMenus()
        {
            Menus.Add(new MenuStorage { MenuName = "Delete", IsInfoPage = true, Values = new List<MSettings> { new MSettings() { SetName = "DELETE?", SValue = "ENTER to Delete, BACK to go back" } } });
            Menus.Add(new MenuStorage { MenuName = "Main", IsMenu = true, Values = new List<MSettings> { new MSettings() { SetName = "Warnings" }, new MSettings() { SetName = "Energy" }, new MSettings() { SetName = "Cargo" }, new MSettings() { SetName = "Connectors" }, new MSettings() { SetName = "Airlocks" }, new MSettings() { SetName = "MainSettings", Hidden = true } } });
            Menus.Add(new MenuStorage { MenuName = "Warnings", IsInfoPage = true, InfoType = "Warning" });
            Menus.Add(new MenuStorage { MenuName = "Energy" , IsMenu = true, Values = new List<MSettings> { new MSettings() { SetName = "EnergyInfos"}, new MSettings() { SetName = "EnergySettings" } } });
            Menus.Add(new MenuStorage { MenuName = "EnergyInfos", IsInfoPage = true, InfoType = "Energy" });
            Menus.Add(new MenuStorage { MenuName = "EnergySettings", IsSetting = true, InfoType = "EnergySetting" });
        }


        public void ShowDataOnOtherScreen()
        {

        }


        #endregion

        #region GetData

        public int GetPercent(float Max, float Current)
        {
            int Out = 0;
            float One = Max / 100;
            float two = (Current / One);

            Out = Convert.ToInt32(two);

            return Out;
        }


        public string ReturnIndicator(int Percent)
        {

            string Out = "[";
            int Mathe = 0;

            Mathe = (Percent / 5);
            //Mathe = Math.Round(Mathe);
            int I = 0;
            do
            {
                if (I < Mathe)
                {
                    Out = Out + "|";
                }
                else
                {
                    Out = Out + " ";
                }
                I++;

            } while (I < 20);
            Out = Out + "]";

            // string Out = "";
            return Out;
        }






        public void GetEnergyData()
        {

            int Index = Menus.FindIndex(a => a.MenuName == CurrentMenu);
            if (Index != -1)
            {
                Menus[Index].Values.Clear();




                //Reacors
                int ReactorCount = AllReactors.Count;
                int RRunning = 0;
                float ROutput = 0;
                float RMaxOut = 0;

                foreach (IMyReactor reactor in AllReactors)
                {
                    if (reactor.Enabled)
                    {
                        RRunning++;
                        ROutput = ROutput + reactor.CurrentOutput;
                    }
                    RMaxOut = RMaxOut + reactor.MaxOutput;


                }
                int RPercent = GetPercent(RMaxOut, ROutput);
                string ROutputIndi = ReturnIndicator(RPercent);


                Menus[Index].Values.Add(new MSettings { SetName = "Reactors Running:", SValue = RRunning.ToString() });
                Menus[Index].Values.Add(new MSettings { SetName = "Reactor Output", SValue = ROutputIndi });




                //BAtterys

                foreach(IMyBatteryBlock myBattery in AllBatterys)
                {


                }



            }
        }

        public void GetEnergySetting()
        {


        }

        public void GetStorageData()
        {

        }

        public void GetConnectorData()
        {

        }

        public void GetHydrogenData()
        {

        }






        #endregion


        #region Arirlock
        public void FindAirLock()
        {

        }

        public void CycleAirlock(string AirLockName)
        {

        }

        #endregion

        public void EmergStartThruster()
        {

        }



    }
}

