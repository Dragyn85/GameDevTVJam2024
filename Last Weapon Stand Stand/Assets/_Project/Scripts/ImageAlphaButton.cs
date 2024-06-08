using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ImageAlphaButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public UnityEvent OnPointerEnterEvent;
    public UnityEvent OnPointerExitEvent;
    public UnityEvent onClick;
    
    [Tooltip("Enable read / write on the sprite2D")]
    [SerializeField] Image targetImage;
    [SerializeField] float alphaHitTestMinimumThreshold = 0.1f;
    [SerializeField] bool debug;

    private void OnValidate()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();
    }

    private void Start()
    {
        targetImage.alphaHitTestMinimumThreshold = alphaHitTestMinimumThreshold;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (debug)
        {
            Debug.Log("Entered");
        }

        OnPointerEnterEvent.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (debug)
        {
            Debug.Log("Exited");
        }

        OnPointerExitEvent.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (debug)
        {
            Debug.Log("Clicked");
        }

        if (eventData.button == PointerEventData.InputButton.Left)
            onClick.Invoke();
    }
}