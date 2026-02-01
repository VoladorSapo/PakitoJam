using Reflex.Attributes;
using UnityEngine;

public class tutorialArrow : MonoBehaviour
{

    [SerializeField] Transform[] ArrowPoints;
    int index;

    [Inject] GameEvents gameEvents;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        index = -1;
        gameEvents.OnRoundStarted += goNextPoint;
        gameEvents.FirstPlayerInRing += Hide;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Hide()

    {
        gameEvents.OnRoundStarted -= goNextPoint;
        gameEvents.FirstPlayerInRing -= Hide;
        Destroy(gameObject);
    }
   public void goNextPoint()
    {
        index = Mathf.Min(index+1, ArrowPoints.Length-1);
        transform.position = ArrowPoints[index].position;
        transform.eulerAngles = ArrowPoints[index].eulerAngles;
    }
}
