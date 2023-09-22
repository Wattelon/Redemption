using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private float lightTime;
    [SerializeField] private string itemName;

    public string ItemName => itemName;

    private void Emit()
    {
        
    }
}
