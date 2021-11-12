using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tutorialPage : MonoBehaviour
{
    public void next(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void back(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void main(string scene){
        Application.LoadLevel(scene);
    }
}
