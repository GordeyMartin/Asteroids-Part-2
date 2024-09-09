using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroidgenerator : MonoBehaviour
{
    public int maxAst = 9;
    public int numAst = 0;
    private float spawnInterval = 0.5f;
    private float timeSinceLastSpawn = 0f;

    private float timeSinceLastIncrease = 0f;
    private float increaseInterval = 3f;

    private List<GameObject> asteroids = new List<GameObject>();
    public float asteroidSpeed = 2f;

    public GameObject redAsteroidPrefab;

    public GameObject asteroidPrefab;
    public GameObject asteroidPrefab1;
    public GameObject asteroidPrefab2;
    public GameObject asteroidPrefab3;
    public GameObject asteroidPrefab4;
    public GameObject asteroidPrefab5;

    public GameObject satellitePrefab;

    void Update()
    {
        //I increase speed of asteroids by 5% every 3 seconds.
        timeSinceLastIncrease += Time.deltaTime;
        if (timeSinceLastIncrease >= increaseInterval)
        {
            asteroidSpeed *= 1.05f;
            timeSinceLastIncrease = 0f;
        }

        //There are max 9 asteroids at the same time on the screen.
        if (numAst < maxAst)
        {
            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn >= spawnInterval && numAst <= maxAst)
            {
                SpawnAsteroid();
                timeSinceLastSpawn = 0f;
                numAst++;
            }
        }
        MoveAsteroids();
        //Satellite code
        if (Random.Range(0, 1000) < 1)
        {
            SpawnSatellite();
        }

    }

    void SpawnAsteroid()
    {
        Vector3 spawnPosition = GetRandomOffscreenPosition();

        GameObject asteroidPrefabToUse = null;

        float randomNumber = Random.Range(0, 10);

        if (randomNumber == 0)
        {
            asteroidPrefabToUse = redAsteroidPrefab;
        }
        else if (randomNumber == 1)
        {
            asteroidPrefabToUse = asteroidPrefab;
        }
        else if (randomNumber == 2)
        {
            asteroidPrefabToUse = asteroidPrefab1;
        }
        else if (randomNumber == 3)
        {
            asteroidPrefabToUse = asteroidPrefab2;
        }
        else if (randomNumber == 4)
        {
            asteroidPrefabToUse = asteroidPrefab3;
        }
        else if (randomNumber == 5)
        {
            asteroidPrefabToUse = asteroidPrefab4;
        }
        else if (randomNumber >= 6)
        {
            asteroidPrefabToUse = asteroidPrefab5;
        }

        GameObject asteroid = Instantiate(asteroidPrefabToUse, spawnPosition, Quaternion.identity);

        asteroids.Add(asteroid);
        //Red asteroid is 15% faster than others.
        if (asteroidPrefabToUse == redAsteroidPrefab)
        {
            asteroid.GetComponent<Asteroid>().SetSpeed(asteroidSpeed * 1.15f);
        }
        else
        {
            asteroid.GetComponent<Asteroid>().SetSpeed(asteroidSpeed);
        }
    }

    void SpawnSatellite()
    {
        Vector3 spawnPosition = GetRandomScreenPosition();
        Instantiate(satellitePrefab, spawnPosition, Quaternion.identity);
    }

    Vector3 GetRandomScreenPosition()
    {
        float x = Random.Range(0f, Screen.width);
        float y = Random.Range(0f, Screen.height);
        Vector3 screenPosition = new Vector3(x, y, Camera.main.nearClipPlane);

        return Camera.main.ScreenToWorldPoint(screenPosition);
    }

    Vector3 GetRandomOffscreenPosition()
    {
        float x = Random.Range(-15f, 15f);
        float y = Random.Range(-15f, 15f);

        if (Mathf.Abs(x) < 10f && Mathf.Abs(y) < 10f)
        {
            if (Random.Range(0, 2) == 0)
            {
                x = Mathf.Sign(x) * 15f;
            }
            else
            {
                y = Mathf.Sign(y) * 15f;
            }
        }

        return new Vector3(x, y, 0f);
    }
    public void MoveAsteroids()
    {
        foreach (GameObject asteroid in asteroids)
        {
            if (asteroid != null)
            {
                Vector3 direction = (Vector3.zero - asteroid.transform.position).normalized;

                asteroid.transform.position += direction * asteroid.GetComponent<Asteroid>().speed * Time.deltaTime;
            }
        }
    }
}


