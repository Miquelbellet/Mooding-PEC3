using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditMenuController : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void EditLevel()
    {
        SceneManager.LoadScene("EditLevel");
    }

    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }
}
