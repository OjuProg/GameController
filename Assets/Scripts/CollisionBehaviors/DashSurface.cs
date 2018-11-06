using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSurface : OnCollisionCustomAction
{
    private Engine engine;

    public override void InitializeData()
    {
        engine = PhysicsManager.Instance.GetRegisteredPlayerEngine();
    }

    public override Vector2 OnCollisionDo()
    {
        if(engine != null && PhysicsManager.Instance != null)
        {
            return new Vector2(engine.Speed.x * PhysicsManager.Instance.dashSurfaceMultiplier, 0) * Time.deltaTime;
        }
        return Vector2.zero;
    }
}
