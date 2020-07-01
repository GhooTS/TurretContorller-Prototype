using UnityEngine;

public class Selectable : MonoBehaviour
{
    public GameObject root;
    private void Start()
    {
        if(root == null)
        {
            root = gameObject;
        }
    }
}
