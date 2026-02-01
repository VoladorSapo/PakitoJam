using Reflex.Attributes;
using UnityEngine;

public class PlayerSlot : MonoBehaviour
{
    [Inject] GameEvents gameEvents;
    public Transform SpawnSpot;
    [SerializeField] private CountdownTimer goldTimer;
    
    BasePlayerCharacter attachedPlayer;
    public bool HasPlayer => attachedPlayer != null;
    public void AssignPlayer(BasePlayerCharacter player)
    {
        attachedPlayer = player;
    }
    public void LeavePlayer(BasePlayerCharacter player)
    {
        if(attachedPlayer != player) return;
        
        attachedPlayer = null;
        goldTimer.ResetCountdown();
    }

    void Awake()
    {
        goldTimer.ResetCountdown();
    }
    void Update()
    {
        if (goldTimer.Decrement(Time.deltaTime) && HasPlayer)
        {
            gameEvents.NotifyCoinCollection(1);
            attachedPlayer.PlayParticleEffect(3);
        }
    }
}