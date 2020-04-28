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

                if(Block is IMyCargoContainer)
                {
                    MyCargoContainers.Add((IMyCargoContainer)Block);
                }


            }


           // string MMenus = "Info|Warning|SystemStatus|Settings[LEER]|Reset";
           // string SystemStatusM = "Energy|Weapons|Fuel|Inventory";
           // string ResetMenu = "Reset Warnings|Reset Info|Reset All";

            //Main
            Channellist.Add(new Channels { MainChannel = "MainMenu",Type = "Menu", Subs = new List<Sub>() { new Sub() { SubValue = "Info" },new Sub() { SubValue = "Warning"}, new Sub() { SubValue = "SystemStatus"}, new Sub() { SubValue = "Settings"}, new Sub() { SubValue = "Reset"} } });
            Channellist.Add(new Channels { MainChannel = "Info", Type = "Info" });
            Channellist.Add(new Channels { MainChannel = "Warning", Type = "Info" });
            //systemstatus
            Channellist.Add(new Channels { MainChannel = "SystemStatus", Type = "Menu", Subs = new List<Sub>() { new Sub() { SubValue = "Energy", }, new Sub() { SubValue = "Wapons"}, new Sub() { SubValue = "Fuel"}, new Sub() { SubValue = "Inventory"} } });
            Channellist.Add(new Channels { MainChannel = "Energy", Type = "Info" });
            Channellist.Add(new Channels { MainChannel = "Weapons", Type = "Info" });
            Channellist.Add(new Channels { MainChannel = "Fuel", Type = "Info" });
            Channellist.Add(new Channels { MainChannel = "Inventory", Type = "Info" });
            //Settings
            Channellist.Add(new Channels { MainChannel = "Settings", Type = "Menu",Subs = new List<Sub>() { new Sub() { SubValue = "SEnergy" }, new Sub() { SubValue = "SWeapons"}, new Sub() { SubValue = "SFuel"}, new Sub() { SubValue = "SInventory" } } });
            Channellist.Add(new Channels { MainChannel = "SEnergy", Type = "Setting" });
            Channellist.Add(new Channels { MainChannel = "SWeapons", Type = "Setting" });
            Channellist.Add(new Channels { MainChannel = "SFuel", Type = "Setting" });
            Channellist.Add(new Channels { MainChannel = "SInventory", Type = "Setting" });
            SettingsList.Add(new Set { Channel = "SEnergy", Sets = new List<Options>() { new Options() { Setting = "MinALL", Description = "Turn on ALL Below % Charge: ", SettingRange = "10|20|30|40|50|60|70|80|90|100|OFF", SettingStatus = "20" }, new Options() { Setting = "MinOne", Description = "Turn on ONE Below % Charge", SettingRange = "10|20|30|40|50|60|70|80|90|100|OFF", SettingStatus = "30" },
                new Options() { Setting = "MaxAll",Description = "Turn OFF ALL Gens Over %: ", SettingRange = "10|20|30|40|50|60|70|80|90|100|OFF", SettingStatus = "80" }, new Options() { Setting = "Test4", Description = "Test4", SettingRange = "AN|AUS", SettingStatus = "AUS" } , new Options() { Setting = "Test5", Description = "Test5", SettingRange = "AN|AUS", SettingStatus = "AUS" },
                new Options() { Setting = "Test6", Description = "Test6", SettingRange = "AN|AUS", SettingStatus = "AUS" }, new Options() { Setting = "Test7", Description = "Test7", SettingRange = "AN|AUS", SettingStatus = "AUS" },new Options() { Setting = "Test8", Description = "Test8", SettingRange = "AN|AUS", SettingStatus = "AUS" } } });
            //reset
            Channellist.Add(new Channels { MainChannel = "Reset", Type = "Menu", Subs = new List<Sub>() { new Sub() { SubValue = "ResetWarnings"}, new Sub() { SubValue = "Reset Info"}, new Sub() { SubValue = "Reset All"} } });
            Channellist.Add(new Channels { MainChannel = "ResetWarnings", Type = "Reset" });
            Channellist.Add(new Channels { MainChannel = "Reset Info", Type = "Reset" });
            Channellist.Add(new Channels { MainChannel = "Reset All", Type = "Reset" });
            

            foreach(Channels Sub in Channellist)
            {
                Sub.MenuCount = Sub.Subs.Count();

            }
            
            /*
            RegisterMessage(0, 1, "test4", "ywerInc4");
            RegisterMessage(0, 1, "test5", "ywerInc5");
            RegisterMessage(0, 1, "test6", "ywerInc6");
            RegisterMessage(1, 1, "test1", "ywerInc1");
            RegisterMessage(1, 1, "test2", "ywerInc2");
            RegisterMessage(1, 1, "test3", "ywerInc3");
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
        double Version = 0.226;
        int MyPos = 0;
        int deep = 0;
        int Deletecount = 0;
        string Site = "";
        int Page = 0;
        int MaxPages = 0;
        int MenuCount = 0;
        int MaxRowPerSite = 8; //Max One Row Values per site
        List<MValue> SiteValue = new List<MValue>();

        int ResetAll = 0;
        int ResetWarning = 0;
        int ResetInfo = 0;
        
        IMyTextPanel MLCD;
        List<Channels> Channellist = new List<Channels>();
       // List<SubChannel> SubChannelList = new List<SubChannel>();
        List<Set> SettingsList = new List<Set>();
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
        List<IMyCargoContainer> MyCargoContainers = new List<IMyCargoContainer>();
        string[] Steps = new string[20]; 
        List<Inf> InfoM = new List<Inf>();
        List<Inf> InfoMTemp = new List<Inf>();
        List<Warn> WarnM = new List<Warn>();
        List<Warn> WarnMTemp = new List<Warn>();
        string LastWSource = "";
        string LastISource = "";
        int IID = 0;
        int WID = 0;

        //battery
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
        float BatteryPercent = 0;
        int BatteryPercentInt = 0;

        //Weapons
        int MissTurretsActive = 0;
        int GatTurretsActive = 0;
        int IntTurretsActive = 0;
        int GatAIEnabled = 0;
        int GatIsShooting = 0;
        int IntIsShooting = 0;
        int MissIsShooting = 0;
        int IntAIEnabled = 0;
        int MissAIEnabled = 0;

        //Fuel
        float MaxFuel = 0;
        float MaxFuelFloat = 0;
        float CurrentFuelFloat = 0;
        double CurrentFuel = 0;
        float FuelPercent = 0;

        //Cargo
        MyFixedPoint MaxCargo = 0;
        MyFixedPoint UsedCargo = 0;
        IMyInventory test = null;
        float CargoPercent = 0;



        /*
        class Channels
        {
            public string Name { get; set; }

            public string Type { get; set; }

            public string Menus { get; set; }

            public int MenuCount { get; set; }

        }
        */

        class Channels
        {
            public string MainChannel { get; set; }

            public string Type { get; set; }

            public int MenuCount { get; set; }

            public List<Sub> Subs { get; set; } = new List<Sub>();

        }

        public class Sub
        {
            public string SubValue { get; set; }
            
        }

        class Set
        {
            public string Channel { get; set; }

            public List<Options> Sets { get; set; } = new List<Options>();
        }

        class Options
        {
            public string Setting { get; set; }

            public string Description { get; set; }
            public string SettingStatus { get; set; }
            public string SettingRange { get; set; }

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

        class MValue
        {
            public int Max { get; set; }

            public string Value { get; set; }

            public List<Rows> RowValue { get; set; } = new List<Rows>();
        }

        public class Rows
        {
            public string Row { get; set; }
        }

        #endregion




        #region Main

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
            DoEveryTime();
            ShowMenu();



        }


        public void DoEveryTime()
        {

            //battery
            MaxPower = 0;
             PowerUsed = 0;
             ReacIsRunning = 0;
             MaxSolarPower = 0;
             OutputSolarPower = 0;
             BatMaxLoad = 0;
            BatCurrentLoad = 0;
            MaxReac = 0;
            BatOutput = 0;
            BatInput = 0;
            BatCount = 0;
            BatteryPercent = 0;
            BatteryPercentInt = 0;

            //Weapons
            MissTurretsActive = 0;
             GatTurretsActive = 0;
            IntTurretsActive = 0;
            GatAIEnabled = 0;
            GatIsShooting = 0;
            IntIsShooting = 0;
            MissIsShooting = 0;
            IntAIEnabled = 0;
            MissAIEnabled = 0;

            //Fuel
            MaxFuel = 0;
            MaxFuelFloat = 0;
            CurrentFuelFloat = 0;
            CurrentFuel = 0;
            FuelPercent = 0;

            //Cargo

            CargoPercent = 0;

            //Energy
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
                BatMaxLoad = BatMaxLoad + Bat.MaxStoredPower;
                BatCurrentLoad = BatCurrentLoad + Bat.CurrentStoredPower;


                BatInput = BatInput + Bat.CurrentInput;
                BatOutput = BatOutput + Bat.CurrentOutput;

            }
            int Current = Convert.ToInt32(BatCurrentLoad);
            int Max = Convert.ToInt32(BatMaxLoad);

            float one = (BatMaxLoad / 100);
            BatteryPercent = (BatCurrentLoad / one);
            BatteryPercentInt = Convert.ToInt32(BatteryPercent);




            int Index95 = SettingsList.FindIndex(a => a.Channel == "SEnergy");
            int Index96 = 0;
            int Index97 = 0;
            int Index98 = 0;
            if (Index95 != -1)
            {
               Index96 = SettingsList[Index95].Sets.FindIndex(a => a.Setting == "MinALL");
                Index97 = SettingsList[Index95].Sets.FindIndex(a => a.Setting == "MinOne");
                Index98 = SettingsList[Index95].Sets.FindIndex(a => a.Setting == "MaxAll");
            }
            if (SettingsList[Index95].Sets[Index96].SettingStatus != "OFF")
            {
                string MinAll = SettingsList[Index95].Sets[Index96].SettingStatus;
                float MinAllFloat = Convert.ToSingle(MinAll);


                if (BatteryPercent < MinAllFloat)
                {
                    foreach(IMyReactor Rea in AllMyReactors)
                    {
                        Rea.Enabled = true;

                    }

                }
             }

            if (SettingsList[Index95].Sets[Index97].SettingStatus != "OFF")
            {
                string MinOne = SettingsList[Index95].Sets[Index97].SettingStatus;
                float MinOneFloat = Convert.ToSingle(MinOne);

                if (BatteryPercent < MinOneFloat)
                {
                    Random random = new Random();
                    int Ran = random.Next(AllMyReactors.Count -1);

                    AllMyReactors[Ran].Enabled = true;
                }
            }

            if (SettingsList[Index95].Sets[Index98].SettingStatus != "OFF")
            {
                string MaxAll = SettingsList[Index95].Sets[Index98].SettingStatus;
                float MaxAllFloat = Convert.ToSingle(MaxAll);
      
                if (BatteryPercent > MaxAllFloat)
                {
                    foreach (IMyReactor Rea in AllMyReactors)
                    {
                        Rea.Enabled = false;

                    }

                }
            }



            //Energy End

            //Weapons
            foreach (IMyLargeGatlingTurret Gat in MyGatlingTurrets)
            {
                if (Gat.Enabled)
                {
                    GatTurretsActive++;
                }
                if (Gat.AIEnabled)
                {
                    GatAIEnabled++;
                }
                if (Gat.IsShooting)
                {
                    GatIsShooting++;
                }


            }

            foreach (IMyLargeMissileTurret Miss in MyMissleTurrets)
            {
                if (Miss.Enabled)
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

            foreach (IMyLargeInteriorTurret Int in MyIntTurrets)
            {
                if (Int.Enabled)
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

            //Weapons end

            //fuel



            
            foreach (IMyGasTank Fuel in MyFuelTanks)
            {
                MaxFuel = MaxFuel + Fuel.Capacity;
                CurrentFuel = CurrentFuel + Fuel.FilledRatio;

            }
            int Temp1 = Convert.ToInt32(MaxFuel);
             MaxFuelFloat = Convert.ToSingle(Temp1);
            int Temp2 = Convert.ToInt32(CurrentFuel);
            CurrentFuelFloat = Convert.ToSingle(Temp2);

            float OneFuel = MaxFuel / 100;
            FuelPercent = (CurrentFuelFloat / OneFuel);
            //fuel end


            //Cargo
             MaxCargo = 0;
             UsedCargo = 0;
             test = null;
            

            foreach (IMyCargoContainer Cargo in MyCargoContainers)
            {

                test = Cargo.GetInventory();
                MaxCargo = MaxCargo + test.MaxVolume;
                UsedCargo = UsedCargo + test.CurrentVolume;
            }

            //Cargo End


            return;
        }
        #endregion


        #region Menu



        public void ChangePos(String Direction)
        {
            string Direct = Direction;
            int Index = Channellist.FindIndex(a => a.MainChannel == Site);
            if (Index != -1)
            {
                int MenuCount = 0;
               string Type = Channellist[Index].Type;

                if (Site == "Warning")
                {
                    MenuCount = ReturnMaxMessages(1);
                }
                else if (Site == "Info")
                {
                    MenuCount = ReturnMaxMessages(0);
                }
                else if (Type == "Menu")
                {
                    MenuCount = SiteValue[Page].Max;
                }
                else if (Type == "Setting")
                {
                    int Index93 = SettingsList.FindIndex(a => a.Channel == Site);
                    MenuCount = SiteValue[Page].Max;
                }


                if (Direct == "UP")
                {
                    MyPos--;
                    if (MyPos > -1)
                    {
                        //MyPos--;
                        
                        ShowMenu();
                        return;
                    }
                    else
                    {
                        
                        MyPos++;
                        if(MaxPages >0)
                        {
                            if(Page >0)
                            {
                                Page--;
                                MyPos = 0;
                                return;
                            }

                        }
                        
                    }




                    return;
                }
                else if (Direct == "Down")
                {
                    MyPos++;
                    if (MyPos < MenuCount )
                    {
                        // MyPos++;
                        ShowMenu();
                        return;

                    }
                    else
                    {
                        MyPos--;
                        if (MaxPages > 0)
                        {
                         if(Page < MaxPages -1)
                            {

                                Page++;
                                MyPos = 0;
                                return;

                            }
                            
                        }
                        
                    }

                    return;
                }
                else if (Direct == "Back")
                {
                    if(deep > 0)
                    {
                        deep--;
                    }
                    SiteValue.Clear();
                    
                    Site = Steps[deep];
                    MyPos = 0;

                    ShowMenu();

                }
                else if (Direct == "Enter")
                {
                    if (Type == "Menu")
                    {

                        int Index2 = Channellist.FindIndex(a => a.MainChannel == Site);
                        
                        if (Index2 != -1)
                        {
                            Steps[deep] = Site;
                            


                            deep++;
                            string Temp = SiteValue[Page].RowValue[MyPos].Row;
                            //string Temp = Channellist[Index2].Subs[MyPos].SubValue;
                            

                            Site = Temp;
                          
                            MyPos = 0;
                            Page = 0;
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
                        
                    }else if(Type == "Reset")
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
                    }else if(Type == "Setting")
                    {
                        int Index91 = 0;
                       Index91 = SettingsList.FindIndex(a => a.Channel == Site);
                        if (Index91 != -1)
                        {
                            string SetValue = SettingsList[Index91].Sets[MyPos].SettingRange;
                            string[] Value = SetValue.Split('|');
                            string Current = SettingsList[Index91].Sets[MyPos].SettingStatus;
                            int temp = Array.IndexOf(Value, Current);
                            temp++;
                            if(temp > Value.Length - 1)
                            { 
                                temp = 0;
                            }
                            SettingsList[Index91].Sets[MyPos].SettingStatus = Value[temp];
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
            MenuCount = 0;
            if (Site == "")
            {
                Site = "MainMenu";
                ShowMenu();
                return;
            }


            int Index = Channellist.FindIndex(a => a.MainChannel == Site);

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
                            Out = "No Info" + Environment.NewLine + Environment.NewLine;
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
                    else if (Site == "Energy")
                    {




                        Out = "Energy Monitor" + Environment.NewLine + "Reactors: " + Environment.NewLine + "Currently Running of: " + ReacIsRunning + "/" + MaxReac + Environment.NewLine + "Output/Max Output: " + PowerUsed + "/" + MaxPower + Environment.NewLine + Environment.NewLine + "Solar Power: " + Environment.NewLine + "Output/Max Output: " + OutputSolarPower + "/" + MaxSolarPower + Environment.NewLine + Environment.NewLine + "Batterys:" + Environment.NewLine + "Count: " + BatCount + Environment.NewLine + "Batterload load %: " + BatteryPercent + Environment.NewLine + "Input/Output: " + BatInput + "/" + BatOutput + Environment.NewLine + Environment.NewLine;



                        DirectShow(Out);



                        return;

                    }
                    else if (Site == "Weapons")
                    {


                        Out = "Weapons: " + Environment.NewLine + "Int. Turrets: " + Environment.NewLine + "Aktive/Max: " + IntTurretsActive + "/" + MyIntTurrets.Count + Environment.NewLine + "Ai Enabled: " + IntAIEnabled + "/" + MyIntTurrets.Count + Environment.NewLine + "Shooting: " + IntIsShooting + "/" + MyIntTurrets.Count + Environment.NewLine + Environment.NewLine + "Gatling Guns: " + Environment.NewLine + "Aktive/Max: " + GatTurretsActive + "/" + MyGatlingTurrets.Count + Environment.NewLine + "Ai Enabled: " + GatAIEnabled + "/" + MyGatlingTurrets.Count + Environment.NewLine + "Shooting: " + GatIsShooting + "/" + MyGatlingTurrets.Count + Environment.NewLine + Environment.NewLine + "Missle Turrets: " + Environment.NewLine + "Aktive/Max: " + MissTurretsActive + "/" + MyMissleTurrets.Count + Environment.NewLine + "Ai Enabled: " + MissAIEnabled + "/" + MyMissleTurrets.Count + Environment.NewLine + "Shooting: " + MissIsShooting + "/" + MyMissleTurrets.Count + Environment.NewLine + Environment.NewLine;

                        DirectShow(Out);
                        return;
                    }
                    else if (Site == "Fuel")
                    {


                        Out = "Fuel: " + Environment.NewLine + "Tanks: " + MyFuelTanks.Count + Environment.NewLine + "Fuel " + CurrentFuel + Environment.NewLine + "Max Fuel: " + Environment.NewLine + MaxFuel + Environment.NewLine + "Filling in Percent: " + FuelPercent + Environment.NewLine;

                        DirectShow(Out);
                        return;
                    }
                    else if (Site == "Inventory")
                    {

                        Out = "Cargo: " + Environment.NewLine + "Used Volumen/Max Volumen: " + Environment.NewLine + UsedCargo + "/" + MaxCargo + Environment.NewLine;
                        DirectShow(Out);
                        return;
                    }
                    else if (Site == "BatteryInfo")
                    {



                        return;
                    }

                }
                else if (Type == "Menu")
                {
                    SiteValue.Clear();
                    Out = "";
                    int SettingCount = Channellist[Index].Subs.Count;

                        int U1 = 0;
                        int U2 = MaxRowPerSite;
                        do
                        {
                            if (U1 == U2)
                            {
                                SiteValue.Add(new MValue { Max = U1 });

                                U2 = (U2 + MaxRowPerSite);
                            }

                            if (U2 >= SettingCount)
                            {
                                SiteValue.Add(new MValue { Max = U1 });

                                break;

                            }

                            U1++;

                        } while (U1 < SettingCount + +1);

                    string MenuName = Channellist[Index].MainChannel;


                    if (Channellist[Index].Subs.Count > 0)
                    {
                        int T1 = 0;
                        int T3 = 0;
                        int TSite = 0;
                        foreach(Sub MSub in Channellist[Index].Subs)
                        {

                            Out = MSub.SubValue;
                            SiteValue[TSite].RowValue.Add(new Rows { Row = Out });
                            T3++;
                            T1++;
                            if (T1 == MaxRowPerSite)
                            {
                                SiteValue[TSite].Max = T1;
                                Out = "";
                                T1 = 0;
                                TSite++;
                            }
                            if(T3 >= SettingCount)
                            {
                                SiteValue[TSite].Max = T1;
                                T1 = 0;
                                TSite++;
                                Out = "";
                                break;
                            }


                        }
                         MaxPages = TSite;
                        /*
                        do
                        {

                            Out = SettingsList[Index92].Sets[I2].Description + " = " + SettingsList[Index92].Sets[I2].SettingStatus;
                            SiteValue[ISite].RowValue.Add(new Rows { Row = Out });


                            if (I == MaxRowPerSite)
                            {
                                SiteValue[ISite].Max = SiteValue[ISite].RowValue.Count;
                                ISite++;
                                I = 0;
                                Out = "";
                            }
                            if (I2 >= SettingCount - 1)
                            {
                                SiteValue[ISite].Max = SiteValue[ISite].RowValue.Count;
                                ISite++;
                                I = 0;
                                Out = "";
                                break;

                            }
                            I++;
                            I2++;
                        } while (I2 <= SettingCount);

                        */


                        string Uff = Site + Environment.NewLine;
                        int T4 = 0;
                        foreach(Rows  Var in SiteValue[Page].RowValue)
                        {
                            if(T4 == MyPos)
                            {
                                string LoL = Var.Row + "<----";
                                Uff = Uff + LoL + Environment.NewLine;
                            }
                            else
                            {
                                Uff = Uff + Var.Row + Environment.NewLine;
                            }
                            T4++;

                        }
                        DirectShow(Uff);



                        /*
                        Out = MenuName + ":" + Environment.NewLine;
                        int C1 = 0;
                        int C2 = 0;
                        foreach (Sub MSub in Channellist[Index].Subs)
                        {
                            if (C1 == MyPos)
                            {
                                string Uff = MSub.SubValue + "<---";
                                Out = Out + Uff + Environment.NewLine;
                            }
                            else
                            {
                                Out = Out + MSub.SubValue + Environment.NewLine;

                            }


                            C1++;
                        }
                        */
                        //DirectShow(Out);
                        return;

                    }


                }
                else if (Type == "Reset")
                {
                    Out = "";
                    if (Site == "Reset All")
                    {
                        Out = "Enter to Delete all Infos/Warnings, Back to go back to Menu";
                        ResetAll = 1;
                        DirectShow(Out);

                    }
                    else if (Site == "Reset Warnings")
                    {
                        Out = "Enter to Delete all Warnings, Back to go back to Menu";
                        ResetWarning = 1;
                        DirectShow(Out);

                    }
                    else if (Site == "Reset Info")
                    {
                        ResetInfo = 1;
                        Out = "Enter to Delete all Infos, Back to go back to Menu";
                        DirectShow(Out);

                    }


                }
                else if (Type == "Setting")
                {
                    SiteValue.Clear();


                        
                        Out = "";
                        int I = 0;
                        int I2 = 0;
                        int ISite = 0;
                        int Index92 = SettingsList.FindIndex(a => a.Channel == Site);
                        int C2 = 0;
                        if (Index92 != -1)
                        {
                            if (SettingsList[Index92].Sets.Count > 0)
                            {

                                int SettingCount = SettingsList[Index92].Sets.Count;

                                int U1 = 0;
                                int U2 = MaxRowPerSite;

                                do
                                {
                                    if (U1 == U2)
                                    {
                                        SiteValue.Add(new MValue { Max = U1 });

                                        U2 = (U2 + MaxRowPerSite);
                                    }

                                    if (U2 >= SettingCount)
                                    {
                                        SiteValue.Add(new MValue { Max = U1 });

                                        break;

                                    }

                                    U1++;

                                } while (U1 < SettingCount  +1);
                            
                                do
                                {

                                    Out = SettingsList[Index92].Sets[I2].Description + " = " + SettingsList[Index92].Sets[I2].SettingStatus;
                                    SiteValue[ISite].RowValue.Add(new Rows { Row = Out });

                                I++;
                                I2++;
                                if (I == MaxRowPerSite)
                                    {
                                        SiteValue[ISite].Max = SiteValue[ISite].RowValue.Count;
                                        ISite++;
                                        I = 0;
                                        Out = "";
                                    }
                                    if (I2 >= SettingCount -1)
                                    {
                                        SiteValue[ISite].Max = SiteValue[ISite].RowValue.Count;
                                        ISite++;
                                        I = 0;
                                        Out = "";
                                        break;

                                    }


                            } while (I2 <= SettingCount);
                                MaxPages = ISite;

                                string FU = "";
                                 FU = Site + Environment.NewLine;
                                foreach (Rows Value in SiteValue[Page].RowValue)
                                {
                                    if (C2 == MyPos)
                                    {
                                        string Uff = Value.Row + " <----";
                                        FU = FU + Uff + Environment.NewLine;
                                    }
                                    else
                                    {
                                        FU = FU + Value.Row + Environment.NewLine;
                                    }

                                    C2++;
                                }
                                DirectShow(FU);
                                return;
                            }
                            else
                            {
                                Out = "No Settings Aviable";
                                DirectShow(Out);
                            }
                        }
                        else
                        {
                            Out = "Something is Wrong";
                            DirectShow(Out);
                        }
                }
                return;
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
            
            int Index = Channellist.FindIndex(a => a.MainChannel == Site);
            int Math = MyPos + 1;

            if (Index != -1)
            {
                string Type = Channellist[Index].Type;

                 if (Type != "Info")
                {
                                

                MenuCount = 0;
               

                if (Site == "Warning")
                {
                    MenuCount = ReturnMaxMessages(1);
                }
                else if (Type == "Menu")
                {
                    MenuCount = SiteValue[Page].Max;
                }
                else if (Type == "Setting")
                {
                    int Index93 = SettingsList.FindIndex(a => a.Channel == Site);
                        // MenuCount = SettingsList[Index93].Sets.Count;
                        MenuCount = SiteValue[Page].Max;
                }

                if(MaxPages >0)
                    {
                        int Math2 = Page + 1;
                        int Math3 = MaxPages;
                        int Math4 = MenuCount;
                        Show = Show + Environment.NewLine + "Position[" + Math + "/" + Math4 + "]   Page[" + Math2 + "/"+ Math3 + "] " + Environment.NewLine;
                    }
                    else
                    {
                        Show = Show + Environment.NewLine + "Position[" + Math + "/" + MenuCount + "]" + Environment.NewLine;
                    }
                    
                   // Show = Show + Environment.NewLine + "Position[" + Math + "/" + MenuCount + "]" + Environment.NewLine;
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


        #region Func

        public string ReturnIndicator(int Percent)
        {
            string Out = "[";
            Double Mathe = 0;

            Mathe = (Percent / 10);
            Mathe = Math.Round(Mathe);

            int I = 0;

            do
            {
                Out = Out + "|";



            } while (I < Mathe);

            Out = Out + "]";

            return Out;
        }


        #endregion

    }
}
