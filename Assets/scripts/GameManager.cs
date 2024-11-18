using UnityEngine;

public class GameManager : MonoBehaviour
{
    // -----------------------------------------------------------------------------------------------------------------
    // P R O P E R T I E S
    // -----------------------------------------------------------------------------------------------------------------
    
    [SerializeField] private BattleManager battleManagerPrefab;
    private BattleManager _battleManagerInstance;
    
    [SerializeField] private GameOverMenu gameOverCanvasPrefab;
    private GameOverMenu _gameOverCanvasInstance;

    private Timer _timer;
    public bool victory;
    
    // -----------------------------------------------------------------------------------------------------------------
    // H E R I T E D   M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------

    void Start()
    {
        InitializeBattleManager();
        InitializeGameOverCanvas();
    }
    
    void Update()
    {
        switch (_battleManagerInstance.state)
        {
            case GameState.Playing:
                break;
            case GameState.Lost:
                victory = false;
                _gameOverCanvasInstance.ShowGameOver(victory);
                break;
            case GameState.Win:
                victory = true;
                _gameOverCanvasInstance.ShowGameOver(victory);
                break;
        }
    }


    // -----------------------------------------------------------------------------------------------------------------
    // M E T H O D S
    // -----------------------------------------------------------------------------------------------------------------

    private void InitializeBattleManager()
    {
        _battleManagerInstance = Instantiate(battleManagerPrefab, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
    }
    
    private void InitializeGameOverCanvas()
    {
        _gameOverCanvasInstance = Instantiate(gameOverCanvasPrefab, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
    }
}