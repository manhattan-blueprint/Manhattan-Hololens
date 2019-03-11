using System;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    /// <summary>
    /// Stores the state of the server to be read by classes involved in either networking or server management.
    /// </summary>
    public class ServerState
    {
        [Tooltip("Message for if an instruction has been seen before but the minigame has note yet been completed.")]
        public readonly string rejectionCode = "Not Complete";

        [Tooltip("Message for when an object is received that no processing method has been implemented.")]
        public readonly string invalidCode = "Invalid Message";

        private Dictionary<int, Spawnable> spawnables;

        public ServerState()
        {
            spawnables = new Dictionary<int, Spawnable>();
        }

        /// <summary>
        /// Interprets an instruction and adds it to the of minigames to spawn if unique.
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        public string ProcessInstruction(String instruction)
        {
            Debug.Log("Checking " + instruction + " for an instruction");
            if (instruction[0] == 'I')
            {
                Debug.Log("Instruction found");
                Spawnable spawnable = new Spawnable(instruction);

                if (!spawnables.ContainsKey(spawnable.uniqueID))
                {
                    Debug.Log("Adding instruction " + instruction);
                    spawnables.Add(spawnable.uniqueID, spawnable);
                }
                else
                {
                    if (spawnables[spawnable.uniqueID].collected)
                    {
                        return spawnables[spawnable.uniqueID].GetCollectedString();
                    }
                }
            }
            else if (instruction[0] == 'D')
            {
                return "NO RESPONSE";
            }
            else
            {
                Debug.Log("Message is not an instruction");
                return invalidCode;
            }
            return rejectionCode;
        }

        /// <summary>
        /// Returns the minigames that are ready to be spawned.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Spawnable> GetSpawnables()
        {
            return spawnables;
        }
        
        /// <summary>
        /// Notifies the ServerState of a minigame being complete.
        /// </summary>
        /// <param name="uniqueID"></param>
        public void NotifyComplete(int uniqueID, int amount)
        {
            spawnables[uniqueID].collected = true;
            spawnables[uniqueID].amount = amount;
        }
    }
}
