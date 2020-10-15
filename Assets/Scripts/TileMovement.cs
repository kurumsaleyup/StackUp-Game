using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMovement : MonoBehaviour
{
    private const float BOUNDS_SIZE = 3.5f;
    private float tileTransition = 0.0f;
    private float tileSpeed = 2.5f;
    public void MoveTile(bool isMovingX, GameObject tile, int scoreCount, float secondaryPosition)
    {
        tileTransition += Time.deltaTime * tileSpeed;
        if (isMovingX)
        {
            tile.transform.localPosition =
                new Vector3(Mathf.Sin(tileTransition) * BOUNDS_SIZE, scoreCount, secondaryPosition);
        }
        else
        {
            tile.transform.localPosition =
           new Vector3(secondaryPosition, scoreCount, Mathf.Sin(tileTransition) * BOUNDS_SIZE);
        }
    }
}
