using UnityEngine;

public enum EquipmentType
{
    Helmet,
    Chestplate,
    Gloves,
    Boots,
    Weapon1,
    Weapon2,
    Acceessory1,
    Acceessory2,
}

[CreateAssetMenu]
public class EquippableItem : Item
{
    public int StrengthBonus;
    public int AgilityBonus;
    public int IntelligenceBonus;
    public int VitalityBonus;
    [Space]
    public float StrengthPercentBonus;
    public float AgilityPercentBonus;
    public float IntelligencePercentBonus;
    public float VitalityPercentBonus;
    [Space]
    public EquipmentType EquipmentType;
}
