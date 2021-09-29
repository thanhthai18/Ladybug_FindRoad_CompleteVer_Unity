using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint_LadybugMinigame : MonoBehaviour
{
    [SerializeField] private Waypoint_LadybugMinigame nextWaypoint;
    [SerializeField] private bool isEndWaypoint;


    public Waypoint_LadybugMinigame NextWaypoint { get => nextWaypoint; set => nextWaypoint = value; }
    public bool IsEndWaypoint { get => isEndWaypoint; set => isEndWaypoint = value; }


}
