using UnityEngine;
using UnityEngine.UI;

public class ActionButtonsActivator : MonoBehaviour
{
    public ActionButton prefab;
    public GridLayoutGroup actionbuttonsLayout;
    public int maxActionButtons = 10;
    private ActionButton[] actionButtons;
    private Selection selection;


    private void Awake()
    {
        selection = FindObjectOfType<Selection>();
    }

    private void Start()
    {
        actionButtons = new ActionButton[maxActionButtons];

        for (int i = 0; i < actionButtons.Length; i++)
        {
            actionButtons[i] = Instantiate(prefab, actionbuttonsLayout.transform);
            actionButtons[i].gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        selection.OnSelected.AddListener(ShowButtons);   
        selection.OnDiselected.AddListener(HideButtons);   
    }

    private void OnDisable()
    {
        selection.OnSelected.RemoveListener(ShowButtons);
        selection.OnDiselected.RemoveListener(HideButtons);
    }

    private void ShowButtons()
    {
        if(selection.HasSelection() && selection.selected.TryGetComponent(out Unit unit) && selection.selected.TryGetComponent(out ActionManager manager))
        {
            for (int i = 0; i < manager.actionContainers.Length; i++)
            {
                actionButtons[i].Activate(unit, manager.actionContainers[i]);
            }
        }
    }

    private void HideButtons()
    {
        for (int i = 0; i < actionButtons.Length; i++)
        {
            actionButtons[i].gameObject.SetActive(false);
        }
    }
}
