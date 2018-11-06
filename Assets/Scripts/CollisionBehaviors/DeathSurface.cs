using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSurface : OnCollisionCustomAction {

    private Engine engine;
    private Color deathColor;

    public override void InitializeData()
    {
        engine = PhysicsManager.Instance.GetRegisteredPlayerEngine();
        deathColor = new Color((float) 167 / 255, (float) 32 / 255, (float) 29 / 255) ;
    }

    public override Vector2 OnCollisionDo()
    {
        if(engine != null)
        {
            engine.gameObject.GetComponent<SpriteRenderer>().color = deathColor;
        }
        return Vector2.zero;
    }

} 