﻿using System.Collections.Generic;
using UnityEngine;

public class TilesIndicator 
{
    GameObject[] _views;

    public TilesIndicator(List<Tile> inTiles)
    {
        _views = new GameObject[inTiles.Count];

        for (int i = 0; i < inTiles.Count; i++)
        {
            GameObject tileView = GameObject.Instantiate(Resources.Load<GameObject>("Prefab_WalkableTile"));

            tileView.transform.position = new Vector3(inTiles[i].worldPosition.x + 0.5f, 1, inTiles[i].worldPosition.y + 0.5f);

            _views[i] = tileView;
        }
    }

    public virtual void Destroy()
    {
        foreach (GameObject view in _views)
            GameObject.Destroy(view);
    }
}