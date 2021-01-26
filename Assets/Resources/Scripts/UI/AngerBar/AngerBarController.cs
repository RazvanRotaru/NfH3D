using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngerBarController : MonoBehaviour
{
    enum Faces
    {
        Agry,
        Aware,
        Neutral,
        Rage
    }

    Image angerBar;
    Image face;
    string facesPath;
    public Sprite[] faces;

    NeighbourController neighbour;

    public float updateAmmount;
    bool seenPlayer = false;

    void Start()
    {
        face = transform.Find("Face").Find("Image")
                                .GetComponent<Image>();
        angerBar = transform.Find("Mask").Find("Image")
                                .GetComponent<Image>();
        angerBar.fillAmount = 0;

        faces = Resources.LoadAll<Sprite>("Graphics/Sprites/Faces");

        neighbour = NeighbourController.instance;
        neighbour.OnPrank += UpdateAnger;
        neighbour.OnPrank += ChangeFace;
    }

    private void Update()
    {
        if (neighbour.Calm && !seenPlayer)
            angerBar.fillAmount -= Time.deltaTime / 100;
    }

    void UpdateAnger(GameObject instance)
    {
        if (instance.CompareTag("Player"))
        {
            angerBar.fillAmount = 1;
            seenPlayer = true;
            return;
        }

        face.sprite = faces[(int)Faces.Aware];
        //angerBar.fillAmount += updateAmmount;
        //ChangeFace(instance);

        if (angerBar.fillAmount == 1)
        {
            StopCoroutine(nameof(StayAngry));
            Debug.Log("Neighbour has a stroke");
            neighbour.Fall();
            return;
        }

        StopCoroutine(nameof(StayAngry));
        StartCoroutine(nameof(StayAngry), instance);
    }

    IEnumerator StayAngry(GameObject instance)
    {
        yield return new WaitUntil(() =>
                            neighbour.animator.GetBool("angry"));
        angerBar.fillAmount += updateAmmount;
        
        if (angerBar.fillAmount == 1)
        {
            Debug.Log("Neighbour has a stroke");
            neighbour.Fall();
            yield return null;
        }
        else
        {
            ChangeFace(instance);
            yield return new WaitUntil(() =>
                                !neighbour.animator.GetBool("angry"));
            neighbour.Calm = true;
        }
    }

    void ChangeFace(GameObject instance)
    {
        //if (neighbour.Target != null 
        //            && !instance.CompareTag("Player"))
        //{
        //    face.sprite = faces[(int)Faces.Aware];
        //    return;
        //}

        if (angerBar.fillAmount < updateAmmount)
            face.sprite = faces[(int)Faces.Neutral];

        if (angerBar.fillAmount >= updateAmmount)
            face.sprite = faces[(int)Faces.Agry];

        if (angerBar.fillAmount > 0.8f)
        {
            face.sprite = faces[(int)Faces.Rage];
            face.transform.localScale = new Vector3(1.5f, 1.5f);
            face.color = Color.red;
        }
        else
        {
            face.transform.localScale = Vector3.one;
            face.color = Color.white;
        }

    }

    private void OnDisable()
    {
        neighbour.OnPrank -= UpdateAnger;
    }
}
