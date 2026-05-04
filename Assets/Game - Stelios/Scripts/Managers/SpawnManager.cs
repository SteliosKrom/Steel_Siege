using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float assignRefsDelay = 0.1f;
    private float spawnEnemyDelay;
    private float spawnPowerUpsDelay;
    private float despawnPowerUpsDelay;
    private float showWavesUIDelay = 3f;

    private int currentWave = 0;
    private int enemiesToSpawn = 0;
    private int enemiesAlive = 0;

    [SerializeField] private string[] powerUpTags;
    private string powerUpTag;

    private const string ENEMY_TAG = "Enemy";

    #region SCRIPTABLE OBJECTS
    [Header("SCRIPTABLE OBJECTS")]
    [SerializeField] private GameEventsSO gameEvents;
    [SerializeField] private UIEventsSO uiEvents;
    [SerializeField] private AudioEventsSO audioEvents;
    #endregion

    [SerializeField] private GameObject enemyTank;

    #region POINTS
    [Header("POINTS")]
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform player1SpawnPointPVP;
    [SerializeField] private Transform player2SpawnPointPVP;
    [SerializeField] private List<Transform> spawnEnemyPoints;
    [SerializeField] private List<Transform> spawnPowerUpPoints;
    private List<Transform> availablePoints;
    #endregion

    public int EnemiesAlive { get => enemiesAlive; set => enemiesAlive = value; }

    private void Start()
    {
        StartCoroutine(SpawnPowerUpsDelay());

        if (GameManager.Instance.CurrentGameMode == GameMode.PVE)
            StartCoroutine(SpawnEnemyWavesDelay());
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
        UIManager.Instance.MainRefs.player1.transform.position = player1SpawnPointPVP.position;
        UIManager.Instance.MainRefs.player2.transform.position = player2SpawnPointPVP.position;

        UIManager.Instance.MainRefs.player1.SetActive(true);
        UIManager.Instance.MainRefs.player2.SetActive(true);
    }

    public void SpawnPlayerOnPVE()
    {
        UIManager.Instance.MainRefs.player1.transform.position = playerSpawnPoint.position;
        UIManager.Instance.MainRefs.player1.SetActive(true);
    }

    public GameObject SpawnPowerUpAtRandomPoints()
    {
        int randPoint = Random.Range(0, 6);
        int randTag = Random.Range(0, powerUpTags.Length);

        powerUpTag = ChooseRandomPowerUps(randTag);
        audioEvents.RaiseSpawnPowerUp();

        GameObject powerUp = ObjectPoolManager.Instance.GetObject(powerUpTag);
        powerUp.transform.position = spawnPowerUpPoints[randPoint].transform.position;
        return powerUp;
    }

    public string ChooseRandomPowerUps(int randTag)
    {
        for (int i = 0; i < powerUpTags.Length; i++)
        {
            if (powerUpTags[i] == powerUpTags[randTag])
            {
                powerUpTag = powerUpTags[i];
            }
        }
        return powerUpTag;
    }

    public void SpawnEnemiesAtRandomPoints()
    {
        int rand = Random.Range(0, availablePoints.Count);
        GameObject enemy = ObjectPoolManager.Instance.GetObject(ENEMY_TAG);
        enemy.transform.position = availablePoints[rand].position;
        enemiesAlive++;
        availablePoints.RemoveAt(rand);
    }

    public int EnemiesToSpawn(int wave)
    {
        int enemies = 1 + wave / 3;
        if (enemies > 4) enemies = 4;
        return enemies;
    }

    public IEnumerator SpawnPowerUpsDelay()
    {
        while (true)
        {
            if (GameManager.Instance.CurrentGameState != GameState.Playing)
                yield break;

            spawnPowerUpsDelay = Random.Range(10, 14);

            yield return new WaitForSeconds(spawnPowerUpsDelay);

            if (GameManager.Instance.CurrentGameState != GameState.Playing)
                yield break;

            GameObject powerUp = SpawnPowerUpAtRandomPoints();

            despawnPowerUpsDelay = Random.Range(11, 13);
            yield return new WaitForSeconds(despawnPowerUpsDelay);

            if (GameManager.Instance.CurrentGameState != GameState.Playing)
                yield break;

            ObjectPoolManager.Instance.ReturnObject(powerUpTag, powerUp);
        }
    }

    public IEnumerator SpawnEnemyWavesDelay()
    {
        while (true)
        {
            if (GameManager.Instance.CurrentGameState != GameState.Playing) break;

            spawnEnemyDelay = Random.Range(2f, 4f);
            yield return new WaitForSeconds(showWavesUIDelay);

            currentWave++;
            UIManager.Instance.MainRefs.wavesCountText.text = currentWave.ToString();
            uiEvents.RaiseEnableWavesUI();

            yield return new WaitForSeconds(spawnEnemyDelay);
            uiEvents.RaiseDisableWavesUI();
            enemiesToSpawn = EnemiesToSpawn(currentWave);

            availablePoints = new List<Transform>(spawnEnemyPoints);

            for (int i = 0; i < enemiesToSpawn; i++)
                SpawnEnemiesAtRandomPoints();

            yield return new WaitUntil(() => enemiesAlive == 0);
        }
    }

    public IEnumerator EnablePVEPlayerDelay()
    {
        yield return new WaitForSeconds(assignRefsDelay);
        SpawnPlayerOnPVE();
    }

    public IEnumerator EnablePVPPlayersDelay()
    {
        yield return new WaitForSeconds(assignRefsDelay);
        SpawnPlayersOnPVP();
    }
}
