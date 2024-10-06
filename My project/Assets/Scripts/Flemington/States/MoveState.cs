using UnityEngine;


public class MoveState : State
{
    public readonly System.Func<Vector2> GetDestination;
    readonly float speedPercentage;

    public MoveState(Flemington flemington, System.Func<Vector2> getDestination, float speedPercentage = 1, State nextState = null, State rootState = null) : base(flemington, nextState, rootState)
    {
        this.GetDestination = getDestination;
        this.speedPercentage = speedPercentage;
    }

    public override void Update(float deltaTime)
    {
        Vector2 destination = GetDestination.Invoke();

        // Update path visual
        flemington.Destination = destination;

        FixedUpdate(destination);

        if (flemington.AtPosition(destination, 0.05f))
            flemington.StateMachine.SetState(NextState);
    }

    private void FixedUpdate(Vector2 destination)
    {
        Move(destination);
    }

    void Move(Vector2 destination)
    {
        Vector2 dist = (destination - flemington.Position);
        Vector2 movement = dist.normalized * Time.fixedDeltaTime * flemington.Stats.speed * speedPercentage;

        if (Mathf.Abs(flemington.Position.x - destination.x) < 1f)
            flemington.SetYVelocity(movement.y);

        flemington.SetXVelocity(movement.x);
    }

    public override void Enter()
    {
        flemington.Traveling = true;
    }

    public override void Exit()
    {
        flemington.SetXVelocity(0f);
        flemington.SetYVelocity(0f);
        flemington.Traveling = false;
    }

    public override string GetInspectText()
    {
        string str = $"Moving to {GetDestination.Invoke()}";
        return str;
    }
}