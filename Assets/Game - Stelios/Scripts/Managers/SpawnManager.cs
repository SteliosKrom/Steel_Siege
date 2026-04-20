using NUnit.Framework;
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

    public void SpawnEnemiesAtRandomPoints()
    {
        int rand = Random.Range(0, availablePoints.Count);
        GameObject enemy = ObjectPoolManager.Instance.GetObject("Enemy");
        enemy.transform.position = availablePoints[rand].position;
        enemiesAlive++;
        availablePoints.RemoveAt(rand);
    }

    public IEnumerator SpawnEnemyWavesDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            currentWave++;

            if (currentWave > 3)
                enemiesToSpawn = 3;
            else
                enemiesToSpawn = currentWave;

            availablePoints = new List<Transform>(spawnPoints);

            for (int i = 0; i < enemiesToSpawn; i++)
                SpawnEnemiesAtRandomPoints();

            yield return new WaitUntil(() => enemiesAlive == 0);
        }
    }

    public IEnumerator EnablePVEPlayerDelay()
    {
        yield return new WaitForSeconds(assignRefsDelay);
        UIManager.Instance.MainRefs.player1.SetActive(true);
    }

    public IEnumerator EnablePVPPlayersDelay()
    {
        yield return new WaitForSeconds(assignRefsDelay);
        UIManager.Instance.MainRefs.player1.SetActive(true);
        UIManager.Instance.MainRefs.player2.SetActive(true);
    }
}
