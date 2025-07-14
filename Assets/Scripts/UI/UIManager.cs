using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text diceResultText;
    [SerializeField] private Button rollButton;
    [SerializeField] private GameObject directionPanel; // Panel chứa các nút chọn hướng
    [SerializeField] private Button directionButtonPrefab; // Prefab nút chọn hướng

    private List<Button> directionButtons = new List<Button>();

    public Button RollButton => rollButton;

    private void Awake()
    {
        if (diceResultText == null)
            diceResultText = FindObjectOfType<TMP_Text>();
        if (rollButton == null)
            rollButton = FindObjectOfType<Button>();
    }

    public void SetDiceResult(int value)
    {
        diceResultText.text = $"Dice: {value}";
    }

    public void SetRollButtonInteractable(bool interactable)
    {
        rollButton.interactable = interactable;
    }

    public void ShowDirectionChoices(List<Spot> choices, Action<Spot> onChoose)
    {
        // Xóa nút cũ
        foreach (var btn in directionButtons)
            Destroy(btn.gameObject);
        directionButtons.Clear();

        directionPanel.SetActive(true);

        for (int i = 0; i < choices.Count; i++)
        {
            var spot = choices[i];
            var btn = Instantiate(directionButtonPrefab, directionPanel.transform);
            btn.GetComponentInChildren<TMP_Text>().text = $"Đi tới {spot.name}";
            btn.onClick.AddListener(() => {
                directionPanel.SetActive(false);
                onChoose?.Invoke(spot);
            });
            directionButtons.Add(btn);
        }
    }
} 