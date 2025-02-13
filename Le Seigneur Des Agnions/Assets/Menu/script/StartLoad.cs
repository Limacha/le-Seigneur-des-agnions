using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class StartLoad : MonoBehaviour
{
    public Button newSave;          // button de nouvelle sauvegarde
    public Button startButton;          // button de start
    public Animator canvasAnimator;     // Animator Canvas
    public GameObject loadingCanvas;   // Canvas de chargement

    /*void Start() // lors du clique button start lance Demarrage ButtonClique
    {
        //startButton.onClick.AddListener(DemarrageButtonClique);
    }*/

    public void loadNewSave()
    {
        startButton = newSave;
        DemarrageButtonClique();
    }

    public void DemarrageButtonClique() // lors du button cliqué lance l'anim de load et charge l'autre scene
    {
        StartCoroutine(LanceAnimationChargeScene());
    }

    IEnumerator LanceAnimationChargeScene()
    {
        // Joue l'animation de transition
        canvasAnimator.SetTrigger("Start");

        // Active le Canvas de chargement et démarre l'animation de chargement
        loadingCanvas.SetActive(true);
        canvasAnimator.SetBool("IsLoading", true); // Active l'animation de chargement

        // Attends que l'animation de transition se termine
        yield return new WaitForSeconds(canvasAnimator.GetCurrentAnimatorStateInfo(0).length);


        GameObject.Find("GameManager").GetComponent<GameManager>().Save = startButton.name;
        // Charge a la scène de jeux
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");

        // attendre que le jeux soit chargée
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Désactiver l'animation de chargement après avoir chargé la page
        canvasAnimator.SetBool("IsLoading", false);
        loadingCanvas.SetActive(false);
    }
}