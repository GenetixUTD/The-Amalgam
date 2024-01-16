using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float despawnTime = 5;
    private float MovementSpeed = 500;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, despawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * MovementSpeed);
    }
}
