using UnityEngine;


[CreateAssetMenu(fileName = "New Need", menuName = "Need")]
public class Need : ScriptableObject
{
    [SerializeField] float timeToZero;
    [SerializeField] Color barColor;
    [SerializeField] Sprite icon;

    public float TimeToZero => timeToZero;
    public Color BarColor => barColor;
    public Sprite Icon => icon;
}