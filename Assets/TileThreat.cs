using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileThreat : MonoBehaviour
{
    [SerializeField] Color startColor;
    [SerializeField] Color endColor;
    Color outputColor;
    Color offColor = new Color(0, 0, 0, 0);
    TileScript tileScript;
    SpriteRenderer spriteRenderer;
    float speed = 3;
    Vector3 startSize;
    Vector3 minSize;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tileScript = GetComponentInParent<TileScript>();
        startSize = transform.localScale;
        minSize = transform.localScale * 0.9f;
    }

    // Update is called once per frame
    void Update()
    {
        if(tileScript.threats > 0)
        {
            float lerpValue = Mathf.Sin(speed * Time.time) * 0.5f + 0.5f;
            outputColor = Color.Lerp(startColor, endColor, lerpValue);
            spriteRenderer.color = outputColor;
            //spriteRenderer.color = startColor;

            transform.localScale = Vector3.Lerp(startSize, minSize, lerpValue);
        }
        else
        {
            spriteRenderer.color = offColor;
        }
    }
}
