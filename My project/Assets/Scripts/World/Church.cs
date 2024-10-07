using UnityEngine;

public class Church : Building
{
    protected override void OnDestroy()
    {
        base.OnDestroy();
        // Game over
        Debug.Log("Game over!");
    }

    public override string Name => "Church";
    public override string Content => "Protect at all costs!";
}