using UnityEngine;

public class Gizmo : MonoBehaviour
{
  [SerializeField]  float gizmoSize;
    [SerializeField] Color gizmoColor = Color.blue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoSize);
    }
}
