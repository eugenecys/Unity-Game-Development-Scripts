using UnityEngine;
using System.Collections;

public class Pulser : MonoBehaviour {

    private SpriteRenderer sprite;
    public bool autoPulse;
    public float pulseDuration;
    private bool executing;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        executing = false;
        if (pulseDuration < 0.5f)
        {
            pulseDuration = 0.5f;
        }
    }

    public void reset()
    {
        executing = false;
    }

    public void pulse(float duration)
    {
        if (!executing)
        {
            executing = true;
            StartCoroutine(_pulse(duration));
        }
    }

    IEnumerator _pulse(float duration)
    {
        float t = 0;
        Color spriteColor = sprite.color;
        while (t < 1)
        {
            float opacity = 0.5f * (Mathf.Cos(t * Mathf.PI * 2) + 1);
            sprite.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, opacity);
            t += Time.deltaTime / duration;
            yield return null;
        }
        executing = false;
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        pulse(pulseDuration);
	}
}
