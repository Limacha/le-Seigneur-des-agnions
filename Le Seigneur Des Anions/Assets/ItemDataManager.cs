using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    [SerializeField] private ItemData[] itemList;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in itemList)
        {
            item.SetPatern();
        }
    }
}
