using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
	[SerializeField] private GameObject pauseMenu;
	[SerializeField] private GameObject playerUI;
	[SerializeField] private StarterAssetsInputs inputs;
	[SerializeField] private Toggle toggle;
	
	public static bool Paused = false;
	public static bool ReloadAmmoType = false;

	// Update is called once per frame
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ShowPauseMenu(!pauseMenu.activeSelf);
		}
	}

	private void ShowPauseMenu(bool argShow)
	{
		Paused = argShow;
		pauseMenu.SetActive(argShow);
		playerUI.SetActive(!argShow);
		
		Time.timeScale = argShow ? 0f : 1f;
		AudioListener.pause = argShow;
		
		Cursor.visible = argShow;
		inputs.LookInput(Vector2.zero);  // Fixes camera spinning if the look input left as non-zero number
		inputs.SetCursorLocked(argShow == false);
		inputs.cursorInputForLook = argShow == false;
	}
	
	public void ResumeGame()
	{
		ShowPauseMenu(false);
	}
	
	public void ResetScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		ShowPauseMenu(false);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void SetReloadBool()
	{
		ReloadAmmoType = toggle.isOn;
	}
	
}
