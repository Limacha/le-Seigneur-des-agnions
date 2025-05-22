using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace entreprise
{
    [Serializable]
    public class Dette
    {
        //obliger payer la totaliter

        [SerializeField] private string nom; //nom de la dette
        [SerializeField] private string description; //description qui explique pq
        [SerializeField] private uint montant; //montant de la dette
        [SerializeField] private DateTime jourRecu; //jour ou sa a ete recu
        [SerializeField] private ushort durer; //durer avant payement
        [SerializeField] private byte interet; //les interet subi
        [SerializeField] private ushort durerMax; //durer max avant punition default + 1 ans
        [SerializeField] private byte reputEmployer; //reputation d'ou viens la dette

        public string Nom
        {
            get
            {
                return nom;
            }
        }

        public string Description
        {
            get => description;
        }

        public uint Montant
        {
            get => montant;
        }

        public DateTime JourRecu
        {
            get => jourRecu;
        }

        public ushort Durer
        {
            get => durer;
        }

        public byte Interet
        {
            get => interet;
        }

        public ushort DurerMax
        {
            get => durerMax;
        }

        /// <param name="nom">le nom de la dette</param>
        /// <param name="description">la description de la dette</param>
        /// <param name="montant">le montant de la dette</param>
        /// <param name="jourRecu">le jour ou la dette a ete envoie</param>
        /// <param name="durer">la durer avant arriver interet</param>
        /// <param name="interet">les interes de la dette 0-100</param>
        /// <param name="durerMax">la durer max avant fin du jeux</param>
        public Dette(string nom, string description, uint montant, DateTime jourRecu, ushort durer, byte interet, ushort durerMax)
        {
            this.nom = nom;
            this.description = description;
            this.montant = montant;
            this.jourRecu = jourRecu;
            this.durer = durer;
            this.interet = interet;
            this.durerMax = durerMax;
        }

        /// <summary>
        /// affiche toute les infos possible
        /// </summary>
        /// <returns>un string avec toute les infos</returns>
        public string Info()
        {
            return $"Nom : {Nom}\n" +
                   $"Description : {Description}\n" +
                   $"Montant : {Montant}€\n" +
                   $"Recue le : {JourRecu:dd/MM/yyyy}\n" +
                   $"Duree avant interet : {Durer} jours\n" +
                   $"Interet : {Interet}%\n" +
                   $"Duree max avant sanction : {DurerMax} jours\n";
        }

        /// <summary>
        /// ajoute les interets au montant
        /// </summary>
        public void AppliquerInteret()
        {
            montant += montant * interet / 100;
        }

        /// <summary>
        /// fait rembourser de force le joueur
        /// </summary>
        /// <param name="entreprise">l'entreprise qui rembourse</param>
        public void RembourcementForcer(Entreprise entreprise)
        {
            entreprise.Transaction(-montant);
        }

        /// <summary>
        /// supprimer 1 au durer
        /// </summary>
        public void NewDay()
        {
            if(durer != 0) durer--;
            if(durerMax != 0) durerMax--;
        }
    }
}
