using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    //Lives Button
    public float lives = 3f;
    public TextMeshProUGUI livesText;

    //Death Screen
    public GameObject deathScreenPanel;
    public Button restartButton;
    public bool isDead = false;

    //Survival Button
    public float survivalTime = 0f;
    public TextMeshProUGUI timerText;

    //Satellites Button
    public TextMeshProUGUI satelliteCounterText;
    private int satellitesDestroyed = 0;

    //Bullet things
    public float bulletSpeed = 30f;
    public Transform shootPoint;
    public GameObject bulletPrefab;

    public List<GameObject> bullets = new List<GameObject>();

    void Start()
    {
        UpdateLivesDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            RotateRight();
        }
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            RotateLeft();
        }

        MoveBullets();

        UpdateLivesDisplay();

        survivalTime += Time.deltaTime;
        if (timerText != null)
        {
            timerText.text = $"You have survived for: {Mathf.Floor(survivalTime)} seconds";
        }

        if (lives <= 0 && !isDead)
        {
            isDead = true;
            TriggerDeathScreen();
        }
    }

    void Shoot()
    {
        Vector3 shootDirection = shootPoint.up;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.Initialize(shootDirection, bulletSpeed);

        bullet.transform.up = shootDirection;

        bullets.Add(bullet);
    }


    void MoveBullets()
    {
        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            GameObject bullet = bullets[i];
            if (bullet != null)
            {
                bullet.transform.position += shootPoint.up * bulletSpeed * Time.deltaTime;

                if (Vector3.Distance(bullet.transform.position, Vector3.zero) > 10f)
                {
                    Destroy(bullet);
                    bullets.RemoveAt(i);
                }

           }
        }
    }

    void RotateRight()
    {
        transform.Rotate(0, 0, -200f * Time.deltaTime); 
        //for some reason making rotation speed a variable did not work. Will investigate later
    }

    void RotateLeft()
    {
        transform.Rotate(0, 0, 200f * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Asteroid"))
        {
            asteroidgenerator generator = FindObjectOfType<asteroidgenerator>();
            generator.numAst--;

            Destroy(other.gameObject);
            lives--;
        }
    }

    public void UpdateLivesDisplay()
    {
        livesText.text = "Lives: " + lives;
    }

    public void IncreaseSatelliteCount()
    {
        satellitesDestroyed++;
        satelliteCounterText.text = "Satellites destroyed: " + satellitesDestroyed;
    }

    public void TriggerDeathScreen()
    {
        if (deathScreenPanel != null)
        {
            deathScreenPanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}