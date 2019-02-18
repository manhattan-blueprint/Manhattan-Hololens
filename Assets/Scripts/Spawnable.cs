/*
Retrieves the IP address and port to communicate on
*/
using System;
using UnityEditor;

public class Spawnable
{
    public String instruction;
    public Boolean spawned;
    public Boolean collected;

    public Spawnable(String instruction)
    {
        this.instruction = instruction;
        this.spawned = false;
        this.collected = false;
    }
}
