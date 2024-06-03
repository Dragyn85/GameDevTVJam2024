using UnityEngine;
using UnityEngine.EventSystems;

public class HideOnMouseExit : MonoBehaviour, IPointerExitHandler
{
    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.SetActive(false);
    }
}
