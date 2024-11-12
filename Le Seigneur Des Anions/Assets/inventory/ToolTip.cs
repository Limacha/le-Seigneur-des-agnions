using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace inventory
{
    public class ToolTip : MonoBehaviour
    {
        [SerializeReference] private TMP_Text nom;
        [SerializeReference] private TMP_Text description;
        [SerializeReference] private TMP_Text stack;
        [SerializeReference] private TMP_Text poids;
        Inventory inv;
        public void Awake()
        {
            inv = GameObject.Find("Inventory").GetComponent<Inventory>();
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
            if (item.ImgRef != null)
            {


                GetComponent<RectTransform>().GetChild(0).GetComponent<Image>().sprite = item.ImgRef;
            }
            else
            {
                //Debug.Log(inv);
                GetComponent<RectTransform>().GetChild(0).GetComponent<Image>().sprite = inv.TransImage;
            }
            nom.SetText(item.Nom);
            description.SetText(item.Description);
            if (item.Stackable)
            {
                stack.gameObject.SetActive(true);
                stack.SetText(item.Stack.ToString() + "/" + item.StackLimit.ToString());
            }
            else
            {
                stack.gameObject.SetActive(false);
            }
            poids.SetText("Poids: " + (item.Poids * item.Stack).ToString());
        }
    }
}