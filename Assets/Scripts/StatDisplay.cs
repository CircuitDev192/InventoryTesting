using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CircuitDev.CharacterStats;

public class StatDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CharacterStat _stat;
    public CharacterStat Stat 
    {
        get { return _stat; }
        set
        {
            _stat = value;
            UpdateStatValue();
        }
    }

    private string _name;
    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            nameText.text = _name.ToLower();
        }
    }

    [SerializeField] private Text nameText;
    [SerializeField] private Text valueText;
    [SerializeField] private StatTooltip tooltip;

    private void OnValidate()
    {
        Text[] text = GetComponentsInChildren<Text>();
        nameText = text[0];
        valueText = text[1];

        if (tooltip == null)
            tooltip = FindObjectOfType<StatTooltip>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowToolTip(Stat, Name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideToolTip();
    }

    public void UpdateStatValue()
    {
        valueText.text = _stat.Value.ToString();
    }
}
