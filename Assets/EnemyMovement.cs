using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class EnemyMovement : MonoBehaviour
{
    public AIPath AiPath;
    Vector2 Direction;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        faceVelocity();
    }

    void faceVelocity()
    {
        Direction = AiPath.desiredVelocity;
        transform.right = Direction;
    }
}
