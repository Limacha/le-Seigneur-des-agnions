using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEditor.UIElements;

public class ToolTip : MonoBehaviour
{
    [SerializeReference] private TMP_Text nom;
    [SerializeReference] private TMP_Text description;
    Inventory inventory;
    public void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void SetInfo(ItemData item)
    {
        item.imgRef = (item.imgRef != null) ? item.imgRef : inventory.GetTransImage();
        GetComponent<RectTransform>().GetChild(0).GetComponent<Image>().sprite = item.imgRef;
        nom.SetText(item.nom);
        description.SetText(item.description);
    }
}
