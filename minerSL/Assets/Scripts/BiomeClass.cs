using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BiomeClass
{
    public string biomeName;
    public Color biomeColor;
    [Header("Noise Settings")]
    public float caveFreq = 0.05f;
    public float terrainFreq = 0.05f;
    public Texture2D caveNoiseTexture;

    [Header("Generation Settings")]
    public int dirtLayerHeight = 5;
    public bool generateCaves = true;
    public float surfaceValue = 0.25f;
    public float heightMultiplier = 4f;

    [Header("Ore Settings")]
    public OreClasses[] ores;

}
