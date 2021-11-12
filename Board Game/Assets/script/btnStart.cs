using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnStart : MonoBehaviour
{
    public static int player1StartingPoint = 0;
    public static int player2StartingPoint = 0;
    public Transform[] points;
    [HideInInspector]
    public int pointsIndex = 0;

    public void startScene(string scene){
        Application.LoadLevel(scene);
        transform.position = points[pointsIndex].transform.position;
    }
    
}
