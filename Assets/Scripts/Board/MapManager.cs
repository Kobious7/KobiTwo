using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }
    [SerializeField] private List<Spot> allSpots = new List<Spot>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Auto load all Spot trên scene nếu chưa có
        if (allSpots == null || allSpots.Count == 0)
            allSpots = GetComponentsInChildren<Spot>().ToList();
    }

    public List<Spot> GetSpotsByType(SpotType type)
        => allSpots.Where(s => s.spotType == type).ToList();

    public Spot GetRandomSpawnSpot()
    {
        var spawns = GetSpotsByType(SpotType.Spawn);
        if (spawns.Count == 0) return null;
        return spawns[Random.Range(0, spawns.Count)];
    }
} 