using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryController : MonoBehaviour
{
    public ProjectileController Projectile;
    public ParticleGnome Blood;
    public ParticleGnome Dust;
    public ExplosionController Explosion;
    public SpawnableController TestSpawn;
    public WeaponController WeaponSpawn;
    public RoundManager RM;
    public List<ColorPair> ColorSeeds;
    public Dictionary<IColors, Material> ColorDict = new Dictionary<IColors, Material>();

    void Awake()
    {
        God.Library = this;
        foreach(ColorPair cp in ColorSeeds)
            ColorDict.Add(cp.A,cp.B);
    }

    public Material GetColor(IColors c)
    {
        if(!ColorDict.ContainsKey(c)) return new Material("TEMP MAT");
        return ColorDict[c];
    }
}

[System.Serializable]
public class ColorPair
{
    public IColors A;
    public Material B;
}


public enum ItemTypes
{
    None,
    Points,
    Healing,
    Jump,
    Teleport,
}

public enum WeaponTypes
{
    None,
    Projectile,
    Hitscan,
    Grenade,
}

public enum IColors
{
    None,
    Black,
    Red,
    White,
    Yellow,
    Green,
    Glass,
    Blue,
    Pink,
    Purple,
    Orange,
    Gray,
    Slate
}

public enum GameModes
{
    None,
    Deathmatch,
    Farming,
    Elim
}