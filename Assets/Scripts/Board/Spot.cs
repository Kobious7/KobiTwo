using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    public SpotType spotType;

    [Tooltip("Các Spot tiếp theo (chiều xuôi)")]
    [SerializeField] private List<Spot> nextSpots = new List<Spot>();

    [Tooltip("Các Spot chiều ngược (chỉ dùng cho Spot cam, xanh dương đậm)")]
    [SerializeField] private List<Spot> prevSpots = new List<Spot>();

    public List<Spot> GetNextSpots(bool forward = true)
    {
        return forward ? nextSpots : prevSpots;
    }
} 