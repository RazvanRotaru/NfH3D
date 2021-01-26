using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplayController : Interactible
{
    List<Sprite> sprites;
    new MeshRenderer renderer;

    int index = 0;
    public Sprite noSignalSprite;

    void Start()
    {
        sprites = new List<Sprite>(Resources.LoadAll<Sprite>
                        ("Models/Furniture/Livingroom/TVDisplay/Textures"));
        renderer = GetComponent<MeshRenderer>();

        StartCoroutine(nameof(PlayClipCoroutine));
    }

    IEnumerator PlayClipCoroutine()
    {
        while (true)
        {
            renderer.material.SetTexture("_MainTex", sprites[index].texture);
            index = (index + 1) % sprites.Count;
            
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void Break()
    {
        StopCoroutine(nameof(PlayClipCoroutine));
        renderer.material.SetTexture("_MainTex", noSignalSprite.texture);
    }
}
