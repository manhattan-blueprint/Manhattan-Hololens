using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames
{
    public enum MinigameState
    {
        Idle,       // Ready to start but not doing anything
        Started,    // Currently in progress
        Completed,  // Completed but not finalised
        Done        //
    }

    public interface Minigame
    {
        // Returns the current game state.
        MinigameState GetState();

        // Called once upon start of minigame.
        void Start(int spawnQuantity);

        // Called repeatedly in line with the manager during the minigame.
        void Update();

        // Called once after minigame is complete.
        int Finish();
        
        Vector3 GetEpicentre();
    }
}
