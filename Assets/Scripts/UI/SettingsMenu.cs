using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
	private const float VOL_OFFSET = -45;
	private const float VOL_MULTIPLIER = 0.65f;
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
	[Space] [SerializeField] private AudioMixer mixer;
	
	// TODO: connect volume and sensitivity to actual controllers
	
	private void Start()
	{
		sensSlider.value = PlayerPrefs.GetFloat("Sensitivity", 0.1f);
		sensText.text = PlayerPrefs.GetFloat("Sensitivity", 0.1f).ToString();
		
		masterSlider.value = PlayerPrefs.GetFloat("MasterVol", 100f);
		masterText.text = PlayerPrefs.GetFloat("MasterVol", 100f).ToString();
		mixer.SetFloat("MasterVol", masterSlider.value * VOL_MULTIPLIER + VOL_OFFSET);
		
		environSlider.value = PlayerPrefs.GetFloat("EnvironVol", 100f);
		environText.text = PlayerPrefs.GetFloat("EnvironVol", 100f).ToString();
		mixer.SetFloat("EnvironVol", environSlider.value * VOL_MULTIPLIER + VOL_OFFSET);
		
		enemySlider.value = PlayerPrefs.GetFloat("EnemyVol", 100f);
		enemyText.text = PlayerPrefs.GetFloat("EnemyVol", 100f).ToString();
		mixer.SetFloat("EnemyVol", enemySlider.value * VOL_MULTIPLIER + VOL_OFFSET);
		
		playerSlider.value = PlayerPrefs.GetFloat("PlayerVol", 100f);
		playerText.text = PlayerPrefs.GetFloat("PlayerVol", 100f).ToString();
		mixer.SetFloat("PlayerVol", playerSlider.value * VOL_MULTIPLIER + VOL_OFFSET);
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
		mixer.SetFloat("MasterVol", vol * VOL_MULTIPLIER + VOL_OFFSET);
	}
	
	public void SetEnvironVol(float argVol)
	{
		float vol = MathF.Round(argVol, 1);
		PlayerPrefs.SetFloat("EnvironVol", vol);
		environText.text = vol.ToString();
		mixer.SetFloat("EnvironVol", vol * VOL_MULTIPLIER + VOL_OFFSET);
	}
	
	public void SetEnemyVol(float argVol)
	{
		float vol = MathF.Round(argVol, 1);
		PlayerPrefs.SetFloat("EnemyVol", vol);
		enemyText.text = vol.ToString();
		mixer.SetFloat("EnemyVol", vol * VOL_MULTIPLIER + VOL_OFFSET);
	}
	
	public void SetPlayerVol(float argVol)
	{
		float vol = MathF.Round(argVol, 1);
		PlayerPrefs.SetFloat("PlayerVol", vol);
		playerText.text = vol.ToString();
		mixer.SetFloat("PlayerVol", vol * VOL_MULTIPLIER + VOL_OFFSET);
	}
}
