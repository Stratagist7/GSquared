using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
	[SerializeField] private Slider sensSlider;
	[SerializeField] private TextMeshProUGUI sensText;
	[Space]
	[SerializeField] private Slider masterSlider;
	[SerializeField] private TextMeshProUGUI masterText;
	[Space] 
	[SerializeField] private Slider environSlider;
	[SerializeField] private TextMeshProUGUI environText;
	[Space] 
	[SerializeField] private Slider enemySlider;
	[SerializeField] private TextMeshProUGUI enemyText;
	[Space]
	[SerializeField] private Slider playerSlider;
	[SerializeField] private TextMeshProUGUI playerText;
	
	// TODO: connect volume and sensitivity to actual controllers
	
	private void Start()
	{
		sensSlider.value = PlayerPrefs.GetFloat("Sensitivity", 0.1f);
		sensText.text = PlayerPrefs.GetFloat("Sensitivity", 0.1f).ToString();
		
		masterSlider.value = PlayerPrefs.GetFloat("MasterVol", 100f);
		masterText.text = PlayerPrefs.GetFloat("MasterVol", 100f).ToString();
		
		environSlider.value = PlayerPrefs.GetFloat("EnvironVol", 100f);
		environText.text = PlayerPrefs.GetFloat("EnvironVol", 100f).ToString();
		
		enemySlider.value = PlayerPrefs.GetFloat("EnemyVol", 100f);
		enemyText.text = PlayerPrefs.GetFloat("EnemyVol", 100f).ToString();
		
		playerSlider.value = PlayerPrefs.GetFloat("PlayerVol", 100f);
		playerText.text = PlayerPrefs.GetFloat("PlayerVol", 100f).ToString();
	}

	public void SetSensitivity(float argSens)
	{
		float sens = MathF.Round(argSens, 3);
		PlayerPrefs.SetFloat("Sensitivity", sens);
		sensText.text = sens.ToString();
	}
	
	public void SetMasterVol(float argVol)
	{
		float vol = MathF.Round(argVol, 1);
		PlayerPrefs.SetFloat("MasterVol", vol);
		masterText.text = vol.ToString();
	}
	
	public void SetEnvironVol(float argVol)
	{
		float vol = MathF.Round(argVol, 1);
		PlayerPrefs.SetFloat("EnvironVol", vol);
		environText.text = vol.ToString();
	}
	
	public void SetEnemyVol(float argVol)
	{
		float vol = MathF.Round(argVol, 1);
		PlayerPrefs.SetFloat("EnemyVol", vol);
		enemyText.text = vol.ToString();
	}
	
	public void SetPlayerVol(float argVol)
	{
		float vol = MathF.Round(argVol, 1);
		PlayerPrefs.SetFloat("PlayerVol", vol);
		playerText.text = vol.ToString();
	}
}
