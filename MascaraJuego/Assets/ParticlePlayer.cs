using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticlePlayer : MonoBehaviour
{
   [SerializeField] ParticleSystem[] particles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayParticle(int particle)
    {
        particles[particle].Play();
    }
}
