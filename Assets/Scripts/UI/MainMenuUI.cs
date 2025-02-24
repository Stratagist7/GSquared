using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
	[SerializeField] private Animation anim;
	[SerializeField] private CinemachineDollyCart dollyCart;
	private bool hasAnimated = false;

	private void Update()
	{
		if (hasAnimated)
		{
			return;
		}

		if (Input.GetMouseButtonDown(0))
		{
			hasAnimated = true;
			dollyCart.m_Speed = 2;
			anim.Play();
		}
	}

	public void StartButton()
	{
		SceneManager.LoadScene("IntroLevel");
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
