using UnityEngine;

namespace AllFonction
{
    public struct Fonction
    {
        /// <summary>
        /// debug le contenu de l'inventaire avec "sprite in {cells[y, x]}({x}, {y})"
        /// </summary>
        public void show2DSpriteContent(Sprite[,] content)
        {
            for (var y = 0; y < content.GetLength(1); y++)
            {
                for (var x = 0; x < content.GetLength(0); x++)
                {
                    Debug.Log($"sprite in {content[x, y]}({x}, {y})");
                }
            }
        }

        /// <summary>
        /// debug le contenu de l'inventaire avec "item in {cells[y, x]}({x}, {y})"
        /// </summary>
        public void show2DItemDataContent(ItemData[,] content)
        {
            for (var y = 0; y < content.GetLength(1); y++)
            {
                for (var x = 0; x < content.GetLength(0); x++)
                {
                    Debug.Log($"item in {content[x, y]}({x}, {y})");
                }
            }
        }

        /// <summary>
        /// rotate les 2DSprite de 90°
        /// </summary>
        /// <param name="array">array a tourner</param>
        /// <returns>2Dsprite de la rotation finel</returns>
        public Sprite[,] rotate2DSprite(Sprite[,] array)
        {
            Sprite[,] newArray = new Sprite[array.GetLength(1), array.GetLength(0)];
            for (var y = 0; y < array.GetLength(1); y++)
            {
                for (var x = 0; x < array.GetLength(0); x++)
                {
                    //Debug.Log($"x{x}y{y} => x{array.GetLength(1) - 1 - y}y{x}");
                    newArray[array.GetLength(1) - 1 - y, x] = array[x, y];
                }
            }
            //show2DSpriteContent(array);
            //show2DSpriteContent(newArray);
            return newArray;
        }

        public void SetLeftRt(RectTransform rt, float left)
        {
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        }
        public void SetRightRt(RectTransform rt, float right)
        {
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        }

        public void SetTopRt(RectTransform rt, float top)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        }

        public void SetBottomRt(RectTransform rt, float bottom)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }
    }
}