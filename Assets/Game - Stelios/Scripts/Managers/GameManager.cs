using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.Profiling;
using UnityEngine.Video;
using System.Text;

public enum GameState
{
    Title,
    SelectModes,
    Playing,
    GameOver,
    Player1Wins,
    Player2Wins,
    Draw,
    Leaderboard
}

public enum GameMode
{
    None,
    PVP,
    PVE
}

public enum PlayerState
{
    Idle,
    Moving
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int currentModeIndex;
    [SerializeField] private int currentLetterIndex = 0;
    [SerializeField] private int enterLetterIndex = 0;
    [SerializeField] private int currentCharIndex = 0;

    private float fpsTimer = 0f;
    private float drawCallsTimer = 0f;
    private float fps;
    private float demoModeTimer = 0f;

    private int drawCalls = 0;
    private int ramUsage;
    private int activeGameObjects;
    private int activeRigidbodies;

    private float checkGameResultDelay = 0.1f;
    private float returnBackToMainTitleDelay = 3f;
    private float moveToEnterYourNameDelay = 3f;
    private float returnBackToMainTitleFromLeaderboard = 10f;

    private bool player1Dead = false;
    private bool player2Dead = false;
    [SerializeField] private bool onEnterYourName = false;
    [SerializeField] private bool onDemoMode = false;
    private bool[] slotHasLetter = new bool[3];
    private bool[] slotWasConfirmed = new bool[3];

    private char[] allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    private string currentLeaderboardBestScorePlayerName;

    private ProfilerRecorder drawCallsRecorder;
    private ProfilerRecorder memoryRecorder;

    private VideoPlayer videoPlayer;

    #region INPUT
    private PlayerControls playerControls;
    private Vector2 navigateInput;
    #endregion

    #region EVENTS
    [Header("EVENTS")]
    [SerializeField] private GameEventsSO gameEvents;
    [SerializeField] private UIEventsSO uiEvents;
    [SerializeField] private ScoreEventsSO scoreEvents;
    [SerializeField] private AudioEventsSO audioEvents;
    #endregion

    #region STATES
    [Header("STATES")]
    [SerializeField] private GameState currentGameState;
    [SerializeField] private GameMode currentGameMode;
    #endregion

    #region OBJECTS
    private GameObject mainCamera;
    #endregion

    #region PROPERTIES
    public bool OnDemoMode => onDemoMode;
    public string CurrentLeaderboardBestScorePlayerName
    {
        get => currentLeaderboardBestScorePlayerName;
        set => currentLeaderboardBestScorePlayerName = value;
    }
    public int CurrentModeIndex { get => currentModeIndex; }
    public int CurrentLetterIndex { get => currentLetterIndex; set => currentLetterIndex = value; }
    public int EnterLetterIndex
    {
        get => enterLetterIndex;
        set => enterLetterIndex = Mathf.Clamp(value, 0, 3);
    }
    public int CurrentCharIndex
    {
        get => currentCharIndex;
        set => currentCharIndex = Mathf.Clamp(value, 0, allowedChars.Length - 1);
    }
    public float FPS
    {
        get
        {
            return fps;
        }
        set
        {
            if (value >= 60)
                fps = 60;
        }
    }
    public GameState CurrentGameState { get => currentGameState; set => currentGameState = value; }
    public GameMode CurrentGameMode { get => currentGameMode; set => currentGameMode = value; }
    #endregion

    private void Awake()
    {
        playerControls = new PlayerControls();

        transform.SetParent(null);

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        playerControls.Enable();
        playerControls.UI.Navigate.started += OnNavigate;
        playerControls.UI.Submit.started += OnSubmit;
        playerControls.UI.NameInput.started += OnEnterYourName;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        playerControls.UI.Navigate.started -= OnNavigate;
        playerControls.UI.Submit.started -= OnSubmit;
        playerControls.UI.NameInput.started -= OnEnterYourName;
        playerControls.Disable();

        if (drawCallsRecorder.Valid)
            drawCallsRecorder.Dispose();

        if (memoryRecorder.Valid)
            memoryRecorder.Dispose();
    }

    private void Start()
    {
        currentModeIndex = 0;
        currentGameState = GameState.Title;
        currentGameMode = GameMode.None;
    }

    private void Update()
    {
        if (!UIManager.Instance.IsUIReady())
            return;

        CalculateDrawCalls();
        CalculateFPS();
        ramUsage = (int)CalculateRamUsage();
        activeGameObjects = CalculateActiveGameObjects();
        activeRigidbodies = CalculateActiveRigidBodies();

        UIManager.Instance.DisplayFPS(FPS);
        UIManager.Instance.DisplayNumberOfDrawCalls(drawCalls);
        UIManager.Instance.DisplayRamUsage(ramUsage);
        UIManager.Instance.DisplayActiveGameObjects(activeGameObjects);
        UIManager.Instance.DisplayActiveRigidBodies(activeRigidbodies);

        InputForGameModeSelection();
        AnyKey();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (drawCallsRecorder.Valid)
            drawCallsRecorder.Dispose();

        if (memoryRecorder.Valid)
            memoryRecorder.Dispose();

        drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
        memoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        switch (scene.name)
        {
            case "Title":
                videoPlayer = GameObject.Find("VideoPlayer").GetComponent<VideoPlayer>();
                mainCamera = GameObject.Find("MainCamera");
                break;
            case "Main":
                if (onDemoMode)
                {
                    mainCamera = GameObject.Find("MainCamera");
                    mainCamera.GetComponent<CRTFilterEffect>().enabled = true;
                    return;
                }
                else
                    mainCamera = GameObject.Find("MainCamera");
                break;
            case "Leaderboard":
                mainCamera = GameObject.Find("MainCamera");
                break;
        }
    }

    public void OnNavigate(InputAction.CallbackContext cxt)
    {
        if (onDemoMode) return;

        navigateInput = cxt.ReadValue<Vector2>();
    }

    public void OnSubmit(InputAction.CallbackContext cxt)
    {
        if (onDemoMode) return;

        if (cxt.control.name == "enter")
            EnterGameMode();

        if (cxt.control.name == "t")
        {
            if (UIManager.Instance.CurrentDebugOverlay.activeInHierarchy)
                UIManager.Instance.CurrentDebugOverlay.SetActive(false);
            else
                UIManager.Instance.CurrentDebugOverlay.SetActive(true);
        }

        if (cxt.control.name == "y")
        {
            if (mainCamera.GetComponent<CRTFilterEffect>().enabled)
                mainCamera.GetComponent<CRTFilterEffect>().enabled = false;
            else
                mainCamera.GetComponent<CRTFilterEffect>().enabled = true;
        }
    }

    public void AnyKey()
    {
        demoModeTimer += Time.deltaTime;
        bool inputThisFrame = playerControls.UI.AnyInput.WasPressedThisFrame();

        if (!onDemoMode)
        {
            if (inputThisFrame)
            {
                if (currentGameState == GameState.Title)
                {
                    JoinGame();
                }
                demoModeTimer = 0f;
                return;
            }
            else
            {
                if ((currentGameState == GameState.Title || currentGameState == GameState.SelectModes)
                    && demoModeTimer > 30)
                {
                    EnterDemoMode();
                    return;
                }
            }
        }
        else
        {
            if (inputThisFrame)
            {
                demoModeTimer = 0f;
                ExitDemoMode();
                return;
            }
        }
    }

    public void OnEnterYourName(InputAction.CallbackContext cxt)
    {
        if (!onEnterYourName) return;
        if (CurrentLetterIndex >= UIManager.Instance.MainRefs.letters.Length) return;

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            CurrentCharIndex = (CurrentCharIndex + 1) % allowedChars.Length;
            UIManager.Instance.MainRefs.letters[CurrentLetterIndex].text = allowedChars[CurrentCharIndex].ToString();
            slotHasLetter[CurrentLetterIndex] = true;
            slotWasConfirmed[CurrentLetterIndex] = false;
        }

        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            CurrentCharIndex = (CurrentCharIndex - 1 + allowedChars.Length) % allowedChars.Length;
            UIManager.Instance.MainRefs.letters[CurrentLetterIndex].text = allowedChars[CurrentCharIndex].ToString();
            slotHasLetter[CurrentLetterIndex] = true;
            slotWasConfirmed[CurrentLetterIndex] = false;
        }

        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            if (slotHasLetter[CurrentLetterIndex] && !slotWasConfirmed[CurrentLetterIndex])
            {
                EnterLetterIndex++;
                slotWasConfirmed[CurrentLetterIndex] = true;
            }

            CurrentLetterIndex++;
            CurrentCharIndex = 0;

            if (CurrentLetterIndex == 3)
            {
                TryConfirmName();
                CurrentLetterIndex = 2;
            }
        }

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            if (slotWasConfirmed[CurrentLetterIndex])
            {
                EnterLetterIndex--;
                slotHasLetter[CurrentLetterIndex] = false;
                slotWasConfirmed[CurrentLetterIndex] = false;
            }

            UIManager.Instance.MainRefs.letters[CurrentLetterIndex].text = "_";
            slotHasLetter[CurrentLetterIndex] = false;
            slotWasConfirmed[CurrentLetterIndex] = false;
            CurrentLetterIndex--;
            CurrentCharIndex = 0;

            if (CurrentLetterIndex < 0)
            {
                CurrentLetterIndex = 0;
            }
        }

        switch (CurrentLetterIndex)
        {
            case 0:
                uiEvents.RaiseFirstLetterStay();
                uiEvents.RaiseSecondLetterExit();
                uiEvents.RaiseThirdLetterExit();
                break;
            case 1:
                uiEvents.RaiseFirstLetterExit();
                uiEvents.RaiseSecondLetterStay();
                uiEvents.RaiseThirdLetterExit();
                break;
            case 2:
                uiEvents.RaiseFirstLetterExit();
                uiEvents.RaiseSecondLetterExit();
                uiEvents.RaiseThirdLetterStay();
                break;
        }
    }

    public void TryConfirmName()
    {
        if (EnterLetterIndex == 0 || EnterLetterIndex == 3)
            BuildFinalName();
    }

    public void BuildFinalName()
    {
        StringBuilder sb = new StringBuilder(3);

        for (int i = 0; i < UIManager.Instance.MainRefs.letters.Length; i++)
            sb.Append(UIManager.Instance.MainRefs.letters[i].text);

        currentLeaderboardBestScorePlayerName = sb.ToString();
        InsertAndValidateName();
    }

    public void InsertAndValidateName()
    {
        onEnterYourName = false;
        CurrentLetterIndex = 0;

        int finalScore = ScoreManager.Instance.CurrentScore;
        string playerName = currentLeaderboardBestScorePlayerName;

        LeaderboardManager.Instance.InsertScore(finalScore, playerName);
        SaveManager.Instance.SaveAll();

        StartCoroutine(MoveToLeaderboardDelay());
    }

    public void JoinGame()
    {
        if (UIManager.Instance.IsWaiting) return;
        if (currentGameState == GameState.SelectModes) return;

        if (currentGameState == GameState.Title)
        {
            switch (UIManager.Instance.CreditCounter)
            {
                case 0:
                    uiEvents.RaiseInsertCoin();
                    break;
                case 1:
                    currentGameState = GameState.SelectModes;
                    uiEvents.RaiseEnableGameModes();
                    uiEvents.RaisePVPStay();
                    break;
            }
        }
    }

    public void EnterDemoMode()
    {
        LoadDemoMode();
    }

    public void ExitDemoMode()
    {
        LoadMenu();
    }

    public float CalculateFPS()
    {
        fpsTimer += Time.deltaTime;

        if (fpsTimer >= 1f)
        {
            FPS = Mathf.RoundToInt(1f / Time.deltaTime);
            fpsTimer = 0f;
        }
        return FPS;
    }

    public int CalculateActiveGameObjects()
    {
        GameObject[] activeGameObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        int count = activeGameObjects.Length;
        return count;
    }

    public int CalculateActiveRigidBodies()
    {
        Rigidbody2D[] activeRigidbodies = FindObjectsByType<Rigidbody2D>(FindObjectsSortMode.None);
        int count = activeRigidbodies.Length;
        return count;
    }

    public int CalculateDrawCalls()
    {
        drawCallsTimer += Time.deltaTime;

        if (drawCallsTimer >= 1f)
        {
            drawCalls = (int)drawCallsRecorder.LastValue;
            drawCallsTimer = 0f;
        }
        return (int)drawCalls;
    }

    public float CalculateRamUsage()
    {
        long usedBytes = memoryRecorder.LastValue;
        float usedMB = usedBytes / (1024 * 1024);
        float totalMB = SystemInfo.systemMemorySize;
        float percent = (usedMB / totalMB) * 100f;
        return Mathf.RoundToInt(percent);
    }

    public void EnterGameMode()
    {
        if (currentGameState != GameState.SelectModes) return;

        if (currentGameState == GameState.SelectModes)
        {
            if (currentModeIndex == 0)
                LoadPVPMode();
            else
                LoadPVEMode();
        }
    }

    public void InputForGameModeSelection()
    {
        if (currentGameState != GameState.SelectModes) return;

        if (navigateInput.y < 0 && currentModeIndex == 0)
            SelectPVEMode();
        else if (navigateInput.y > 0 && currentModeIndex == 1)
            SelectPVPMode();

        navigateInput = Vector2.zero;
    }

    public void SelectPVPMode()
    {
        uiEvents.RaisePVEExit();
        uiEvents.RaisePVPStay();
        uiEvents.RaisePVPSelected();
        currentModeIndex = 0;
    }

    public void SelectPVEMode()
    {
        uiEvents.RaisePVPExit();
        uiEvents.RaisePVEStay();
        uiEvents.RaisePVESelected();
        currentModeIndex = 1;
    }

    public void LoadPVPMode()
    {
        currentGameState = GameState.Playing;
        currentGameMode = GameMode.PVP;

        ScoreManager.Instance.ResetRunScores();
        StartCoroutine(RaiseAfterLoad(true));
        uiEvents.RaiseEnablePVPLives();
    }

    public void LoadPVEMode()
    {
        currentGameState = GameState.Playing;
        currentGameMode = GameMode.PVE;

        ScoreManager.Instance.ResetRunScores();
        StartCoroutine(RaiseAfterLoad(false));

        uiEvents.RaiseEnablePVELives();
        uiEvents.RaiseShowPVEScore();
        uiEvents.RaiseHideRedLives();
    }

    public void LoadDemoMode()
    {
        onDemoMode = true;
        AudioManager.Instance.StopSoundtrack(AudioManager.SoundType.MenuMusic);
        videoPlayer.Play();
    }

    public void LoadMenu()
    {
        onDemoMode = false;
        AudioManager.Instance.PlaySoundtrack(AudioManager.SoundType.MenuMusic);
        videoPlayer.Stop();
    }

    public void PlayerDied(PlayerData.PlayerID id, GameObject obj)
    {
        if (id == PlayerData.PlayerID.P1)
            player1Dead = true;
        else if (id == PlayerData.PlayerID.P2)
            player2Dead = true;

        obj.SetActive(false);
        StartCoroutine(CheckGameResultDelay());
    }

    public void CheckGameResult()
    {
        if (currentGameMode == GameMode.PVP)
        {
            if (player1Dead && player2Dead)
            {
                Draw();
            }
            else if (player1Dead)
            {
                Player2Wins();
            }
            else if (player2Dead)
            {
                Player1Wins();
            }
            StartCoroutine(ReturnToMainTitleDelay());
        }
        else if (currentGameMode == GameMode.PVE)
        {
            if (player1Dead)
            {
                GameOver();
                StartCoroutine(MoveToEnterYourNamePanelDelay());
            }
        }
    }

    public void Draw()
    {
        gameEvents.RaiseDraw();
        currentGameState = GameState.Draw;
    }

    public void Player1Wins()
    {
        gameEvents.RaiseP1Win();
        currentGameState = GameState.Player1Wins;
    }

    public void Player2Wins()
    {
        gameEvents.RaiseP2Win();
        currentGameState = GameState.Player2Wins;
    }

    public void GameOver()
    {
        if (onDemoMode)
        {
            LoadDemoMode();
            return;
        }

        gameEvents.RaiseGameOver();
        audioEvents.RaiseGameOver();

        currentGameState = GameState.GameOver;
    }

    public void ResetMatchState()
    {
        switch (currentGameMode)
        {
            case GameMode.PVP:
                player1Dead = false;
                player2Dead = false;
                break;
            case GameMode.PVE:
                player1Dead = false;
                break;
        }
    }

    public IEnumerator RaiseAfterLoad(bool isPVP)
    {
        UIManager.Instance.SetUIReady(false);

        SceneManager.LoadScene("Main");
        yield return null;

        if (isPVP)
            gameEvents.RaisePVPLoad();
        else
            gameEvents.RaisePVELoad();
    }

    public IEnumerator MoveToEnterYourNamePanelDelay()
    {
        yield return new WaitForSeconds(moveToEnterYourNameDelay);
        onEnterYourName = true;
        UIManager.Instance.MainRefs.gameOverPanel.SetActive(false);
        uiEvents.RaiseShowEnterYourNamePanel();
        uiEvents.RaiseFirstLetterStay();
    }

    public IEnumerator MoveToLeaderboardDelay()
    {
        yield return null;

        SceneManager.LoadScene("Leaderboard");
        currentGameState = GameState.Leaderboard;

        yield return null;
        uiEvents.RaiseLeaderboardScoresAndNamesUIChanged();

        yield return new WaitForSeconds(returnBackToMainTitleFromLeaderboard);

        SceneManager.LoadScene("Title");

        yield return null;
        CurrentLetterIndex = 0;
        CurrentCharIndex = 0;
        EnterLetterIndex = 0;

        for (int i = 0; i < 3; i++)
        {
            slotHasLetter[i] = false;
            slotWasConfirmed[i] = false;
        }

        uiEvents.RaiseHighScoreUIChanged();
        ResetMatchState();
        currentGameState = GameState.Title;
    }

    public IEnumerator ReturnToMainTitleDelay()
    {
        yield return new WaitForSeconds(returnBackToMainTitleDelay);
        SceneManager.LoadScene("Title");

        yield return null;
        uiEvents.RaiseHighScoreUIChanged();
        ResetMatchState();
        currentGameState = GameState.Title;
    }

    public IEnumerator CheckGameResultDelay()
    {
        yield return new WaitForSeconds(checkGameResultDelay);
        CheckGameResult();
    }
}
