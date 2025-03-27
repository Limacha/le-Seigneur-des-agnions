using UnityEngine;
using System.Collections;

public class TroupAgnions : MonoBehaviour
{
    public GameObject agnionPrefab;
    public Transform[] lieuxDeSpawn; // Trois points de spawn à assigner dans l'éditeur
    private int maxAgnions = 10;
    private int agnionsActuels = 0;

    void Start()
    {
        // Spawning des agneaux au démarrage
        SpawnAgnionDepuisLieuAleatoire();
        SpawnAgnionDepuisLieuAleatoire();
        SpawnAgnionDepuisLieuAleatoire();
    }

    void Update()
    {
        // Si on a moins de 10 agneaux, on peut en ajouter d'autres
        if (agnionsActuels < maxAgnions)
        {
            SpawnAgnionDepuisLieuAleatoire();
        }
    }

    public void SpawnAgnionDepuisLieuAleatoire()
    {
        if (agnionsActuels < maxAgnions)
        {
            int indexSpawn = Random.Range(0, lieuxDeSpawn.Length);
            Vector3 positionSpawn = lieuxDeSpawn[indexSpawn].position;
            GameObject agnion = Instantiate(agnionPrefab, positionSpawn, Quaternion.identity);
            agnionsActuels++;
        }
    }

    public int ObtenirNombreAgnions()
    {
        return agnionsActuels;
    }

    public GameObject SpawnNouveauAgnion(Vector3 position)
    {
        agnionsActuels++;
        return Instantiate(agnionPrefab, position, Quaternion.identity);
    }
}
