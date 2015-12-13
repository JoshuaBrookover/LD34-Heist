using UnityEngine;
using System.Collections;

public class CoverPositionController : MonoBehaviour {

    protected float moveXAxis = 0;

    public float GetMovmement()
    {
        return moveXAxis;
    }

    public void BroadcastPlayerMovment (float playerMoveXAxis)
    {
        moveXAxis = -playerMoveXAxis;
    }
}
