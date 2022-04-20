using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour
{
    public static Transform humanoid;
    public static void Rotate()
    {
        humanoid.Rotate(0, Random.Range(-180.0f, 180.0f), 0, relativeTo:Space.Self);
    }
    // Start is called before the first frame update
    void Start()
    {
        humanoid = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
