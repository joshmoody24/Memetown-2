using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TransparentFade : MonoBehaviour {

    public float transitionSpeed = .5f;
    public bool fadeChildren = false;
    [Range(0f,1f)]
    public float maximumTransparency = .5f;
    public bool isFading = false;
    private SpriteRenderer sr;
    private SpriteRenderer[] children;
    private int originalLayer;
    private int playerLayer;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.white;
        originalLayer = sr.sortingOrder;
        playerLayer = GameObject.FindWithTag("Player").GetComponent<SortingGroup>().sortingOrder;
        children = GetComponentsInChildren<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(isFading)
        {
            sr.sortingOrder = playerLayer + 10;
            if (sr.color.a > maximumTransparency)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - (transitionSpeed * Time.deltaTime));
            }
            //do the same but for children
            if (fadeChildren)
            {

                foreach (SpriteRenderer src in children)
                {
                    if (!src.gameObject.name.Contains("Base") && !src.gameObject.name.Contains("base"))
                    {
                        src.sortingOrder = sr.sortingOrder + 1;
                        if (sr.color.a > maximumTransparency)
                        {
                            src.color = new Color(sr.color.r, src.color.g, src.color.b, src.color.a - (transitionSpeed * Time.deltaTime));
                        }
                    }
                }
            }
        }
        else if (isFading == false)
        {
            sr.sortingOrder = originalLayer;
            if (sr.color.a < 1)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a + (transitionSpeed * Time.deltaTime));
            }
            //assumes children are 1 above parent
            if (fadeChildren)
            {
                foreach (SpriteRenderer src in children)
                {
                    if (!src.gameObject.name.Contains("Base") && !src.gameObject.name.Contains("base"))
                    {
                        src.sortingOrder = sr.sortingOrder + 1;
                        if (src.color.a < 1)
                        {
                            src.color = new Color(src.color.r, src.color.g, src.color.b, src.color.a + (transitionSpeed * Time.deltaTime));
                        }
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            isFading = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isFading = false;
        }
    }
}
