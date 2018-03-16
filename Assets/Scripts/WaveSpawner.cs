using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

    //References
    public Transform enemyPrefab;           //Prefab of the enemy (for now single enemy)
    public Transform spawnPoint;            //Spawn point
    public Text countdownText;              //Text that shows the countdown timer

    //Variables
    public float timeBetweenWaves = 5.5f;     //Delay between the spawns
    private float countdown = 3f;           //Delay until the first wave
    private int waveIndex = 0;              //Actual wave number 


    //Methods
    private void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;

        countdownText.text = Mathf.Round(countdown).ToString();
    }

    IEnumerator SpawnWave()
    {
        waveIndex++;
        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }

    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }


}
