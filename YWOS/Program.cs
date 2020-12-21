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
                List<IMyTerminalBlock> tmp = new List<IMyTerminalBlock>();
                Echo("Starting Startup....");
                WriteToLog("Starting Startup....");
                WriteToLog("Serching for Screen and Controller..");
                Echo("Serching for Screen and Controller..");
                tmp.Add(GridTerminalSystem.GetBlockWithName(MainLCD));
                tmp.Add(GridTerminalSystem.GetBlockWithName(MainLCD));
                if (tmp.Count != 2)
                {
                    ErrorHandler("No MainScreen or MainController Found");
                    return;
                }
                else
                {
                    foreach (IMyTerminalBlock Block in tmp)
                    {
                        if(Block is IMyTextPanel)
                        {
                            IMyTextPanel LCD = (IMyTextPanel)Block;
                            MainScreen = LCD;

                        } 

                        if (Block is IMyControlPanel)
                        {
                            IMyControlPanel myControl = (IMyControlPanel)Block;
                            MainControll = myControl;

                        }



                    }
                    Echo("Setup Finished");
                    WriteToLog("Setup Finished");
                    Setup = true;
                    FindBlocks();
                    return;
                }


            }
            return;
        }



        public void FindBlocks()
        {
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
            NewData[50] = TimeNow + ":" + Text;
            return;
        }


        #endregion


    }
}

