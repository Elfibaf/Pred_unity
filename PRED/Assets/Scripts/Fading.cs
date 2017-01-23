using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fading : MonoBehaviour {

    private Texture2D currentFadingTex;
    private Texture2D fadeTextureCol;
    private Texture2D fadeTextureWhite;
    public float fadeSpeed = 0.05f;
	public float audioFadeSpeed;
    private int drawDepth = -1000;
    private float alpha = 0.0f;
    private int fadeDir = -1;
    private Color fadingColor = Color.black;
	public AudioSource audioSource;
	private float volume;

	public bool exitingHomeRoom;
	private bool isFadeSpeedSet; // To set the AudioFadeSpeed right before exiting HomeRoom

    public float getAlpha()
    {
        return (alpha);
    }

    public void setAlpha(float a)
    {
        alpha = a;
    }

    public void setFadingColor(Color col)
    {
        fadingColor = col;
        createColTexture(fadingColor);
    }

    public void setCurrentFadingTex(int index)
    {
        switch(index)
        {
            case 0: currentFadingTex = fadeTextureWhite;
                break;
            case 1: currentFadingTex = fadeTextureCol;
                break;
        }
    }

    public void createWhiteTexture()
    {
        // Create a new 2x2 texture ARGB32 (32 bit with alpha) and no mipmaps
        fadeTextureWhite = new Texture2D(2, 2, TextureFormat.ARGB32, false);

        // set the pixel values
        fadeTextureWhite.SetPixel(0, 0, Color.white);
        fadeTextureWhite.SetPixel(1, 0, Color.white);
        fadeTextureWhite.SetPixel(0, 1, Color.white);
        fadeTextureWhite.SetPixel(1, 1, Color.white);

        // Apply all SetPixel calls
        fadeTextureWhite.Apply();
    }

    public void createColTexture(Color col)
    {
        // Create a new 2x2 texture ARGB32 (32 bit with alpha) and no mipmaps
        fadeTextureCol = new Texture2D(2, 2, TextureFormat.ARGB32, false);

        // set the pixel values
        fadeTextureCol.SetPixel(0, 0, col);
        fadeTextureCol.SetPixel(1, 0, col);
        fadeTextureCol.SetPixel(0, 1, col);
        fadeTextureCol.SetPixel(1, 1, col);

        // Apply all SetPixel calls
        fadeTextureCol.Apply();
    }

    void Start()
    {
		exitingHomeRoom = false;
		isFadeSpeedSet = false;
        if (SceneManager.GetActiveScene().name == "HomeRoom")
        {
            fadeSpeed = 0.1f;
        }
        else
        {
            fadeSpeed = 0.05f;
        }

        createWhiteTexture();
        setCurrentFadingTex(0);

		// Setting fade speed for audio fading
		if (SceneManager.GetActiveScene ().name == "RelaxingEnv1") {
			audioFadeSpeed = audioSource.volume * fadeSpeed;
		} else if (SceneManager.GetActiveScene ().name == "HomeRoom") {
			audioFadeSpeed = 0.5f * fadeSpeed;
		}
		else {
			audioFadeSpeed = GameObject.FindGameObjectWithTag ("Patient").GetComponent<PatientController> ().chosenVolume * fadeSpeed;
		}
    }

    void Update()
    {
    }

    void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
		// Fading volume
		if (SceneManager.GetActiveScene ().name == "HomeRoom" && exitingHomeRoom) {
			if (!isFadeSpeedSet) {
				audioFadeSpeed = audioSource.volume * fadeSpeed;
				isFadeSpeedSet = true;
				volume = audioSource.volume;
			}
			volume -= fadeDir * audioFadeSpeed * Time.deltaTime;
			volume = Mathf.Clamp (volume, 0, audioFadeSpeed/fadeSpeed);
			audioSource.volume = volume;
		} else if (SceneManager.GetActiveScene ().name != "HomeRoom") {
			volume -= fadeDir * audioFadeSpeed * Time.deltaTime;
			volume = Mathf.Clamp(volume, 0, audioFadeSpeed / fadeSpeed);
			audioSource.volume = volume;
		}
        // clamp between 0 and 1
        alpha = Mathf.Clamp01(alpha);
        //print("alpha: " + alpha);
        // set Color of GUI
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), currentFadingTex);
    }

    public float BeginFade ( int direction )
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    public void FadeIn() // opening fade
    {
        print("fadeIn");
        alpha = 1.0f;
        fadeDir = -1;
    }

    public void FadeOut() // closing fade
    {
        print("fadeOut");
        alpha = 0.0f;
        fadeDir = 1;
    }

    /*void OnLevelWasLoaded()
    {
        BeginFade(-1);
    }*/
}
