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
            string Menus = "Info|Warnings|SystemStatus|Settings[LEER]|ResetAll";
            string[] AllMenus = Menus.Split('|');


            Channellist.Add(new Channels { Name = "MainMenu", Menus = Menus, Type = "Menu", MenuCount = AllMenus.Length });
            Channellist.Add(new Channels { Name = "Info", Menus = "", Type = "Info", MenuCount = 0 });
            Channellist.Add(new Channels { Name = "Warning", Menus = "", Type = "Info", MenuCount = 0 });
            Channellist.Add(new Channels { Name = "SystemStatus", Menus = "", Type = "Info", MenuCount = 0 });


            Channellist.Add(new Channels { Name = "Settings", Menus = "LEER", Type = "Menu", MenuCount = 0 });
            Channellist.Add(new Channels { Name = "Reset", Menus = "LEER", Type = "Menu", MenuCount = 0 });

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
        int ShowOnly = 0;

        //Setting end ----> DONT EDIT BELOW
        #endregion

        #region Stuff
        double Version = 0.1;
        int MyPos = 0;
        string Site = "";
        List<Channels> Channellist = new List<Channels>();
        List<Info> InfoMenu = new List<Info>();


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
        }

        class Warnings
        {
            public string Message { get; set; }

            public string ScriptName { get; set; }

            public int Prio { get; set; }
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


                hier
            }



        }

        public void ChangePos(String Direction)
        {
            string Direct = Direction;
            int Index = Channellist.FindIndex(a => a.Name == Site);
            if (Index != -1)
            {
                string Type = Channellist[Index].Type;
                int MenuCount = Channellist[Index].MenuCount;

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
                    if (MyPos < MenuCount)
                    {
                        MyPos++;
                        ShowMenu();
                        return;

                    }
                    return;
                }
                else if (Direct == "Back")
                {
                    hier
                }
                else if (Direct == "Enter")
                {
                    undhier

                }



            }



        }



        public void ShowMenu()
        {
            if (Site == "")
            {
                Site = "MainMenu";

                return;
            }
            int Index = Channellist.FindIndex(a => a.Name == Site);
            if (Index != -1)
            {


            }



        }

        #endregion


    }
}
