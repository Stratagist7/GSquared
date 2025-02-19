using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PillarMovement : MonoBehaviour
{
    [SerializeField] private float initialDelay;
    [SerializeField] private float duration;
    [SerializeField] private Transform ammoSpawner;
    [SerializeField] private GameObject ammoPrefab;
    private DroppedAmmo ammo;

    private const float UP_Y = 2f;
    private const float DOWN_Y = -7.5f;

    void Start()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, DOWN_Y, transform.localPosition.z);
        StartCoroutine(StartUp());
    }

    private IEnumerator StartUp()
    {
        SpawnAmmo();
        yield return new WaitForSeconds(initialDelay);
        StartCoroutine(MoveUp());
    }

    private IEnumerator MoveUp()
    {
        Vector3 startPos = new Vector3(transform.localPosition.x, DOWN_Y, transform.localPosition.z);
        Vector3 endPos = new Vector3(transform.localPosition.x, UP_Y, transform.localPosition.z);
        float t = 0f;

        while (t < duration)
        {
            transform.localPosition = Vector3.Lerp(startPos, endPos, t/duration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = endPos;
        
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(MoveDown());
    }
    
    private IEnumerator MoveDown()
    {
        Vector3 startPos = new Vector3(transform.localPosition.x, UP_Y, transform.localPosition.z);
        Vector3 endPos = new Vector3(transform.localPosition.x, DOWN_Y, transform.localPosition.z);
        float t = 0f;

        while (t < duration)
        {
            transform.localPosition = Vector3.Lerp(startPos, endPos, t/duration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = endPos;
        
        SpawnAmmo();
        yield return new WaitForSeconds(7.5f);
        StartCoroutine(MoveUp());
    }

    private void SpawnAmmo()
    {
        if (ammo != null)
        {
            Destroy(ammo.gameObject);
        }
        ammo = Instantiate(ammoPrefab, ammoSpawner.transform.position, Quaternion.identity).GetComponent<DroppedAmmo>();
        ammo.SetAmmo(15, (DamageType)Random.Range(0, 6));
    }

    
}
