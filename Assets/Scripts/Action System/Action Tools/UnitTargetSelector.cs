using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UnitTargetSelector : MonoBehaviour
{
    public List<Unit> units;
    public Unit CurrentTarget { get { return units[currentTargetIndex]; } }

    private int currentTargetIndex = -1;
    private Unit actionSource;
    private ActionController actionController;
    private Action action;
    private bool isActive = false;


    public void Awake()
    {
        FindAllUnits();
    }

    //TEMPORARY SOLUTION-----------------
    public void FindAllUnits()
    {
        units = FindObjectsOfType<Unit>().ToList();
    }
    //------------------------------------

    public void Applay()
    {
        actionSource.EnqueueAction(new QueuedAction
        {
            ActionController = actionController,
            Action = action,
            ActionTarget = new ActionTarget { target = CurrentTarget }
        });
        Deactivate();
    }

    public void Activate(Unit actionSource,ActionController actionController,Action action)
    {
        this.actionSource = actionSource;
        this.actionController = actionController;
        this.action = action;
        isActive = true;
    }

    private void Deactivate()
    {
        this.actionSource = null;
        this.actionController = null;
        this.action = null;
        isActive = false;
    }

    private void SwitchTarget()
    {
        //loop around from current target to next, if exist
        for (int i = currentTargetIndex + 1; i != currentTargetIndex; i++)
        {

            if (i == units.Count)
            {
                if (currentTargetIndex == -1) break;
                else i = 0;
            }

            if (ValidTarget(i))
            {
                currentTargetIndex = i;
                break;
            }
        }
    }

    private bool ValidTarget(int index)
    {
        if(units[index] == actionSource && action.targetKind.HasFlag(Action.TargetKind.Self))
        {
            return true;
        }

        //select ally
        if (units[index].controlBy == actionSource.controlBy && action.targetKind.HasFlag(Action.TargetKind.Ally))
        {
            return (units[index].transform.position - actionSource.transform.position).magnitude < action.range;
        }

        //select enemy
        if (units[index].controlBy != actionSource.controlBy && action.targetKind.HasFlag(Action.TargetKind.Enemy))
        {
            return (units[index].transform.position - actionSource.transform.position).magnitude < action.range;
        }

        return false;
    }

}
