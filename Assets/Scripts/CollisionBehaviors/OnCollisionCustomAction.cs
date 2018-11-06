using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnCollisionCustomAction : MonoBehaviour {

    public abstract Vector2 OnCollisionDo();

    public abstract void InitializeData();

}
