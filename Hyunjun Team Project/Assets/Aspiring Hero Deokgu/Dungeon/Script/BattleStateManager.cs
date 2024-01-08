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
    public void EnterState(BattleStateManager manager) { /* 초기화 로직 */ }
    public void UpdateState(BattleStateManager manager) { /* 업데이트 로직 */ }
    public void ExitState(BattleStateManager manager) { /* 청소 로직 */ }
}

public class EnemyTurnState : IBattleState
{
    public void EnterState(BattleStateManager manager) { /* 적 턴 시작 로직 */ }
    public void UpdateState(BattleStateManager manager) { /* 적 턴 업데이트 로직 */ }
    public void ExitState(BattleStateManager manager) { /* 적 턴 종료 로직 */ }
}

public class PlayerTurnState : IBattleState
{
    public void EnterState(BattleStateManager manager) { /* 플레이어 턴 시작 로직 */ }
    public void UpdateState(BattleStateManager manager) { /* 플레이어 턴 업데이트 로직 */ }
    public void ExitState(BattleStateManager manager) { /* 플레이어 턴 종료 로직 */ }
}

public class WonState : IBattleState
{
    public void EnterState(BattleStateManager manager) { /* 승리 로직 */ }
    public void UpdateState(BattleStateManager manager) { /* 승리 상태 업데이트 로직 */ }
    public void ExitState(BattleStateManager manager) { /* 승리 상태 종료 로직 */ }
}

public class LostState : IBattleState
{
    public void EnterState(BattleStateManager manager) { /* 패배 로직 */ }
    public void UpdateState(BattleStateManager manager) { /* 패배 상태 업데이트 로직 */ }
    public void ExitState(BattleStateManager manager) { /* 패배 상태 종료 로직 */ }
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