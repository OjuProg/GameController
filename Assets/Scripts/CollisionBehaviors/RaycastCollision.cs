using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCollision : MonoBehaviour
{
    private enum RayType {SouthWestDown, SouthEastDown, NorthWestTop,
                          NorthEastTop, SouthWestLeft, NorthWestLeft,
                          NorthEastRight, SouthEastRight};

    [SerializeField]
    private Engine engine;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private float spriteHalfSizeX;
    private float spriteHalfSizeY;
    private float spriteScaleX;
    private float spriteScaleY;

    private float castingDistanceX;
    private float castingDistanceY;

    // Casting points origins
    private Vector2 southWestOrigin;
    private Vector2 southEastOrigin;
    private Vector2 northWestOrigin;
    private Vector2 northEastOrigin;

    // Has a ray hit something on this frame?
        // Down
    private bool downIsBeingClamped;
    private bool hitSouthWestDown;
    private bool hitSouthEastDown;
        // Top
    private bool topIsBeingClamped;
    private bool hitNorthWestTop;
    private bool hitNorthEastTop;
        // Left 
    private bool leftIsBeingClamped;
    private bool hitSouthWestLeft;
    private bool hitNorthWestLeft;
        // Left 
    private bool rightIsBeingClamped;
    private bool hitSouthEastRight;
    private bool hitNorthEastRight;

    [SerializeField]
    private LayerMask collisionXLayer;
    [SerializeField]
    private LayerMask collisionYLayer;

    // Use this for initialization
    private void Start()
    {
        if (spriteRenderer != null)
        {
            Vector2 spriteSize = spriteRenderer.size;
            Vector2 spriteScale = spriteRenderer.gameObject.transform.localScale;

            spriteHalfSizeX = spriteSize.x / 2;
            spriteHalfSizeY = spriteSize.y / 2;
            spriteScaleX = spriteScale.x;
            spriteScaleY = spriteScale.y;

            castingDistanceX = spriteHalfSizeX * spriteScaleX * 0.75f;
            castingDistanceY = spriteHalfSizeY * spriteScaleY * 0.75f;

            downIsBeingClamped = false;
            hitSouthWestDown = false;
            hitSouthEastDown = false;

            topIsBeingClamped = false;
            hitNorthWestTop = false;
            hitNorthEastTop = false;

            leftIsBeingClamped = false;
            hitSouthWestLeft = false;
            hitNorthWestLeft = false;

            rightIsBeingClamped = false;
            hitSouthEastRight = false;
            hitNorthEastRight = false;
        }
    }

    private void UpdateRaycastOrigins()
    {
        Vector2 currentPosition = transform.position;
        southWestOrigin = currentPosition + new Vector2(-0.99f * spriteHalfSizeX * spriteScaleX, -0.99f * spriteHalfSizeY * spriteScaleY);
        southEastOrigin = currentPosition + new Vector2(0.99f *spriteHalfSizeX * spriteScaleX, -0.99f * spriteHalfSizeY * spriteScaleY);
        northWestOrigin = currentPosition + new Vector2(-0.99f * spriteHalfSizeX * spriteScaleX, 0.99f * spriteHalfSizeY * spriteScaleY);
        northEastOrigin = currentPosition + new Vector2(0.99f * spriteHalfSizeX * spriteScaleX, 0.99f * spriteHalfSizeY * spriteScaleY);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRaycastOrigins();
        ShootRays();
        CheckUnclamp();
    }

    private void CheckCollision(RaycastHit2D hit, RayType rayType)
    {
        if (hit.collider != null)
        {
            OnCollisionCustomAction customAction = hit.collider.GetComponent<OnCollisionCustomAction>();
            float hitPointCoordinate = 0;

            switch(hit.collider.name)
            {
                case "Top":
                    if(rayType == RayType.SouthWestDown)
                    {
                        hitPointCoordinate = hit.point.y + spriteHalfSizeY * spriteScaleY;
                        hitSouthWestDown = true;
                    }
                    else if (rayType == RayType.SouthEastDown)
                    {
                        hitPointCoordinate = hit.point.y + spriteHalfSizeY * spriteScaleY;
                        hitSouthEastDown = true;
                    }

                    if((hitSouthWestDown || hitSouthEastDown) && !downIsBeingClamped) {
                        engine.ClampYBelow(true, hitPointCoordinate, customAction);
                        downIsBeingClamped = true;
                    }
                    break;

                case "Bottom":
                    if (rayType == RayType.NorthWestTop)
                    {
                        hitPointCoordinate = hit.point.y - spriteHalfSizeY * spriteScaleY;
                        hitNorthWestTop = true;
                    }
                    else if (rayType == RayType.NorthEastTop)
                    {
                        hitPointCoordinate = hit.point.y - spriteHalfSizeY * spriteScaleY;
                        hitNorthEastTop = true;
                    }

                    if((hitNorthWestTop || hitNorthEastTop) && !topIsBeingClamped)
                    {
                        engine.ClampYAbove(true, hitPointCoordinate, customAction);
                        topIsBeingClamped = true;
                    }
                    break;

                case "Left":
                    if (rayType == RayType.SouthWestLeft)
                    {
                        hitPointCoordinate = hit.point.x + spriteHalfSizeX * spriteScaleX;
                        hitSouthWestLeft = true;
                    }
                    else if (rayType == RayType.NorthWestLeft)
                    {
                        hitPointCoordinate = hit.point.x + spriteHalfSizeX * spriteScaleX;
                        hitNorthWestLeft = true;
                    }

                    if((hitSouthWestLeft || hitNorthWestLeft) && !leftIsBeingClamped)
                    {
                        engine.ClampXBelow(true, hitPointCoordinate, customAction);
                        leftIsBeingClamped = true;
                    }
                    break;

                case "Right":
                    if (rayType == RayType.SouthEastRight)
                    {
                        hitPointCoordinate = hit.point.x - spriteHalfSizeX * spriteScaleX;
                        hitSouthEastRight = true;
                    }
                    else if (rayType == RayType.NorthEastRight)
                    {
                        hitPointCoordinate = hit.point.x - spriteHalfSizeX * spriteScaleX;
                        hitNorthEastRight = true;
                    }

                    if((hitNorthEastRight || hitSouthEastRight) && !rightIsBeingClamped)
                    {
                        engine.ClampXAbove(true, hitPointCoordinate, customAction);
                        rightIsBeingClamped = true;
                    }
                    break;

                default:
                    break;
            }
        }

        else
        {
            switch(rayType)
            {
                // Bottom
                case RayType.SouthWestDown:
                    downIsBeingClamped = false;
                    hitSouthWestDown = false;
                    break;
                case RayType.SouthEastDown:
                    downIsBeingClamped = false;
                    hitSouthEastDown = false;
                    break;
                // Top
                case RayType.NorthWestTop:
                    topIsBeingClamped = false;
                    hitNorthWestTop = false;
                    break;
                case RayType.NorthEastTop:
                    topIsBeingClamped = false;
                    hitNorthEastTop = false;
                    break;
                // Left
                case RayType.SouthWestLeft:
                    leftIsBeingClamped = false;
                    hitSouthWestLeft = false;
                    break;
                case RayType.NorthWestLeft:
                    leftIsBeingClamped = false;
                    hitNorthWestLeft = false;
                    break;
                // Right
                case RayType.SouthEastRight:
                    rightIsBeingClamped = false;
                    hitSouthEastRight = false;
                    break;
                case RayType.NorthEastRight:
                    rightIsBeingClamped = false;
                    hitNorthEastRight = false;
                    break;
                default:
                    break;
            }
        }
    }

    private void CheckUnclamp()
    {
        // Top
        if(!hitSouthWestDown && !hitSouthEastDown)
        {
            engine.ClampYBelow(false);
        }
        // Bottom
        if(!hitNorthWestTop && !hitNorthEastTop)
        {
            engine.ClampYAbove(false);
        }
        // Left
        if(!hitSouthWestLeft && !hitNorthWestLeft)
        {
            engine.ClampXBelow(false);
        }
        // Right
        if(!hitNorthEastRight && !hitSouthEastRight)
        {
            engine.ClampXAbove(false);
        }
    }

    private void ShootRays()
    {
        // Shooting rays in all directions
            // Down
        RaycastHit2D southWestDownHit = Physics2D.Raycast(southWestOrigin, Vector2.down, castingDistanceY, collisionYLayer);
        RaycastHit2D southEastDownHit = Physics2D.Raycast(southEastOrigin, Vector2.down, castingDistanceY, collisionYLayer);
            // Top
        RaycastHit2D northWestDownHit = Physics2D.Raycast(northWestOrigin, Vector2.up, castingDistanceY, collisionYLayer);
        RaycastHit2D northEastDownHit = Physics2D.Raycast(northEastOrigin, Vector2.up, castingDistanceY, collisionYLayer);
            // Left
        RaycastHit2D southWestLeftHit = Physics2D.Raycast(southWestOrigin, Vector2.left, castingDistanceX, collisionXLayer);
        RaycastHit2D northWestLeftHit = Physics2D.Raycast(northWestOrigin, Vector2.left, castingDistanceX, collisionXLayer);
            // Right
        RaycastHit2D southEastRightHit = Physics2D.Raycast(southEastOrigin, Vector2.right, castingDistanceX, collisionXLayer);
        RaycastHit2D northEastRightHit = Physics2D.Raycast(northEastOrigin, Vector2.right, castingDistanceX, collisionXLayer);


        // Checking all the hit
            // Down
        CheckCollision(southWestDownHit, RayType.SouthWestDown);
        CheckCollision(southEastDownHit, RayType.SouthEastDown);
            // Top
        CheckCollision(northWestDownHit, RayType.NorthWestTop);
        CheckCollision(northEastDownHit, RayType.NorthEastTop);
            //Left
        CheckCollision(southWestLeftHit, RayType.SouthWestLeft);
        CheckCollision(northWestLeftHit, RayType.NorthWestLeft);
            //Right
        CheckCollision(southEastRightHit, RayType.SouthEastRight);
        CheckCollision(northEastRightHit, RayType.NorthEastRight);
    }
}
