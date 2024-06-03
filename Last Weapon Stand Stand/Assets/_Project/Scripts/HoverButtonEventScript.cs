using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverButtonEventScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent OnPointerEnterEvent;
    public UnityEvent OnPointerExitEvent;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        
            OnPointerEnterEvent.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
            OnPointerExitEvent.Invoke();
    }
}
