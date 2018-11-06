using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CollisionCustomActionManager : MonoBehaviour {

    [SerializeField]
    private GameObject platformTopCollider;
    [SerializeField]
    private MonoScript topCustomAction;

    [SerializeField]
    private GameObject platformBottomCollider;
    [SerializeField]
    private MonoScript bottomCustomAction;

    [SerializeField]
    private GameObject platformRightCollider;
    [SerializeField]
    private MonoScript rightCustomAction;

    [SerializeField]
    private GameObject platformLeftCollider;
    [SerializeField]
    MonoScript leftCustomAction;

    private void Start()
    {
        if(platformTopCollider != null && topCustomAction != null && topCustomAction.GetClass().IsSubclassOf(typeof(OnCollisionCustomAction)))
        {
            OnCollisionCustomAction customAction = platformTopCollider.AddComponent(topCustomAction.GetClass()) as OnCollisionCustomAction;
            customAction.InitializeData();
        }

        if (platformBottomCollider != null && bottomCustomAction != null && bottomCustomAction.GetClass().IsSubclassOf(typeof(OnCollisionCustomAction)))
        {
            OnCollisionCustomAction customAction = platformBottomCollider.AddComponent(bottomCustomAction.GetClass()) as OnCollisionCustomAction;
            customAction.InitializeData();
        }

        if (platformRightCollider != null && rightCustomAction != null && rightCustomAction.GetClass().IsSubclassOf(typeof(OnCollisionCustomAction)))
        {
            OnCollisionCustomAction customAction = platformRightCollider.AddComponent(rightCustomAction.GetClass()) as OnCollisionCustomAction;
            customAction.InitializeData();
        }

        if (platformLeftCollider != null && leftCustomAction != null && leftCustomAction.GetClass().IsSubclassOf(typeof(OnCollisionCustomAction)))
        {
            OnCollisionCustomAction customAction = platformLeftCollider.AddComponent(leftCustomAction.GetClass()) as OnCollisionCustomAction;
            customAction.InitializeData();
        }
    }
}
