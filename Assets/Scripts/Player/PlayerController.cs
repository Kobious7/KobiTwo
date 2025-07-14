using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Spot currentSpot;
    [SerializeField] private float moveSpeed = 5f;

    public Spot CurrentSpot => currentSpot;
    public float MoveSpeed => moveSpeed;

    private int stepsLeft;
    private Action onMoveComplete;
    private Spot previousSpot = null;
    private bool isForward = true;

    public void PlaceAtSpot(Spot spot)
    {
        currentSpot = spot;
        transform.position = spot.transform.position;
    }

    public void MoveSteps(int steps, Action onComplete = null)
    {
        stepsLeft = steps;
        onMoveComplete = onComplete;
        previousSpot = null;
        isForward = true;
        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        while (stepsLeft > 0)
        {
            var nextSpots = currentSpot.GetNextSpots(isForward);

            // Nếu là DirectionChoice hoặc TwoWayDirectionChoice, loại bỏ previousSpot khỏi danh sách chọn
            if ((currentSpot.spotType == SpotType.DirectionChoice || currentSpot.spotType == SpotType.TwoWayDirectionChoice) && previousSpot != null)
                nextSpots = nextSpots.FindAll(s => s != previousSpot);

            if (nextSpots.Count == 0) break;

            Spot next = null;
            if (nextSpots.Count == 1)
            {
                next = nextSpots[0];
            }
            else
            {
                bool waiting = true;
                UIManager ui = FindObjectOfType<UIManager>();
                ui.ShowDirectionChoices(nextSpots, spot => {
                    next = spot;
                    waiting = false;
                });
                while (waiting) yield return null;
            }

            // Nếu đang đi xuôi, next là TwoWayDirectionChoice => chuyển sang đi ngược
            bool shouldSwitchToBackward = isForward && next.spotType == SpotType.TwoWayDirectionChoice;
            // Nếu đang đi ngược, next là Branch => sau khi move tới Branch thì set lại isForward = true
            bool shouldResetForwardAfterMove = !isForward && next.spotType == SpotType.Branch;

            yield return StartCoroutine(MoveToSpot(next));

            if (shouldSwitchToBackward)
                isForward = false;
            if (shouldResetForwardAfterMove)
                isForward = true;

            previousSpot = currentSpot;
            currentSpot = next;
            stepsLeft--;

            if (currentSpot.spotType == SpotType.DeadEnd || currentSpot.spotType == SpotType.Goal)
                break;
        }
        onMoveComplete?.Invoke();
    }

    private IEnumerator MoveToSpot(Spot target)
    {
        while (Vector3.Distance(transform.position, target.transform.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target.transform.position;
    }
} 