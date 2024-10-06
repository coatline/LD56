using UnityEngine;


[CreateAssetMenu(fileName = "New Need", menuName = "Need")]
public class Need : ScriptableObject
{
    [SerializeField] float timeToZero;
    [SerializeField] Color barColor;
    [SerializeField] Sprite icon;
    [SerializeField] NeedType type;

    public float TimeToZero => timeToZero;
    public Color BarColor => barColor;
    public NeedType Type => type;
    public Sprite Icon => icon;
}


public enum NeedType
{
    Food,
    Sleep,
    Social
}