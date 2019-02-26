/*
Keeps track of instructions that have been sent, only adding new items to the list
of items to spawn if they have not yet been sent yet (unique only).
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class ServerState
    {
        public readonly string rejectionCode = "Not Complete";
        public readonly string invalidCode = "Invalid Message";
        public Dictionary<int, Spawnable> spawnables;

        public ServerState()
        {
            spawnables = new Dictionary<int, Spawnable>();
        }

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
            else
            {
                Debug.Log("Message is not an instruction");
                return invalidCode;
            }
            return rejectionCode;
        }
        
        public void NotifyComplete(int uniqueID)
        {
            spawnables[uniqueID].collected = true;
        }
    }
}
