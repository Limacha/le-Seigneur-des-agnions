using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    [SerializeReference] private TMP_Text nom;
    [SerializeReference] private TMP_Text description;
    [SerializeReference] private TMP_Text stack;
    [SerializeReference] private TMP_Text poids;
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
        item.imgRef = (item.imgRef != null) ? item.imgRef : inventory.TransImage;
        GetComponent<RectTransform>().GetChild(0).GetComponent<Image>().sprite = item.imgRef;
        nom.SetText(item.nom);
        description.SetText(item.description);
        if (item.stackable)
        {
            stack.SetText(item.stack.ToString() + "/" + item.stackLimit.ToString());
        }
        poids.SetText("Poids: " + item.poids.ToString());
    }
}