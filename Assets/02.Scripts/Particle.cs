using UnityEngine;
public enum ParticleType
{
    Explosion,
    Item,
}
public class Particle : MonoBehaviour
{
    public ParticleType Type;
}
