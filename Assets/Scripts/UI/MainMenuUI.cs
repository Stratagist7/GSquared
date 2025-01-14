using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
	public void StartButton()
	{
		SceneManager.LoadScene("MainLevel");
	}

	public void PracticeButton()
	{
		SceneManager.LoadScene("Testing");
	}
	
	public void QuitButton()
	{
		Application.Quit();
	}
}
