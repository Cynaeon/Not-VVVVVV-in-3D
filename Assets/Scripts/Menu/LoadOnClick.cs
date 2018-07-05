using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour {

    public int levelIndex;

	public void LoadLevel()
    {
        SceneManager.LoadScene(levelIndex);
    }
}
