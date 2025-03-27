using UnityEngine;

namespace mob
{
    public class Agnion: MonoBehaviour
    {
        [SerializeField, Range(0, 10)] private int quality; //la qualiter de l'agnion
        [SerializeField]  private bool selected = false; //si il est selectionner ou pas

        public bool Selected { get { return selected; } set { selected = value; } }

        public int Quality 
        { 
            get { return quality; } 
            set
            {
                //si on le set verifi les bornes
                if (value < 0) quality = 0;
                if (value > 10) quality = 10;
                else quality = value;
            } 
        }

        public Agnion(int quality)
        {
            this.quality = quality;
        }

        public void Start()
        {
            //verifier les bornes
            if (quality < 1) quality = 1;
            if (quality > 10) quality = 10;
        }

        public void FixedUpdate()
        {
            //verifier la qualiter
            if (Quality <= 0)
            {
                //suprime soit le script soit l'object
                if (gameObject.name.Contains("Conteneur") || gameObject.name.Contains("conteneur"))
                {
                    Destroy(this);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        /// <summary>
        /// permet de degrader le mouton de 1
        /// </summary>
        public void Degrader()
        {
            quality--;
        }

        public void ChangeSelect()
        {
            selected = !selected;
        }
    }
}
