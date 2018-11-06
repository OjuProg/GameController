using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour {

    [SerializeField]
    private float gravityValue;
    public float GravityValue
    {
        get
        {
            return gravityValue;
        }

        private set
        {
            this.gravityValue = value;
        }
    }

    [SerializeField] [Range(0.0f, 1.0f)]
    private float wallJumpSurfaceStickiness;
    public float WallJumpSurfaceStickiness
    {
        get
        {
            return wallJumpSurfaceStickiness;
        }

        private set
        {
            this.wallJumpSurfaceStickiness = value;
        }
    }

    [Range(1.0f, 4.0f)]
    public float dashSurfaceMultiplier;

    private int wallSurfaceCounter;

    [SerializeField]
    private Engine playerEngine;

    public static PhysicsManager Instance
    {
        get;
        private set;
    }


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is multiple instance of singleton PhysicsManager");
            return;
        }

        Instance = this;

        wallSurfaceCounter = 0;
    }

    public int AssignWallJumpSurfaceID(WallJumpSurface wallJumpSurface)
    {
        if(wallJumpSurface != null)
        {
            int id = wallSurfaceCounter;
            wallSurfaceCounter++;
            return id;
        }

        else
        {
            return -1;
        }
    }

    public Engine GetRegisteredPlayerEngine()
    {
        return playerEngine ?? null;
    }
}
