using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleState
{
    void EnterState(BattleStateManager manager);
    void UpdateState(BattleStateManager manager);
    void ExitState(BattleStateManager manager);
}
public class StartState : IBattleState
{
    public void EnterState(BattleStateManager manager) { /* �ʱ�ȭ ���� */ }
    public void UpdateState(BattleStateManager manager) { /* ������Ʈ ���� */ }
    public void ExitState(BattleStateManager manager) { /* û�� ���� */ }
}

public class EnemyTurnState : IBattleState
{
    public void EnterState(BattleStateManager manager) { /* �� �� ���� ���� */ }
    public void UpdateState(BattleStateManager manager) { /* �� �� ������Ʈ ���� */ }
    public void ExitState(BattleStateManager manager) { /* �� �� ���� ���� */ }
}

public class PlayerTurnState : IBattleState
{
    public void EnterState(BattleStateManager manager) { /* �÷��̾� �� ���� ���� */ }
    public void UpdateState(BattleStateManager manager) { /* �÷��̾� �� ������Ʈ ���� */ }
    public void ExitState(BattleStateManager manager) { /* �÷��̾� �� ���� ���� */ }
}

public class WonState : IBattleState
{
    public void EnterState(BattleStateManager manager) { /* �¸� ���� */ }
    public void UpdateState(BattleStateManager manager) { /* �¸� ���� ������Ʈ ���� */ }
    public void ExitState(BattleStateManager manager) { /* �¸� ���� ���� ���� */ }
}

public class LostState : IBattleState
{
    public void EnterState(BattleStateManager manager) { /* �й� ���� */ }
    public void UpdateState(BattleStateManager manager) { /* �й� ���� ������Ʈ ���� */ }
    public void ExitState(BattleStateManager manager) { /* �й� ���� ���� ���� */ }
}

public class BattleStateManager : MonoBehaviour
{
    private IBattleState currentState;

    void Start()
    {
        SetState(new StartState());
    }

    void Update()
    {
        if (currentState != null)
            currentState.UpdateState(this);
    }

    public void SetState(IBattleState newState)
    {
        if (currentState != null)
            currentState.ExitState(this);

        currentState = newState;

        if (currentState != null)
            currentState.EnterState(this);
    }
}