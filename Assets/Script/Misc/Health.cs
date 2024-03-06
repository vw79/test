using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Health : MonoBehaviour
{
    public VisualEffect explosionVFX;
    private Material sharedMaterial;
    public bool isDead;

    void Start()
    {
        explosionVFX.Stop();

        Renderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            sharedMaterial = renderer.material;
        }

        if (sharedMaterial != null)
        {
            Renderer[] allRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (Renderer childRenderer in allRenderers)
            {
                childRenderer.material = sharedMaterial;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(other.gameObject); 
            explosionVFX.Play();

            if (sharedMaterial != null)
            {
                StartCoroutine(AdjustBurnOverTime(2.1f, 0.3f, 1.1f, 3));
            }
        }
    }

    IEnumerator AdjustBurnOverTime(float delayBeforeStart, float startValue, float endValue, float duration)
    {
        yield return new WaitForSeconds(delayBeforeStart);

        float time = 0;
        while (time < duration)
        {
            float t = time / duration;
            float currentValue = Mathf.Lerp(startValue, endValue, t);
            sharedMaterial.SetFloat("_Burn", currentValue);
            time += Time.deltaTime;
            yield return null;
        }

        sharedMaterial.SetFloat("_Burn", endValue);

        isDead = true;
        gameObject.SetActive(false);
    }
}
