using System.Text;
using UnityEngine;
using UnityEngine.UI;
using CircuitDev.CharacterStats;

public class StatTooltip : MonoBehaviour
{
    [SerializeField] private Text StatNameText;
    [SerializeField] private Text StatModifiersLabelText;
    [SerializeField] private Text StatModifiersText;

    private StringBuilder sb = new StringBuilder();

    public void ShowToolTip(CharacterStat stat, string statName)
    {
        StatNameText.text = GetStatTopText(stat, statName);

        StatModifiersText.text = GetStatModifersText(stat);

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }

    private string GetStatTopText(CharacterStat stat, string statName)
    {
        sb.Length = 0;
        sb.Append(statName);
        sb.Append(" ");
        sb.Append(stat.Value);

        if (stat.Value != stat.BaseValue)
        {
            sb.Append(" (");
            sb.Append(stat.BaseValue);

            if (stat.Value > stat.BaseValue)
                sb.Append("+");

            sb.Append(System.Math.Round(stat.Value - stat.BaseValue, 4));
            sb.Append(")");
        }

        return sb.ToString();
    }

    private string GetStatModifersText(CharacterStat stat)
    {
        sb.Length = 0;

        foreach (StatModifier modifier in stat.StatModifiers)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }

            if (modifier.Value > 0)
                sb.Append("+");

            if (modifier.Type == StatModType.Flat)
            {
                sb.Append(modifier.Value);
            }
            else
            {
                sb.Append(modifier.Value * 100);
                sb.Append("%");
            }

            EquippableItem item = modifier.Source as EquippableItem;

            if (item != null)
            {
                sb.Append(" ");
                sb.Append(item.ItemName);
            }
            else
            {
                Debug.LogError("Modifier is not an equippable item!");
            }
        }

        return sb.ToString();
    }
}
