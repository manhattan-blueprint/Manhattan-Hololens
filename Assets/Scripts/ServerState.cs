/*
Keeps track of instructions that have been sent, only adding new items to the list
of items to spawn if they have not yet been sent yet (unique only).
*/
using System;
using System.Collections.Generic;
using UnityEngine;

public class ServerState {
    
    private readonly List<Spawnable> spawnables;

    public ServerState() {
        spawnables = new List<Spawnable>();
    }
    
    public void AddInstruction(String instruction) {
        if (instruction[0] == 'I') { // Only want instructions to be added as spawnables.
            if (Unique(instruction)) { // Don't want duplicates in case the phone sends multiple.
                Debug.Log("Adding instruction " + instruction);
                Spawnable spawnable = new Spawnable(instruction);
                spawnables.Add(spawnable);
            }
        }
    }

    public String GetFreshSpawn() {
        int i = 0;
        while (i < spawnables.Count) {
            if (spawnables[i].spawned == false) {
                spawnables[i].spawned = true;
                return spawnables[i].instruction;
            }
            i++;
        }
        return "";
    }

    public Boolean Unique(String instruction) {
        Boolean unique = false;
        foreach (var spawnable in spawnables) {
            if (string.Equals(spawnable, instruction)) {
                unique = false;
            }
        }
        return unique;
    }
}
