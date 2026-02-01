using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticlePlayer : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particles;
    public void PlayParticle(int particle)
    {
        if(particle >= 0 && particle < particles.Length) particles[particle]?.Play();
    }
}
