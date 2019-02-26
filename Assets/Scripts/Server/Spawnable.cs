/*
Minigame instances that can be spawned in.
*/
#if NETFX_CORE
using Windows.Foundation;
using System.Text.RegularExpressions;
#else
#endif
using System;
using UnityEngine;

namespace Server
{
    public class Spawnable
    {
        public String instruction;
        public Boolean spawned;
        public Boolean collected;
        public string type;
        public int uniqueID, amount;

        private float xCo, zCo;

        public Spawnable(String instruction)
        {
            this.instruction = instruction;
            this.spawned = false;
            this.collected = false;

            instruction = instruction.Replace("\n", "");
#if NETFX_CORE
                string[] temp = Regex.Split(instruction, ";");
#else
            string[] temp = instruction.Split(new string[] { ";" }, StringSplitOptions.None);
#endif
            uniqueID = int.Parse(temp[1], System.Globalization.CultureInfo.InvariantCulture);
            xCo = float.Parse(temp[2], System.Globalization.CultureInfo.InvariantCulture);
            zCo = float.Parse(temp[3], System.Globalization.CultureInfo.InvariantCulture);
            type = temp[4];
            amount = int.Parse(temp[5], System.Globalization.CultureInfo.InvariantCulture);
        }

        public string GetCollectedString()
        {
            string returnString = instruction;
            returnString = returnString.Substring(0, 29);
            returnString += amount.ToString().PadLeft(3, '0');
            return returnString;
        }

        public Vector3 GetPosition()
        {
            return new Vector3(xCo, 0.0f, zCo);
        }

        public void LogInfo()
        {
            Debug.Log("SpawnInfo for " + uniqueID + ": " + amount + " x " + type + " at location (" + xCo + ", " + zCo + ")");
        }
    }
}
