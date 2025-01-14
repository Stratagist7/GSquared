using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
	[SerializeField] private GameObject pauseMenu;
	[SerializeField] private GameObject deathScreen;
	[SerializeField] private GameObject playerUI;
	[SerializeField] private StarterAssetsInputs inputs;
	[SerializeField] private Toggle toggle;
	
	public static bool Paused = false;
	public static bool ReloadAmmoType = false;

	private void Start()
	{
		UnlockCursor(false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ShowPauseMenu(!pauseMenu.activeSelf);
		}
	}
	
	private void ShowPauseMenu(bool argShow)
	{
		FreezePlayer(argShow);
		pauseMenu.SetActive(argShow);
	}

	public void ShowDeathScreen()
	{
		FreezePlayer(true);
		deathScreen.SetActive(true);
	}

	private void FreezePlayer(bool argShow)
	{
		Paused = argShow;
		playerUI.SetActive(!argShow);
		
		Time.timeScale = argShow ? 0f : 1f;
		AudioListener.pause = argShow;

		UnlockCursor(argShow);
	}

	private void UnlockCursor(bool argShowCursor)
	{
		Cursor.visible = argShowCursor;
		inputs.LookInput(Vector2.zero);  // Fixes camera spinning if the look input left as non-zero number
		inputs.SetCursorLocked(argShowCursor == false);
		inputs.cursorInputForLook = argShowCursor == false;
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

	public void MainMenuButton()
	{
		UnlockCursor(true);
		SceneManager.LoadScene("MainMenu");
	}

	public void SetReloadBool()
	{
		ReloadAmmoType = toggle.isOn;
	}
	
}
