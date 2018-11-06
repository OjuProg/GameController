using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpSurface : OnCollisionCustomAction {

    [SerializeField]
    private int wallID;

    private Engine engine;

    public int WallID
    {
        get
        {
            return WallID;
        }

        private set
        {
            wallID = value;
        }
    }

    public override Vector2 OnCollisionDo()
    {
        if(engine != null)
        {
            engine.lastWallHitID = wallID;

            if (engine.lastWallJumpPerformedID != wallID)
            {
                engine.canWallJump = true;
                engine.ResetJumpCount();

                float fallSlowedValue = 0;

                if (PhysicsManager.Instance != null)
                {
                    fallSlowedValue = PhysicsManager.Instance.GravityValue * PhysicsManager.Instance.WallJumpSurfaceStickiness * Time.deltaTime;
                    engine.PercentageOfGravity = 1;
                }
                return new Vector2(0, fallSlowedValue);
            }
        }

        return Vector2.zero;
    }

    public override void InitializeData()
    {
        wallID = PhysicsManager.Instance.AssignWallJumpSurfaceID(this);
        engine = PhysicsManager.Instance.GetRegisteredPlayerEngine();
    }
}
