[System.Serializable]
public class InventorySaveData
{
    public ItemSaveData[] itemSaveDatas;
    public InventorySaveData(Inventory inv)
    {
        itemSaveDatas = new ItemSaveData[inv.ContentWidth * inv.ContentHeight]; //taille max des items possible de stocker
        int k = 0;
        for (int i = 0; i < inv.ContentWidth; i++) //parcour de l'inv
        {
            for (int j = 0; j < inv.ContentHeight; j++)
            {
                if (inv.Content[i, j] != null) //si l'item n'est pas null
                {
                    if (inv.Content[i, j] != inv.ItemDataSprite) //si se n'est pas un item de blockage
                    {
                        ItemSaveData item = null;

                        //definition des variable en fonction de sont type
                        if (inv.Content[i, j].GetType() == typeof(RessourceData))
                        {
                            item = new RessourceSaveData(inv.Content[i, j] as RessourceData);
                        }
                        else if(inv.Content[i, j].GetType() == typeof(ConsomableData))
                        {
                            item = new ConsomableSaveData(inv.Content[i, j] as ConsomableData);
                        }
                        else
                        {
                            item = new ItemSaveData(inv.Content[i, j]);
                        }

                        itemSaveDatas[k++] = item; //ajout de l'item en format saveData
                    }
                }
            }
        }
    }
}
