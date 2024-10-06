using UnityEngine;
using System.Collections.Generic;

public class TalkState : State
{
    Flemington toTalkTo;
    bool inPosition;

    public TalkState(Flemington flemington, Flemington toTalkTo) : base(flemington)
    {
        this.toTalkTo = toTalkTo;
        toTalkTo.Died += ToTalkToDied;
    }

    public override void Update(float deltaTime)
    {
        if (inPosition)
        {
            if (flemington.NeedToBehavior[NeedType.Social].Amount < 0.99f)
            {
                flemington.DoingBubble.ShowTalking();
                flemington.NeedToBehavior[NeedType.Social].ModifyAmount(deltaTime);

                if (flemington.AtPosition(toTalkTo.Position, 1.35f) == false)
                    inPosition = false;
            }
            else
            {
                toTalkTo.Died -= ToTalkToDied;
                ToNextState();
            }
        }
        // Are we at the site of work?
        else if (flemington.AtPosition(toTalkTo.Position, 0.35f) == false)
        {
            MoveState moveState = new MoveState(flemington, () => toTalkTo.Position, 1, this, this);
            flemington.StateMachine.SetState(moveState);
        }
        else
            inPosition = true;
    }

    void ToTalkToDied(Flemington died)
    {
        toTalkTo.Died -= ToTalkToDied;
        ToNextState();
    }

    public override string GetInspectText()
    {
        string str = $"Chatting with {toTalkTo}";

        return str;
    }
}
