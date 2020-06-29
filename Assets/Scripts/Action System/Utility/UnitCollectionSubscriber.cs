using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitCollectionSubscriber : MonoBehaviour
{
    public UnitsCollection unitsCollection;
    private Unit unit;


    private void OnEnable()
    {
        if (unit == null) unit = GetComponent<Unit>();
        unitsCollection.Add(unit);  
    }

    private void OnDisable()
    {
        unitsCollection.Remove(unit);
    }
}
