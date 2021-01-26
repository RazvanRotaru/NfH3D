using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public int seconds = 15;
    public bool fade = false;

    float elapsedTime = 0;
    new MeshRenderer renderer;

    void Start()
    {
        StartCoroutine(nameof(DestroyCoroutine));

        if (fade)
            renderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        elapsedTime = 0;
    }

    private void Update()
    {
        if (!fade)
            return;
        
        elapsedTime += Time.deltaTime;

        Color32 col = renderer.material.GetColor("_Color");
        col.a = (byte)(1 - ((float)(elapsedTime / seconds)) * byte.MaxValue);
        renderer.material.SetColor("_Color", col);
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
