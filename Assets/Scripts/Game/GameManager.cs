using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerPrefab;
    [SerializeField] private DiceRoller diceRoller;
    [SerializeField] private UIManager uiManager;

    private PlayerController player;

    private void Start()
    {
        // Auto ref
        if (diceRoller == null) diceRoller = FindObjectOfType<DiceRoller>();
        if (uiManager == null) uiManager = FindObjectOfType<UIManager>();

        // Spawn player ở 1 Spot Spawn bất kỳ
        Spot spawnSpot = MapManager.Instance.GetRandomSpawnSpot();
        player = Instantiate(playerPrefab);
        player.PlaceAtSpot(spawnSpot);

        uiManager.RollButton.onClick.AddListener(OnRollDice);
    }

    private void OnRollDice()
    {
        int dice = diceRoller.Roll();
        uiManager.SetDiceResult(dice);
        uiManager.SetRollButtonInteractable(false);

        player.MoveSteps(dice, EnableRollButton);
    }

    private void EnableRollButton()
    {
        uiManager.SetRollButtonInteractable(true);
    }
} 