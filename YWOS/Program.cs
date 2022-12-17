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

        //INFO:ModulName = Energy;
        //SETTING:Blala = 20:10|20|30;
        //INFO:EnergyUse = 20;


        #region settings
        //settings
        string MainLCDName = "MainLCD"; //LCD
        string MainControllerName = "MainController"; //button Panel
        int MaxLoggerRows = 200;//Set this too high will cause lag
        //Setting end ----> DONT EDIT BELOW
        #endregion

        #region Private
        double Version = 3.002;
        double UVersion = 0.1;
        int Error = 0;
        string ErrorText = "";
        bool Setup = false;
        IMyTextPanel MainScreen;
        IMyButtonPanel MainControll;
        int Tick = 0;
        int Maxtick = 2000;
        IMyCubeGrid MyGrid;
        int Maxrows = 11;//max rows per page
        bool EnergyOverride = false;
        bool EnergySaver = false;
        int MinBatEnergy = 0;
        int ReactorMode = 2;
        int BatteryMode = 4;
        int SolarMode = 2;
        string ModulBlockName = "M1337";
        List<WarningValue> Warnings = new List<WarningValue>();

        class WarningValue
        {
            public string Warning { get; set; }

            public string From { get; set; }


        }

        class ModulInfo
        {
            public string ModulName { get; set; }
            
            public IMyProgrammableBlock Block { get; set; }

            public List<Modulsettings> Settings { get; set; } = new List<Modulsettings>();

            public List<ModuleValues> Values { get; set; } = new List<ModuleValues>();

        }

        class Modulsettings
        {
            public string Name { get; set; }

            public string Value { get; set; }

            public string Range { get; set; }


        }

        class ModuleValues
        {
            public string VName { get; set; }

            public string VValue { get; set; }
        }



        #endregion


        #region Main
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

            if (Input != null || Input != "")
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
            DoEveryTime();
            ShowMenu();
            
            return;
        }


        #endregion

        #region Turnaroundaroundaroundaround

        public void DoEveryTime()
        {
            
        Tick++;
        if(Tick >= Maxtick)
        {
        Tick = -1;
        }

            

            if(Tick == 0)
            {
                ReloadModulesSettings();
                ReloadModulValues();

            }





        }

        #endregion


        
        #region Startup/Findblocks/Modules


        public void Startup()
        {
            if (Setup == false)
            {
                Echo("Starting Startup....");
                WriteToLog("Starting Startup....");
                WriteToLog("Serching for Screen and Controller..");
                Echo("Serching for Screen and Controller..");
                MainScreen = (IMyTextPanel)GridTerminalSystem.GetBlockWithName(MainLCDName);
                MainControll = (IMyButtonPanel)GridTerminalSystem.GetBlockWithName(MainControllerName);
                if (MainScreen == null || MainControll == null)
                {
                    if (MainScreen == null)
                    {
                        ErrorHandler("No MainScreen Found");
                    }

                    if (MainControll == null)
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
                        if (uff == 30)
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
        List<ModulInfo> Modules = new List<ModulInfo>();


        List<IMyTerminalBlock> allBlocks = new List<IMyTerminalBlock>();
        public void FindBlocks()
        {
            allBlocks.Clear();

            GridTerminalSystem.GetBlocks(allBlocks);
            foreach (IMyTerminalBlock Block in allBlocks)
            {

                if(Block is IMyProgrammableBlock)
                {
                    if(Block.IsSameConstructAs(Me))
                    {
                        if (Block.CustomName.Contains(ModulBlockName))
                        {
                            WriteToLog("Debug: Modul Found!");
                            string Modulname = "";
                            string CustomData = Block.CustomData;
                            string CustomDataAll = CustomData.Replace(" ", "");
                            string[] Data = CustomDataAll.Split(';');
                            if (Data.Length > 1)
                            {
                               
                                foreach (string CData in Data)
                                {
                                    WriteToLog("DEBUG: Data: " + CData);

                                    string[] Sdata = CData.Split('=');
                                    if (Sdata.Length == 2)
                                    {
                                        WriteToLog("DEBUG: SData = " + Sdata[0]);
                                        WriteToLog("DEBUG: SData2 = " + Sdata[1]);
                                        string[] firstData = Sdata[0].Split(':');
                                        WriteToLog("DEBUG: Firstdata1 " + firstData[1]);
                                        if (firstData[1].Contains("ModulName"))
                                        {
                                            WriteToLog("Debug: Modulname found!");
                                            Modulname = Sdata[1];
                                            WriteToLog("Modulname: " + Modulname);
                                            break;
                                        }
                                    }

                            }

                                if (Modulname != "")
                                {
                                    Modules.Add(new ModulInfo { Block = (IMyProgrammableBlock)Block, ModulName = Modulname });
                                }
                                WriteToLog("DEBUG: Modules1: " + Modules.Count);
                                WriteToLog("DEBUG: MODULANE: " + Modules[0].ModulName);
                                int Index = Modules.FindIndex(a => a.ModulName == Modulname);
                                WriteToLog("DEBUG: Indes: " + Index);
                                if (Index != -1)
                                {
                                    WriteToLog("DEBUG: NameFound!");
                                    if (Data.Length > 1)
                                    {
                                        foreach (string CData in Data)
                                        {
                                            //INFO:ModuleName = Energy;
                                            //SETTING:Blala = 20:10|20|30;
                                            string[] Sdata = CData.Split('=');
                                            if (Sdata.Length == 2)
                                            {
                                                string[] TempData = Sdata[0].Split(':');
                                                if (TempData.Length > 1)
                                                {
                                                    if (TempData[0] == "INFO")
                                                    {
                                                        Modules[Index].Values.Add(new ModuleValues { VName = Sdata[0], VValue = Sdata[1] });
                                                        WriteToLog("DEBUG: values Count: " + Modules[Index].Values.Count);

                                                    }
                                                    else if (TempData[0] == "SETTING")
                                                    {
                                                        string[] TempData2 = Sdata[1].Split(':');
                                                        if (TempData2.Length > 2)
                                                        {
                                                            if (TempData2.Length > 1)
                                                            {
                                                                Modules[Index].Settings.Add(new Modulsettings { Name = TempData[1], Value = TempData2[0], Range = TempData2[1] });
                                                                WriteToLog("DEBUG: Setting Count: " + Modules[Index].Settings.Count);
                                                                
                                                            }
                                                        }
                                                        else
                                                        {
                                                            WriteToLog("Wrong Setting Lenght!");
                                                            return;
                                                        }
                                                        
                                                    }
                                                    else
                                                    {
                                                        WriteToLog("KatError");
                                                        hier //Rausfinden warum wir hier landen
                                                        return;
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    WriteToLog("Modulname not found!");
                                }
                            }
                        }


                    }

                }
               

            }

            // Menus.Add(new MenuStorage { MenuName = "Main", InfoType = "Menu", UpperChannel = "none", Subchannels = new List<Sub> { new Sub() { SubValue = "TEst1" }, new Sub() { SubValue = "TEst2" }, new Sub() { SubValue = "TEst3" }, new Sub() { SubValue = "TEst4" }, new Sub() { SubValue = "TEst4.1" }, new Sub() { SubValue = "TEst hidden " , Hidden = true }, new Sub() { SubValue = "TEst5" }, new Sub() { SubValue = "TEst6" }, new Sub() { SubValue = "TEst7" }, new Sub() { SubValue = "TEst8" } } });
            // MenuValueStorage.Add(new MValues { MenuName = "Main", Subchannels = new List<string> { new string(t) } })

            AddMenus();
            WriteToLog("DEBUG: Modul Count: " + Modules.Count);


            return;
        }

        public void ReloadModulesSettings()
        {
            //SETTING:Blala = 20:10|20|30;

            foreach (ModulInfo MInfo in Modules)
            {
                IMyProgrammableBlock Block = MInfo.Block;
                string SettingName = MInfo.ModulName + " Settings";
                int Index3 = Menus.FindIndex(a => a.MenuName == SettingName);
                if(Index3 != -1)
                {
                    Menus[Index3].Values.Clear();
                }
                string CustomDataAll = Block.CustomData;
                string[] Data = CustomDataAll.Split(';');
                if (Data.Length > 1)
                {

                    foreach (string CData in Data)
                    {
                        //INFO:ModuleName = Energy;
                        string[] Sdata = CData.Split('=');
                        if (Sdata.Length == 2)
                        {
                            string[] TempData = Sdata[0].Split(':');


                            if (TempData[0] == "SETTING")
                            {
                                string[] TempData2 = Sdata[1].Split(':');
                                if (TempData2.Length > 1)
                                {
                                    MInfo.Settings.Add(new Modulsettings { Name = TempData[1], Value = TempData2[0], Range = TempData2[1] });


                                   Menus[Index3].Values.Add(new MSettings { SetName = TempData[1], SValue = TempData2[0], SRange = TempData2[1] });

                                    




                                }
                            }


                        }

                    }



                }
            }

            WriteToLog("CatData Settings Updated..");





            return;
        }



        public void ReloadModulValues()
        {
            //INFO:EnergyUse = 20;
            foreach (ModulInfo MInfo in Modules)
            {
                string ValueName = MInfo + " Data";
                int Index3 = Menus.FindIndex(a => a.MenuName == ValueName);
                if(Index3 != -1)
                {
                    Menus[Index3].Values.Clear();
                }

                IMyProgrammableBlock Block = MInfo.Block;
                string CustomDataAll = Block.CustomData;
                string[] Data = CustomDataAll.Split(';');
                if (Data.Length > 1)
                {

                    foreach (string CData in Data)
                    {
                        //INFO:ModuleName = Energy;
                        string[] Sdata = CData.Split('=');
                        if (Sdata.Length == 2)
                        {
                            string[] TempData = Sdata[0].Split(':');

                            if (TempData.Length > 1)
                            {
                                if (TempData[0] == "INFO")
                                {

                                    MInfo.Values.Add(new ModuleValues { VName = TempData[1], VValue = Sdata[1] });

                                    Menus[Index3].Values.Add(new MSettings { SetName = TempData[1], SValue = Sdata[1] });

                                }
                            }

                        }

                    }



                }
            }
            WriteToLog("CatData Values Updated..");

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




            string[] NewData = new string[Lines.Length + 2];
            DateTime TimeNow = System.DateTime.Now;
            if (Lines.Length >= MaxLoggerRows - 1)
            {

                if (Lines.Length > 0)
                {

                    Array.Copy(Lines, 1, NewData, 0, Lines.Length - 1);
                }
                NewData[MaxLoggerRows] = TimeNow + ":" + Text + Environment.NewLine;

            }
            else
            {

                if (Lines.Length > 0)
                {

                    Array.Copy(Lines, 0, NewData, 0, Lines.Length);
                }
                NewData[Lines.Length + 1] = TimeNow + ":" + Text + Environment.NewLine;
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

        List<MenuStorage> Menus = new List<MenuStorage>();
        //List<MValues> MenuValueStorage = new List<MValues>();
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

            public string SetText { get; set; }

            public bool NOValue { get; set; }

            public string SValue { get; set; }

            public string SRange { get; set; }

            public int IValue { get; set; }

            public int IValueRange { get; set; }


            public bool BValue { get; set; }

            public bool CanBeDeleted { get; set; }

            public bool Hidden { get; set; }

        }



        /*
        class MValues
        {
            public string MenuName { get; set; }
            public List<Sub> Subchannels { get; set; } = new List<Sub>();

            public List<MSettings> Values { get; set; } = new List<MSettings>();

            public List<string> VSettings { get; set; } = new List<string>();


        }
        */

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
                if (Deletecounter != 0)
                {
                    Deletecounter2 = 1;
                }

                switch (Direction)
                {
                    case "ENTER":
                        if (IsSetting == true)
                        {
                            if (Menus[Index].Values[CurrentPos].NOValue == false)
                            {
                                string Range1 = Menus[Index].Values[CurrentPos].SRange;
                                string[] Range2 = Range1.Split('|');
                                string CurrentValue = Menus[Index].Values[CurrentPos].SValue;
                                int temp = Array.IndexOf(Range2, CurrentValue);
                                temp++;

                                if (temp > Range2.Length - 1)
                                {
                                    temp = 0;
                                }

                                Menus[Index].Values[CurrentPos].SValue = Range2[temp];
                                break;
                            }
                            else
                            {
                                break;
                            }

                        }
                        else if (IsMenu == true)
                        {
                            Steps[CurrentStage] = CurrentMenu;
                            CurrentStage++;
                            CurrentPos = 0;
                            CurrentPage = 0;

                            CurrentMenu = MenuPoint;

                            int Index33 = Menus.FindIndex(a => a.MenuName == CurrentMenu);

                            if (Index33 != -1)
                            {
                                if (Menus[Index].Values[CurrentPos].NOValue == true || Menus[Index].Values[CurrentPos].Hidden == true)
                                {
                                    WriteToLog("DEBUG: MOVE DOWN ENTER!");
                                    MMove("DOWN");

                                }
                            }

                            break;
                        }
                        else if (CanBeDeleted == true)
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
                            /*
                            string Type = Menus[Index].InfoType;
                            if (Type != null)
                            {
                                Menus[Index].Values.Clear();
                            }
                            */
                            CurrentStage--;
                            CurrentMenu = Steps[CurrentStage];
                            CurrentPos = 0;
                            CurrentPage = 0;


                            int Index33 = Menus.FindIndex(a => a.MenuName == CurrentMenu);

                            if (Index33 != -1)
                            {
                                if (Menus[Index].Values[CurrentPos].NOValue == true || Menus[Index].Values[CurrentPos].Hidden == true)
                                {
                                    WriteToLog("DEBUG: MOVE DOWN back!");
                                    MMove("DOWN");

                                }
                            }

                            break;

                        }

                        break;
                    case "UP":
                        if (CurrentPos > 0)
                        {
                            if (IsInfoSite == false)
                            {
                                CurrentPos--;

                                if(Menus[Index].Values[CurrentPos].NOValue == true || Menus[Index].Values[CurrentPos].Hidden == true)
                                {
                                    WriteToLog("DEBUG: MOVE UP1!");
                                    MMove("UP");
                                    
                                }

                            }
                            else
                            {
                                if (CurrentPage > 0)
                                {
                                    CurrentPage--;
                                    CurrentPos--;
                                }

                            }

                            break;
                        }
                        else if (CurrentPage > 0)
                        {
                            if (CurrentPos == 0)
                            {
                                CurrentPage--;
                                CurrentPos --;

                                if (Menus[Index].Values[CurrentPos].NOValue == true || Menus[Index].Values[CurrentPos].Hidden == true)
                                {
                                    WriteToLog("DEBUG: MOVE UP2!");
                                    MMove("UP");
                                }

                            }



                           break;
                        }

                        break;

                    case "DOWN":
                        int MaxMenu = Menus[Index].Values.Count;

                        if (IsInfoSite == false)
                        {

                            if (CurrentPos < MaxMenus - 1)
                            {

                                CurrentPos++;

                                if (Menus[Index].Values[CurrentPos].NOValue == true || Menus[Index].Values[CurrentPos].Hidden == true)
                                {
                                    WriteToLog("DEBUG: MOVE DOWN!");
                                    MMove("DOWN");
                                }

                                break;
                            }
                            else
                            {
                                if (CurrentPage < MaxPages - 1)
                                {
                                    CurrentPage++;
                                    CurrentPos++;

                                    if (Menus[Index].Values[CurrentPos].NOValue == true || Menus[Index].Values[CurrentPos].Hidden == true)
                                    {
                                        WriteToLog("DEBUG: MOVE DOWN2!");
                                        MMove("DOWN");
                                    }
                                    break;

                                }
                                break;
                            }


                        }
                        else if (IsInfoSite == true)
                        {
                            if (CurrentPage < MaxPages)
                            {
                                CurrentPage++;
                                CurrentPos++;
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
               
                /*
                if(Type != null)
                {
                    Menus[Index].Values.Clear();

                }
                */
                /*
                if (Type == "Energy")
                {
                   // GetEnergyData(true);
                }
                else if (Type == "Sotrage")
                {
                   // GetStorageData();
                }
                else if (Type == "Connector")
                {
                   // GetConnectorData();
                }
                else if (Type == "Hydrogen")
                {
                   // GetHydrogenData();
                }
                */
                if (Type == "Warning")
                {

                        foreach (WarningValue Warn in Warnings)
                        {
                            Menus[Index].Values.Add(new MSettings() { SetName = Warn.From, SValue = Warn.Warning, CanBeDeleted = true }); ;

                        }

                }
                /*
                else if(Type == "EnergySetting")
                {
                    //GetEnergySetting();
                }
                */


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
                                    /*
                                    if(Value.IValueRange != 0)
                                    {
                                        Out = Out + Environment.NewLine + Value.SetName + "   " + Value.IValue + "<----";
                                    }
                                    */
                                    if (Value.NOValue == false)
                                    {

                                    Out = Out + Environment.NewLine + Value.SetText + "   " + Value.SValue + "<----";
                                        
                                    }
                                    else
                                    {

                                      Out = Out + Environment.NewLine + Value.SetText + "<----";
                                        

                                    }
                                    /*
                                    else
                                    {
                                        Out = Out + Environment.NewLine + Value.SetName + "   " + Value.BValue + "<----";
                                    }
                                    */
                                    
                                }
                            }
                            else
                            {
                                if (Value.Hidden == false)
                                {
                                    if (Value.NOValue == false)
                                    {
                                        Out = Out + Environment.NewLine + Value.SetText + "   " + Value.SValue;
                                    }
                                    else
                                    {
                                        Out = Out + Environment.NewLine + Value.SetText ;
                                    }

                                    
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
                    Out = Out + Environment.NewLine + "Menu is Empty!";
                    MainScreen.WriteText(Out, false);
                    MMove("BACK");
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
            Menus.Add(new MenuStorage { MenuName = "Main", IsMenu = true, Values = new List<MSettings> { new MSettings() { SetName = "Warnings" } } });
            Menus.Add(new MenuStorage { MenuName = "Warnings", IsInfoPage = true, InfoType = "Warning" });
            
            int Index = Menus.FindIndex(a => a.MenuName == "Main");
            if (Index != -1)
            {
                foreach (ModulInfo Modul in Modules)
                {
                    string Modulname = Modul.ModulName;
                    Menus[Index].Values = new List<MSettings> { new MSettings() { SetName = Modulname } };



                }
            }
            else
            {
                WriteToLog("Something went Wrong whit Menu adding");
                return;
            }

            foreach(MSettings Menu in Menus[Index].Values)
            {
                string SettingName = Menu.SetName + " Settings";
                string ValueName = Menu.SetName + " Data";
                Menus.Add(new MenuStorage { MenuName = Menu.SetName, IsMenu = true, Values = new List<MSettings> { new MSettings() { SetName = ValueName }, new MSettings() { SetName = SettingName } } });

                Menus.Add(new MenuStorage { MenuName = ValueName, IsInfoPage = true });
                Menus.Add(new MenuStorage { MenuName = SettingName, IsSetting = true});
                int Index4 = Modules.FindIndex(a => a.ModulName == Menu.SetName);
                int Index2 = Menus.FindIndex(a => a.MenuName == ValueName);
                if(Index2 != -1)
                {
                   if(Index4 != -1)
                    {
                        //VALUE!!!

                        //Menus[Index].Values.Add(new MSettings { SetName = "UranSaver", SetText = "Enable UranSaver", SValue = "ON", SRange = "ON|OFF" });
                        foreach (ModuleValues Value in  Modules[Index4].Values)
                        {
                            Menus[Index2].Values.Add(new MSettings { SetName = Value.VName, SValue = Value.VValue });

                        }


                    }
                }
                int Index3 = Menus.FindIndex(a => a.MenuName == SettingName);
                if(Index3 != -1)
                {
                    if (Index4 != -1)
                    {
                        //SETTINGS!!

                        foreach (Modulsettings Setting in Modules[Index4].Settings)
                        {
                            Menus[Index3].Values.Add(new MSettings { SetName = Setting.Name, SValue = Setting.Value, SRange = Setting.Range });

                        }


                    }
                }



            }




           // Menus.Add(new MenuStorage { MenuName = "Energy", IsMenu = true, Values = new List<MSettings> { new MSettings() { SetName = "EnergyInfos" }, new MSettings() { SetName = "EnergySettings" } } });
           // Menus.Add(new MenuStorage { MenuName = "EnergyInfos", IsInfoPage = true, InfoType = "Energy" });
           // Menus.Add(new MenuStorage { MenuName = "EnergySettings", IsSetting = true, InfoType = "EnergySetting" });

            return;
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



        /*
        int ReactorCount = 0;
        int RRunning = 0;
        float ROutput = 0;
        float RMaxOut = 0;
        int BRunning = 0;
        float BOutput = 0;
        float BMaxOutput = 0;
        float BInput = 0;
        float BMaxInput = 0;
        float BLoad = 0;
        float BMaxLoad = 0;
        int SRunning = 0;
        float SOutput = 0;
        float SMaxOutput = 0;
        int AllPowerMaxOutput = 0;
        int AllPowerOutput = 0;
        int ALLPercent = 0;
        string AllIndicator = "";
        string AllOut = "";
        */
        /*
        public void GetEnergyData(bool FromMenu)
        {

            int Index = Menus.FindIndex(a => a.MenuName == CurrentMenu);
            if (Index != -1)
            {
                Menus[Index].Values.Clear();



                
                //Reacors
                 ReactorCount = AllReactors.Count;
                RRunning = 0;
                ROutput = 0;
                 RMaxOut = 0;

                foreach (IMyReactor reactor in AllReactors)
                {
                    if (reactor.Enabled == true)
                    {
                        RRunning++;
                        ROutput = ROutput + reactor.CurrentOutput;
                    }
                    RMaxOut = RMaxOut + reactor.MaxOutput;


                }
                int RPercent = GetPercent(RMaxOut, ROutput);
                string ROutputIndi = ReturnIndicator(RPercent);
                int ROutputInt = Convert.ToInt32(ROutput);
                int RMaxOutInt = Convert.ToInt32(RMaxOut);
                string RInfo = ROutputInt + "MW " + ROutputIndi + " " + RMaxOutInt + "MW ";



                //Menus[Index].Values.Add(new MSettings { SetName = "Reactors Running:", SValue = RRunning.ToString() });
                //Menus[Index].Values.Add(new MSettings { SetName = "Reactor Output", SValue = RInfo });

                
                //BAtterys
                BRunning = 0;
                BOutput = 0;
                BMaxOutput = 0;
                BInput = 0;
                BMaxInput = 0;
                BLoad = 0;
                BMaxLoad = 0;

                foreach (IMyBatteryBlock myBattery in AllBatterys)
                {
                    if (myBattery.Enabled == true)
                    {
                        BRunning++;
                        BOutput = BOutput + myBattery.CurrentOutput;
                        BMaxOutput = BMaxOutput + myBattery.MaxOutput;
                        BInput = BInput + myBattery.CurrentInput;
                        BMaxInput = BMaxInput + myBattery.MaxInput;
                        BLoad = BLoad + myBattery.CurrentStoredPower;
                        BMaxLoad = BMaxLoad + myBattery.MaxStoredPower;

                    }



                }
                //int BInputPercent = GetPercent(BMaxInput, BInput);
                //int BOutputPercent = GetPercent(BMaxOutput, BOutput);
                //string BInputIndicator = ReturnIndicator(BInputPercent);
                //string BOutputIndicator = ReturnIndicator(BOutputPercent);

                int BLoadPercent = GetPercent(BMaxLoad, BLoad);
                string BloadIndicator = ReturnIndicator(BLoadPercent);
                int BInputInt = Convert.ToInt32(BInput);
                int BMaxInputInt = Convert.ToInt32(BMaxInput);
                int BOutputInt = Convert.ToInt32(BOutput);
                int BMaxOutputInt = Convert.ToInt32(BMaxOutput);


                string BLoading = BLoadPercent + "% " + BLoad + "MWh " + BloadIndicator + " " + BMaxLoad + "MWh ";
                string BIN = BInputInt + " / " + BMaxInputInt + " -- " + BOutputInt + " / " + BMaxOutputInt;
                //string Bout = BOutput + " " + BOutputIndicator + " " + BMaxOutput;

                // Menus[Index].Values.Add(new MSettings { SetName = "--------------------"});
                //Menus[Index].Values.Add(new MSettings { SetName = "Battery Active", SValue = BRunning.ToString() });
                //Menus[Index].Values.Add(new MSettings { SetName = "Battery Active/Load", SValue = BLoading });
                //Menus[Index].Values.Add(new MSettings { SetName = "Battery Input -- Output", SValue =  BIN});
                //Menus[Index].Values.Add(new MSettings { SetName = "Battery Output", SValue = Bout});

                
                //solar
                SRunning = 0;
                 SOutput = 0;
                 SMaxOutput = 0;


                foreach (IMySolarPanel Solar in AllSolarPanels)
                {
                    if (Solar.Enabled == true)
                    {
                        SRunning++;
                        SOutput = SOutput + Solar.CurrentOutput;
                        SMaxOutput = SMaxOutput + Solar.MaxOutput;

                    }

                }

                int SPercent = GetPercent(SMaxOutput, SOutput);
                string SIndocator = ReturnIndicator(SPercent);

                int SOutputInt = Convert.ToInt32(SOutput);
                int SMaxOutputInt = Convert.ToInt32(SMaxOutput);

                string SOut = SOutputInt + "MW " + SIndocator + " " + SMaxOutputInt + "MW ";

                // Menus[Index].Values.Add(new MSettings { SetName = "Solar: Active --- Output/MaxOutput ",SValue = SOut });

                
                AllPowerMaxOutput = BMaxOutputInt + RMaxOutInt + SMaxOutputInt;
                AllPowerOutput = BOutputInt + ROutputInt + SOutputInt;
                ALLPercent = GetPercent(AllPowerMaxOutput, AllPowerOutput);
                AllIndicator = ReturnIndicator(ALLPercent);
                AllOut = AllPowerOutput + "MW " + AllIndicator + " " + AllPowerMaxOutput + "MW ";

                if (FromMenu == true)
                {
                    Menus[Index].Values.Clear();


                    Menus[Index].Values.Add(new MSettings { SetName = "ALL Power Output/Max Output", SValue = AllOut });


                    Menus[Index].Values.Add(new MSettings { SetName = "Reactor Output", SValue = RInfo });

                    Menus[Index].Values.Add(new MSettings { SetName = "Battery Active", SValue = BRunning.ToString() });
                    Menus[Index].Values.Add(new MSettings { SetName = "Stored Power", SValue = BLoading });
                    //Menus[Index].Values.Add(new MSettings { SetName = "Battery Input -- Output", SValue = BIN });

                    Menus[Index].Values.Add(new MSettings { SetName = "Solar Active", SValue = SRunning.ToString() });
                    Menus[Index].Values.Add(new MSettings { SetName = "Solar Output/MaxOutput ", SValue = SOut });
                }


                return;
            }
        }
        */


        /*
        int SettingDone = 0;
        public void GetEnergySetting()
        {
            int Index = Menus.FindIndex(a => a.MenuName == CurrentMenu);
            if (Index != -1)
            {
                if (SettingDone == 0)
                {
                    Menus[Index].Values.Add(new MSettings { SetName = "UranSaverTExt", SetText = "test", NOValue = true });
                    Menus[Index].Values.Add(new MSettings { SetName = "UranSaver",SetText = "Enable UranSaver", SValue = "ON", SRange = "ON|OFF" });

                    //if false
                    
                    Menus[Index].Values.Add(new MSettings { SetName = "AllReactor", SetText = "Reactor Control" ,SValue = "NORMAL", SRange = "OFF|ON|NORMAL", Hidden = true });
                    Menus[Index].Values.Add(new MSettings { SetName = "AllSolar",SetText = "Solar Control", SValue = "NORMAL", SRange = "OFF|ON|NORMAL", Hidden = true });
                    Menus[Index].Values.Add(new MSettings { SetName = "ALLBattery", SetText = "Battery Control", SValue = "NORMAL", SRange = "OFF|RECHARGE|DISCHARGE|NORMAL", Hidden = true });

                    //if true
                    Menus[Index].Values.Add(new MSettings { SetName = "BatteryMin", SetText = "Battery min % befor turn on Rector", SValue = "20", SRange = "10|20|30|40|50|60|70|80|90|100|OFF", Hidden = false }); ;
                    
                    Menus[Index].Values.Add(new MSettings { SetName = "BatteryWarning", SetText = "Warning when Batter is under %", SValue = "10", SRange = "10|20|30|40|50|60|70|80|90|100|OFF", Hidden = false });
                    SettingDone = 1;
                }

                int Index2 = Menus[Index].Values.FindIndex(a => a.SetName == "UranSaver");

                if(Index2 != -1)
                {
                    int Index3 = Menus[Index].Values.FindIndex(a => a.SetName == "AllReactor");
                    int Index4 = Menus[Index].Values.FindIndex(a => a.SetName == "AllSolar");
                    int Index5 = Menus[Index].Values.FindIndex(a => a.SetName == "ALLBattery");
                    int Index6 = Menus[Index].Values.FindIndex(a => a.SetName == "BatteryMin");
                    int Index7 = Menus[Index].Values.FindIndex(a => a.SetName == "BatteryWarning");

                    if (Menus[Index].Values[Index2].SValue == "ON")
                    {
                        EnergySaver = true;

                        if(Index3 != -1)
                        {
                            Menus[Index].Values[Index3].Hidden = true;
                        }
                        if (Index4 != -1)
                        {
                            Menus[Index].Values[Index4].Hidden = true;
                        }
                        if (Index5 != -1)
                        {
                            Menus[Index].Values[Index5].Hidden = true;
                        }
                        if (Index6 != -1)
                        {
                            Menus[Index].Values[Index6].Hidden = false;
                        }


                    }
                    else
                    {
                        EnergySaver = false;

                        if (Index3 != -1)
                        {
                            Menus[Index].Values[Index3].Hidden = false;
                        }
                        if (Index4 != -1)
                        {
                            Menus[Index].Values[Index4].Hidden = false;
                        }
                        if (Index5 != -1)
                        {
                            Menus[Index].Values[Index5].Hidden = false;
                        }
                        if (Index6 != -1)
                        {
                            Menus[Index].Values[Index6].Hidden = true;
                        }


                    }


                }




            }
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
        */





        #endregion






    }
}

