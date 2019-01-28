/*
Retrieves the IP address and port to communicate on
*/
using System;
using UnityEditor;

public class Spawnable
{
    public String instruction;
    public Boolean spawned;

    public Spawnable(String instruction)
    {
        this.instruction = instruction;
        this.spawned = false;
    }
}
