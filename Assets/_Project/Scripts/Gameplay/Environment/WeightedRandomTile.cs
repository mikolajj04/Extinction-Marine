using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Weighted Random Tile", menuName = "Tiles/Weighted Random Tile")]
public class WeightedRandomTile : Tile
{
    [Serializable]
    public struct WeightedSprite
    {
        public Sprite Sprite;
        [Min(1)] public int Weight; 
    }

    [Header("Tile Configuration")]
    public List<WeightedSprite> Sprites;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

        if (Sprites == null || Sprites.Count == 0) return;

        int hash = position.x * 374761393 + position.y * 668265263;


        hash = (hash ^ (hash >> 13)) * 1274126177;
        int seed = hash ^ (hash >> 16);

        System.Random random = new System.Random(seed);

        int totalWeight = 0;
        foreach (var item in Sprites)
        {
            totalWeight += item.Weight;
        }

        int randomValue = random.Next(0, totalWeight);
        int currentWeight = 0;

        foreach (var item in Sprites)
        {
            currentWeight += item.Weight;
            if (randomValue < currentWeight)
            {
                tileData.sprite = item.Sprite;
                break;
            }
        }
    }
}