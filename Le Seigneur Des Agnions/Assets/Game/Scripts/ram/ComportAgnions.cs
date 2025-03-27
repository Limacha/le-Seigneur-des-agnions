using System.Collections.Generic;
using UnityEngine;

public class ComportAgnions : MonoBehaviour
{
    public GameObject squarePrefab; // Cube au centre du groupe
    public GameObject leaderPrefab; // Leader que les Agnions suivent
    public GameObject targetPrefab; // L'objet à fuir
    public float detectionRadius = 10f;
    public float agnionSpeed = 3f;
    public float minDistance = 2f;
    public float leaderSpeed = 2f;
    public float leaderMoveRadius = 15f; // Le leader va plus loin
    public float leaderChangeDistance = 5f; // Un Agnion doit être à moins de 5f pour que le leader bouge
    public float maxLeaderDistance = 300f; // Distance max par rapport au GameObject Manager
    public float timeToWait = 1f; // Temps d'attente avant de bouger après avoir atteint le leader
    public float randomMovementRadius = 1f; // Degré de mouvement aléatoire autour du leader
    public float separationDistance = 3f; // Distance minimale entre les Agnions pour la séparation
    public float separationForce = 5f; // Force qui sépare les Agnions
    public float targetFleeDistance = 20f; // Distance minimale pour que les Agnions fuient l'objet
    public float minGroupSizeForNewAgnion = 2; // Nombre minimum d'Agnions pour qu'un nouveau apparaisse
    public float minTimeForNewAgnion = 600f; // Temps minimum pour un nouvel Agnion (en secondes, ici 10 minutes)
    public float maxTimeForNewAgnion = 1800f; // Temps maximum pour un nouvel Agnion (en secondes, ici 30 minutes)

    public GameObject agnionPrefab; // Le prefab de l'Agnion
    public int minAgnions = 2; // Nombre minimum d'Agnions à spawn
    public int maxAgnions = 5; // Nombre maximum d'Agnions à spawn

    private Dictionary<int, GameObject> groupSquares = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> groupLeaders = new Dictionary<int, GameObject>();
    private Dictionary<int, Vector3> leaderTargets = new Dictionary<int, Vector3>();
    private Dictionary<Transform, float> agnionTimers = new Dictionary<Transform, float>(); // Pour chaque Agnion, on garde une timer d'attente
    private Dictionary<Transform, Vector3> lastRandomOffsets = new Dictionary<Transform, Vector3>(); // Dernier déplacement aléatoire pour chaque Agnion
    private Dictionary<int, float> groupTimers = new Dictionary<int, float>(); // Timer pour ajouter un nouvel Agnion dans un groupe

    void Start()
    {
        SpawnAgnions(); // Appeler la méthode pour spawn des Agnions
    }

    void Update()
    {
        UpdateGroups();
        UpdateNewAgnionTimers();
    }

    void SpawnAgnions()
    {
        // Nombre d'Agnions à spawn entre minAgnions et maxAgnions
        int nombreAgnions = Random.Range(minAgnions, maxAgnions + 1);

        for (int i = 0; i < nombreAgnions; i++)
        {
            // Position de spawn aléatoire autour du GameObject qui porte ce script
            Vector3 spawnPosition = transform.position + new Vector3(
                Random.Range(-5f, 5f), // Variation en x
                0, // On garde la même hauteur que l'objet qui porte le script
                Random.Range(-5f, 5f) // Variation en z
            );

            // Instancier l'Agnion en tant qu'enfant de l'objet contenant le script
            GameObject newAgnion = Instantiate(agnionPrefab, spawnPosition, Quaternion.identity);
            newAgnion.transform.SetParent(transform); // Le rendre enfant du GameObject qui porte le script
            newAgnion.tag = "Agnion"; // Assurez-vous que l'Agnion est bien taggé
        }
    }

    void UpdateGroups()
    {
        // Récupérer les Agnions qui sont des enfants directs de l'objet
        List<Transform> agnions = new List<Transform>();
        foreach (Transform child in transform) // Parcours des enfants du GameObject
        {
            if (child.CompareTag("Agnion")) // On ajoute uniquement ceux qui ont le tag "Agnion"
            {
                agnions.Add(child);
            }
        }

        // Trouver les groupes d'Agnions
        List<List<Transform>> groupes = TrouverGroupes(agnions);
        HashSet<int> groupesActuels = new HashSet<int>();

        for (int i = 0; i < groupes.Count; i++)
        {
            Vector3 centre = CalculerCentre(groupes[i]);
            int groupID = i;

            groupesActuels.Add(groupID);

            // Créer ou mettre à jour le carré qui représente le centre du groupe
            if (!groupSquares.ContainsKey(groupID))
            {
                GameObject square = Instantiate(squarePrefab, centre, Quaternion.identity);
                square.transform.SetParent(transform); // Définir l'objet parent
                groupSquares[groupID] = square;
            }
            else
            {
                groupSquares[groupID].transform.position = centre;
            }

            // Créer ou mettre à jour le leader du groupe
            if (!groupLeaders.ContainsKey(groupID))
            {
                GameObject leader = Instantiate(leaderPrefab, centre, Quaternion.identity);
                leader.transform.SetParent(transform); // Définir l'objet parent
                groupLeaders[groupID] = leader;
                leaderTargets[groupID] = centre;
            }

            DéplacerLeader(groupID, centre, groupes[i]);

            // Déplacer chaque Agnion du groupe
            foreach (Transform agnion in groupes[i])
            {
                DéplacerVersLeader(agnion, groupLeaders[groupID].transform, groupes[i]);
                FuirCible(agnion);
            }
        }

        // Supprimer les groupes non actifs
        List<int> groupesÀSupprimer = new List<int>();
        foreach (var key in groupSquares.Keys)
        {
            if (!groupesActuels.Contains(key))
            {
                Destroy(groupSquares[key]);
                Destroy(groupLeaders[key]);
                groupesÀSupprimer.Add(key);
            }
        }

        foreach (var key in groupesÀSupprimer)
        {
            groupSquares.Remove(key);
            groupLeaders.Remove(key);
            leaderTargets.Remove(key);
        }
    }

    void UpdateNewAgnionTimers()
    {
        foreach (var groupID in groupTimers.Keys)
        {
            if (groupTimers[groupID] <= Time.time && groupLeaders.ContainsKey(groupID))
            {
                List<Transform> group = new List<Transform>(groupLeaders[groupID].GetComponentsInChildren<Transform>());
                if (group.Count >= minGroupSizeForNewAgnion)
                {
                    InstantiateAgnion(groupID);
                    groupTimers[groupID] = Time.time + Random.Range(minTimeForNewAgnion, maxTimeForNewAgnion);
                }
            }
        }
    }

    void InstantiateAgnion(int groupID)
    {
        // Instancier un nouvel Agnion au centre du groupe
        Vector3 centre = leaderTargets[groupID];
        GameObject newAgnion = Instantiate(leaderPrefab, centre, Quaternion.identity);
        newAgnion.tag = "Agnion"; // Assurez-vous que l'Agnion est bien taggé
        newAgnion.transform.parent = groupLeaders[groupID].transform; // Faire de l'Agnion un enfant du leader
    }

    void DéplacerLeader(int groupID, Vector3 centre, List<Transform> groupe)
    {
        Transform leader = groupLeaders[groupID].transform;
        Vector3 target = leaderTargets[groupID];

        bool agnionProche = false;
        foreach (Transform agnion in groupe)
        {
            if (Vector3.Distance(agnion.position, leader.position) < leaderChangeDistance)
            {
                agnionProche = true;
                break;
            }
        }

        if (agnionProche)
        {
            target = centre + new Vector3(
                Random.Range(-leaderMoveRadius, leaderMoveRadius),
                0, // On garde la hauteur constante
                Random.Range(-leaderMoveRadius, leaderMoveRadius)
            );

            // Calculer la distance du leader par rapport à l'AgnionManager
            Vector3 managerPosition = transform.position;
            float distanceFromManager = Vector3.Distance(target, managerPosition);

            // Si le leader dépasse 300 unités, on le ramène à la limite
            if (distanceFromManager > maxLeaderDistance)
            {
                target = managerPosition + (target - managerPosition).normalized * maxLeaderDistance;
            }

            leaderTargets[groupID] = target;
        }

        leader.position = Vector3.Lerp(leader.position, target, Time.deltaTime * leaderSpeed);
        leader.position = new Vector3(leader.position.x, centre.y, leader.position.z);
    }

    void DéplacerVersLeader(Transform agnion, Transform leader, List<Transform> groupe)
    {
        // Récupère l'Animator de l'Agnion
        Animator agnionAnimator = agnion.GetComponent<Animator>();

        if (agnionAnimator != null)
        {
            // Si l'Agnion fuit la cible
            if (targetPrefab != null && Vector3.Distance(agnion.position, targetPrefab.transform.position) < targetFleeDistance)
            {
                // Jouer l'animation de fuite
                agnionAnimator.SetTrigger("Run");

                // Déplacer l'Agnion en fuite
                Vector3 directionOpposee = (agnion.position - targetPrefab.transform.position).normalized;
                agnion.position += directionOpposee * (agnionSpeed + 1) * Time.deltaTime;

                // Faire en sorte que l'Agnion regarde dans la direction opposée
                if (directionOpposee != Vector3.zero)
                {
                    agnion.rotation = Quaternion.Slerp(agnion.rotation, Quaternion.LookRotation(directionOpposee), Time.deltaTime * agnionSpeed);
                }
            }
            // Si l'Agnion marche vers le leader
            else if (Vector3.Distance(agnion.position, leader.position) > minDistance)
            {
                // Jouer l'animation de marche
                agnionAnimator.SetTrigger("Walk");

                // Déplacer l'Agnion vers le leader
                Vector3 direction = (leader.position - agnion.position).normalized;
                float distance = Vector3.Distance(agnion.position, leader.position);

                // Mouvement aléatoire autour du leader
                Vector3 randomOffset = Vector3.zero;
                if (!lastRandomOffsets.ContainsKey(agnion) || Vector3.Distance(agnion.position, leader.position) > randomMovementRadius)
                {
                    randomOffset = new Vector3(
                        Random.Range(-randomMovementRadius, randomMovementRadius),
                        0, // On garde la hauteur constante
                        Random.Range(-randomMovementRadius, randomMovementRadius)
                    );

                    lastRandomOffsets[agnion] = randomOffset;
                }
                else
                {
                    randomOffset = lastRandomOffsets[agnion];
                }

                // Appliquer la séparation entre Agnions
                Vector3 separation = Vector3.zero;
                foreach (var other in groupe)
                {
                    if (other != agnion && Vector3.Distance(agnion.position, other.position) < separationDistance)
                    {
                        separation += agnion.position - other.position;
                    }
                }
                separation *= separationForce;

                // Déplacement de l'Agnion vers le leader
                if (distance > minDistance)
                {
                    agnion.position += (direction + randomOffset + separation).normalized * agnionSpeed * Time.deltaTime;

                    // Faire en sorte que l'Agnion regarde où il va
                    if (direction != Vector3.zero)
                    {
                        agnion.rotation = Quaternion.Slerp(agnion.rotation, Quaternion.LookRotation(direction), Time.deltaTime * agnionSpeed);
                    }
                }
            }
            // Si l'Agnion ne bouge pas, jouer l'animation d'Idle
            else
            {
                // Jouer l'animation d'Idle si l'Agnion est proche du leader
                agnionAnimator.SetTrigger("Idle");
            }
        }

        // Si l'Agnion arrive au leader, on le met en "attente" (si nécessaire)
        float distanceToLeader = Vector3.Distance(agnion.position, leader.position);
        if (distanceToLeader < minDistance && !agnionTimers.ContainsKey(agnion))
        {
            agnionTimers[agnion] = Time.time + timeToWait; // Attendre un peu avant de bouger
        }
    }


    void FuirCible(Transform agnion)
    {
        if (targetPrefab == null) return;

        Vector3 targetPos = targetPrefab.transform.position;
        float distanceToTarget = Vector3.Distance(agnion.position, targetPos);

        if (distanceToTarget < targetFleeDistance)
        {
            // Fuie la cible en allant dans la direction opposée
            Vector3 directionOpposee = (agnion.position - targetPos).normalized;
            agnion.position += directionOpposee * agnionSpeed * Time.deltaTime;

            // Faire en sorte que l'agnion regarde dans la direction opposée
            if (directionOpposee != Vector3.zero)
            {
                agnion.rotation = Quaternion.Slerp(agnion.rotation, Quaternion.LookRotation(directionOpposee), Time.deltaTime * agnionSpeed);
            }
        }
        else
        {
            // Retourne vers le leader ou l'objectif
            // Vous pouvez ici ajouter un comportement de retour vers le centre du groupe ou le leader
        }
    }

    List<List<Transform>> TrouverGroupes(List<Transform> agnions)
    {
        List<List<Transform>> groupes = new List<List<Transform>>();
        HashSet<Transform> visités = new HashSet<Transform>();

        foreach (var agnion in agnions)
        {
            if (!visités.Contains(agnion))
            {
                List<Transform> groupe = new List<Transform>();
                ExplorerGroupe(agnion, agnions, visités, groupe);
                groupes.Add(groupe);
            }
        }
        return groupes;
    }

    void ExplorerGroupe(Transform agnion, List<Transform> agnions, HashSet<Transform> visités, List<Transform> groupe)
    {
        Queue<Transform> file = new Queue<Transform>();
        file.Enqueue(agnion);
        visités.Add(agnion);

        while (file.Count > 0)
        {
            Transform actuel = file.Dequeue();
            groupe.Add(actuel);

            foreach (var voisin in agnions)
            {
                if (!visités.Contains(voisin) && Vector3.Distance(actuel.position, voisin.position) <= detectionRadius)
                {
                    file.Enqueue(voisin);
                    visités.Add(voisin);
                }
            }
        }
    }

    Vector3 CalculerCentre(List<Transform> groupe)
    {
        Vector3 somme = Vector3.zero;
        foreach (var agnion in groupe)
        {
            somme += agnion.position;
        }
        return somme / groupe.Count;
    }
}