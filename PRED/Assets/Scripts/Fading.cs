using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fading : MonoBehaviour {

    private Texture2D fadeOutTexture;
    public float fadeSpeed = 0.05f;
	public float audioFadeSpeed;
    private int drawDepth = -1000;
    private float alpha = 0.0f;
    private int fadeDir = -1;
    public Color fadingColor = Color.black;
	public AudioSource audioSource;
	private float volume;

    void Start()
    {
		fadeSpeed = 0.05f;

        //fadeOutTexture = new Texture2D(2, 2);
        fadingColor = GameObject.FindGameObjectWithTag("Patient").GetComponent<PatientController>().chosenColor;

        // Create a new 2x2 texture ARGB32 (32 bit with alpha) and no mipmaps
        fadeOutTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);

        // set the pixel values
        fadeOutTexture.SetPixel(0, 0, fadingColor);
        fadeOutTexture.SetPixel(1, 0, fadingColor);
        fadeOutTexture.SetPixel(0, 1, fadingColor);
        fadeOutTexture.SetPixel(1, 1, fadingColor);

        // Apply all SetPixel calls
        fadeOutTexture.Apply();

		// Setting fade speed for audio fading
		if (SceneManager.GetActiveScene ().name == "RelaxingEnv1") {
			audioFadeSpeed = audioSource.volume * fadeSpeed;
		} else {
			audioFadeSpeed = GameObject.FindGameObjectWithTag ("Patient").GetComponent<PatientController> ().chosenVolume * fadeSpeed;
		}
    }

    void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
		// Fading volume
		volume -= fadeDir * audioFadeSpeed * Time.deltaTime;
		volume = Mathf.Clamp (volume, 0, audioFadeSpeed / fadeSpeed);
		audioSource.volume = volume;

        // clamp between 0 and 1
        alpha = Mathf.Clamp01(alpha);
        //print("alpha: " + alpha);
        // set Color of GUI
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    public float BeginFade ( int direction )
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    public void FadeIn()
    {
        print("fadeIn");
        alpha = 0.0f;
        fadeDir = 1;
    }

    public void FadeOut()
    {
        print("fadeOut");
        alpha = 1.0f;
        fadeDir = -1;
    }

    /*void OnLevelWasLoaded()
    {
        BeginFade(-1);
    }*/
}
