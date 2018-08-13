using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReusableParticleEffect : MonoBehaviour
{
    public float duration;

    private void OnEnable()
    {
        Invoke("DeactivateSelf", duration);
    }

    private void DeactivateSelf()
    {
        this.gameObject.SetActive(false);
    }
}
