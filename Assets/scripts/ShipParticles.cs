using UnityEngine;

public class ShipParticles : MonoBehaviour
{
    public ParticleSystem thrusterParticles; // Reference to the ParticleSystem
    public KeyCode thrusterKey = KeyCode.Space; // The key used to activate the thruster

    void Start()
    {
        // Disable Play On Awake for the particle system
        var mainModule = thrusterParticles.main;
        mainModule.playOnAwake = false;

        // Ensure the particle system is stopped at the start
        thrusterParticles.Stop();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(thrusterKey))
        {
            // Start emitting particles when the key is pressed
            thrusterParticles.Play();
        }
        else if (Input.GetKeyUp(thrusterKey))
        {
            // Stop emitting particles when the key is released
            thrusterParticles.Stop();
        }
    }
}
