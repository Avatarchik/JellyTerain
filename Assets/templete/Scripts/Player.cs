using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
	private void Start()
	{
		this.gManager = UnityEngine.Object.FindObjectOfType<GameManager>();
		this.levelText.text = "Level: " + this.gManager.level;
		this.rb = base.GetComponent<Rigidbody>();
        //this.rb.velocity = new Vector3(0, 0, 10);
        //this.rb.AddForce(0, 0, 10, ForceMode.Force);
		this.myColor = UnityEngine.Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f);
		base.GetComponent<MeshRenderer>().material.color = this.myColor;
		this.prevColor = Camera.main.backgroundColor;
	}

    IEnumerator Rotate(Vector3 axis, float angle, float duration)
    {
        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;
        to *= Quaternion.Euler(axis * angle);

        float fSize = this.rb.velocity.magnitude;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(from, to, duration/0.1f);
            UnityEngine.Debug.Log(transform.rotation.eulerAngles);

            this.rb.velocity = this.transform.forward * fSize/2;
            
            this.rb.AddForce(this.rb.velocity, ForceMode.VelocityChange);
            elapsed += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        transform.rotation = to;
        rotationcallstate = false;
        curDirection = targetDirection;
        SetForceWithDirection();
    }
    bool rotationcallstate = false;

    void SetForceWithDirection()
    {
        if(curDirection == 0)
        {
//            this.rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            this.rb.velocity = new Vector3(0, 0, 3);
            this.rb.AddForce(0f, 0f, 3f, ForceMode.VelocityChange);
        }
        else if(curDirection == 1)
        {
 //           this.rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            this.rb.velocity = new Vector3(-3f, 0, 0);
            this.rb.AddForce(-3f, 0f, 0f, ForceMode.VelocityChange);
        }
        else if (curDirection == 2)
        {
//            this.rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            this.rb.velocity = new Vector3(3f, 0, 0);
            this.rb.AddForce(3f, 0f, 0f, ForceMode.VelocityChange);
        }
        else if (curDirection == 3)
        {
//            this.rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            this.rb.velocity = new Vector3(0f, 0, -3);
            this.rb.AddForce(0f, 0f, -3f, ForceMode.VelocityChange);
        }
    }

    bool CheckIfNearByRotationPoint(int cDir, int tDir)
    {
        bool state = false;

        if (cDir == tDir)
            return false;

        if (cDir == 0)
        {
            if(tDir == 1)
            {

            }
        }
        return state;
    }

    private void Update()
	{
        if (curDirection != targetDirection && !rotationcallstate)
        {
            rotationcallstate = true;
            StartCoroutine(Rotate(Vector3.up, GetRotationAngle(curEnteredOBj.GetComponent<CRoadObject>().startPos.transform.localPosition, curEnteredOBj.GetComponent<CRoadObject>().endPos.transform.localPosition), 0.5f));
        }

        if (base.transform.position.y < -0.9f)
		{
			this.hit = true;
			UnityEngine.Object.Destroy(base.GetComponent<Player>(), 1f);
			this.failedPanel.gameObject.SetActive(true);
		}

        if (this.counter >= this.maxCount)
        {
            this.fever = true;
            this.counter = 0f;
            this.time1 = Time.timeSinceLevelLoad;
            base.GetComponent<MeshRenderer>().material.color = Color.red;
            this.dLight.GetComponent<Light>().color = Color.red;
            Camera.main.backgroundColor = new Color(0.5f, 1f, 0.8f);
        }
        if (this.time1 < Time.timeSinceLevelLoad - this.maxTime)
        {
            this.fever = false;
            base.GetComponent<MeshRenderer>().material.color = this.myColor;
            Camera.main.backgroundColor = this.prevColor;
            this.dLight.GetComponent<Light>().color = Color.white;
        }
        if (!this.fever)
        {
            this.slider.gameObject.GetComponent<Slider>().value = Mathf.Lerp(this.slider.gameObject.GetComponent<Slider>().value, this.counter / (this.maxCount - 0.9f), Time.deltaTime * 3f);
            this.slider.transform.GetChild(1).gameObject.GetComponent<Image>().color = Color.HSVToRGB((1f - this.slider.gameObject.GetComponent<Slider>().value) * 0.3f, 1f, 1f);
        }
        else if (this.fever)
        {
            this.slider.gameObject.GetComponent<Slider>().value = 1f - (Time.timeSinceLevelLoad - this.time1) / this.maxTime;
            this.slider.transform.GetChild(1).gameObject.GetComponent<Image>().color = Color.HSVToRGB((1f - this.slider.gameObject.GetComponent<Slider>().value) * 0.3f, 1f, 1f);
        }
        if (!this.won && !this.hit)
        {
            //if (this.fever)
            //{
            //    if (this.rb.velocity.z < this.gManager.speed + 10f)
            //    {
            //        this.rb.AddForce(0f, 0f, 3f, ForceMode.Impulse);
            //    }
            //}
            //else if (this.rb.velocity.z < this.gManager.speed)
            //{
            //    this.rb.AddForce(0f, 0f, 3f, ForceMode.Impulse);
            //}
            if(rotationcallstate == false)
            {
                SetForceWithDirection();
            }

            //if (this.rb.velocity.z < this.gManager.speed && rotationcallstate == false)
            //{
            //    this.rb.velocity = new Vector3(0, 0, 3);
            //    this.rb.AddForce(0f, 0f, 3f, ForceMode.VelocityChange);
            //}
            //            SetForce();
        }
        if (this.won)
        {
            base.GetComponent<Animator>().enabled = true;
            this.rb.velocity = Vector3.Lerp(this.rb.velocity, Vector3.zero, Time.deltaTime * 0.05f);
            base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(1.1f, 1.1f, 0.5f), Time.deltaTime * 2f);
            base.transform.position = new Vector3(base.transform.position.x, base.transform.localScale.y / 2f + 0.25f, base.transform.position.z);
        }
        if (Input.GetMouseButtonDown(0) && !this.won && !this.hit)
        {
            this.prevMousePos = new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, 10f);
            this.prevMousePos = Camera.main.ScreenToViewportPoint(this.prevMousePos);
        }
        //size delta
        if (Input.GetMouseButton(0) && !this.won && !this.hit)
        {
            this.mousePos = new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, 10f);
            this.mousePos = Camera.main.ScreenToViewportPoint(this.mousePos);
            this.offset = this.mousePos.y - this.prevMousePos.y;
            if (Time.timeSinceLevelLoad > this.time + 0.05f)
            {
                this.time = Time.timeSinceLevelLoad;
                this.prevMousePos = this.mousePos;
            }
            if (this.offset > 0.005f && base.transform.localScale.x > 0.3f)
            {
                this.desiredScale = new Vector3(base.transform.localScale.x - 0.3f, base.transform.localScale.y, base.transform.localScale.z);
                base.transform.localScale = Vector3.Lerp(base.transform.localScale, this.desiredScale, Time.deltaTime * this.lerpSpeed);
                base.transform.localScale = new Vector3(base.transform.localScale.x, 2.2f - base.transform.localScale.x, base.transform.localScale.z);
                base.transform.position = new Vector3(base.transform.position.x, base.transform.localScale.y / 2f + 0.25f, base.transform.position.z);
            }
            if (this.offset < -0.005f && base.transform.localScale.x < 1.9f)
            {
                this.desiredScale = new Vector3(base.transform.localScale.x + 0.3f, base.transform.localScale.y, base.transform.localScale.z);
                base.transform.localScale = Vector3.Lerp(base.transform.localScale, this.desiredScale, Time.deltaTime * this.lerpSpeed);
                base.transform.localScale = new Vector3(base.transform.localScale.x, 2.2f - base.transform.localScale.x, base.transform.localScale.z);
                base.transform.position = new Vector3(base.transform.position.x, base.transform.localScale.y / 2f + 0.25f, base.transform.position.z);
            }
        }
        //set color mode
        if (!this.won)
        {
            if (!this.fever)
            {
                base.GetComponent<MeshRenderer>().material.color = Vector4.Lerp(base.GetComponent<MeshRenderer>().material.color, this.myColor, Time.deltaTime * 0.5f);
                return;
            }
        }
        else
        {
            base.GetComponent<MeshRenderer>().material.color = Vector4.Lerp(base.GetComponent<MeshRenderer>().material.color, Color.magenta, Time.deltaTime);
        }
    }

    private void SetForce()
    {
//        if(curDirection == targetDirection)
//        {
//            //            this.rb.freezeRotation = true;
//            if (curDirection == 0)
//            {
//                this.gameObject.transform.forward = Vector3.forward;
//                if (this.rb.velocity.z < this.gManager.speed)
//                    this.rb.AddForce(new Vector3(0, 0, 3), ForceMode.Impulse);
//            }
//            else if (curDirection == 1)
//            {
//                if (this.rb.velocity.x < this.gManager.speed)
//                    this.rb.AddForce(new Vector3(3, 0, 0), ForceMode.Impulse);
//            }
//            else if (curDirection == 2)
//            {
//                if (this.rb.velocity.z < this.gManager.speed)
//                    this.rb.AddForce(new Vector3(3, 0, 0), ForceMode.Impulse);
//            }
//            else if (curDirection == 3)
//            {
//                if (this.rb.velocity.z < this.gManager.speed)
//                    this.rb.AddForce(new Vector3(0, 0, 3f), ForceMode.Impulse);
//            }
//        }
//        else
//        {
////            this.rb.freezeRotation = false;
//            if (curDirection == 0)
//            {
//                if(targetDirection == 1)
//                {
//                    if(Math.Abs(this.transform.position.z - curEnteredOBj.GetComponent<CRoadObject>().endPos.transform.position.z) < 1)
//                    {

//                    }

////                    this.rb.AddForce(new Vector3(-10, 0, 10f), ForceMode.Force);
//                }
//                else if (targetDirection == 2)
//                {
//                    if (Math.Abs(this.transform.position.z - curEnteredOBj.GetComponent<CRoadObject>().endPos.transform.position.z) < 1.5)
//                    {
//                        this.transform.forward = new Vector3(1, 0, 1);
//                    }
//                        //this.rb.velocity = new Vector3(5, 0, 5);
//                    else if (this.transform.position.z < curEnteredOBj.GetComponent<CRoadObject>().endPos.transform.position.z - 1.5)
//                    {
//                        this.transform.forward = new Vector3(0, 0, 1);
//                    }
//                        //this.rb.velocity = new Vector3(0, 0, 10);
//                    else if (this.transform.position.x > curEnteredOBj.GetComponent<CRoadObject>().startPos.transform.position.x)
//                    {
//                        this.transform.forward = new Vector3(1, 0, 0);
//                    }
//                        //this.rb.velocity = new Vector3(10, 0, 0);
//                    //                    this.rb.AddForce(new Vector3(10, 0, 10f), ForceMode.Force);
//                }
//            }
//        }
    }

    ROAD_SHAPE curObjShape = ROAD_SHAPE.RS_START;
    GameObject curEnteredOBj = null;
    public bool CheckIfChangeDirectionObject(ROAD_SHAPE rShape)
    {
        bool state = false;

        if (rShape == ROAD_SHAPE.RS_LEFT_ROTATE_UP || rShape == ROAD_SHAPE.RS_LEFT_TURN || rShape == ROAD_SHAPE.RS_RGIHT_TURN || rShape == ROAD_SHAPE.RS_LEFT_TURN_UP)
        {
            return true;
        }

        return state;
    }

    //get trigger enter object to check what kind of object crashed
    private void OnTriggerEnter(Collider col)
	{
        UnityEngine.Debug.Log(col.gameObject.name + " ---- " + col.gameObject.transform.parent.gameObject.name);

        if(col.name == "end")
        {
            if (curDirection != targetDirection)
            {
                curDirection = targetDirection;
                //SetForce();
            }
        }
        else if(col.name == "start")
        {
            curEnteredOBj = col.gameObject.transform.parent.gameObject;
            int dir = col.transform.parent.gameObject.GetComponent<CRoadObject>().curObjDirection;
            ROAD_SHAPE rShap = col.transform.parent.gameObject.GetComponent<CRoadObject>().rShape;

            curObjShape = rShap;

            if (CheckIfChangeDirectionObject(curObjShape))
            {
                //start point
                Vector3 startPosition = col.transform.parent.gameObject.GetComponent<CRoadObject>().startPos.transform.localPosition;
                Vector3 endPosition = col.transform.parent.gameObject.GetComponent<CRoadObject>().endPos.transform.localPosition;

                targetDirection = dir;                
            }


        }
        else if(col.name.Contains("particle"))
        {
            col.gameObject.GetComponent<PassCelebration>().ShowPasseedParticle();
        }
        
        //if (col.CompareTag("obstacle") && !this.fever)
        //{
        //	this.counter += 1f;
        //}
        //if (col.CompareTag("Finish"))
        //{
        //	this.won = true;
        //	PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        //	this.finishPanel.gameObject.SetActive(true);
        //          UnityEngine.Object.Instantiate<GameObject>(this.gManager.particalEffect, col.gameObject.transform.position + new Vector3(0f, 0f, 5f), Quaternion.Euler(-90f, 0f, 0f));
        //}
        //if (col.CompareTag("gems"))
        //{
        //	this.score++;
        //	UnityEngine.Object.Destroy(col.gameObject);
        //	this.scoreText.text = "Gems: " + this.score;
        //}
//        this.rb.AddForce(0f, 0f, 3f, ForceMode.Impulse);
    }

    private float GetRotationAngle(Vector3 startPos, Vector3 endPos)
    {
        float rAngle = 0;
        if (curDirection == targetDirection)
            return 0;

        if(curDirection == 0)
        {
            if(targetDirection == 1)
            {
                return -90f;
            }
            else if(targetDirection == 2)
            {
                return 90f;
            }
            else if(targetDirection == 3)
            {
                return -180;
            }
        }
        else if(curDirection == 1)
        {
            if(targetDirection == 0)
            {
                return 90f;
            }
            else if(targetDirection == 2)
            {
                return -180f;
            }
            else if(targetDirection == 3)
            {
                return -90f;
            }
        }
        else if(curDirection == 2)
        {
            if(targetDirection == 0)
            {
                return -90f;
            }
            else if(targetDirection == 1)
            {
                return -180f;
            }
            else if(targetDirection == 3)
            {
                return 90f;
            }
        }
        else if(curDirection == 3)
        {
            if(targetDirection == 0)
            {
                return -180f;
            }
            else if(targetDirection == 1)
            {
                return 90f;
            }
            else if (targetDirection == 2)
            {
                return -90f;
            }
        }
        return rAngle;
    }

	private void OnCollisionEnter(Collision col)
	{
        //check collision if the player crashed into the objects

		//if (col.gameObject.CompareTag("obstacle"))
		//{
		//	if (!this.fever)
		//	{
		//		col.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
		//		base.GetComponent<MeshRenderer>().material.color = new Color(1f, 0f, 0f);
		//		this.hit = true;
		//		UnityEngine.Object.Destroy(base.GetComponent<Player>(), 1f);
		//		this.failedPanel.gameObject.SetActive(true);
		//		return;
		//	}
		//	GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.particles, col.gameObject.transform.position, col.gameObject.transform.rotation);
		//	gameObject.gameObject.transform.position = col.gameObject.transform.position;
		//	//gameObject.GetComponent<ParticleSystem>().shape.mesh = col.gameObject.GetComponent<MeshFilter>().mesh;
		//	UnityEngine.Object.Destroy(col.gameObject);
		//	UnityEngine.Object.Destroy(gameObject, 1f);
		//}
	}

	private GameManager gManager;

    private int curDirection = 0;

    private int targetDirection = 0;

	private Rigidbody rb;

	private Vector3 prevMousePos;

	private Vector3 mousePos;

	private Vector3 desiredScale;

	private float offset;

	private float lerpSpeed = 20f;

	private float time;

	private float time1;

	private Color myColor;

	private Color desiredColor;

	private Color prevColor;

	private bool hit;

	private bool won;

	private bool fever;

	public int score;

	public float counter;

	public float maxCount = 5f;

	public float maxTime = 5f;

	public Text levelText;

	public Text scoreText;

	public GameObject finishPanel;

	public GameObject failedPanel;

	public GameObject dLight;

	public GameObject particles;

	public GameObject slider;
}
