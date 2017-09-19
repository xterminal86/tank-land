using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour 
{
  public GameObject ObjectsHolder;

  public GameObject TerrainTile;

  public TankPlayer Player;

  void Awake()
  {
    for (int x = 0; x < GlobalConstants.MapSize; x++)
    {
      for (int y = 0; y < GlobalConstants.MapSize; y++)
      {
        Instantiate(TerrainTile, new Vector3(x, y, 0.0f), Quaternion.identity, ObjectsHolder.transform);
      }
    }

    Player.SetPlayerPosition(new Vector3(GlobalConstants.MapSize / 2.0f, GlobalConstants.MapSize / 2.0f, 0.0f));
  }
}
