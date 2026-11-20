using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float restartTime;

    private IEnumerator RestartDeadRoutine()
    {
        yield return new WaitForSeconds(restartTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
    
    public void RestartGameAfterDead()
    {
        StartCoroutine(RestartDeadRoutine());
    }
}
