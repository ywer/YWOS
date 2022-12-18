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
        //WARNING:ENERGY = Low Battery!;


        #region settings
        //settings
        string MainLCDName = "MainLCD"; //LCD
        string MainControllerName = "MainController"; //button Panel
        int MaxLoggerRows = 200;//Set this too high will cause lag
        //Setting end ----> DONT EDIT BELOW
        #endregion

        #region Private
        double Version = 3.002;
        int Error = 0;
        string ErrorText = "";
        bool Setup = false;
        IMyTextPanel MainScreen;
        IMyButtonPanel MainControll;
        int Tick = 0;
        int Maxtick = 2000;
        IMyCubeGrid MyGrid;
        int Maxrows = 11;//max rows per page

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
            Runtime.UpdateFrequency = UpdateFrequency.None;
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
            if(Tick == 1000)
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
                    MyGrid = Me.CubeGrid;
                    Setup = true;
                    FindBlocks();
                    AddMenus();
                    Echo("Setup Finished");
                    WriteToLog("Setup Finished");
                    Runtime.UpdateFrequency = UpdateFrequency.Update100;
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
                           
                            string Modulname = "";
                            string CustomData = Block.CustomData;
                            string CustomDataAll = CustomData.Replace(" ", "");
                            string[] Data = CustomDataAll.Split(';');
                            if (Data.Length > 1)
                            {
                               
                                foreach (string CData in Data)
                                {
                                    

                                    string[] Sdata = CData.Split('=');
                                    if (Sdata.Length == 2)
                                    {

                                        string[] firstData = Sdata[0].Split(':');
                                        if (firstData[1].Contains("ModulName"))
                                        {
                                            Modulname = Sdata[1];
                                            break;
                                        }
                                    }

                            }

                                if (Modulname != "")
                                {
                                    Modules.Add(new ModulInfo { Block = (IMyProgrammableBlock)Block, ModulName = Modulname });
                                }

                                int Index = Modules.FindIndex(a => a.ModulName == Modulname);
                                if (Index != -1)
                                {
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
                                                    if (TempData[0].Contains("INFO"))
                                                    {
                                                        Modules[Index].Values.Add(new ModuleValues { VName = TempData[1], VValue = Sdata[1] });

                                                    }
                                                    else if (TempData[0].Contains("SETTING"))
                                                    {
                                                        string[] TempData2 = Sdata[1].Split(':');
                                                        if (TempData2.Length > 1)
                                                        {
                                                            if (TempData2.Length > 1)
                                                            {
                                                                Modules[Index].Settings.Add(new Modulsettings { Name = TempData[1], Value = TempData2[0], Range = TempData2[1] });
                                                                
                                                            }
                                                        }
                                                        else
                                                        {
                                                            WriteToLog("Wrong Setting Lenght!");
                                                            return;
                                                        }
                                                    }
                                                    else if(TempData[0].Contains("WARNING"))
                                                    {
                                                            Warnings.Add(new WarningValue { From = TempData[1], Warning = Sdata[1] });
                                                    }
                                                    else
                                                    {
                                                        WriteToLog("KatError");
                                                        //hier //Rausfinden warum wir hier landen
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
                    WriteToLog("modul Update Clear1");
                    Menus[Index3].Values.Clear();
                }
                string CustomDataAll = Block.CustomData;
                string[] Data = CustomDataAll.Split(';');
                if (Data.Length > 1)
                {

                    foreach (string CData in Data)
                    {
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
                                   Menus[Index3].Values.Add(new MSettings { SetText = TempData[1], SValue = TempData2[0], SRange = TempData2[1] });
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
                    WriteToLog("modul Update Clear1");
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
                        //WARNING:ENERGY = Low Battery!;
                        string[] Sdata = CData.Split('=');
                        if (Sdata.Length == 2)
                        {
                            string[] TempData = Sdata[0].Split(':');
                            //string[] TempData2 = Sdata[1].Split(':');
                            if (TempData.Length > 1)
                            {
                                if (TempData[0] == "INFO")
                                {
                                    
                                    MInfo.Values.Add(new ModuleValues { VName = TempData[1], VValue = Sdata[1] });
                                    if (!TempData[1].Contains("ModulName"))
                                    {
                                        Menus[Index3].Values.Add(new MSettings { SetName = TempData[1], SValue = Sdata[1] });
                                    }
                                }
                                else if(TempData[0] == "WARNING")
                                {
                                        Warnings.Add(new WarningValue { From = TempData[1], Warning = Sdata[1] });
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

        int Deletecounter = 0;
        int Deletecounter2 = 0;
        public void MMove(string Direction)
        {
            int Index = Menus.FindIndex(a => a.MenuName == CurrentMenu);
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

                                string MenuName = Menus[Index].MenuName;
                                string[] Temp1 = MenuName.Split(' ');

                                int Index1 = Modules.FindIndex(a=> a.ModulName == Temp1[0]);
                                if(Index1 != -1)
                                {
                                   string CD = Modules[Index1].Block.CustomData;
                                    //string CD2 = CD.Replace(" ", "");
                                    WriteToLog("DEBUG CD: " + CD);
                                    string[] Data = CD.Split(';');
                                    int DLenght = Data.Length;
                                    WriteToLog("DEbug: Last " + Data[DLenght - 1]);
                                  string replace = Data[DLenght - 1].Replace(";", "");
                                    Data[DLenght - 1] = replace;
                                    WriteToLog("DEbug: Last2 " + Data[DLenght - 1]);
                                    //SETTING:Blala = 20:10|20|30;
                                    int I = 0;
                                    WriteToLog("DEBUG: Settext = " + Menus[Index].Values[CurrentPos].SetText);
                                    foreach (string Custom in Data)
                                    {
                                        WriteToLog("DEBUG CUSTOM1 " + Custom);
                                        if(Custom.Contains(Menus[Index].Values[CurrentPos].SetText))
                                        {
                                            WriteToLog("DEBUG Custom choosen " + Custom);
                                            string[] Split1 = Custom.Split('=');
                                            if(Split1.Length > 1)
                                            {
                                                string[] Split2 = Split1[1].Split(':');
                                                if(Split2.Length > 1)
                                                {
                                                    Split2[0] = Range2[temp];

                                                    string Tempo = Split1[0] + "= " + Split2[0] + ":" + Split2[1];
                                                    Data[I] = Tempo;
                                                    string TmpCD = "";
                                                    /*
                                                    foreach(string PS  in Data)
                                                    {
                                                        TmpCD = TmpCD + PS + ";";
                                                    }
                                                    */
                                                    int i2 = 0;
                                                    do
                                                    {
                                                        if(i2 != Data.Length -1)
                                                        {
                                                            TmpCD = TmpCD + Data[i2] + ";";
                                                        }
                                                        else
                                                        {
                                                            TmpCD = TmpCD + Data[i2];
                                                        }

                                                        i2++;
                                                    } while (i2 < Data.Length);
                                                    Modules[Index1].Block.CustomData = TmpCD;
                                                    WriteToLog("DEBUG: Out: " + TmpCD);
                                                    break;
                                                }


                                            }
                                        }
                                        I++;
                                    }

                                }

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
                            CurrentStage--;
                            CurrentMenu = Steps[CurrentStage];
                            CurrentPos = 0;
                            CurrentPage = 0;
                            int Index33 = Menus.FindIndex(a => a.MenuName == CurrentMenu);

                            if (Index33 != -1)
                            {
                                if (Menus[Index].Values[CurrentPos].NOValue == true || Menus[Index].Values[CurrentPos].Hidden == true)
                                {

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

                if (Type == "Warning")
                {
                    WriteToLog("Warning menu clear");
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
                                Out = Out + Environment.NewLine + Value.SetName  + "  " + Value.SValue;
                            }

                        }
                        else if (IsSetting == true)
                        {
                            if (C1 == CurrentPos)
                            {
                                if (Value.Hidden == false)
                                {
                                    if (Value.NOValue == false)
                                    {

                                    Out = Out + Environment.NewLine + Value.SetText + "   " + Value.SValue + "<----";

                                    }
                                    else
                                    {
                                      Out = Out + Environment.NewLine + Value.SetText + "<----";


                                    } 
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

        bool Running = false;
        public void AddMenus()
        {
            if (Running == false)
            {
                Running = true;
                Menus.Clear();
                Menus.Add(new MenuStorage { MenuName = "Delete", IsInfoPage = true, Values = new List<MSettings> { new MSettings() { SetText = "DELETE?", SValue = "ENTER to Delete, BACK to go back" } } });
                Menus.Add(new MenuStorage { MenuName = "Main", IsMenu = true, Values =  { new MSettings() {  SetName = "Warnings" } } });

                Menus.Add(new MenuStorage { MenuName = "Warnings", IsInfoPage = true, InfoType = "Warning" });

                int Index = Menus.FindIndex(a => a.MenuName == "Main");
                if (Index != -1)
                {
                    foreach (ModulInfo Modul in Modules)
                    {
                        string Modulname = Modul.ModulName;
                        Menus[Index].Values.Add(new MSettings { SetName = Modulname });


                    }
                }
                else
                {
                    WriteToLog("Something went Wrong whit Menu adding");
                    return;
                }

                foreach (MSettings Menu in Menus[Index].Values)
                {
                    string SettingName = Menu.SetName + " Settings";
                    string ValueName = Menu.SetName + " Data";
                    Menus.Add(new MenuStorage { MenuName = Menu.SetName, IsMenu = true, Values = new List<MSettings> { new MSettings() { SetName = ValueName }, new MSettings() { SetName = SettingName } } });

                    Menus.Add(new MenuStorage { MenuName = ValueName, IsInfoPage = true });
                    Menus.Add(new MenuStorage { MenuName = SettingName, IsSetting = true });
                    int Index4 = Modules.FindIndex(a => a.ModulName == Menu.SetName);
                    int Index2 = Menus.FindIndex(a => a.MenuName == ValueName);
                    if (Index2 != -1)
                    {
                        if (Index4 != -1)
                        {
                            //VALUE!!!
                            foreach (ModuleValues Value in Modules[Index4].Values)
                            {
                                if (!Value.VName.Contains("ModulName"))
                                {
                                    Menus[Index2].Values.Add(new MSettings { SetName = Value.VName, SValue = Value.VValue });
                                }
                            }


                        }
                    }
                    int Index3 = Menus.FindIndex(a => a.MenuName == SettingName);
                    if (Index3 != -1)
                    {
                        if (Index4 != -1)
                        {
                            //SETTINGS!!
                            foreach (Modulsettings Setting in Modules[Index4].Settings)
                            {
                                Menus[Index3].Values.Add(new MSettings { SetText = Setting.Name, SValue = Setting.Value, SRange = Setting.Range });

                            }


                        }
                    }
                }
                Running = false;
            }
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

        #endregion






    }
}

