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


        string MenuLCD = "MenuLCD";
        int ShowOnly = 0; //Only Show, dont Turn on Anything Mode!

        //Setting end ----> DONT EDIT BELOW
        #endregion



        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
            List<IMyTerminalBlock> allBlocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocks(allBlocks);

            foreach (IMyTerminalBlock Block in allBlocks)
            {

                if (Block is IMyTextPanel)
                {


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
                    IMyLargeMissileTurret Turr2 = (IMyLargeMissileTurret)Block;
                    MyMissleTurrets.Add(new RocketTData { Turret = Turr2, AI = Turr2.AIEnabled, Range = Turr2.Range, Aktive = Turr2.Enabled });
                }

                if (Block is IMyLargeGatlingTurret)
                {

                    IMyLargeGatlingTurret Turr = (IMyLargeGatlingTurret)Block;
                    MyGatlingTurrets.Add(new GATData {Turret = Turr, AI = Turr.AIEnabled, Range = Turr.Range, Aktive = Turr.Enabled});
                }

                if (Block is IMyLargeInteriorTurret)
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

                if (Block is IMyCargoContainer)
                {
                    MyCargoContainers.Add((IMyCargoContainer)Block);
                }

                if (Block is IMyShipConnector)
                {
                    MyConnectors.Add((IMyShipConnector)Block);
                }

                if (Block is IMyLightingBlock)
                {
                    IMyLightingBlock BlockL = (IMyLightingBlock)Block;
                    MyLights.Add(new LightSetting { Light = BlockL, Name = BlockL.Name, OldColor = BlockL.Color });
                    
                }

                if(Block is IMyDoor)
                {
                    IMyDoor Door = (IMyDoor)Block;
                    MyDoors.Add(new DoorSettings { Aktive = Door.Enabled, Doorstatus = Door.Status, Door = Door });
                }

                if(Block is IMyAirtightHangarDoor)
                {
                    IMyAirtightHangarDoor Gate = (IMyAirtightHangarDoor)Block;
                    MyGates.Add(new GateSettings { Aktive = Gate.Enabled, Gatestatus = Gate.Status, Gate = Gate });
                    

                }



            }

            // string MMenus = "Info|Warning|SystemStatus|Settings[LEER]|Reset";
            // string SystemStatusM = "Energy|Weapons|Fuel|Inventory";
            // string ResetMenu = "Reset Warnings|Reset Info|Reset All";

            //Main
            Channellist.Add(new Channels { MainChannel = "MainMenu", Type = "Menu", Subs = new List<Sub>() { new Sub() { SubValue = "SystemStatus" }, new Sub() { SubValue = "Settings" }, new Sub() { SubValue = "Reset" } } });


            //systemstatus
            Channellist.Add(new Channels { MainChannel = "SystemStatus", Type = "Menu", Subs = new List<Sub>() { new Sub() { SubValue = "Energy", }, new Sub() { SubValue = "System" } } });
            Channellist.Add(new Channels { MainChannel = "Energy", Type = "Info" });
            Channellist.Add(new Channels { MainChannel = "System", Type = "Info" });
            //Settings
            Channellist.Add(new Channels { MainChannel = "Settings", Type = "Menu", Subs = new List<Sub>() { new Sub() { SubValue = "SEnergy" }, new Sub() { SubValue = "SWeapons" }, new Sub() { SubValue = "SFuel" }, new Sub() { SubValue = "SInventory" }, new Sub() { SubValue = "SGeneral" }, new Sub() { SubValue = "SAlarmmode" },new Sub() { SubValue = "SAutoDoorCloser" } } });
            Channellist.Add(new Channels { MainChannel = "SEnergy", Type = "Setting" });
            Channellist.Add(new Channels { MainChannel = "SWeapons", Type = "Setting" });
            Channellist.Add(new Channels { MainChannel = "SFuel", Type = "Setting" });
            Channellist.Add(new Channels { MainChannel = "SInventory", Type = "Setting" });
            //Energy
            SettingsList.Add(new Set { Channel = "SEnergy", Sets = new List<Options>() { new Options() { Setting = "MinALL", Description = "Turn on ALL Below % Charge: ", SettingRange = "10|20|30|40|50|60|70|80|90|100|OFF", SettingStatus = "20" }, new Options() { Setting = "MinOne", Description = "Turn on ONE Below % Charge", SettingRange = "10|20|30|40|50|60|70|80|90|100|OFF", SettingStatus = "30" },
                new Options() { Setting = "MaxAll",Description = "Turn OFF ALL Gens Over %: ", SettingRange = "10|20|30|40|50|60|70|80|90|100|OFF", SettingStatus = "80" }, new Options() { Setting = "BatWarnUnder", Description = "Warn under % Battery Load", SettingRange = "10|20|30|40|50|60|70|80|90|100|OFF", SettingStatus = "20" } } });
            //General Setting
            Channellist.Add(new Channels { MainChannel = "SGeneral", Type = "Setting" });
            SettingsList.Add(new Set { Channel = "SGeneral", Sets = new List<Options>() { new Options() { Setting = "ShowOnly", Description = "Show OnlyMode(No Automations)", SettingRange = "ON|OFF", SettingStatus = "OFF" } } });
            //Alarmmode
            Channellist.Add(new Channels { MainChannel = "SAlarmmode", Type = "Setting" });
            SettingsList.Add(new Set { Channel = "SAlarmmode", Sets = new List<Options>() { new Options() { Setting = "BlockAlarm", Description = "Block Alarm Mode", SettingRange = "ON|OFF", SettingStatus = "OFF" }, new Options() { Setting = "LightColor", Description = "Change Light Color at Alarm to:", SettingRange = "RED|BLUE|GREEN|YELLOW|ORANGE|OFF", SettingStatus = "RED" }, new Options() { Setting = "AktivateTurrets", Description = "Aktivate All Turrets on ALarm", SettingRange = "ON|OFF", SettingStatus = "ON" },
                new Options() { Setting = "TurretsMaxRange", Description = "Change Turret to Maxrange on Alarm", SettingRange = "ON|OFF", SettingStatus = "ON" },new Options() { Setting = "AktivateAI", Description = "Aktivate Turret AI on ALarm", SettingRange = "ON|OFF", SettingStatus = "ON"} , new Options() { Setting = "CloseAllDoors", Description = "Close all Doors and Gates", SettingRange = "ON|OFF", SettingStatus = "ON"}} });
            //DoorCloser
            Channellist.Add(new Channels { MainChannel = "SAutoDoorCloser", Type = "Setting" });
            SettingsList.Add(new Set { Channel = "SAutoDoorCloser", Sets = new List<Options>() { new Options() { Setting = "CloseTicks", Description = "Close After Ticks", SettingRange = "10|30|50|100|200|300|400|500", SettingStatus = "30" }, new Options() { Setting = "CloseDoors", Description = "Autoclose Doors", SettingRange = "ON|OFF", SettingStatus = "ON" }, new Options() { Setting = "CloseGates", Description = "AutoClose Gates", SettingRange = "ON|OFF", SettingStatus = "ON" } } });
            //Setting = "CloseTicks"
            // Setting = "CloseDoors",
            //Setting = "CloseGates"

            //reset
            Channellist.Add(new Channels { MainChannel = "Reset", Type = "Menu", Subs = new List<Sub>() { new Sub() { SubValue = "ResetWarnings" }, new Sub() { SubValue = "Reset Info" }, new Sub() { SubValue = "Reset All" } } });
            Channellist.Add(new Channels { MainChannel = "ResetWarnings", Type = "Reset" });
            Channellist.Add(new Channels { MainChannel = "Reset Info", Type = "Reset" });
            Channellist.Add(new Channels { MainChannel = "Reset All", Type = "Reset" });



            foreach (Channels Sub in Channellist)
            {
                Sub.MenuCount = Sub.Subs.Count();

            }





        }

        public void Save()
        {

        }



        #region Stuff
        double Version = 0.233;
        int Tick = 0;
        int MyPos = 0;
        int deep = 0;
        int Deletecount = 0;
        string Site = "";
        int Page = 0;
        int MaxPages = 0;
        int MenuCount = 0;
        int MaxRowPerSite = 11; //Max One Row Values per site
        List<MValue> SiteValue = new List<MValue>();

        int ResetAll = 0;
        int ResetWarning = 0;
        int ResetInfo = 0;
        int WarnMenu = 0;
        int InfoMenu = 0;

        IMyTextPanel MLCD;
        List<Channels> Channellist = new List<Channels>();
        // List<SubChannel> SubChannelList = new List<SubChannel>();
        List<Set> SettingsList = new List<Set>();
        List<IMyTextPanel> InfoLCDList = new List<IMyTextPanel>();
        List<IMyTextPanel> WarnLCDList = new List<IMyTextPanel>();
        List<IMyReactor> AllMyReactors = new List<IMyReactor>();
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
        List<BatStatus> BatteryStatus = new List<BatStatus>();
        int BatMenu = 0;

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
        int WeaponMenu = 0;
        List<RocketTData> MyMissleTurrets = new List<RocketTData>();
        List<GATData> MyGatlingTurrets = new List<GATData>();

        //Alarm
        int AlarmMode = 0;
        int AlarmModeDisabled = 0;
        int AlarmD = 0;
        int AlarmMainMenu = 0;
        int RAlarm = 0;
        //Lights
        List<LightSetting> MyLights = new List<LightSetting>();
        Color AlarmColor = new Color(0,0,0);
        //Doors
        List<DoorSettings> MyDoors = new List<DoorSettings>();
        List<GateSettings> MyGates = new List<GateSettings>();
        int DoorTick = 0;

        //Fuel
        float MaxFuel = 0;
        float MaxFuelFloat = 0;
        float CurrentFuelFloat = 0;
        double CurrentFuel = 0;
        float FuelPercent = 0;
        int FuelInfoMenu = 0;

        //Cargo
        MyFixedPoint MaxCargo = 0;
        MyFixedPoint UsedCargo = 0;
        IMyInventory test = null;
        int CargoPercent = 0;
        int CargoInfoMenu = 0;

        //Connectors
        int ConnectorMenu = 0;
        IMyCubeGrid LocalGrid = null;
        List<IMyShipConnector> MyConnectors = new List<IMyShipConnector>();
        List<ConShips> ConnectedShips = new List<ConShips>();



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

        class Rows
        {
            public string Row { get; set; }
        }

       class BatStatus
        {
            public string Name { get; set; }

            public string Status { get; set; }

            public int Percent { get; set; }

        }

        class ConShips
        {
            public string SubName { get; set; }

            public IMyCubeGrid SubGrid { get; set; }

            public string ConnectorName { get; set; }

            public int LocalConnector { get; set; }

            public MyShipConnectorStatus ConnectorStatus { get; set; }

            public int SubBatteryPercen { get; set; }

            public List<IMyBatteryBlock> SubBatterys { get; set; } = new List<IMyBatteryBlock>();

            List<IMyTerminalBlock> allSubBlocks = new List<IMyTerminalBlock>();

        }

        class LightSetting
        {
            public string Name { get; set; }

            public Color OldColor { get; set; }

            public IMyLightingBlock Light { get; set; }

        }


       class GATData
        {
           public IMyLargeGatlingTurret Turret { get; set; }

            public bool Aktive { get; set; }
            public string TargetNeutral { get; set; }

            public bool AI { get; set; }

            public float Range { get; set; }

            public string TargetMet { get; set; }

            public string TargetRocket { get; set; }

            public string TargetSmall { get; set; }

            public string TargetLarge { get; set; }

            public string TargetPlayer { get; set; }

            public string TargetStation { get; set; }

        }

        class RocketTData
        {
            public IMyLargeMissileTurret Turret { get; set; }

            public bool Aktive { get; set; }

            public string TargetNeutral { get; set; }

            public bool AI { get; set; }

            public float Range { get; set; }

            public string TargetMet { get; set; }

            public string TargetRocket { get; set; }

            public string TargetSmall { get; set; }

            public string TargetLarge { get; set; }

            public string TargetPlayer { get; set; }

            public string TargetStation { get; set; }

        }

        class DoorSettings
        {
            public IMyDoor Door { get; set; }
            public bool Aktive { get; set; }

            public DoorStatus Doorstatus { get; set; }


        }

        class GateSettings
        {

            public IMyAirtightHangarDoor Gate { get; set; }
            public bool Aktive { get; set; }

            public DoorStatus Gatestatus { get; set; }


        }

        #endregion




        #region Main

        public void Main(string argument, UpdateType updateSource)
        {
            string Status = argument;
            LocalGrid = Me.CubeGrid;
            if (Status == "Reset")
            {
                DeleteWarnings();
                DeleteInfos();
            }
            else if (Status == "ShowOnly")
            {
                if(ShowOnly == 0)
                {
                    int Index36 = SettingsList.FindIndex(a => a.Channel == "SGeneral");
                    int Index37 = 0;

                    if (Index36 != -1)
                    {
                        Index37 = SettingsList[Index36].Sets.FindIndex(a => a.Setting == "ShowOnly");
                        SettingsList[Index36].Sets[Index37].SettingStatus = "ON";

                    }

                    ShowOnly = 1;
                    Echo("Show Only Aktive");
                }
                else
                {
                    int Index38 = SettingsList.FindIndex(a => a.Channel == "SGeneral");
                    int Index39 = 0;

                    if (Index38 != -1)
                    {
                        Index39 = SettingsList[Index38].Sets.FindIndex(a => a.Setting == "ShowOnly");
                        SettingsList[Index38].Sets[Index39].SettingStatus = "OFF";

                    }
                    ShowOnly = 0;
                    Echo("Show Only Inaktive");
                }
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
            else if (Status.Contains("MSG"))
            {
                //Echo("MSG: " + Status);
                //PREFIX|TYPE|PRIO|ID|TEXT|USER
                //MSG|
                //1 = Warning 0 = info

                string[] Input = Status.Split('|');


                if (Input.Length > 5)
                {
                    Input[0] = Input[0].Replace("|", "");
                    Input[0] = Input[0].Replace("|", "");
                    Input[1] = Input[1].Replace("|", "");
                    Input[1] = Input[1].Replace("|", "");
                    Input[2] = Input[2].Replace("|", "");
                    Input[2] = Input[2].Replace("|", "");
                    Input[3] = Input[3].Replace("|", "");
                    Input[3] = Input[3].Replace("|", "");
                    Input[4] = Input[4].Replace("|", "");
                    Input[4] = Input[4].Replace("|", "");
                    Input[5] = Input[5].Replace("|", "");
                    Input[5] = Input[5].Replace("|", "");
                    int MType = 0;
                    int MPrio = 0;
                    int MID = 0;
                    bool isNumeric = int.TryParse(Input[1], out MType);
                    bool isNumeric2 = int.TryParse(Input[2], out MPrio);
                    bool isNumeric3 = int.TryParse(Input[3], out MID);
                    if (isNumeric & isNumeric2 & isNumeric3)
                    {
                        RegisterMessage(MType, MPrio,MID, Input[4], Input[5]);
                        Echo("New Extern Message");
                    }
                    else
                    {
                        Echo("Wrong Input Formate! MSG|TYPE|PRIO|TEXT|USER");
                        Echo("Example: MSG|1|1|101|HELP|InfoScript");
                        Echo("Type:0=Info,1=Warning");
                    }
                }
                else
                {
                    Echo("Wrong Input Formate! MSG|TYPE|PRIO|TEXT|USER");
                    Echo("Example: MSG|1|1|101|HELP|InfoScript");
                    Echo("Type: ");
                    Echo("0=Info,1=Warning");
                }

            }
            else if (Status == "Alarm")
            {

                if (AlarmModeDisabled == 0) 
                {
                    if (AlarmMode == 0)
                    {
                        AlarmMode = 1;
                    }
                    else
                    {
                        Echo("Alarm Alrdy Aktivated!");
                    }
                }else
                {
                    Echo("Alarmmode Diasabled in Settings!");
                }

            }
                DoEveryTime();
            ShowMenu();
        }


        public void DoEveryTime()
        {




            if (Tick == 0)
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



                if (ShowOnly == 0)
                {
                    int Index95 = SettingsList.FindIndex(a => a.Channel == "SEnergy");
                    int Index96 = 0;
                    int Index97 = 0;
                    int Index98 = 0;
                    int Index99 = 0;
                    if (Index95 != -1)
                    {
                        Index96 = SettingsList[Index95].Sets.FindIndex(a => a.Setting == "MinALL");
                        Index97 = SettingsList[Index95].Sets.FindIndex(a => a.Setting == "MinOne");
                        Index98 = SettingsList[Index95].Sets.FindIndex(a => a.Setting == "MaxAll");
                        Index99 = SettingsList[Index95].Sets.FindIndex(a => a.Setting == "BatWarnUnder");
                    }
                    if (SettingsList[Index95].Sets[Index96].SettingStatus != "OFF")
                    {
                        string MinAll = SettingsList[Index95].Sets[Index96].SettingStatus;
                        float MinAllFloat = Convert.ToSingle(MinAll);


                        if (BatteryPercent < MinAllFloat)
                        {
                            foreach (IMyReactor Rea in AllMyReactors)
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
                            int Ran = random.Next(AllMyReactors.Count - 1);

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
                    //BatWarnUnder
                    if (BatCount > 0)
                    {
                        if (SettingsList[Index95].Sets[Index99].SettingStatus != "OFF")
                        {
                            string WarnP = SettingsList[Index95].Sets[Index99].SettingStatus;
                            float WarnPF = Convert.ToSingle(WarnP);
                            if (BatteryPercent < WarnPF)
                            {
                                RegisterMessage(1, 10, 21, "Warning Energy Critical", "Energy Warning-Setting");
                            }

                        }
                        //BatWarn 0;
                        if (BatteryPercent < 10)
                        {
                            RegisterMessage(1, 10, 22, "Warning Energy Empty", "Energy Warning-Setting");
                        }

                    }

                }

                //Energy End

                //Weapons
                //SystemStatus\Weapons
                //Settings\SWeapons

                if (MyMissleTurrets.Count > 0 ^ MyGatlingTurrets.Count > 0 ^ MyIntTurrets.Count > 0)
                {
                    if (WeaponMenu == 0)
                    {

                        int Index8 = Channellist.FindIndex(a => a.MainChannel == "SystemStatus");
                        int Index64 = Channellist.FindIndex(a => a.MainChannel == "Settings");
                        if (Index8 != -1)
                        {
                            BatMenu = 1;
                            Channellist[Index8].Subs.Add(new Sub() { SubValue = "Weapons" });
                            Channellist.Add(new Channels { MainChannel = "Weapons", Type = "Info" });
                        }
                        if (Index64 != -1)
                        {
                            BatMenu = 1;
                            Channellist[Index8].Subs.Add(new Sub() { SubValue = "SWeapons" });
                            Channellist.Add(new Channels { MainChannel = "SWeapons", Type = "Setting" });
                        }
                    }

                }
                else
                {
                    if (WeaponMenu == 1)
                    {
                        int Index87 = Channellist.FindIndex(a => a.MainChannel == "SystemStatus");
                        int Index86 = Channellist.FindIndex(a => a.MainChannel == "Weapons");

                        if (Index87 != -1)
                        {
                            BatMenu = 0;
                            int Index44 = Channellist[Index87].Subs.FindIndex(a => a.SubValue == "Weapons");
                            if (Index44 != -1)
                            {
                                Channellist[Index87].Subs.RemoveAt(Index44);
                            }
                            if (Index86 != -1)
                            {
                                Channellist.RemoveAt(Index86);
                            }
                        }
                        int Index85 = Channellist.FindIndex(a => a.MainChannel == "Settings");
                        int Index84 = Channellist.FindIndex(a => a.MainChannel == "SWeapons");

                        if (Index85 != -1)
                        {
                            BatMenu = 0;
                            int Index45 = Channellist[Index85].Subs.FindIndex(a => a.SubValue == "SWeapons");
                            if (Index45 != -1)
                            {
                                Channellist[Index85].Subs.RemoveAt(Index45);
                            }
                            if (Index84 != -1)
                            {
                                Channellist.RemoveAt(Index84);
                            }
                        }




                    }


                }




                if (WeaponMenu == 1)
                {
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
                }
                //Weapons end
            }
            //TICK 1 VORBEI!

            //TICK 2!!

            if (Tick == 1)
            {

                //fuel
                //SystemStatus\Fuel

                if (MyFuelTanks.Count > 0)
                {
                    if (FuelInfoMenu == 0)
                    {

                        int Index22 = Channellist.FindIndex(a => a.MainChannel == "SystemStatus");
                        if (Index22 != -1)
                        {
                            FuelInfoMenu = 1;
                            Channellist[Index22].Subs.Add(new Sub() { SubValue = "Fuel" });
                            Channellist.Add(new Channels { MainChannel = "Fuel", Type = "Info" });
                        }
                    }

                }
                else
                {
                    if (FuelInfoMenu == 1)
                    {
                        int Index23 = Channellist.FindIndex(a => a.MainChannel == "SystemStatus");
                        int Index24 = Channellist.FindIndex(a => a.MainChannel == "Fuel");
                        if (Index24 != -1)
                        {
                            FuelInfoMenu = 0;
                            int Index44 = Channellist[Index24].Subs.FindIndex(a => a.SubValue == "Fuel");
                            if (Index44 != -1)
                            {
                                Channellist[Index24].Subs.RemoveAt(Index44);
                            }
                            if (Index23 != -1)
                            {
                                Channellist.RemoveAt(Index23);
                            }
                        }

                    }


                }


                if (FuelInfoMenu == 1)
                {
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

                }
                //fuel end



                //Cargo
                //SystemStatus\Inventory



                MaxCargo = 0;
                UsedCargo = 0;
                test = null;

                if (MyCargoContainers.Count > 0)
                {
                    if (CargoInfoMenu == 0)
                    {

                        int Index29 = Channellist.FindIndex(a => a.MainChannel == "SystemStatus");
                        if (Index29 != -1)
                        {
                            CargoInfoMenu = 1;
                            Channellist[Index29].Subs.Add(new Sub() { SubValue = "Inventory" });
                            Channellist.Add(new Channels { MainChannel = "Inventory", Type = "Info" });
                        }
                    }

                }
                else
                {
                    if (CargoInfoMenu == 1)
                    {
                        int Index28 = Channellist.FindIndex(a => a.MainChannel == "SystemStatus");
                        int Index27 = Channellist.FindIndex(a => a.MainChannel == "Inventory");
                        if (Index27 != -1)
                        {
                            CargoInfoMenu = 0;
                            int Index43 = Channellist[Index27].Subs.FindIndex(a => a.SubValue == "Inventory");
                            if (Index43 != -1)
                            {
                                Channellist[Index27].Subs.RemoveAt(Index43);
                            }
                            if (Index28 != -1)
                            {
                                Channellist.RemoveAt(Index28);
                            }
                        }

                    }


                }



                if (CargoInfoMenu == 1)
                {

                    foreach (IMyCargoContainer Cargo in MyCargoContainers)
                    {

                        test = Cargo.GetInventory();
                        MaxCargo = MaxCargo + test.MaxVolume;
                        UsedCargo = UsedCargo + test.CurrentVolume;
                    }
                    int UCargo = UsedCargo.ToIntSafe();
                    int OneCargo = MaxCargo.ToIntSafe() / 100;
                    CargoPercent = (UCargo / OneCargo);

                }


                //Cargo End



                //Menu-------------

                //Remove Warning/info
                int MaxInf = ReturnMaxMessages(0);
                int MaxWarn = ReturnMaxMessages(1);
                if (MaxInf > 0)
                {
                    if (InfoMenu == 0)
                    {
                        InfoMenu = 1;
                        int Index1 = Channellist.FindIndex(a => a.MainChannel == "MainMenu");
                        if (Index1 != -1)
                        {
                            Channellist[Index1].Subs.Add(new Sub() { SubValue = "Info" });
                            Channellist.Add(new Channels { MainChannel = "Info", Type = "Info" });
                        }
                    }
                }
                else
                {
                    if (InfoMenu == 1)
                    {
                        InfoMenu = 0;
                        int Index2 = Channellist.FindIndex(a => a.MainChannel == "MainMenu");
                        int Index23 = Channellist.FindIndex(a => a.MainChannel == "Info");
                        if (Index2 != -1)
                        {

                            int Index22 = Channellist[Index2].Subs.FindIndex(a => a.SubValue == "Info");
                            if (Index22 != -1)
                            {
                                Channellist[Index2].Subs.RemoveAt(Index22);
                            }

                            if (Index23 != -1)
                            {
                                Channellist.RemoveAt(Index23);
                            }

                        }
                    }
                }
                //new List<Sub>() { new Sub() { SubValue = "Info" },new Sub() { SubValue = "Warning"}
                //Channellist.Add(new Channels { MainChannel = "Info", Type = "Info" });
                //Channellist.Add(new Channels { MainChannel = "Warning", Type = "Info" });
                if (MaxWarn > 0)
                {
                    if (WarnMenu == 0)
                    {
                        WarnMenu = 1;
                        int Index3 = Channellist.FindIndex(a => a.MainChannel == "MainMenu");
                        if (Index3 != -1)
                        {
                            Channellist[Index3].Subs.Add(new Sub() { SubValue = "Warning" });
                            Channellist.Add(new Channels { MainChannel = "Warning", Type = "Info" });
                        }
                    }
                }
                else
                {
                    if (WarnMenu == 1)
                    {
                        WarnMenu = 0;
                        int Index4 = Channellist.FindIndex(a => a.MainChannel == "MainMenu");
                        int Index45 = Channellist.FindIndex(a => a.MainChannel == "Warning");
                        if (Index4 != -1)
                        {

                            int Index44 = Channellist[Index4].Subs.FindIndex(a => a.SubValue == "Warning");
                            if (Index44 != -1)
                            {
                                Channellist[Index4].Subs.RemoveAt(Index44);
                            }
                            if (Index45 != -1)
                            {
                                Channellist.RemoveAt(Index45);
                            }
                        }
                    }

                }
                //Remofe Info/Warning ende

                //Battery Site

                if (BatCount > 0)
                {
                    if (BatMenu == 0)
                    {

                        int Index5 = Channellist.FindIndex(a => a.MainChannel == "SystemStatus");
                        if (Index5 != -1)
                        {
                            BatMenu = 1;
                            Channellist[Index5].Subs.Add(new Sub() { SubValue = "BatteryStatus" });
                            Channellist.Add(new Channels { MainChannel = "BatteryStatus", Type = "Info" });
                        }
                    }

                }
                else
                {
                    if (BatMenu == 1)
                    {
                        int Index6 = Channellist.FindIndex(a => a.MainChannel == "SystemStatus");
                        int Index65 = Channellist.FindIndex(a => a.MainChannel == "BatteryStatus");
                        if (Index6 != -1)
                        {
                            BatMenu = 0;
                            int Index44 = Channellist[Index6].Subs.FindIndex(a => a.SubValue == "BatteryStatus");
                            if (Index44 != -1)
                            {
                                Channellist[Index6].Subs.RemoveAt(Index44);
                            }
                            if (Index65 != -1)
                            {
                                Channellist.RemoveAt(Index65);
                            }
                        }

                    }


                }

                if (BatMenu == 1)
                {
                    BatteryStatus.Clear();
                    foreach (IMyBatteryBlock Bat in MyBatteries)
                    {
                        float MaxLoad = 0;
                        float Load = 0;
                        string BatName = "";

                        MaxLoad = Bat.MaxStoredPower;
                        Load = Bat.CurrentStoredPower;
                        float oneone = (MaxLoad / 100);
                        float BatteryP = (Load / oneone);
                        int BatteryPInt = Convert.ToInt32(BatteryP);
                        BatName = Bat.CustomName;
                        string Stat = ReturnIndicator(BatteryPInt);

                        BatteryStatus.Add(new BatStatus { Name = BatName, Status = Stat, Percent = BatteryPInt });
                    }



                }
                //Battery site end


            }
            if (Tick == 2) //TICK 3--------------
            {

                //Connector Site //DOCKED SHIP DETECTION

                if (MyConnectors.Count > 0)
                {
                    if (ConnectorMenu == 0)
                    {
                        int Index6 = Channellist.FindIndex(a => a.MainChannel == "SystemStatus");
                        if (Index6 != -1)
                        {
                            ConnectorMenu = 1;
                            Channellist[Index6].Subs.Add(new Sub() { SubValue = "Connectors/Connected Ships" });
                            Channellist.Add(new Channels { MainChannel = "Connectors/Connected Ships", Type = "Info" });
                        }
                    }

                }
                else
                {
                    if (ConnectorMenu == 1)
                    {
                        int Index7 = Channellist.FindIndex(a => a.MainChannel == "SystemStatus");
                        int Index75 = Channellist.FindIndex(a => a.MainChannel == "Connectors/Connected Ships");
                        if (Index7 != -1)
                        {
                            BatMenu = 0;
                            int Index54 = Channellist[Index7].Subs.FindIndex(a => a.SubValue == "Connectors/Connected Ships");
                            if (Index54 != -1)
                            {
                                Channellist[Index7].Subs.RemoveAt(Index54);
                            }
                            if (Index75 != -1)
                            {
                                Channellist.RemoveAt(Index75);
                            }
                        }

                    }
                }
                int CLocal = 0;
                if(ConnectorMenu == 1)
                {
                    ConnectedShips.Clear();
                foreach(IMyShipConnector CON in MyConnectors)
                    {
                        if(CON.CubeGrid == LocalGrid)
                        {
                            CLocal = 1;
                        }
                        else
                        {
                            CLocal = 0;
                        }


                        if(CON.Status == MyShipConnectorStatus.Connected)
                        {
                            string Other = CON.OtherConnector.CubeGrid.CustomName;
                            ConnectedShips.Add(new ConShips { SubName = Other, SubGrid = CON.OtherConnector.CubeGrid, ConnectorName = CON.DisplayNameText, ConnectorStatus = CON.Status, LocalConnector = CLocal });
                        }
                        else
                        {
                            ConnectedShips.Add(new ConShips { ConnectorName = CON.DisplayNameText, ConnectorStatus = CON.Status,LocalConnector = CLocal });
                        }
                       
                    }
                }




                //Conenctor Site end




                //Show Only

                int Index33 = SettingsList.FindIndex(a => a.Channel == "SGeneral");
                int Index34 = 0;

                if (Index33 != -1)
                {
                    Index34 = SettingsList[Index33].Sets.FindIndex(a => a.Setting == "ShowOnly");

                }
                if (SettingsList[Index33].Sets[Index34].SettingStatus != "OFF")
                {
                    ShowOnly = 1;

                }
                else
                {
                    ShowOnly = 0;
                }

                // ShowOnly  Ende
                

                //Menu End---------------
            }//Tick 3 End


            if (Tick == 3) //TICK 4--------------
            {
                //connected Ships
                int U34 = 0;
                foreach(ConShips SP in ConnectedShips)
                {
                    float BatMaxLoad2 = 0;
                    string GridName = SP.SubName;
                    GridTerminalSystem.GetBlocksOfType(ConnectedShips[U34].SubBatterys,b => b.CubeGrid.CustomName == GridName);
                    foreach(IMyBatteryBlock Bat in ConnectedShips[U34].SubBatterys)
                    {
                        BatMaxLoad2 = BatMaxLoad2 + Bat.MaxStoredPower;

                    }
                    float one = (BatMaxLoad / 100);
                    BatteryPercent = (BatCurrentLoad / one);
                    BatteryPercentInt = Convert.ToInt32(BatteryPercent);
                    ConnectedShips[U34].SubBatteryPercen = BatteryPercentInt;

                    U34++;
                }

                //GridTerminalSystem.GetBlocks(allBlocks);



                //Conencted ships ende





            } //TICK 4 End

            if(Tick == 4)  //Tick 5!!!
            {
                //ALARMMODE

                int Index11 = SettingsList.FindIndex(a => a.Channel == "SAlarmmode");
                int Index12 = 0;

                if (Index11 != -1)
                {
                    Index12 = SettingsList[Index11].Sets.FindIndex(a => a.Setting == "BlockAlarm");

                }
                if (Index12 != -1)
                {
                    if (SettingsList[Index11].Sets[Index12].SettingStatus != "OFF")
                    {
                        AlarmModeDisabled = 1;
                    }
                    else
                    {
                        AlarmModeDisabled = 0;
                    }
                }

                if (AlarmModeDisabled == 0)
                {
                    if (AlarmMainMenu == 0)
                    {

                        int Index77 = Channellist.FindIndex(a => a.MainChannel == "MainMenu");
                        if (Index77 != -1)
                        {
                            AlarmMainMenu = 1;
                            Channellist.Add(new Channels { MainChannel = "Aktivate Alarm", Type = "Reset" });
                            Channellist[Index77].Subs.Add(new Sub() { SubValue = "Aktivate Alarm" });
                            
                        }
                    }

                }
                else
                {
                    if (AlarmMainMenu == 1)
                    {
                        int Index78 = Channellist.FindIndex(a => a.MainChannel == "MainMenu");
                        int Index79 = Channellist.FindIndex(a => a.MainChannel == "Aktivate Alarm");
                        if (Index78 != -1)
                        {
                            AlarmMainMenu = 0;
                            int Index44 = Channellist[Index78].Subs.FindIndex(a => a.SubValue == "Aktivate Alarm");
                            if (Index44 != -1)
                            {
                                Channellist[Index78].Subs.RemoveAt(Index44);
                            }
                            if (Index79 != -1)
                            {
                                Channellist.RemoveAt(Index79);
                            }
                        }

                    }


                }


                if (AlarmMode == 1)
                {
                   
                    int Index19 = SettingsList.FindIndex(a => a.Channel == "SAlarmmode");
                    int Index20 = 0;

                    if (Index19 != -1)
                    {
                        Index20 = SettingsList[Index19].Sets.FindIndex(a => a.Setting == "LightColor");

                    }
                    if (Index20 != -1)
                    {
                        if (SettingsList[Index19].Sets[Index20].SettingStatus != "OFF")
                        {
                            string Color = SettingsList[Index19].Sets[Index20].SettingStatus;
                            if(Color == "RED")
                            {
                                AlarmColor = new Color(255, 0, 0);

                            }
                            else if(Color == "BLUE")
                            {
                                AlarmColor = new Color(0, 0, 255);
                            }
                            else if(Color == "GREEN")
                            {
                                AlarmColor = new Color(0, 128, 0);
                            }
                            else if(Color == "YELLOW")
                            {
                                AlarmColor = new Color(255, 255, 0);
                            }
                            else if(Color == "ORANGE")
                            {
                                AlarmColor = new Color(255, 165, 0);
                            }
                            foreach(LightSetting Light in MyLights)
                            {
                                IMyLightingBlock LBlock = Light.Light;
                                LBlock.Color = AlarmColor;


                            }

                        }
                    }

                    int Index24 = SettingsList.FindIndex(a => a.Channel == "SAlarmmode");
                    int Index25 = 0;

                    if (Index24 != -1)
                    {
                        Index25 = SettingsList[Index24].Sets.FindIndex(a => a.Setting == "AktivateTurrets");

                    }
                    if (Index25 != -1)
                    {
                        if (SettingsList[Index24].Sets[Index25].SettingStatus != "OFF")
                        {
                            foreach(GATData Turr in MyGatlingTurrets)
                            {
                                Turr.Turret.ApplyAction("OnOff_On");
                               // Turr.Turret.Enabled = true;
                            }

                            foreach (RocketTData Turr in MyMissleTurrets)
                            {
                                //Turr.Turret.Enabled = true;
                                Turr.Turret.ApplyAction("OnOff_On");
                            }

                            int Index36 = SettingsList.FindIndex(a => a.Channel == "SAlarmmode");
                            int Index37 = 0;

                            if (Index36 != -1)
                            {
                                Index37 = SettingsList[Index36].Sets.FindIndex(a => a.Setting == "TurretsMaxRange");

                            }
                            if (Index37 != -1)
                            {
                                if (SettingsList[Index36].Sets[Index37].SettingStatus != "OFF")
                                {
                                    foreach (GATData Turr in MyGatlingTurrets)
                                    {
                                        Turr.Turret.SetValue("Range",800);
                                    }
                                }
                            }

                            int Index66 = SettingsList.FindIndex(a => a.Channel == "SAlarmmode");
                            int Index67 = 0;

                            if (Index66 != -1)
                            {
                                Index67 = SettingsList[Index66].Sets.FindIndex(a => a.Setting == "AktivateAI");

                            }
                            if (Index67 != -1)
                            {
                                if (SettingsList[Index66].Sets[Index67].SettingStatus != "OFF")
                                {
                                    foreach (GATData Turr in MyGatlingTurrets)
                                    {
                                        Turr.Turret.ApplyAction("EnableIdleMovement_On");
                                    }
                                }
                            }
                        }
                    }


                    int Index55 = SettingsList.FindIndex(a => a.Channel == "SAlarmmode");
                    int Index56 = 0;

                    if (Index55 != -1)
                    {
                        Index56 = SettingsList[Index55].Sets.FindIndex(a => a.Setting == "CloseAllDoors");

                    }
                    if (Index56 != -1)
                    {
                        if (SettingsList[Index55].Sets[Index56].SettingStatus != "OFF")
                        {
                            foreach(DoorSettings Door in MyDoors)
                            {
                                IMyDoor DBlock = Door.Door;
                                DBlock.CloseDoor();
                            }

                            foreach(GateSettings Gate in MyGates)
                            {
                                IMyAirtightHangarDoor GBlock = Gate.Gate;
                                GBlock.CloseDoor();
                            }


                        }
                    }







                    //Setting = "TurretsMaxRange"-
                    //Setting = "AktivateTurrets", -
                    //Setting = "AktivateAI"
                    //Setting = "CloseAllDoors",



                }
                else
                {
                    if(RAlarm == 1)
                    {
                        RAlarm = 0;
                        foreach (LightSetting Lights in MyLights)
                        {
                            IMyLightingBlock LBlock = Lights.Light;
                            LBlock.Color = Lights.OldColor;


                        }

                        int Index24 = SettingsList.FindIndex(a => a.Channel == "SAlarmmode");
                        int Index25 = 0;

                        if (Index24 != -1)
                        {
                            Index25 = SettingsList[Index24].Sets.FindIndex(a => a.Setting == "AktivateTurrets");

                        }
                        if (Index25 != -1)
                        {
                            if (SettingsList[Index24].Sets[Index25].SettingStatus != "OFF")
                            {
                                foreach (GATData Turr in MyGatlingTurrets)
                                {
                                    bool AK = Turr.Aktive;
                                    if (AK != true)
                                    {
                                       // Turr.Turret.Enabled = false;
                                        Turr.Turret.ApplyAction("OnOff_Off");
                                    }
                                }

                                foreach (RocketTData Turr in MyMissleTurrets)
                                {
                                    bool AK = Turr.Aktive;
                                    if (AK != true)
                                    {
                                        //Turr.Turret.Enabled = false;
                                        Turr.Turret.ApplyAction("OnOff_Off");
                                    }
                                }


                                int Index36 = SettingsList.FindIndex(a => a.Channel == "SAlarmmode");
                                int Index37 = 0;

                                if (Index36 != -1)
                                {
                                    Index37 = SettingsList[Index36].Sets.FindIndex(a => a.Setting == "TurretsMaxRange");

                                }
                                if (Index37 != -1)
                                {
                                    if (SettingsList[Index36].Sets[Index37].SettingStatus != "OFF")
                                    {
                                        foreach (GATData Turr in MyGatlingTurrets)
                                        {
                                            float Range = Turr.Range;
                                            Turr.Turret.SetValue("Range", Range);
                                        }
                                    }
                                }

                                int Index66 = SettingsList.FindIndex(a => a.Channel == "SAlarmmode");
                                int Index67 = 0;

                                if (Index66 != -1)
                                {
                                    Index67 = SettingsList[Index66].Sets.FindIndex(a => a.Setting == "AktivateAI");

                                }
                                if (Index67 != -1)
                                {
                                    if (SettingsList[Index66].Sets[Index67].SettingStatus != "OFF")
                                    {
                                        foreach (GATData Turr in MyGatlingTurrets)
                                        {
                                            bool AI = Turr.AI;
                                            Turr.Turret.ApplyAction("EnableIdleMovement_Off");
                                        }
                                    }
                                }
                            }
                        }

                        int Index55 = SettingsList.FindIndex(a => a.Channel == "SAlarmmode");
                        int Index56 = 0;

                        if (Index55 != -1)
                        {
                            Index56 = SettingsList[Index55].Sets.FindIndex(a => a.Setting == "CloseAllDoors");

                        }
                        if (Index56 != -1)
                        {
                            if (SettingsList[Index55].Sets[Index56].SettingStatus != "OFF")
                            {
                                foreach (DoorSettings Door in MyDoors)
                                {
                                    DoorStatus Status = Door.Doorstatus;
                                    IMyDoor DBlock = Door.Door;
                                    if(Status == DoorStatus.Closed)
                                    {
                                        DBlock.CloseDoor();
                                    }
                                    else
                                    {
                                        DBlock.OpenDoor();
                                    }
                                    
                                }

                                foreach (GateSettings Gate in MyGates)
                                {
                                    IMyAirtightHangarDoor GBlock = Gate.Gate;
                                    DoorStatus Status = Gate.Gatestatus;
                                    if(Status == DoorStatus.Closed)
                                    {
                                        GBlock.CloseDoor();
                                    }
                                    else
                                    {
                                        GBlock.OpenDoor();
                                    }
                                    
                                }


                            }
                        }




                    }
                }




            //ALARMMODE END



            }//Tick 5 END

            //Door Manager

            //Setting = "CloseTicks"
            // Setting = "CloseDoors",
            //Setting = "CloseGates"

            int Index82 = SettingsList.FindIndex(a => a.Channel == "SAutoDoorCloser");
            int Index83 = 0;

            if (Index82 != -1)
            {
                Index83 = SettingsList[Index82].Sets.FindIndex(a => a.Setting == "CloseTicks");

            }
            if (Index83 != -1)
            {
                string Setting = SettingsList[Index82].Sets[Index83].SettingStatus;
                int STick = Convert.ToInt32(Setting);
                if (DoorTick <= STick)
                {
                    DoorTick++;
                }
                else
                {
                    DoorTick = 0;
                    int Index84 = SettingsList.FindIndex(a => a.Channel == "SAutoDoorCloser");
                    int Index85 = 0;

                    if (Index84 != -1)
                    {
                        Index85 = SettingsList[Index84].Sets.FindIndex(a => a.Setting == "CloseDoors");

                    }
                    if (Index85 != -1)
                    {
                        if (SettingsList[Index84].Sets[Index85].SettingStatus != "OFF")
                        {
                            foreach (DoorSettings Door in MyDoors)
                            {
                                IMyDoor DBlock = Door.Door;
                                if (DBlock.Status == DoorStatus.Open)
                                {
                                    DBlock.CloseDoor();
                                }


                            }
                        }
                    }

                    int Index86 = SettingsList.FindIndex(a => a.Channel == "SAutoDoorCloser");
                    int Index87 = 0;

                    if (Index86 != -1)
                    {
                        Index87 = SettingsList[Index86].Sets.FindIndex(a => a.Setting == "CloseGates");

                    }
                    if (Index87 != -1)
                    {
                        if (SettingsList[Index86].Sets[Index87].SettingStatus != "OFF")
                        {


                            foreach (GateSettings Gate in MyGates)
                            {
                                IMyAirtightHangarDoor GBlock = Gate.Gate;
                                if (GBlock.Status == DoorStatus.Open)
                                {
                                    GBlock.CloseDoor();
                                }

                            }

                        }
                    }

                }
            }



            //Door Manager ende



            //Echo("Tick: " + Tick);
            if (Tick <= 5)
            {
                Tick++;
            }
            else
            {
                Tick = 0;
            }


            return;
        }
        #endregion


        #region Menu



        public void ChangePos(String Direction)
        {
            //Echo("Deep: " + deep);
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
                    if (SiteValue.Count > 0)
                    {
                        MenuCount = SiteValue[Page].Max;
                    }
                    else
                    {
                        MenuCount = 0;
                    }
                }
                else if (Type == "Setting")
                {
                    //int Index93 = SettingsList.FindIndex(a => a.Channel == Site);
                    if(SiteValue.Count > 0)
                    {
                        MenuCount = SiteValue[Page].Max;
                    }
                    else
                    {
                        MenuCount = 0;
                    }
                    

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
                    Page = 0;
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


                        }else if(AlarmD == 1)
                        {
                            if(AlarmMode == 1)
                            {
                                AlarmD = 0;
                                AlarmMode = 0;
                                RAlarm = 1;
                                ChangePos("Back");
                                Echo("Alarm OFF");
                                return;
                            }
                            else
                            {
                                AlarmD = 0;
                                AlarmMode = 1;
                                ChangePos("Back");
                                Echo("Alarm ON");
                                return;
                            }
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




                        Out = "Energy Monitor:" + Environment.NewLine + "Reactors: " + Environment.NewLine + "Currently Running of: " + ReacIsRunning + "/" + MaxReac + Environment.NewLine + "Output/Max Output: " + PowerUsed + "/" + MaxPower + Environment.NewLine + Environment.NewLine + "Solar Power: " + Environment.NewLine + "Output/Max Output: " + OutputSolarPower + "/" + MaxSolarPower + Environment.NewLine + Environment.NewLine + "Batterys:" + Environment.NewLine + "Count: " + BatCount + Environment.NewLine + "Batterload load %: " + BatteryPercent + Environment.NewLine + "Input/Output: " + BatInput + "/" + BatOutput + Environment.NewLine + Environment.NewLine;



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
                        string ind = ReturnIndicator(CargoPercent);
                        Out = "Cargo: " + Environment.NewLine + "Used Volumen/Max Volumen: " + Environment.NewLine + UsedCargo + "/" + MaxCargo + Environment.NewLine + ind + " " + CargoPercent + "%" +Environment.NewLine;
                        DirectShow(Out);
                        return;
                    }
                    else if (Site == "BatteryStatus")
                    {

                        SiteValue.Clear();
                        Out = "";
                        int SettingCount = BatteryStatus.Count;
                        int U11 = 0;
                        int U22 = MaxRowPerSite;
                        do
                        {
                            if (U11 == U22)
                            {
                                SiteValue.Add(new MValue { Max = U11 });

                                U22 = (U22 + MaxRowPerSite);
                            }

                            if (U22 >= SettingCount)
                            {
                                SiteValue.Add(new MValue { Max = U11 });

                                break;

                            }

                            U11++;

                        } while (U11 < SettingCount + +1);


                        int I = 0;
                        int I2 = 0;
                        int ISite = 0;


                        do
                        {

                            Out = BatteryStatus[I2].Name + " = " + BatteryStatus[I2].Status + " " + BatteryStatus[I2].Percent + "%";
                            SiteValue[ISite].RowValue.Add(new Rows { Row = Out });

                            I++;
                            I2++;
                            if (I == MaxRowPerSite)
                            {
                                SiteValue[ISite].Max = 1;
                                ISite++;
                                I = 0;
                                Out = "";
                            }
                            if (I2 >= SettingCount)
                            {
                                SiteValue[ISite].Max = 1;
                                ISite++;
                                I = 0;
                                Out = "";
                                break;

                            }


                        } while (I2 <= SettingCount);
                        MaxPages = ISite;

                        string MenuName = Channellist[Index].MainChannel;
                        string Uff = Site + ":" + Environment.NewLine;
                        
                        foreach (Rows Var in SiteValue[Page].RowValue)
                        {

                                Uff = Uff + Var.Row + Environment.NewLine;
                                

                        }
                        int MyPosmath = MyPos + 1;
                        int MaxI = MaxPages;
                        Uff = Uff + Environment.NewLine + "Site:[" + MyPosmath + "/" + MaxI + "]" + Environment.NewLine;

                        DirectShow(Uff);


                        
                        return;
                    }
                    else if(Site == "System")
                    {
                        Out = "";
                        Out = "Systemstatus:" + Environment.NewLine + Environment.NewLine + "Version: " + Version + Environment.NewLine + "Creator: >>Ywer<<" + Environment.NewLine + Environment.NewLine;
                        DirectShow(Out);




                    }
                    else if(Site == "Connectors/Connected Ships")
                    {
                        SiteValue.Clear();
                        Out = "";
                        int SettingCount = ConnectedShips.Count;
                        int U11 = 0;
                        int MaxPerRowConnector = 3; //ZEILEN EINSTELLUNG CONENCTOR SEITE!!!!!!!!!
                        int U22 = MaxPerRowConnector;
                        
                        do
                        {
                            if (U11 == U22)
                            {
                                SiteValue.Add(new MValue { Max = U11 });

                                U22 = (U22 + MaxPerRowConnector);
                            }

                            if (U22 >= SettingCount)
                            {
                                SiteValue.Add(new MValue { Max = U11 });

                                break;

                            }

                            U11++;

                        } while (U11 < SettingCount + +1);


                        int I = 0;
                        int I2 = 0;
                        int ISite = 0;
                        int M1 = 0;
                        
                        do
                        {
                            if (ConnectedShips[I2].LocalConnector == 1)
                            {
                                if (ConnectedShips[I2].ConnectorStatus == MyShipConnectorStatus.Connected)
                                {
                                    if (ConnectedShips[I2].SubBatteryPercen > 0)
                                    {
                                        string Boo = ReturnIndicator(ConnectedShips[I2].SubBatteryPercen);
                                        Out = ConnectedShips[I2].ConnectorName + ": " + Environment.NewLine + ConnectedShips[I2].SubName + Environment.NewLine + "Battery: " + Boo + " " + ConnectedShips[I2].SubBatteryPercen  + "%" + Environment.NewLine;
                                    }
                                    else
                                    {
                                        Out = ConnectedShips[I2].ConnectorName + ": " + Environment.NewLine + ConnectedShips[I2].SubName + Environment.NewLine;
                                    }

                                }
                                else
                                {
                                    Out = ConnectedShips[I2].ConnectorName + " = " + ConnectedShips[I2].ConnectorStatus + Environment.NewLine;
                                }
                                SiteValue[ISite].RowValue.Add(new Rows { Row = Out });
                                I++;
                                I2++;
                                M1++;
                            }
                            else
                            {
                                I2++;
                            }

                            if (I == MaxPerRowConnector)
                            {

                                SiteValue[ISite].Max = 1;
                                ISite++;
                                I = 0;
                                Out = "";
                            }
                            if (I2 >= SettingCount)
                            {

                                ISite--;
                                SiteValue[ISite].Max = 1;
                                ISite++;
                                I = 0;
                                Out = "";
                                break;

                            }


                        } while (I2 < SettingCount+1);
                        MaxPages = ISite + 1;
                        string MenuName = Channellist[Index].MainChannel;
                        string Uff = Site + ":" + Environment.NewLine;

                        foreach (Rows Var in SiteValue[Page].RowValue)
                        {

                            Uff = Uff + Var.Row + Environment.NewLine;


                        }
                        int MyPosmath = Page + 1;
                        int MaxI = MaxPages;
                        Uff = Uff + Environment.NewLine + "Site:[" + MyPosmath + "/" + MaxI + "]" + Environment.NewLine;
                        DirectShow(Uff);



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

                        string Uff = Site + ":" + Environment.NewLine;
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
                        return;

                    }


                }
                else if (Type == "Reset")
                {
                    Out = "";
                    if (Site == "Reset All")
                    {
                        Out = "Enter to Delete all Infos/Warnings, Back to go back";
                        ResetAll = 1;
                        DirectShow(Out);

                    }
                    else if (Site == "Reset Warnings")
                    {
                        Out = "Enter to Delete all Warnings, Back to go back";
                        ResetWarning = 1;
                        DirectShow(Out);

                    }
                    else if (Site == "Reset Info")
                    {
                        ResetInfo = 1;
                        Out = "Enter to Delete all Infos, Back to go back";
                        DirectShow(Out);

                    }
                    else if(Site == "Aktivate Alarm")
                    {
                        if(AlarmMode == 0)
                        {
                            Out = "Enter to Aktivate, Back to go back";
                            DirectShow(Out);
                            AlarmD = 1;
                        }
                        else
                        {
                            Out = "Enter to DeAktivate, Back to go back";
                            AlarmD = 1;
                            DirectShow(Out);
                        }
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

                                    if (U2 >= SettingCount )
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
                                    if (I2 >= SettingCount )
                                    {
                                        SiteValue[ISite].Max = SiteValue[ISite].RowValue.Count;
                                        ISite++;
                                        I = 0;
                                        Out = "";
                                        break;

                                    }


                            } while (I2 <= SettingCount +1);
                                MaxPages = ISite;

                                string FU = "";
                                 FU = Site + ":" + Environment.NewLine;
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
                                Out = "No Settings Aviable" + Environment.NewLine;
                                DirectShow(Out);
                            }
                        }
                        else
                        {
                            Out = "Something is Wrong, Menu Dont Exist" + Environment.NewLine;
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
                        if (SiteValue.Count > 0)
                        {
                            MenuCount = SiteValue[Page].Max;
                        }
                        else
                        {
                            MenuCount = 0;
                        }
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
                Show = Show + "WARNINGS: " + ReturnMaxMessages(1);
            }
            else
            {
                Show = Show + "No Warnings" ;
            }

            if (ShowOnly == 1)
            {
                Show = Show + "        !!! SHOW ONLY AKTIVE!!! ";
            }

            if (AlarmMode == 1)
            {
                Show = Show + "        !!! ALARMMOE AKTIVE!!! ";
            }


            if (MLCD != null)
            {
                MLCD.WriteText(Show, false);
                return;
            }


            return;
        }

       
       

        public void RegisterMessage(int MType, int Prio, int ID, string Message, string Source)
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

                        int Index2 = InfoM.FindIndex(a => a.ScriptName == Source);
                        int Index4 = InfoM.FindIndex(a => a.ID == ID);
                        if (Index2 != -1)
                        {

                            int temp = InfoM[Index2].ID;
                            InfoM.RemoveAt(Index2);
                            InfoM.Add(new Inf() { ID = temp, Message = Message, Prio = Prio, ScriptName = Source });
                            LastISource = Source;
                            return;
                        }
                        else if(Index4 != -1)
                        {
                            InfoM.RemoveAt(Index4);
                            int temp = ID;
                            InfoM.Add(new Inf() { ID = temp, Message = Message, Prio = Prio, ScriptName = Source });
                            LastISource = Source;
                        return;
                        }
                    else
                    {
                        IID++;
                        InfoM.Add(new Inf() { ID = IID, Message = Message, Prio = Prio, ScriptName = Source });
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


                    int Index3 = WarnM.FindIndex(a => a.ScriptName == Source);
                    int Index5 = InfoM.FindIndex(a => a.ID == ID);

                    if (Index3 != -1)
                     {
                     int temp2 = WarnM[Index3].ID;
                     WarnM.RemoveAt(Index3);
                     WarnM.Add(new Warn() { ID = temp2, Message = Message, Prio = Prio, ScriptName = Source });
                        LastWSource = Source;
                        return;
                     }else if(Index5 != -1)
                     {

                        WarnM.RemoveAt(Index5);
                     int temp = ID;
                     WarnM.Add(new Warn() { ID = temp, Message = Message, Prio = Prio, ScriptName = Source });
                      LastWSource = Source;
                        return;
                      }
                    else
                    {
                        WID++;
                        WarnM.Add(new Warn() { ID = WID, Message = Message, Prio = Prio, ScriptName = Source });
                        return;
                    }

                       

                    

                }
            }
            else
            {

                return;
            }
        }
        //Script by ywer


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
