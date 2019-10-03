using System;
using UnityEngine;
using UnityEngine.UI;

public class ShowInGameIcon : MonoBehaviour
{
	private void Awake()
	{
		this.thisimage = base.GetComponent<Image>();
	}

	private void OnEnable()
	{
		//if (AdManager.instance && AdManager.instance.IconLoaded)
		//{
		//	int index = UnityEngine.Random.Range(0, AdManager.instance.Iconlist.Count - 1);
		//	this.loadedsprite = AdManager.instance.Iconlist[index];
		//	this.loadedlink = AdManager.instance.IconToList[index];
		//	this.thisimage.sprite = this.loadedsprite;
		//	UnityEngine.Debug.Log(this.loadedlink);
		//	return;
		//}
		//base.gameObject.SetActive(false);
	}

	public void OnButtonClick()
	{
		Application.OpenURL(this.loadedlink);
	}

	private Sprite loadedsprite;

	private string loadedlink;

	private Image thisimage;
}
