using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassCelebration : MonoBehaviour {

    bool startAnim = false;

	// Use this for initialization
	void Start () {
		
	}
    float startedTime = 0f;
    public void ShowPasseedParticle()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(true);
        }
        startedTime = Time.time;
        startAnim = true;
    }
	
	// Update is called once per frame
	void Update () {
		if(startAnim && this.transform.localScale.y <= 1.5 && Time.time - startedTime > 0.001f)
        {
            Vector3 lscale = this.transform.localScale;
            lscale += new Vector3(0.002f, 0.006f, 0.002f);
            this.transform.localScale = lscale;
            startedTime = Time.time;
        }
        else if(startAnim && this.transform.localScale.y > 1.5 && Time.time - startedTime >= 0.07f)
        {
            this.gameObject.SetActive(false);
        }
	}

    private void Awake()
    {
        for(int i = 0; i < this.transform.childCount; i ++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
