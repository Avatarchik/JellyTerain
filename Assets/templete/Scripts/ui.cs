using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ui : MonoBehaviour
{
	public void restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void play()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void back()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}

	public void home()
	{
		SceneManager.LoadScene("main");
	}

	public void pause()
	{
		if (Time.timeScale == 0f)
		{
			Time.timeScale = 1f;
		}
		if (Time.timeScale == 1f)
		{
			Time.timeScale = 0f;
		}
	}

	public void whatsapp()
	{
		Application.OpenURL("https://api.whatsapp.com/send?phone=919700040112");
	}

	public void instagram()
	{
		Application.OpenURL("https://www.instagram.com/ibnerahim1/");
	}

	public void facebook()
	{
		Application.OpenURL("https://www.facebook.com/abdul.rab.3762");
	}

	public void credit()
	{
		if (this.credits.activeInHierarchy)
		{
			this.credits.GetComponent<Animator>().Play("creditsClose");
			base.Invoke("reverse", 0.5f);
		}
		if (!this.credits.activeInHierarchy)
		{
			this.credits.SetActive(true);
			this.credits.GetComponent<Animator>().Play("creditsOpen");
		}
	}

	private void reverse()
	{
		this.credits.SetActive(false);
	}

	public void quit()
	{
		Application.Quit();
	}

	public void privacy()
	{
		Application.OpenURL("https://tapbikeracinggames.blogspot.com/");
	}

	public GameObject credits;
}
