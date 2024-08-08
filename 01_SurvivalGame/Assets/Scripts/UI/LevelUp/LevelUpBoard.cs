using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpBoard : MonoBehaviour
{
    LevelUpBonus currentBonus;

    public LevelUpBonus Bonus
    {
        get => currentBonus;
        set
        {
            currentBonus = value;
            currentBonus.isSelected = true;

            image.sprite = currentBonus.sprite;
            textMesh.text = $"{currentBonus.stat.ToString()} + {currentBonus.value}";
        }
    }

    Image image;

    TextMeshProUGUI textMesh;

    Button button;

    private void Awake()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(ButtonClicked);
    }

    // 버튼이 클릭 되면 
    void ButtonClicked()
    {
        LevelUpSlots.Instance.RemoveSlot(Bonus);
    }
}
