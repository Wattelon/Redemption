using UnityEngine;

public class Pillar : MonoBehaviour
{
    [SerializeField] private Vector3 targetScale;
    [SerializeField] private Vector3 rotation;

    private bool _isWaitingResize;

    private void OnTriggerEnter(Collider other)
    {
        var thisTransform = transform;
        if (other.transform == thisTransform.parent)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            thisTransform.localPosition = Vector3.zero;
            thisTransform.localRotation = Quaternion.Euler(rotation);
            _isWaitingResize = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isWaitingResize && transform.localScale == targetScale)
        {
            FindObjectOfType<Basement>().PlacePillar();
            gameObject.layer = 0;
            Destroy(this);
        }
    }
}