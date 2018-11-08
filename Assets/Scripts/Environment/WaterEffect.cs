using UnityEngine;
using System.Collections;

public class WaterEffect : MonoBehaviour
{
    public float scrollSpeed = 0.6f;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = (Time.time * scrollSpeed) % 1f;
        rend.material.SetTextureOffset("_MainTex", new Vector2(0f, offset));
    }
}
