using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class levelSelect : MonoBehaviour
{
	private void Start()
	{
		MonoBehaviour.print(PlayerPrefs.GetInt("level"));
		for (int i = 0; i < this.allLevels.Length; i++)
		{
			base.transform.GetChild(i).name = (i + 1).ToString();
			base.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
		}
		for (int j = 0; j < this.allLevels.Length; j++)
		{
			if (PlayerPrefs.GetInt("level") >= j)
			{
				this.allLevels[j].interactable = true;
			}
			else
			{
				this.allLevels[j].interactable = false;
			}
		}
	}

	public void back()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}

	public void buttonClick()
	{
		levelSelect.call = true;
		levelSelect.level = EventSystem.current.currentSelectedGameObject.transform.name;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public Button[] allLevels;

	public static bool call;

	public static string level;
}
