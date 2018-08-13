using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class Segment : MonoBehaviour
{
    public ReusableParticleEffect breakEffect;
    public AudioClip breakClip;

    //private BoxCollider2D m_collider;
    private SpriteRenderer m_render;

    private ColorClass currentCC;
    public ColorClass SegColorClass { get { return currentCC; } }

    private void Awake()
    {
        //m_collider = GetComponent<BoxCollider2D>();
        m_render = GetComponent<SpriteRenderer>();
    }

    public void UpdateSegment(ColorClass cc, float length, float posX, float thickness = 0.05f)
    {
        UpdateLength(length, posX, thickness);
        UpdateColor(cc);
    }
    public void UpdateColor(ColorClass cc)
    {
        currentCC = cc;
        gameObject.layer = cc.layerFlag;
        m_render.color = cc.color;
        breakEffect.GetComponent<ParticleSystem>().startColor = cc.color;
    }
    public void UpdateLength(float length, float posX, float thickness = 0.05f)
    {
        thickness = Random.Range(0.02f, 0.2f);
        transform.localScale = new Vector3(length, thickness, 1);
        transform.localPosition = new Vector3(posX, 0, 0);
    }

    public void Break(Vector3 pos)
    {
        m_render.enabled = false;
        GameManager.Instance.sound.PlayInGameClip(breakClip, 0.5f);
        breakEffect.transform.position = pos;
        breakEffect.gameObject.SetActive(true);
    }
    public void ResetSelf()
    {
        m_render.enabled = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
