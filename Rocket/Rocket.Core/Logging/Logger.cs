﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Rocket.Core.Logging
{
    public partial class Logger
    {
        private static string lastAssembly = "";

        [Obsolete("Log(string message,bool sendToConsole) is obsolete, use Log(string message,ConsoleColor color) instead",true)]
        public static void Log(string message, bool sendToConsole)
        {
            Log(message, ConsoleColor.White);
        }

        public static void Log(string message, ConsoleColor color = ConsoleColor.White)
        {
            if (message == null) return;
            string assembly = "";
            try
            {
                System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();

                if (stackTrace.FrameCount != 0)
                    assembly = stackTrace.GetFrame(1).GetMethod().DeclaringType.Assembly.GetName().Name;

                if ((assembly.StartsWith("Rocket.") || assembly == "Assembly-CSharp" || assembly == "UnityEngine") && stackTrace.FrameCount > 2)
                {
                    assembly = stackTrace.GetFrame(2).GetMethod().DeclaringType.Assembly.GetName().Name;
                }
                if (assembly == "" || assembly == typeof(Logger).Assembly.GetName().Name || assembly == lastAssembly || assembly.StartsWith("Rocket.") || assembly == "Assembly-CSharp" || assembly == "UnityEngine")
                {
                    assembly = "";
                }
                else
                {
                    assembly = assembly + " >> ";
                }

                lastAssembly = assembly;
                message = assembly + message;
            }
            catch (Exception)
            {
                throw;
            }
            ProcessInternalLog(ELogType.Info, message,color);
        }

        internal static string var_dump(object obj, int recursion)
        {
            StringBuilder result = new StringBuilder();

            if (recursion < 5)
            {
                Type t = obj.GetType();
                PropertyInfo[] properties = t.GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    try
                    {
                        object value = property.GetValue(obj, null);
                        string indent = String.Empty;
                        string spaces = "|   ";
                        string trail = "|...";

                        if (recursion > 0)
                        {
                            indent = new StringBuilder(trail).Insert(0, spaces, recursion - 1).ToString();
                        }

                        if (value != null)
                        {
                            string displayValue = value.ToString();
                            if (value is string) displayValue = String.Concat('"', displayValue, '"');
                            result.AppendFormat("{0}{1} = {2}\n", indent, property.Name, displayValue);

                            try
                            {
                                if (!(value is ICollection))
                                {
                                    result.Append(var_dump(value, recursion + 1));
                                }
                                else
                                {
                                    int elementCount = 0;
                                    foreach (object element in ((ICollection)value))
                                    {
                                        string elementName = String.Format("{0}[{1}]", property.Name, elementCount);
                                        indent = new StringBuilder(trail).Insert(0, spaces, recursion).ToString();
                                        result.AppendFormat("{0}{1} = {2}\n", indent, elementName, element.ToString());
                                        result.Append(var_dump(element, recursion + 2));
                                        elementCount++;
                                    }

                                    result.Append(var_dump(value, recursion + 1));
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            result.AppendFormat("{0}{1} = {2}\n", indent, property.Name, "null");
                        }
                    }
                    catch
                    {
                        // Some properties will throw an exception on property.GetValue()
                        // I don't know exactly why this happens, so for now i will ignore them...
                    }
                }
            }

            return result.ToString();
        }

        public static void LogWarning(string message)
        {
            if (message == null) return;
            ProcessInternalLog(ELogType.Warning, message);
        }

        public static void LogError(string message)
        {
            if (message == null) return;
            ProcessInternalLog(ELogType.Error, message);
        }

        internal static void LogError(Exception ex, string v)
        {
            LogException(ex,v);
        }

        public static void Log(Exception ex)
        {
            LogException(ex);
        }

        public static void LogException(Exception ex,string message = null)
        {
            string source = "";
            string assembly = "";
            try
            {
                System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
                if (stackTrace.FrameCount != 0)
                {
                    source = stackTrace.GetFrame(1).GetMethod().DeclaringType.Assembly.GetName().Name;
                    assembly = stackTrace.GetFrame(1).GetMethod().DeclaringType.Assembly.GetName().Name;
                }
                if (assembly.StartsWith("Rocket.") && stackTrace.FrameCount > 1)
                {
                    source = stackTrace.GetFrame(2).GetMethod().Name;
                    assembly = stackTrace.GetFrame(2).GetMethod().DeclaringType.Assembly.GetName().Name;
                }
                lastAssembly = assembly;

                if (assembly != "") assembly += " >> ";
            }
            catch (Exception)
            {
                LogError("Caught exception while logging an exception! Ouch...");
            }

            ProcessInternalLog(ELogType.Exception, assembly + (message != null ? message +" -> ": "") + "Exception in " + source + ": " + ex);
        }

        private static void ProcessInternalLog(ELogType type, string message, ConsoleColor color = ConsoleColor.White)
        {
            if (type == ELogType.Error || type == ELogType.Exception)
            {
                SDG.Unturned.CommandWindow.LogError(message);
            }
            else if (type == ELogType.Warning)
            {
                SDG.Unturned.CommandWindow.LogWarning(message);
            }
            else
            {
                SDG.Unturned.CommandWindow.Log(message);
            }

            // LDM: originally Rocket called ProcessLog here to write the message to their log file as well as the Console.
            // After LDM was updated to use the CommandWindow methods instead of the Console directly this caused each 
            // message to be logged twice because Rocket logs the output of CommandWindow.onCommandWindowOutputted.
        }
        
        /// <summary>
        /// LDM: this method writes a line to the Rocket log file.
        /// </summary>
        private static void ProcessLog(ELogType type, string message,bool rcon = true)
        {
            AsyncLoggerQueue.Current.Enqueue(new LogEntry() { Severity = type, Message = message, RCON = rcon });
        }

        /// <summary>
        /// LDM: Rocket.Unturned.U relays the CommandWindow.onCommandWindowOutputted output here.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void ExternalLog(object message, ConsoleColor color)
        {
            ELogType severity;
            switch (color)
            {
                case ConsoleColor.Red:
                    severity = ELogType.Error;
                    break;
                case ConsoleColor.Yellow:
                    severity = ELogType.Warning;
                    break;
                default:
                    severity = ELogType.Info;
                    break;
            }
            ProcessLog(severity, message.ToString());
        }
    }
}
