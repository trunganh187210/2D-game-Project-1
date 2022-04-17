using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public int SceneToLoad;

    public Vector2 PlayerPosition;

    public VectorValue playerStorage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerStorage.initialValue = PlayerPosition;
            SceneManager.LoadScene(SceneToLoad);
        }
    }
}
