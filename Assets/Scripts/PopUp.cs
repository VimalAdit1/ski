using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    // Start is called before the first frame update
    public float ttl = 3f;
    public Vector3 randomizeIntensity = new Vector3(0.5f, 0, 0);    
    void Start()
    {
        Destroy(this.gameObject, ttl);
        transform.localPosition += new Vector3(Random.Range(-randomizeIntensity.x, randomizeIntensity.x), Random.Range(-randomizeIntensity.y, randomizeIntensity.y), Random.Range(-randomizeIntensity.z, randomizeIntensity.z));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
