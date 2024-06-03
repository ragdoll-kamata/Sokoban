using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private float lifeTime;
    private float leftLifeTime;
    private Vector3 velocity;
    private Vector3 defaultScale;


    // Start is called before the first frame update
    void Start()
    {
        lifeTime = 0.3f;
        leftLifeTime = lifeTime;
        defaultScale = transform.localScale;
        float maxVeloctiy = 5.0f;
        velocity = new Vector3(
                Random.Range(-maxVeloctiy, maxVeloctiy),
                Random.Range(-maxVeloctiy, maxVeloctiy),
                0
            );
    }

    // Update is called once per frame
    void Update()
    {
        leftLifeTime -= Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
        transform.localScale = Vector3.Lerp(
                new Vector3(0, 0, 0),
                defaultScale,
                leftLifeTime / lifeTime
             );
        if (leftLifeTime <= 0) { Destroy(gameObject); }
    }
}
