using System;
using System.Collections.Generic;
using System.Globalization;
using Rocket.API;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace Rocket.Unturned.Chat
{
    public sealed class UnturnedChat : MonoBehaviour
    {
        private void Awake()
        {
            ChatManager.onChatted += HandleChat;
        }

        private void HandleChat(SteamPlayer steamPlayer, EChatMode chatMode, ref Color incomingColor, ref bool rich, string message, ref bool cancel)
        {
            cancel = false;

            var color = incomingColor;
            try
            {
                var player = UnturnedPlayer.FromSteamPlayer(steamPlayer);
                color = UnturnedPlayerEvents.firePlayerChatted(player, chatMode, player.Color, message, ref cancel);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            cancel = !cancel;
            incomingColor = color;
        }

        public static Color GetColorFromName(string colorName, Color fallback)
        {
            switch (colorName.Trim().ToLower())
            {
                case "black": return Color.black;
                case "blue": return Color.blue;
                case "clear": return Color.clear;
                case "cyan": return Color.cyan;
                case "gray": return Color.gray;
                case "green": return Color.green;
                case "grey": return Color.grey;
                case "magenta": return Color.magenta;
                case "red": return Color.red;
                case "white": return Color.white;
                case "yellow": return Color.yellow;
                case "rocket": return GetColorFromRGB(90, 206, 205);
            }

            var color = GetColorFromHex(colorName);
            return color ?? fallback;
        }

        public static Color? GetColorFromHex(string hexString)
        {
            hexString = hexString.Replace("#", "");
            if(hexString.Length == 3)
            {
                hexString = hexString.Insert(1, Convert.ToString(hexString[0])); // #999f
                hexString = hexString.Insert(3, Convert.ToString(hexString[2])); // #9999f
                hexString = hexString.Insert(5, Convert.ToString(hexString[4])); // #9999ff
            }
            
            if (hexString.Length != 6 || !int.TryParse(hexString, NumberStyles.HexNumber, null, out var argb))
            {
                return null;
            }

            var r = (byte)((argb >> 16) & 0xff);
            var g = (byte)((argb >> 8) & 0xff);
            var b = (byte)(argb & 0xff);
            
            return GetColorFromRGB(r, g, b);
        }

		public static Color GetColorFromRGB(byte R,byte G,byte B)
		{
			return GetColorFromRGB (R, G, B, 100);
		}

        public static Color GetColorFromRGB(byte R,byte G,byte B,short A)
        {
            return new Color(1f / 255f * R, 1f / 255f * G, 1f / 255f * B,1f/100f * A);
        }

        public static void Say(string message, bool rich)
        {
            Say(message, Palette.SERVER, rich);
        }

        public static void Say(string message)
        {
            Say(message, false);
        }

        public static void Say(IRocketPlayer player, string message)
        {
            Say(player, message, false);
        }

        public static void Say(IRocketPlayer player, string message, Color color, bool rich)
        {
            if (player is ConsolePlayer)
            {
                Logger.Log(message, ConsoleColor.Gray);
            }
            else
            {
                Say(new CSteamID(ulong.Parse(player.Id)), message, color);
            }
        }

        public static void Say(IRocketPlayer player, string message, Color color)
        {
            Say(player, message, color, false);
        }

        public static void Say(string message, Color color, bool rich)
        {
            Logger.Log("Broadcast: " + message, ConsoleColor.Gray);
            foreach (var m in wrapMessage(message))
            {
                ChatManager.instance.channel.send("tellChat", ESteamCall.OTHERS, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, CSteamID.Nil, string.Empty, (byte)EChatMode.GLOBAL, color, rich, m);
            }
        }
        
        public static void Say(string message, Color color)
        {
            Say(message, color, false);
        }
        
        public static void Say(IRocketPlayer player, string message, bool rich)
        {
            Say(player, message, Palette.SERVER, rich);
        }

        public static void Say(CSteamID CSteamID, string message, bool rich)
        {
            Say(CSteamID, message, Palette.SERVER, rich);
        }

        public static void Say(CSteamID CSteamID, string message)
        {
            Say(CSteamID, message, false);
        }

        public static void Say(CSteamID CSteamID, string message, Color color, bool rich)
        {
            if (CSteamID == null || CSteamID.ToString() == "0")
            {
                Logger.Log(message, ConsoleColor.Gray);
            }
            else
            {
                foreach (var m in wrapMessage(message))
                {
                    ChatManager.instance.channel.send("tellChat", CSteamID, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, CSteamID.Nil, string.Empty, (byte)EChatMode.SAY, color, rich, m);
                }
            }
        }

        public static void Say(CSteamID CSteamID, string message, Color color)
        {
            Say(CSteamID, message, color, false);
        }

         public static List<string> wrapMessage(string text)
         {
             if (text.Length == 0)
             {
                 return new List<string>();
             }

             const int maxLength = 90;

             var lines = new List<string>();
             var currentLine = "";

             foreach (var currentWord in text.Split(' '))
             {
  
                 if (currentLine.Length > maxLength ||
                     currentLine.Length + currentWord.Length > maxLength)
                 {
                     lines.Add(currentLine);
                     currentLine = "";
                 }
  
                 if (currentLine.Length > 0)
                     currentLine += " " + currentWord;
                 else
                     currentLine += currentWord;
  
             }
  
             if (currentLine.Length > 0)
             {
                 lines.Add(currentLine);
             }

             return lines;
         }
    }
}