
/*

using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRage;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
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


        public class MessageHandling
        {
            List<Inf> InfoM = new List<Inf>();
            List<Inf> InfoMTemp = new List<Inf>();
            List<Warn> WarnM = new List<Warn>();
            List<Warn> WarnMTemp = new List<Warn>();
            string LastWSource = "";
            string LastISource = "";
            int IID = 0;
            int WID = 0;


            public  void RegisterMessage(int MType, int Prio, string Message, string Source)
            {
                
                //0 - Info
                //1 - Warn

                if (MType == 0)
                {
                    if(Source == "User")
                    {
                        IID++;
                        InfoM.Add(new Inf() { ID = IID, Message = Message, Prio = Prio, ScriptName = Source });
                        return ;
                    }
                    else
                    {
                        if(Source == LastISource)
                        {
                            int Index2 = InfoM.FindIndex(a => a.ScriptName == Source);
                            if(Index2 != -1)
                            {
                                int temp = InfoM[Index2].ID;
                                InfoM.RemoveAt(Index2);
                                InfoM.Add(new Inf() { ID = temp, Message = Message, Prio = Prio, ScriptName = Source });
                                return ;
                            }
                            
                            return ;
                        }
                        else
                        {
                            IID++;
                            InfoM.Add(new Inf() { ID = WID, Message = Message, Prio = Prio, ScriptName = Source });
                            return ;
                        }
                    }


                    
                } else if (MType == 1)
                {
                    if(Source == "User")
                    {
                        WID++;
                        WarnM.Add(new Warn() { ID = WID, Message = Message, Prio = Prio, ScriptName = Source });
                        return ;


                    }
                    else
                    {

                        if(LastWSource == Source)
                        {
                            int Index3 = WarnM.FindIndex(a => a.ScriptName == Source);
                            if(Index3 != -1)
                            {
                                int temp2 = WarnM[Index3].ID;
                                InfoM.RemoveAt(Index3);
                                WarnM.Add(new Warn() { ID = temp2, Message = Message, Prio = Prio, ScriptName = Source });
                                return ;
                            }
                        }
                        else
                        {
                            WID++;
                            WarnM.Add(new Warn() { ID = WID, Message = Message, Prio = Prio, ScriptName = Source });
                            return ;

                        }

                    }




                    return ;
                }
                else
                {

                    return ;
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
                    InfoM.AddRange(InfoMTemp);
                    return 0;
                        //counter rückwärts, alles in nem array rein und neu ins die liste
                   


                }
                else if(MType == 1)
                {
                        WarnM.RemoveAt(ID);
                        WID--;
                    WarnMTemp.AddRange(WarnM);
                    WarnM.Clear();
                    WarnM.AddRange(WarnMTemp);

                    //counter rückwärts, alles in nem array rein und neu ins die liste

                    return 0;

                }
                else
                {
                    
                    return 1;
                }



                
            }



            public string ReturnMessage(int MType,int ID)
            {
                //mtype:
                //0 - Info
                //1 - Warn

                if (MType == 0)
                {
                    if (ID < InfoM.Count)
                    {
                        string Out1 =  "Source: " + InfoM[ID].ScriptName + Environment.NewLine + "Prio: " + InfoM[ID].Prio + Environment.NewLine + "Message: " + InfoM[ID].Message + Environment.NewLine;
                        return Out1;
                    }
                    else
                    {
                        return "LEER";
                    }
                }
                else if(MType == 1)
                {
                    if (ID < WarnM.Count)
                    {
                        string Out2 = "Source: " + WarnM[ID].ScriptName + Environment.NewLine + "Prio: " +  WarnM[ID].Prio + Environment.NewLine + "Message: " +  WarnM[ID].Message + Environment.NewLine;
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

                if(MType == 0)
                {
                    MAX = InfoM.Count;
                }else if(MType == 1)
                {
                    MAX = WarnM.Count;
                }


                return MAX;
            }









        }
    }
}


*/