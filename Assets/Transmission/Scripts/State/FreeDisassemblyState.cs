using UnityEngine;

public class FreeDisassemblyState : ASState
{
    [SerializeField]
    private string stateName;
    public override void Enter(ASState from)
    {
        this.gameObject.SetActive(true);
    }

    public override void Exit(ASState to)
    {
        this.gameObject.SetActive(false);
    }

    public override string GetName()
    {
        return stateName;
    }

    public void GotoOperatingPrincipleState()
    {
        stateManager.SwitchState("OperatingPrinciple");
    }

    public void GotoHomePageState()
    {
        stateManager.SwitchState("HomePage");
    }

    public void GotoShowAnimationState()
    {
        stateManager.SwitchState("ShowAnimation");
    }
}
