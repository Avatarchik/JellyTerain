using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ROAD_SHAPE
{
    RS_STRAIGHT,
    RS_RGIHT_TURN,
    RS_LEFT_TURN,
    RS_LEFT_TURN_UP,
    RS_LEFT_ROTATE_UP,
    RS_BRIDGE_1,
    RS_BRDIGE_2,
    RS_HILL_DOWN,
    RS_TRAMPLINE,
    RS_START,
    RS_END,
    RS_EMPTY
}


public class CRoadObject : MonoBehaviour
{
    public ROAD_SHAPE rShape;
    public GameObject startPos, endPos;
    public Vector3 playerDirection;

    public int curObjDirection = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(ROAD_SHAPE shape, Vector3 startpos)
    {

    }

    public void Awake()
    {
        if(rShape == ROAD_SHAPE.RS_START)
        {
            curObjDirection = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
