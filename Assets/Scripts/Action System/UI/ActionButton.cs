using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    public Button button;
    public Image icon;

    private ActionContainer actionContainer;

    private Unit unit;
    private LocationTargetSelector locationTargetSelector;
    private UnitTargetSelector unitTargetSelector;

    private void Awake()
    {
        locationTargetSelector = FindObjectOfType<LocationTargetSelector>();
        unitTargetSelector = FindObjectOfType<UnitTargetSelector>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(ActiveTargetSelection);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(ActiveTargetSelection);
        actionContainer = null;
    }


    public void Activate(Unit unit,ActionContainer actionContainer)
    {
        this.unit = unit;
        this.actionContainer = actionContainer;
        button.interactable = unit.CanPerformeAction(this.actionContainer.action);
        icon.sprite = this.actionContainer.action.icon;
        gameObject.SetActive(true);
    }

    private void ActiveTargetSelection()
    {
        if(unit != null && actionContainer != null)
        {
            switch (actionContainer.action.targetType)
            {
                case Action.TargetType.Unit:
                    break;
                case Action.TargetType.Location:
                    locationTargetSelector.Activate(unit, actionContainer);
                    break;
                default:
                    break;
            }
        }
    }
}