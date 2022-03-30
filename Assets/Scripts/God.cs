using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class God
{
    public static Color GetColor(MColors color)
    {
        Color c = Color.white;
        switch (color)
        {
            case MColors.Player:
            {
                
                c = Color.cyan; 
                break;
            }
            case MColors.Red: c = Color.red; break;
            case MColors.Green: c = Color.green; break;
            case MColors.Yellow: c = Color.yellow; break;
            case MColors.Pink: c = Color.magenta; break;
            case MColors.Orange: c = new Color(1,0.5f,0); break;
            case MColors.Blue: c = Color.blue; break;
            case MColors.Purple: c = new Color(0.5f,0,1); break;
            case MColors.White: c = new Color(0.8f,0.8f,0.8f); break;
            case MColors.Ebony: c = new Color(0.1f,0.1f,0.1f); break;
            case MColors.Tan: c = new Color(0.8f,0.7f,0.6f); break;
            case MColors.Algea: c = new Color(0.1f,0.66f,0.56f); break;
            case MColors.Algae: c = new Color(0.1f,0.66f,0.56f); break;
            case MColors.Slate: c = new Color(0.4f,0.4f,0.4f); break;
            case MColors.WallWhite: c = Color.white; break;
            case MColors.BG: c = new Color(0.2993058f,0.5377358f,0.5229081f); break;
        }

        return c;
    }
}

public enum MColors
{
    None=0,
    Player=1,
    Red=2,
    Green=3,
    Yellow=4,
    Pink=5,
    Orange=6,
    Blue=7,
    Purple=8,
    White=9,
    Ebony=10,
    Tan=11,
    Algea=12,
    Slate=13,
    WallWhite=14,
    BG=15,
    Algae=16,
    
}