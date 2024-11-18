using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    private GameObject clone;

    public void OnBeginDrag(PointerEventData eventData)
    {
        clone = Instantiate(gameObject, transform.parent);
        clone.transform.SetAsLastSibling();

        DraggableItem cloneDraggableItem = clone.GetComponent<DraggableItem>();
        cloneDraggableItem.parentAfterDrag = transform.parent;

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}