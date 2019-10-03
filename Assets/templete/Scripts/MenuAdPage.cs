using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuAdPage : MonoBehaviour
{
	private void Awake()
	{
		MenuAdPage.instance = this;
		base.gameObject.SetActive(false);
	}

	public void Openad()
	{
		Application.OpenURL(this.url);
	}

	public void Close()
	{
		base.gameObject.SetActive(false);
	}

	public IEnumerator LoadImg()
	{
		//int index = UnityEngine.Random.Range(0, AdManager.instance.MgImgList.Count - 1);
		//this.MenuAdImg.sprite = AdManager.instance.MgImgList[index];
		//this.url = AdManager.instance.MgLinkToList[index];
		//yield return new WaitForSeconds(3f);
		//base.gameObject.SetActive(true);
		yield break;
	}

	public IEnumerator Loadland()
	{
		//int index = UnityEngine.Random.Range(0, AdManager.instance.LandList.Count - 1);
		//this.MenuAdImg.sprite = AdManager.instance.LandList[index];
		//this.url = AdManager.instance.LandToList[index];
		//yield return new WaitForSeconds(3f);
		//base.gameObject.SetActive(true);
		yield break;
	}

	public IEnumerator ShowAdInGamea()
	{
		//int index = UnityEngine.Random.Range(0, AdManager.instance.MgImgList.Count - 1);
		//this.MenuAdImg.sprite = AdManager.instance.MgImgList[index];
		//this.url = AdManager.instance.MgLinkToList[index];
		//yield return new WaitForSeconds(0.2f);
		//base.gameObject.SetActive(true);
		yield break;
	}

	public Image MenuAdImg;

	public GameObject CloseBtn;

	public static MenuAdPage instance;

	public bool portraitloaded;

	public bool iconLoaded;

	public bool LandscapeLoaded;

	private string url;
}
