using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaloonController : MonoBehaviour
{
    //public Vector3 origin;
    Vector3 originPos;
    public Vector3 target;
    Vector3 direction;

    GameObject stainPrefab;
    GameObject splashPrefab;
    
    readonly float speed = 1f;
    bool launched = false;
    float projectileDist;
    float path = 0f;

    bool instantiated = false;

    private void Start()
    {
        stainPrefab = Resources.Load<GameObject>("Models/WaterBallon/Stain/Model");
        splashPrefab = Resources.Load<GameObject>("Models/WaterBallon/Spalsh/Particles");
    }

    public void Init(Transform origin, Vector3 target)
    {
        transform.position = origin.position;
        transform.rotation = origin.rotation;
        originPos = transform.position;
        this.target = target;

        direction = (target - transform.position);

        launched = true;
        projectileDist = (target - originPos).magnitude;
    }

  
    void Update()
    {
        if (!launched)
            return;

        float percentRemaining = (target - transform.position).magnitude / projectileDist;

        float heightOffset = Mathf.Sin(percentRemaining * Mathf.PI);
        transform.position = originPos;
        transform.position += direction * path;
        transform.position += heightOffset * Vector3.up;
        //transform.position += direction.normalized * speed * Time.deltaTime;
        path += speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (instantiated)
            return;

        instantiated = true;

        GameObject hitObject = collision.collider.gameObject;

        if (!hitObject.name.Contains("Wall"))
            return;

        GameObject stain = Instantiate(stainPrefab, hitObject.transform);
       
        

        Vector3 position = collision.contacts[0].point + 0.2f * hitObject.transform.forward;
        
        stain.layer = hitObject.layer;
        stain.transform.position = position;
        
        Instantiate(splashPrefab, stain.transform);
        stain.transform.localScale = DivideVectors(stainPrefab.transform.localScale,
                                                        hitObject.transform.localScale);

        Destroy(gameObject);
    }

    Vector3 DivideVectors(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }
}
