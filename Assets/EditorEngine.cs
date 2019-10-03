using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
using System.IO;
using System;
public class EditorEngine : MonoBehaviour {

    public GameObject roadParent;
    public GameObject levelsContent;
    public GameObject roadContent;
    public Button levelButton;
    public Button objButton;
    public GameObject[] roadPrefabs;

    JsonObject jLevelsInformation;
    public int curSelLevel = 0;
    JsonObject curSelJsonLevel = null;

    int maxLevel = 0;

    // Use this for initialization
    void Start() {

    }

    public void InitInfoState()
    {
        for(int i = roadParent.transform.childCount - 1; i >= 0; i --)
        {
            Destroy(roadParent.transform.GetChild(i).gameObject);
        }

        for (int i = roadContent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(roadContent.transform.GetChild(i).gameObject);
        }
    }

    public void AddNewLevel()
    {
        InitInfoState();
        maxLevel++;
        JsonObject nLevelJson = new JsonObject();
        if (jLevelsInformation.ContainsKey(maxLevel.ToString()))
        {

        }
        else
        {
            //            jLevelsInformation.Add(maxLevel.ToString(), nLevelJson);
            nLevelJson.Add("info", new JsonArray());
            curSelJsonLevel = nLevelJson;

            curSelLevel = maxLevel;

            string name = "Level" + maxLevel;

            GameObject nBtn = GameObject.Instantiate(levelButton.gameObject) as GameObject;
            nBtn.transform.GetChild(0).gameObject.GetComponent<Text>().text = name;
            nBtn.transform.parent = levelsContent.transform;
            nBtn.gameObject.name = name;

            LoadLevelObjectsWithJson(curSelJsonLevel);
        }
    }

    //public enum ROAD_SHAPE
    //{
    //    RS_STRAIGHT,
    //    RS_RGIHT_TURN,
    //    RS_LEFT_TURN,
    //    RS_LEFT_TURN_UP,
    //    RS_LEFT_ROTATE_UP,
    //    RS_BRIDGE_1,
    //    RS_BRDIGE_2,
    //    RS_HILL_DOWN,
    //    RS_TRAMPLINE,
    //    RS_START,
    //    RS_END,
    //    RS_EMPTY
    //}

    public void LoadLevelObjectsWithJson(JsonObject jLevel)
    {
        if (!jLevel.ContainsKey("info"))
        {
            return;
        }
        JsonArray jLevelObjs = (JsonArray)jLevel["info"];
        if (jLevelObjs.Count == 0)
            return;

        for (int i = 0; i < jLevelObjs.Count; i++)
        {
            string objName = Convert.ToString(jLevelObjs[i]);
            GameObject gmRoadObj = GetGameObjectFromName(objName);
            LocateGameObjectToPosition(gmRoadObj, i);
        }
    }

    public void RoadObjectButtonSelected(GameObject gmRoadObj)
    {

    }

    public string GetStringFromShape(ROAD_SHAPE rShape)
    {
        string str = "";

        switch(rShape)
        {
            case ROAD_SHAPE.RS_START:
                str = "RS_START";
                break;
            case ROAD_SHAPE.RS_END:
                str = "RS_END";
                break;
            case ROAD_SHAPE.RS_BRDIGE_2:
                str = "RS_BRDIGE_2";
                break;
            case ROAD_SHAPE.RS_BRIDGE_1:
                str = "RS_BRIDGE_1";
                break;
            case ROAD_SHAPE.RS_HILL_DOWN:
                str = "RS_HILL_DOWN";
                break;
            case ROAD_SHAPE.RS_LEFT_ROTATE_UP:
                str = "RS_LEFT_ROTATE_UP";
                break;
            case ROAD_SHAPE.RS_LEFT_TURN:
                str = "RS_LEFT_TURN";
                break;
            case ROAD_SHAPE.RS_LEFT_TURN_UP:
                str = "RS_LEFT_TURN_UP";
                break;
            case ROAD_SHAPE.RS_RGIHT_TURN:
                str = "RS_RGIHT_TURN";
                break;
            case ROAD_SHAPE.RS_STRAIGHT:
                str = "RS_STRAIGHT";
                break;
            case ROAD_SHAPE.RS_TRAMPLINE:
                str = "RS_TRAMPLINE";
                break;
            default:
                break;
        }

        return str;
    }

    public void AddObjectInfoToObjList(ROAD_SHAPE rShape, int idx)
    {
        GameObject gmObj = GameObject.Instantiate(objButton.gameObject) as GameObject;
        gmObj.name = idx.ToString();
        gmObj.transform.GetChild(0).gameObject.GetComponent<Text>().text = idx.ToString() + "__" + GetStringFromShape(rShape);
        gmObj.transform.parent = roadContent.transform;

        UpdateScrollSizeDelta();
    }

    public void UpdateScrollSizeDelta()
    {
        roadContent.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 35 * roadContent.transform.childCount + 1);
    }

    public void LocateGameObjectToPosition(GameObject gmObj, int idx)
    {
        ROAD_SHAPE nRshape = gmObj.GetComponent<CRoadObject>().rShape;
        if(gmObj.GetComponent<CRoadObject>().rShape == ROAD_SHAPE.RS_START)
        {
            GameObject gmobjNew = GameObject.Instantiate(gmObj) as GameObject;
            gmobjNew.name = "start";
            gmobjNew.transform.parent = roadParent.transform;
            AddObjectInfoToObjList(nRshape, idx);
            return;
        }

        if(roadParent.transform.childCount > 0 && gmObj.GetComponent<CRoadObject>().rShape == ROAD_SHAPE.RS_START)
        {
            return;
        }

        if(roadParent.transform.childCount < idx - 2)
        {
            return;
        }

        GameObject prevObject = roadParent.transform.GetChild(idx - 1).gameObject;

        int nDirection = GetDirectionFromPevObj(prevObject, nRshape);

        GameObject nAddedObj = GameObject.Instantiate(gmObj) as GameObject;
        nAddedObj.name = roadParent.transform.childCount.ToString();
        nAddedObj.transform.parent = roadParent.transform;

        float rotationAngle = 0;

        if(prevObject.GetComponent<CRoadObject>().curObjDirection == 0)
        {
            rotationAngle = 0;
        }
        else if(prevObject.GetComponent<CRoadObject>().curObjDirection == 1)
        {
            rotationAngle = -90f;
        }
        else if(prevObject.GetComponent<CRoadObject>().curObjDirection == 2)
        {
            rotationAngle = 90f;
        }
        else if(prevObject.GetComponent<CRoadObject>().curObjDirection == 3)
        {
            rotationAngle = 180;
        }
        nAddedObj.transform.localEulerAngles = new Vector3(0, rotationAngle, 0);
//        nAddedObj.transform.localRotation.eulerAngles = new Vector3(0, rotationAngle, 0);

        nAddedObj.transform.position = prevObject.GetComponent<CRoadObject>().endPos.transform.position;
        nAddedObj.GetComponent<CRoadObject>().curObjDirection = nDirection;

        if (nRshape == ROAD_SHAPE.RS_STRAIGHT)
        {
            if (nDirection == 0)
            {
                nAddedObj.transform.localPosition += new Vector3(0, 0, 5);
            }
            else if(nDirection == 1)
            {
                nAddedObj.transform.localPosition += new Vector3(-5, 0, 0);
            }
            else if (nDirection == 2)
            {
                nAddedObj.transform.localPosition += new Vector3(5, 0, 0);
            }
            else if (nDirection == 3)
            {
                nAddedObj.transform.localPosition += new Vector3(0, 0, -5);
            }
        }

        AddObjectInfoToObjList(nRshape, idx);
    }

    public int GetDirectionFromPevObj(GameObject prevObj, ROAD_SHAPE nRshape)
    {
        int direction = 0;

        int prevDirection = prevObj.GetComponent<CRoadObject>().curObjDirection;

        ROAD_SHAPE prevRShape = prevObj.GetComponent<CRoadObject>().rShape;
        if(CheckIfChangeDirectionObject(nRshape))
        {
            if(prevDirection == 0)
            {
                switch(nRshape)
                {
                    case ROAD_SHAPE.RS_LEFT_TURN:
                        direction = 1;
                        break;
                    case ROAD_SHAPE.RS_RGIHT_TURN:
                        direction = 2;
                        break;
                    case ROAD_SHAPE.RS_LEFT_TURN_UP:
                        direction = 1;
                        break;
                    case ROAD_SHAPE.RS_LEFT_ROTATE_UP:
                        direction = 3;
                        break;
                }
            }
            else if(prevDirection == 1)
            {
                switch (nRshape)
                {
                    case ROAD_SHAPE.RS_LEFT_TURN:
                        direction = 3;
                        break;
                    case ROAD_SHAPE.RS_RGIHT_TURN:
                        direction = 0;
                        break;
                    case ROAD_SHAPE.RS_LEFT_TURN_UP:
                        direction = 3;
                        break;
                    case ROAD_SHAPE.RS_LEFT_ROTATE_UP:
                        direction = 2;
                        break;
                }
            }
            else if (prevDirection == 2)
            {
                switch (nRshape)
                {
                    case ROAD_SHAPE.RS_LEFT_TURN:
                        direction = 0;
                        break;
                    case ROAD_SHAPE.RS_RGIHT_TURN:
                        direction = 3;
                        break;
                    case ROAD_SHAPE.RS_LEFT_TURN_UP:
                        direction = 0;
                        break;
                    case ROAD_SHAPE.RS_LEFT_ROTATE_UP:
                        direction = 1;
                        break;
                }
            }
            else if (prevDirection == 3)
            {
                switch (nRshape)
                {
                    case ROAD_SHAPE.RS_LEFT_TURN:
                        direction = 2;
                        break;
                    case ROAD_SHAPE.RS_RGIHT_TURN:
                        direction = 1;
                        break;
                    case ROAD_SHAPE.RS_LEFT_TURN_UP:
                        direction = 1;
                        break;
                    case ROAD_SHAPE.RS_LEFT_ROTATE_UP:
                        direction = 0;
                        break;
                }
            }
        }
        else
        {
            return prevDirection;
        }
        return direction;
    }

    public bool CheckIfChangeDirectionObject(ROAD_SHAPE rShape)
    {
        bool state = false;

        if(rShape == ROAD_SHAPE.RS_LEFT_ROTATE_UP || rShape == ROAD_SHAPE.RS_LEFT_TURN || rShape == ROAD_SHAPE.RS_RGIHT_TURN || rShape == ROAD_SHAPE.RS_LEFT_TURN_UP)
        {
            return true;
        }

        return state;
    }

    public GameObject GetGameObjectFromName(string name)
    {
        GameObject gmReturn = null;

        switch(name)
        {
            case "RS_STRAIGHT":
                gmReturn = roadPrefabs[0];
                break;
            case "RS_RGIHT_TURN":
                gmReturn = roadPrefabs[1];
                break;
            case "RS_LEFT_TURN":
                gmReturn = roadPrefabs[2];
                break;
            case "RS_LEFT_TURN_UP":
                gmReturn = roadPrefabs[3];
                break;
            case "RS_LEFT_ROTATE_UP":
                gmReturn = roadPrefabs[4];
                break;
            case "RS_BRIDGE_1":
                gmReturn = roadPrefabs[5];
                break;
            case "RS_BRDIGE_2":
                gmReturn = roadPrefabs[6];
                break;
            case "RS_HILL_DOWN":
                gmReturn = roadPrefabs[7];
                break;
            case "RS_TRAMPLINE":
                gmReturn = roadPrefabs[8];
                break;
            case "RS_START":
                gmReturn = roadPrefabs[9];
                break;
            case "RS_END":
                gmReturn = roadPrefabs[10];
                break;
            case "RS_EMPTY":
                break;
            default:
                break;
        }

        return gmReturn;
    }

    public GameObject GetGameObjectFromShap(ROAD_SHAPE rShap)
    {
        GameObject gmReturn = null;

        switch (rShap)
        {
            case ROAD_SHAPE.RS_STRAIGHT:
                gmReturn = roadPrefabs[0];
                break;
            case ROAD_SHAPE.RS_RGIHT_TURN:
                gmReturn = roadPrefabs[1];
                break;
            case ROAD_SHAPE.RS_LEFT_TURN:
                gmReturn = roadPrefabs[2];
                break;
            case ROAD_SHAPE.RS_LEFT_TURN_UP:
                gmReturn = roadPrefabs[3];
                break;
            case ROAD_SHAPE.RS_LEFT_ROTATE_UP:
                gmReturn = roadPrefabs[4];
                break;
            case ROAD_SHAPE.RS_BRIDGE_1:
                gmReturn = roadPrefabs[5];
                break;
            case ROAD_SHAPE.RS_BRDIGE_2:
                gmReturn = roadPrefabs[6];
                break;
            case ROAD_SHAPE.RS_HILL_DOWN:
                gmReturn = roadPrefabs[7];
                break;
            case ROAD_SHAPE.RS_TRAMPLINE:
                gmReturn = roadPrefabs[8];
                break;
            case ROAD_SHAPE.RS_START:
                gmReturn = roadPrefabs[9];
                break;
            case ROAD_SHAPE.RS_END:
                gmReturn = roadPrefabs[10];
                break;
            case ROAD_SHAPE.RS_EMPTY:
                break;
            default:
                break;
        }

        return gmReturn;
    }

    public void SaveCurrentLevel()
    {
        if (roadParent.transform.GetChild(roadParent.transform.childCount - 1).gameObject.GetComponent<CRoadObject>().rShape != ROAD_SHAPE.RS_END)
            return;
        if (curSelJsonLevel == null)
        {
            curSelJsonLevel = new JsonObject();
            curSelJsonLevel.Add("info", new JsonArray());
        }
        if (!curSelJsonLevel.ContainsKey("info"))
            return;

        JsonArray jArray = (JsonArray)curSelJsonLevel["info"];
        for(int i = 0; i < roadParent.transform.childCount; i ++)
        {
            jArray.Add(GetStringFromShape(roadParent.transform.GetChild(i).gameObject.GetComponent<CRoadObject>().rShape));
        }

        if(jLevelsInformation.ContainsKey(curSelLevel.ToString()))
        {
            jLevelsInformation[curSelLevel.ToString()] = curSelJsonLevel;
        }
        else
        {
            jLevelsInformation.Add(curSelLevel.ToString(), curSelJsonLevel);
        }
        File.WriteAllText(Application.dataPath + "/Level.json", SimpleJson.SimpleJson.SerializeObject(jLevelsInformation));
    }

    public void AddLevelObject(GameObject gmObj)
    {
        ROAD_SHAPE rShape = gmObj.GetComponent<RoadObjectCell>().rShape;
        GameObject gmTargetObj = GetGameObjectFromShap(rShape);
        if (roadParent.transform.childCount == 0 && rShape != ROAD_SHAPE.RS_START)
            return;
        else if(roadParent.transform.childCount == 0 && rShape == ROAD_SHAPE.RS_START)
        {
            LocateGameObjectToPosition(gmTargetObj, 0);
        }
        else
        {
            LocateGameObjectToPosition(gmTargetObj, roadParent.transform.childCount);
        }
    }

    public void DeleteObjectFromLevel()
    {

    }

    private void Awake()
    {
        LoadLevelInfo();
    }
        
    public void LevelSelected(GameObject gmObjBtn)
    {
        if (gmObjBtn == null)
            return;
        InitInfoState();
        curSelLevel = Convert.ToInt32(gmObjBtn.name.Replace("Level", ""));
        curSelJsonLevel = new JsonObject();
        if(jLevelsInformation.ContainsKey(curSelLevel.ToString()))
        {
            curSelJsonLevel = (JsonObject)jLevelsInformation[curSelLevel.ToString()];
        }
        else
        {
            curSelJsonLevel = new JsonObject();
            curSelJsonLevel.Add("info", new JsonArray());
        }

        LoadLevelObjectsWithJson(curSelJsonLevel);
    }

    public void LoadLevelInfo()
    {
        string str = File.ReadAllText(Application.dataPath + "/Level.json");
        jLevelsInformation = (JsonObject)SimpleJson.SimpleJson.DeserializeObject(str);

        foreach(string key in jLevelsInformation.Keys)
        {
            string name = "Level" + key;

            if(Convert.ToInt32(key) > maxLevel)
            {
                maxLevel = Convert.ToInt32(key);
            }

            GameObject nBtn = GameObject.Instantiate(levelButton.gameObject) as GameObject;
            nBtn.transform.GetChild(0).gameObject.GetComponent<Text>().text = name;
            nBtn.transform.parent = levelsContent.transform;
            nBtn.gameObject.name = name;
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
