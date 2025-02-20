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
	[SerializeField] private GameObject caughtScreen;

	public static MenuUI instance;
	public static bool Paused = false;
	public static bool ReloadAmmoType = false;

	private void Awake()
	{
		if (instance == null) {
			instance = this;
		}  else
			Destroy(gameObject);
	}
	
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

	public void ShowCaughtScreen()
	{
		FreezePlayer(true);
		caughtScreen.SetActive(true);
	}

	public void FreezePlayer(bool argShow)
	{
		playerUI.SetActive(!argShow);
		
		Time.timeScale = argShow ? 0f : 1f;
		AudioListener.pause = argShow;

		UnlockCursor(argShow);
	}

	private void UnlockCursor(bool argShowCursor)
	{
		Paused = argShowCursor;
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
		Time.timeScale = 1f;
		AudioListener.pause = false;
		SceneManager.LoadScene("MainMenu");
	}

	public void SetReloadBool()
	{
		ReloadAmmoType = toggle.isOn;
	}
	
}
