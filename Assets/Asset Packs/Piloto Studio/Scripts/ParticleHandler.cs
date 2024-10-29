using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace PilotoStudio
{
	public class ParticleHandler : MonoBehaviour
	{
		[FormerlySerializedAs("builtUpParticle")] public ParticleSystem buildUpParticle;
		public float buildUpDuration;
		public ParticleSystem loopingParticle;
		public float loopDuration;
		public ParticleSystem endParticle;
		public float endDuration;

		float startEmission;
		private void OnEnable()
		{
			Cast();
		}

		public void Cast()
		{
			StartCoroutine(Flow());
		}

		IEnumerator Flow()
		{
			if (buildUpParticle)
			{
				PlayParticles(buildUpParticle);
				yield return new WaitForSeconds(buildUpDuration);
			}

			if (loopingParticle)
			{
				PlayParticles(loopingParticle, loopDuration);
				yield return new WaitForSeconds(loopDuration);
			}

			if (endParticle)
			{
				PlayParticles(endParticle);
				yield return new WaitForSeconds(endDuration);
			}

			Destroy(gameObject);
		}
		
		private IEnumerator WaitUntilParticleSystemStops(ParticleSystem particles)
		{
			while (particles.IsAlive(true))
			{
				yield return null;
			}
		}
		
		private void PlayParticles(ParticleSystem particles, float duration = -1f)
		{
			if (duration == 0) return;
			
			particles.gameObject.SetActive(true);

			if (float.IsPositiveInfinity(particles.main.startLifetime.constantMax))
				StartCoroutine(WaitUntilParticleSystemStops(particles));

			particles.Play();

			if (duration > -1f && !float.IsPositiveInfinity(particles.main.startLifetime.constantMax))
			{
				StartCoroutine(StopParticleAfterTime(particles, duration));
			}
		}

		IEnumerator StopParticleAfterTime(ParticleSystem particles, float duration)
		{
			yield return new WaitForSeconds(duration);
			var particleSystemMain = particles.emission;
			particleSystemMain.rateOverTimeMultiplier = 0;
			//particles.gameObject.SetActive(false);
		}
	}




}