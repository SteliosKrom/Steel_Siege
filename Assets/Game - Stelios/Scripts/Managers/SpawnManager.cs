using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float assignRefsDelay = 0.1f;

    #region SCRIPTABLE OBJECTS
    [Header("SCRIPTABLE OBJECTS")]
    [SerializeField] private GameEventsSO gameEvents;
    #endregion

    [SerializeField] private GameObject enemyTank;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform player1SpawnPointPVP;
    [SerializeField] private Transform player2SpawnPointPVP;
    [SerializeField] private List<Transform> spawnPoints;
    private List<Transform> availablePoints;
    private int currentWave = 0;
    private int enemiesToSpawn = 0;
    private int enemiesAlive = 0;

    public int EnemiesAlive { get => enemiesAlive; set => enemiesAlive = value; }

    private void Start()
    {
        if (GameManager.Instance.CurrentGameMode == GameMode.PVE)
        {
            StartCoroutine(SpawnEnemyWavesDelay());
        }
    }

    private void OnEnable()
    {
        gameEvents.OnPVPLoad += EnablePVPTanks;
        gameEvents.OnPVELoad += EnablePVETanks;
    }

    private void OnDisable()
    {
        gameEvents.OnPVPLoad -= EnablePVPTanks;
        gameEvents.OnPVELoad -= EnablePVETanks;
    }

    public void EnablePVPTanks()
    {
        StartCoroutine(EnablePVPPlayersDelay());
    }

    public void EnablePVETanks()
    {
        StartCoroutine(EnablePVEPlayerDelay());
    }

    public void SpawnPlayersOnPVP()
    {
        Vector3 player1Pos = UIManager.Instance.MainRefs.player1.transform.position;
        Vector3 player2Pos = UIManager.Instance.MainRefs.player2.transform.position;
        player1Pos = player1SpawnPointPVP.position;
        player2Pos = player2SpawnPointPVP.position;
    }

    public void SpawnPlayerOnPVE()
    {
        UIManager.Instance.MainRefs.player1.transform.position = playerSpawnPoint.transform.position;
        UIManager.Instance.MainRefs.player1.SetActive(true);
    }

    public void SpawnEnemiesAtRandomPoints()
    {
        int rand = Random.Range(0, availablePoints.Count);
        GameObject enemy = ObjectPoolManager.Instance.GetObject("Enemy");
        enemy.transform.position = availablePoints[rand].position;
        enemiesAlive++;
        availablePoints.RemoveAt(rand);
    }

    public int EnemiesToSpawn(int wave)
    {
        if (wave == 1) return 1;
        else if (wave == 2) return 2;
        else if (wave <= 6) return 3;
        else if (wave <= 12) return 4;
        else return 5;
    }

    public IEnumerator SpawnEnemyWavesDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            currentWave++;

            enemiesToSpawn = EnemiesToSpawn(currentWave);

            availablePoints = new List<Transform>(spawnPoints);

            for (int i = 0; i < enemiesToSpawn; i++)
                SpawnEnemiesAtRandomPoints();

            yield return new WaitUntil(() => enemiesAlive == 0);
        }
    }

    public IEnumerator EnablePVEPlayerDelay()
    {
        yield return new WaitForSeconds(assignRefsDelay);
        UIManager.Instance.MainRefs.player1.transform.position = playerSpawnPoint.position;
        UIManager.Instance.MainRefs.player1.SetActive(true);
    }

    public IEnumerator EnablePVPPlayersDelay()
    {
        yield return new WaitForSeconds(assignRefsDelay);
        UIManager.Instance.MainRefs.player1.transform.position = player1SpawnPointPVP.position;
        UIManager.Instance.MainRefs.player2.transform.position = player2SpawnPointPVP.position;

        UIManager.Instance.MainRefs.player1.SetActive(true);
        UIManager.Instance.MainRefs.player2.SetActive(true);
    }
}
