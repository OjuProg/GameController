using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : OnCollisionCustomAction {

    private Transform parentTransform;
    private bool isFalling;

    public override void InitializeData()
    {
        parentTransform = gameObject.transform.parent;
        isFalling = false;
    }

    public override Vector2 OnCollisionDo()
    {
        isFalling = true;
        return Vector2.zero;
    }

    private void Update()
    {
        if(isFalling && parentTransform != null)
        {
            parentTransform.position = new Vector2(parentTransform.position.x, 
                                                   parentTransform.position.y - PhysicsManager.Instance.GravityValue * Time.deltaTime);

            Vector3 parentPositionCamera = Camera.main.WorldToScreenPoint(parentTransform.position);
            if (parentPositionCamera.y < 0)
            {
                gameObject.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
