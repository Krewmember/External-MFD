using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowSizeManager : MonoBehaviour {

	//Text that displays current window size
	public Text sizeText;

	int size = 512;
	int sizeFieldVel;


	void Start () 
	{
		if (PlayerPrefs.HasKey ("Screen_Size")) 
		{
			size = PlayerPrefs.GetInt ("Screen_Size");
		} 
		else 
		{
			size = 512;
		}
	}

	void Update () 
	{
		Screen.SetResolution (size, size, false, 30);
		sizeText.text = Screen.width.ToString("Window Size:0000");
	}

	//Increase window size
	public void AddSize()
	{
		if (size < 1024) 
		{
			size += 1;
		}
	}

	//Decrease window size
	public void SubSize()
	{
		if (size > 256) 
		{
			size -= 1;
		}
	}

	//Save current window size to PlayerPrefs
	public void SaveWindowSize()
	{
		PlayerPrefs.SetInt ("Screen_Size", Screen.width);
	}

	//Loads last saved window size from PlayerPrefs
	public void LoadWindowSize()
	{
		if(PlayerPrefs.HasKey("Screen_Size"))
		{
			size = PlayerPrefs.GetInt ("Screen_Size");
		}
	}
}
