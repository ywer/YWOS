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
        string MainLCD = "MainLCD";
        string MainController = "MainController";

        //Setting end ----> DONT EDIT BELOW
        #endregion

        #region Private
        double Version = 3.001;
        double UV = 1;
        int Error = 0;
        string ErrorText = "";
        bool Setup = false;
        IMyTextPanel MainScreen;
        IMyControlPanel MainControll;
        int Tick = 0;
        int Maxtick = 100;
        IMyCubeGrid MyGrid; 
        #endregion

        public Program()
        {
            WriteToLog("First Startup");

            Startup();
        }

        public void Save()
        {

        }










        public void Main(string argument, UpdateType updateSource)
        {
            Tick++;
            if(Tick >= Maxtick)
            {
                Tick = -1;
            }
            WriteToLog("Running on Tick " + Tick);




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
                MainScreen = (IMyTextPanel) GridTerminalSystem.GetBlockWithName(MainLCD);
                MainControll  =(IMyControlPanel) GridTerminalSystem.GetBlockWithName(MainLCD);
                if (MainScreen == null|| MainControll == null)
                {
                    ErrorHandler("No MainScreen or MainController Found");
                    return;
                }
                else
                {

                    MyGrid = Me.CubeGrid;
                    Echo("Setup Finished");
                    WriteToLog("Setup Finished");
                    Setup = true;
                    FindBlocks();
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
            ö
            return;
        }



        #endregion

        #region ErrorHandling
        public void ErrorHandler(string ErrorText)
        {
            Error = 1;
            Echo(ErrorText);
            MainScreen.WriteText(ErrorText,false);
            WriteToLog(ErrorText);
            return;
        }

        #endregion

        #region Logger

        public void WriteToLog(string Text)
        {
            string Data = Me.CustomData;

            string[] Lines = Data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string[] NewData = new string[50];
            if (Lines.Length >= 50)
            {
                int L = Lines.Length;
                Array.Copy(Lines, 1, NewData, 0, 49);
            }
            string TimeNow = System.DateTime.Now.ToString();
            NewData[50] = TimeNow + ":" + Text + Environment.NewLine;

            Me.CustomData = NewData.ToString();
            return;
        }


        #endregion


    }
}

