using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItemDuplicate : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    private GameObject clone;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Créez une copie de l'objet
        clone = Instantiate(gameObject, transform.parent);
        clone.transform.SetAsLastSibling();

        // Configurez la copie
        DraggableItemDuplicate cloneDraggableItem = clone.GetComponent<DraggableItemDuplicate>();
        cloneDraggableItem.parentAfterDrag = transform.parent;

        // Configurez l'objet en cours de drag
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