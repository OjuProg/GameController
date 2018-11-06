using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SHOULD BE USED ON AS A TOP SURFACE
public class TopMovingSurface : OnCollisionCustomAction {
    private MovingPlatform parentPlatform;
    private Engine engine;

    public override void InitializeData()
    {
        parentPlatform = gameObject.transform.parent.gameObject.GetComponent<MovingPlatform>();
        engine = PhysicsManager.Instance.GetRegisteredPlayerEngine();
    }

    public override Vector2 OnCollisionDo()
    {
        if (parentPlatform != null && engine != null)
        {
            float speed = parentPlatform.speed;
            Vector2 currentAmplitude = parentPlatform.moveAmplitude;
            engine.clampYBelowValue += speed * currentAmplitude.y * Time.deltaTime;
            return new Vector2(speed * currentAmplitude.x * Time.deltaTime, 0);
        }

        return Vector2.zero;
    }
}
