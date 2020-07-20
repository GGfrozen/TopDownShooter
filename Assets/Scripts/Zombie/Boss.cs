using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Zombie zombie;
    SceneLoader sceneLoader;

    private void Start()
    {
        zombie = GetComponent<Zombie>();
        sceneLoader = FindObjectOfType<SceneLoader>();
    }
    private void Update()
    {
        if( zombie.SetAlive() == false)
        {
            sceneLoader.LoadNextLevel();
        }
    }
}
