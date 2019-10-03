using System;
using System.Collections;
using System.Xml;
using UnityEngine;

public class XMLReader : MonoBehaviour
{
	private void Start()
	{
		base.StartCoroutine("LoadAllCommonXML");
	}

	private IEnumerator LoadAllCommonXML()
	{
		//if (AdManager.instance.isWifi_OR_Data_Availble())
		//{
		//	WWW xmlData = new WWW(AdManager.instance.AllCommonUrl);
		//	yield return xmlData;
		//	if (xmlData.error != null)
		//	{
		//		UnityEngine.Debug.LogError("---------- error DoSomething");
		//	}
		//	else
		//	{
		//		UnityEngine.Debug.Log("implement code");
		//		XmlDocument xmlDocument = new XmlDocument();
		//		try
		//		{
		//			xmlDocument.LoadXml(xmlData.text);
		//		}
		//		catch (Exception arg)
		//		{
		//			UnityEngine.Debug.LogError("----------------Error loading :\n" + arg);
		//		}
		//		finally
		//		{
		//			this.MainNode = xmlDocument.SelectSingleNode("GameData");
		//			if (AdManager.instance.IsPortrait)
		//			{
		//				this.ReadAllCommonXmlDataPortrail();
		//			}
		//			else
		//			{
		//				this.ReadAllCommonXmlDataLandscape();
		//			}
		//			this.ReadAllCommonXmlDataCion();
		//		}
		//	}
		//	xmlData = null;
		//}
		//else
		//{
		//	UnityEngine.Debug.Log("No Internet Connection to get XML data");
		//}
		yield break;
	}

	private void ReadAllCommonXmlDataPortrail()
	{
		UnityEngine.Debug.Log("---------- ReadingXmlData ------------");
		//if (this.MainNode.SelectSingleNode("moregames") != null)
		//{
		//	this.MoreGamesNode = this.MainNode.SelectSingleNode("moregames");
		//	AdManager.instance.MgImgLinkList.Clear();
		//	for (int i = 0; i < 10; i++)
		//	{
		//		if (this.MoreGamesNode.Attributes.GetNamedItem("mgImgLink" + (i + 1)) != null)
		//		{
		//			string value = this.MoreGamesNode.Attributes.GetNamedItem("mgImgLink" + (i + 1)).Value;
		//			AdManager.instance.MgImgLinkList.Add(value);
		//		}
		//	}
		//	AdManager.instance.MgLinkToList.Clear();
		//	for (int j = 0; j < AdManager.instance.MgImgLinkList.Count; j++)
		//	{
		//		if (this.MoreGamesNode.Attributes.GetNamedItem("mgLinkto" + (j + 1)) != null)
		//		{
		//			string value2 = this.MoreGamesNode.Attributes.GetNamedItem("mgLinkto" + (j + 1)).Value;
		//			AdManager.instance.MgLinkToList.Add(value2);
		//		}
		//	}
		//	AdManager.instance.MgImgList.Clear();
		//	for (int k = 0; k < AdManager.instance.MgImgLinkList.Count; k++)
		//	{
		//		AdManager.instance.MgImgList.Add(null);
		//	}
		//	base.StartCoroutine(this.Downloadportrait(AdManager.instance.MgImgLinkList[0], 0));
		//}
	}

	private void ReadAllCommonXmlDataCion()
	{
		UnityEngine.Debug.Log("---------- ReadingXmlData ------------");
		//if (this.MainNode.SelectSingleNode("iconpromo") != null)
		//{
		//	this.IconGameNode = this.MainNode.SelectSingleNode("iconpromo");
		//	AdManager.instance.IconLinkList.Clear();
		//	for (int i = 0; i < 10; i++)
		//	{
		//		if (this.IconGameNode.Attributes.GetNamedItem("iconimg" + (i + 1)) != null)
		//		{
		//			string value = this.IconGameNode.Attributes.GetNamedItem("iconimg" + (i + 1)).Value;
		//			AdManager.instance.IconLinkList.Add(value);
		//		}
		//	}
		//	AdManager.instance.IconToList.Clear();
		//	for (int j = 0; j < AdManager.instance.IconLinkList.Count; j++)
		//	{
		//		if (this.IconGameNode.Attributes.GetNamedItem("iconLinkto" + (j + 1)) != null)
		//		{
		//			string value2 = this.IconGameNode.Attributes.GetNamedItem("iconLinkto" + (j + 1)).Value;
		//			AdManager.instance.IconToList.Add(value2);
		//		}
		//	}
		//	AdManager.instance.Iconlist.Clear();
		//	for (int k = 0; k < AdManager.instance.IconLinkList.Count; k++)
		//	{
		//		AdManager.instance.Iconlist.Add(null);
		//	}
		//	base.StartCoroutine(this.DownloadiConImg(AdManager.instance.IconLinkList[0], 0));
		//}
	}

	private void ReadAllCommonXmlDataLandscape()
	{
		UnityEngine.Debug.LogError("---------- ReadingXmlData ------------");
		//if (this.MainNode.SelectSingleNode("landscapepromo") != null)
		//{
		//	this.MoreGamesNode = this.MainNode.SelectSingleNode("landscapepromo");
		//	AdManager.instance.LandLinkList.Clear();
		//	for (int i = 0; i < 10; i++)
		//	{
		//		if (this.MoreGamesNode.Attributes.GetNamedItem("landscImgLink" + (i + 1)) != null)
		//		{
		//			string value = this.MoreGamesNode.Attributes.GetNamedItem("landscImgLink" + (i + 1)).Value;
		//			AdManager.instance.LandLinkList.Add(value);
		//		}
		//	}
		//	AdManager.instance.LandToList.Clear();
		//	for (int j = 0; j < AdManager.instance.LandLinkList.Count; j++)
		//	{
		//		if (this.MoreGamesNode.Attributes.GetNamedItem("landLinkto" + (j + 1)) != null)
		//		{
		//			string value2 = this.MoreGamesNode.Attributes.GetNamedItem("landLinkto" + (j + 1)).Value;
		//			AdManager.instance.LandToList.Add(value2);
		//		}
		//	}
		//	AdManager.instance.LandList.Clear();
		//	for (int k = 0; k < AdManager.instance.LandLinkList.Count; k++)
		//	{
		//		AdManager.instance.LandList.Add(null);
		//	}
		//	base.StartCoroutine(this.Downloadlandscape(AdManager.instance.LandLinkList[0], 0));
		//}
	}

	private IEnumerator Downloadportrait(string url, int Index)
	{
		int num = this.IsFoundMGLink(Index);
		if (num == -1)
		{
			this.NextMGIndex = Index + 1;
			WWW menuAdView = new WWW(url);
			yield return menuAdView;
			Texture2D texture = menuAdView.texture;
			//AdManager.instance.MgImgList[Index] = Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), new Vector2(0f, 0f));
			//if (this.NextMGIndex < AdManager.instance.MgLinkToList.Count)
			//{
			//	base.StartCoroutine(this.Downloadportrait(AdManager.instance.MgImgLinkList[this.NextMGIndex], this.NextMGIndex));
			//}
			//if (this.NextMGIndex > AdManager.instance.MgLinkToList.Count - 1)
			//{
			//	base.StartCoroutine(MenuAdPage.instance.LoadImg());
			//	MenuAdPage.instance.portraitloaded = true;
			//	base.StartCoroutine(this.RemoveCurrentApplink());
			//}
			menuAdView = null;
			yield break;
		}
		//this.NextMGIndex = Index + 1;
		//AdManager.instance.MgImgList[Index] = AdManager.instance.MgImgList[num];
		//if (this.NextMGIndex < AdManager.instance.MgLinkToList.Count)
		//{
		//	base.StartCoroutine(this.Downloadportrait(AdManager.instance.MgImgLinkList[this.NextMGIndex], this.NextMGIndex));
		//}
		yield break;
	}

	private IEnumerator DownloadiConImg(string url, int Index)
	{
		//int num = this.IsFoundicLink(Index);
		//if (num == -1)
		//{
		//	this.NextIConIndex = Index + 1;
		//	WWW menuAdView = new WWW(url);
		//	yield return menuAdView;
		//	Texture2D texture = menuAdView.texture;
		//	AdManager.instance.Iconlist[Index] = Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), new Vector2(0f, 0f));
		//	if (this.NextIConIndex < AdManager.instance.IconToList.Count)
		//	{
		//		base.StartCoroutine(this.DownloadiConImg(AdManager.instance.IconLinkList[this.NextIConIndex], this.NextIConIndex));
		//	}
		//	if (this.NextIConIndex > AdManager.instance.IconToList.Count - 1)
		//	{
		//		AdManager.instance.IconLoaded = true;
		//	}
		//	menuAdView = null;
		//	yield break;
		//}
		//this.NextIConIndex = Index + 1;
		//AdManager.instance.Iconlist[Index] = AdManager.instance.Iconlist[num];
		//if (this.NextIConIndex < AdManager.instance.IconToList.Count)
		//{
		//	base.StartCoroutine(this.DownloadiConImg(AdManager.instance.IconLinkList[this.NextIConIndex], this.NextIConIndex));
		//}
		yield break;
	}

	private IEnumerator Downloadlandscape(string url, int Index)
	{
		//int num = this.IsFoundlaLink(Index);
		//if (num == -1)
		//{
		//	this.NextLandIndex = Index + 1;
		//	WWW menuAdView = new WWW(url);
		//	yield return menuAdView;
		//	Texture2D texture = menuAdView.texture;
		//	AdManager.instance.LandList[Index] = Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), new Vector2(0f, 0f));
		//	if (this.NextLandIndex < AdManager.instance.LandToList.Count)
		//	{
		//		base.StartCoroutine(this.Downloadlandscape(AdManager.instance.LandLinkList[this.NextLandIndex], this.NextLandIndex));
		//	}
		//	if (this.NextLandIndex > AdManager.instance.LandToList.Count - 1)
		//	{
		//		base.StartCoroutine(MenuAdPage.instance.Loadland());
		//		MenuAdPage.instance.LandscapeLoaded = true;
		//	}
		//	menuAdView = null;
		//	yield break;
		//}
		//this.NextLandIndex = Index + 1;
		//AdManager.instance.LandList[Index] = AdManager.instance.LandList[num];
		//if (this.NextLandIndex < AdManager.instance.LandToList.Count)
		//{
		//	base.StartCoroutine(this.Downloadlandscape(AdManager.instance.LandLinkList[this.NextLandIndex], this.NextLandIndex));
		//}
		yield break;
	}

	private int IsFoundMGLink(int Index)
	{
		//for (int i = 0; i < AdManager.instance.MgImgLinkList.Count; i++)
		//{
		//	if (Index != i && AdManager.instance.MgImgLinkList[Index] == AdManager.instance.MgImgLinkList[i] && AdManager.instance.MgImgList[i] != null)
		//	{
		//		return i;
		//	}
		//}
		return -1;
	}

	private int IsFoundicLink(int Index)
	{
		//for (int i = 0; i < AdManager.instance.IconLinkList.Count; i++)
		//{
		//	if (Index != i && AdManager.instance.IconLinkList[Index] == AdManager.instance.IconLinkList[i] && AdManager.instance.Iconlist[i] != null)
		//	{
		//		return i;
		//	}
		//}
		return -1;
	}

	private int IsFoundlaLink(int Index)
	{
		//for (int i = 0; i < AdManager.instance.LandLinkList.Count; i++)
		//{
		//	if (Index != i && AdManager.instance.LandLinkList[Index] == AdManager.instance.LandLinkList[i] && AdManager.instance.LandList[i] != null)
		//	{
		//		return i;
		//	}
		//}
		return -1;
	}

	private IEnumerator RemoveCurrentApplink()
	{
		yield return new WaitForSeconds(5f);
		//for (int i = 0; i > AdManager.instance.MgLinkToList.Count; i++)
		//{
		//	UnityEngine.Debug.Log("+++++++++++++++++++++++");
		//	UnityEngine.Debug.Log("checking");
		//	if (AdManager.instance.MgLinkToList[i].Contains("com.FridayBoxGames.AxeHit"))
		//	{
		//		AdManager.instance.MgImgLinkList.RemoveAt(i);
		//		AdManager.instance.MgImgList.RemoveAt(i);
		//		AdManager.instance.MgImgLinkList.RemoveAt(i);
		//	}
		//}
		yield break;
	}

	private XmlNode MainNode;

	private XmlNode MoreGamesNode;

	private XmlNode IconGameNode;

	private XmlNode LandscapeNode;

	private int NextMGIndex;

	private int NextIConIndex;

	private int NextLandIndex;
}
