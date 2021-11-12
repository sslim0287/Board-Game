using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnTutorial : MonoBehaviour
{
    public void startScene(string scene){
        Application.LoadLevel(scene);
    }
}