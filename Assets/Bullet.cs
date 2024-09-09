using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;

    public void Initialize(Vector3 dir, float spd)
    {
        direction = dir;
        speed = spd;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Asteroid"))
        {
            asteroidgenerator generator = FindObjectOfType<asteroidgenerator>();
            generator.numAst--;

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Sat"))
        {
            Player player = FindObjectOfType<Player>();
            player.IncreaseSatelliteCount();

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
