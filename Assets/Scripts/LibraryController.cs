using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryController : MonoBehaviour
{
    public ProjectileController Projectile;
    public ParticleGnome Blood;
    public ParticleGnome Dust;
    public SpawnableController TestSpawn;
    
    void Awake()
    {
        God.Library = this;
    }
}
