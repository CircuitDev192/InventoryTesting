using UnityEngine;
using UnityEngine.UI;

public class StatDisplay : MonoBehaviour
{
    public Text NameText;
    public Text ValueText;

    private void OnValidate()
    {
        Text[] text = GetComponentsInChildren<Text>();
        NameText = text[0];
        ValueText = text[1];
    }
}
