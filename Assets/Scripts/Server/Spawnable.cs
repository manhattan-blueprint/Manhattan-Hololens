/*
Minigame instances that can be spawned in.
*/
#if NETFX_CORE
using System.Text.RegularExpressions;
#else
#endif
using System;
using UnityEngine;

namespace Server
{
    /// <summary>
    /// Contains and interprets information from an instruction about spawning a minigame.
    /// </summary>
    public class Spawnable
    {
        [Tooltip("Whether the instructions minigame has been spawned in the Unity world or not.")]
        public Boolean spawned;

        [Tooltip("Whether the instructions minigame has been completed or not.")]
        public Boolean collected;
        
        [Tooltip("The type of the object to spawn (4 letters).")]
        public string type;

        [Tooltip("The unique code associated with the instruction.")]
        public int uniqueID;

        [Tooltip("The amount of objects to spawn.")]
        public int amount;

        private float xCo, zCo;
        private String instruction;

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
            // Weirdness required for UWP.
            uniqueID = int.Parse(temp[1], System.Globalization.CultureInfo.InvariantCulture);
            xCo = float.Parse(temp[2], System.Globalization.CultureInfo.InvariantCulture);
            zCo = float.Parse(temp[3], System.Globalization.CultureInfo.InvariantCulture);
            type = temp[4];
            amount = int.Parse(temp[5], System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the instruction string to the correct response for the number of items collected.
        /// TODO: Update this to allow for failure; currently responds as all item collections having
        /// been successful.
        /// </summary>
        /// <returns></returns>
        public string GetCollectedString()
        {
            string returnString = instruction;
            returnString = returnString.Substring(0, 29);
            returnString += amount.ToString().PadLeft(3, '0');
            return returnString;
        }

        /// <summary>
        /// Gets the Unity game coordinate of the location to spawn the object.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPosition()
        {
            return new Vector3(xCo, 0.0f, zCo);
        }

        /// <summary>
        /// Logs the instruction interpretation.
        /// </summary>
        public void LogInfo()
        {
            Debug.Log("SpawnInfo for " + uniqueID + ": " + amount + " x " + type + " at location (" + xCo + ", " + zCo + ")");
        }
    }
}
