using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entreprise
{
    public class Dette
    {
        //obliger payer la totaliter

        private string nom; //nom de la dette
        private string description; //description qui explique pq
        private uint montant; //montant de la dette
        private DateTime jourRecu; //jour ou sa a ete recu
        private ushort durer; //durer avant payement
        private byte interet; //les interet subi
        private ushort durerMax; //durer max avant punition default + 1 ans
        private byte reputEmployer; //reputation d'ou viens la dette

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

        public byte ReputEmployer
        {
            get => reputEmployer;
        }

        // Constructeur
        public Dette(string nom, string description, uint montant, DateTime jourRecu, ushort durer, byte interet, ushort durerMax, byte reputEmployer)
        {
            this.nom = nom;
            this.description = description;
            this.montant = montant;
            this.jourRecu = jourRecu;
            this.durer = durer;
            this.interet = interet;
            this.durerMax = durerMax;
            this.reputEmployer = reputEmployer;
        }

        public string Info()
        {
            return $"Nom : {Nom}\n" +
                   $"Description : {Description}\n" +
                   $"Montant : {Montant}€\n" +
                   $"Recue le : {JourRecu:dd/MM/yyyy}\n" +
                   $"Duree avant paiement : {Durer} jours\n" +
                   $"Interet : {Interet}%\n" +
                   $"Duree max avant sanction : {DurerMax} jours\n" +
                   $"Reputation de l'employeur : {ReputEmployer}/100";
        }
    }
}
