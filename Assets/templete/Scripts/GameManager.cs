using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleJson;
using System.IO;


public class GameManager : MonoBehaviour
{
	private void Awake()
	{
		this.lSelect = UnityEngine.Object.FindObjectOfType<levelSelect>();

        this.LoadLevelsRoadInfo();

		this.LevelGenerate();
	}

    private void LoadLevelsRoadInfo()
    {
        //if(File.Exists(Application.dataPath + "/Level.json"))
        //{
        //    return;
        //}

        //jLevelsInfo = new JsonObject();
        //jCurLevel = new JsonObject();

        //string strAll = File.ReadAllText(Application.dataPath + "/Level.json");
        //jLevelsInfo = (JsonObject)SimpleJson.SimpleJson.DeserializeObject(strAll);
    }

	private void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		if (UnityEngine.Input.GetKeyDown("d"))
		{
			PlayerPrefs.DeleteAll();
		}
		if (UnityEngine.Input.GetKey("n"))
		{
			PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
		}
	}

	private void LevelGenerate()
	{
		if (!PlayerPrefs.HasKey("level"))
		{
			PlayerPrefs.SetInt("level", 1);
		}
		if (levelSelect.call)
		{
			this.level = int.Parse(levelSelect.level);
			MonoBehaviour.print(this.level);
			levelSelect.call = false;
		}
		else
		{
			this.level = PlayerPrefs.GetInt("level");
		}

        //if (!jLevelsInfo.ContainsKey(this.level.ToString()))
        //    return;

        //jCurLevel = (JsonObject)jLevelsInfo[this.level.ToString()];
        //LoadLevelObjectsWithJson(jCurLevel);

        int prefIdx = this.level % 10;

        GameObject levelObj = GameObject.Instantiate(levelsPrefab[prefIdx - 1]) as GameObject;
        for(int i = 0; i < levelObj.transform.GetChild(0).childCount; i ++)
        {
            levelObj.transform.GetChild(0).GetChild(i).localPosition -= new Vector3(0, 0.01f * i, 0);
        }
//        levelObj.transform.GetChild(1).gameObject.SetActive(false);
        this.speed = 10f;
    

		//if (this.level < 10)
		//{
		//	this.speed = 10f;
		//	this.maxRange = 2;
		//	this.maxDis = (this.level * 2 + 5) * 10;
		//}
		//else if (this.level < 20)
		//{
		//	this.speed = 12f;
		//	this.maxRange = 5;
		//	this.maxDis = (int)((double)this.level * 1.8 + 5.0) * 10;
		//}
		//else if (this.level < 30)
		//{
		//	this.speed = 12f;
		//	this.maxRange = 9;
		//	this.maxDis = (int)((double)this.level * 1.6 + 5.0) * 10;
		//}
		//else if (this.level < 60)
		//{
		//	this.speed = 15f;
		//	this.maxRange = 12;
		//	this.maxDis = (int)((double)this.level * 1.4 + 5.0) * 10;
		//}
		//else if (this.level < 100)
		//{
		//	this.speed = 17f;
		//	this.maxRange = 17;
		//	this.maxDis = (int)((double)this.level * 1.2 + 5.0) * 10;
		//}
		//else if (this.level >= 100)
		//{
		//	this.speed = 20f;
		//	this.maxRange = 23;
		//	this.maxDis = (int)((float)this.level * 1f + 5f) * 10;
		//}
		//this.temp = UnityEngine.Object.Instantiate<GameObject>(this.floor[0], Vector3.zero, Quaternion.identity);
		//this.temp.transform.localScale = new Vector3(1f, 1f, (float)(this.maxDis + 30));
		//UnityEngine.Object.Instantiate<GameObject>(this.floor[3], new Vector3(0f, 0f, (float)this.maxDis), Quaternion.identity);
		//for (int i = 0; i < this.maxDis + 10; i += 10)
		//{
		//	UnityEngine.Object.Instantiate<GameObject>(this.floor[2], new Vector3(-3f, -3f, (float)i), Quaternion.identity);
		//}
		//for (int j = 10; j < this.maxDis; j += 20 / UnityEngine.Random.Range(1, 10))
		//{
		//	UnityEngine.Object.Instantiate<GameObject>(this.floor[4], new Vector3(0f, 0.3f, (float)j), Quaternion.identity);
		//}
		//for (int k = 30; k < this.maxDis; k += this.shuffle[UnityEngine.Random.Range(0, this.shuffle.Length)])
		//{
		//	int[] array = new int[3];
		//	array = new int[]
		//	{
		//		0,
		//		0,
		//		1
		//	};
		//	int num = array[UnityEngine.Random.Range(0, array.Length)];
		//	if (num != 0)
		//	{
		//		if (num == 1)
		//		{
		//			this.temp.transform.localScale = new Vector3(1f, 1f, (float)(k - this.prevFloor));
		//			this.temp = UnityEngine.Object.Instantiate<GameObject>(this.floor[1], new Vector3(0f, 0f, (float)k), Quaternion.identity);
		//			UnityEngine.Object.Instantiate<GameObject>(this.obstacles[0], new Vector3(0f, 0f, (float)(k + this.railSize)), Quaternion.identity);
		//			this.prevFloor = k + this.railSize;
		//			this.temp.transform.localScale = new Vector3(1f, 1f, (float)this.railSize);
		//			this.maxDis1 = this.maxDis - k - this.railSize;
		//			this.temp = UnityEngine.Object.Instantiate<GameObject>(this.floor[0], new Vector3(0f, 0f, (float)(k + this.railSize)), Quaternion.identity);
		//			this.temp.transform.localScale = new Vector3(1f, 1f, (float)(this.maxDis1 + 30));
		//			k += this.shuffle[0];
		//		}
		//	}
		//	else
		//	{
		//		UnityEngine.Object.Instantiate<GameObject>(this.obstacles[UnityEngine.Random.Range(0, this.maxRange)], new Vector3(0f, 0f, (float)k), Quaternion.identity);
		//		MonoBehaviour.print("0");
		//	}
		//}
	}

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

    private void GenerateMap(JsonObject jMapInfo)
    {
        JsonArray jObjArray = (JsonArray)jMapInfo["info"];
        for(int i = 0; i < jObjArray.Count; i ++)
        {
            string objName = Convert.ToString(jObjArray[i]);
        }
    }

    public GameObject GetGameObjectFromName(string name)
    {
        GameObject gmReturn = null;

        switch (name)
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

    public bool CheckIfChangeDirectionObject(ROAD_SHAPE rShape)
    {
        bool state = false;

        if (rShape == ROAD_SHAPE.RS_LEFT_ROTATE_UP || rShape == ROAD_SHAPE.RS_LEFT_TURN || rShape == ROAD_SHAPE.RS_RGIHT_TURN || rShape == ROAD_SHAPE.RS_LEFT_TURN_UP)
        {
            return true;
        }

        return state;
    }

    public int GetDirectionFromPevObj(GameObject prevObj, ROAD_SHAPE nRshape)
    {
        int direction = 0;

        int prevDirection = prevObj.GetComponent<CRoadObject>().curObjDirection;

        ROAD_SHAPE prevRShape = prevObj.GetComponent<CRoadObject>().rShape;
        if (CheckIfChangeDirectionObject(nRshape))
        {
            if (prevDirection == 0)
            {
                switch (nRshape)
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
            else if (prevDirection == 1)
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
    public void LocateGameObjectToPosition(GameObject gmObj, int idx)
    {
        ROAD_SHAPE nRshape = gmObj.GetComponent<CRoadObject>().rShape;
        if (gmObj.GetComponent<CRoadObject>().rShape == ROAD_SHAPE.RS_START)
        {
            GameObject gmobjNew = GameObject.Instantiate(gmObj) as GameObject;
            gmobjNew.name = "start";
            gmobjNew.transform.parent = roadParent.transform;
            return;
        }

        if (roadParent.transform.childCount > 0 && gmObj.GetComponent<CRoadObject>().rShape == ROAD_SHAPE.RS_START)
        {
            return;
        }

        if (roadParent.transform.childCount < idx - 2)
        {
            return;
        }

        GameObject prevObject = roadParent.transform.GetChild(idx - 1).gameObject;

        int nDirection = GetDirectionFromPevObj(prevObject, nRshape);

        GameObject nAddedObj = GameObject.Instantiate(gmObj) as GameObject;
        nAddedObj.name = roadParent.transform.childCount.ToString();
        nAddedObj.transform.parent = roadParent.transform;

        float rotationAngle = 0;

        if (prevObject.GetComponent<CRoadObject>().curObjDirection == 0)
        {
            rotationAngle = 0;
        }
        else if (prevObject.GetComponent<CRoadObject>().curObjDirection == 1)
        {
            rotationAngle = -90f;
        }
        else if (prevObject.GetComponent<CRoadObject>().curObjDirection == 2)
        {
            rotationAngle = 90f;
        }
        else if (prevObject.GetComponent<CRoadObject>().curObjDirection == 3)
        {
            rotationAngle = 180;
        }
        nAddedObj.transform.localEulerAngles = new Vector3(0, rotationAngle, 0);
        //        nAddedObj.transform.localRotation.eulerAngles = new Vector3(0, rotationAngle, 0);

        nAddedObj.transform.position = prevObject.GetComponent<CRoadObject>().endPos.transform.position;
        Vector3 pos = nAddedObj.transform.localPosition;
        pos.y -= 0.01f;
        nAddedObj.transform.localPosition = pos;
        nAddedObj.GetComponent<CRoadObject>().curObjDirection = nDirection;

        if (nRshape == ROAD_SHAPE.RS_STRAIGHT)
        {
            if (nDirection == 0)
            {
                nAddedObj.transform.localPosition += new Vector3(0, 0, 5);
            }
            else if (nDirection == 1)
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
    }
    public void Retry()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

    //new logic
    JsonObject jLevelsInfo;
    JsonObject jCurLevel;
    int curLevel;
    public GameObject[] roadPrefabs;
    public GameObject roadParent;
    public GameObject[] levelsPrefab;
    //end

	private levelSelect lSelect;

	public GameObject[] food;

	public GameObject[] floor;

	public GameObject[] obstacles;

	[HideInInspector]
	public int maxDis;

	[HideInInspector]
	public int maxDis1;

	[HideInInspector]
	public int maxRange;

	[HideInInspector]
	public int prevFloor;

	public int[] shuffle;

	public int level;

	public int railSize = 7;

	public float speed;

	public GameObject particalEffect;

	public GameObject temp;
}
