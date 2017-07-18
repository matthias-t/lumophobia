using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public float health = 1f;
    public float damagePerSecond = 0.05f;
    public float fadeDuration = 1.5f;

    Vector3 oppositeToLight;
    Material mat;
    AudioSource highHealth;
    AudioSource lowHealth;
    Light glow;

    public static Text text;

	void Start () {
        //Direction opposite to light for raycasting
        oppositeToLight = -(transform.FindChild("Sun").rotation * Vector3.forward); 

        mat = GetComponent<Renderer>().material;
        glow = transform.FindChild("Glow").GetComponent<Light>();

        AudioSource[] sources = GetComponents<AudioSource>();
        highHealth = sources[0];
        lowHealth = sources[1];

        text = GameObject.Find("Text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!World.start) return;

        //Light or shadow, damage
        Ray ray = new Ray(transform.position, oppositeToLight);
        Debug.DrawRay(transform.position, oppositeToLight, Color.magenta);
        if (!Physics.Raycast(ray, 10f))
        {
            health -= damagePerSecond * Time.deltaTime;

            //Sound clips volume
            highHealth.volume = health / 2f;
            lowHealth.volume = (1f - health) / 2f;
        }
        else
        {
            health += damagePerSecond * Time.deltaTime;
            highHealth.volume = 0f;
            lowHealth.volume = 0f;
        }

        //Color and emission
        Color baseColor = new Color(2f * health, health, health);
        mat.color = baseColor;
        mat.SetColor("_EmissionColor", baseColor * Mathf.LinearToGammaSpace(1 - health));
        glow.intensity = 4f * (0.5f - Mathf.Abs(0.5f - health));

        if (health < 0)
        {
            GameOver();
        }
	}

    public void GameOver() {
        text.text = "the sun is too bright";
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn() {
        text.color = new Color(1, 1, 1, 0);
        text.gameObject.SetActive(true);
        World.start = false;
        highHealth.volume = 0.5f;
        lowHealth.volume = 0.5f;
        float fadeDelay = fadeDuration / 100f;
        yield return new WaitForSeconds(1f);
        for (int i = 1; i < 101; i++) {
            text.color = new Color(1f, 1f, 1f, i / 100f);
            yield return new WaitForSeconds(fadeDelay);
            highHealth.volume = 0.5f / i;
            lowHealth.volume = 0.5f / i;
        }
        highHealth.volume = 0f;
        lowHealth.volume = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        text.text = "you didn't win";
        StartCoroutine(FadeIn());
        StartCoroutine(Win());
    }

    IEnumerator Win() {
        yield return new WaitForSeconds(5f);
        text.text = "the player did";
        yield return new WaitForSeconds(3f);
        text.text = "a game made in 8 hours";
        yield return new WaitForSeconds(2f);
        text.text = "at berlin mini jam";
        yield return new WaitForSeconds(2f);
        text.text = "code: matthias";
        yield return new WaitForSeconds(2f);
        text.text = "3d art: johannes";
        yield return new WaitForSeconds(2f);
        text.text = "sound: david";
        yield return new WaitForSeconds(2f);
        text.text = "thanks for playing";
    }
}
