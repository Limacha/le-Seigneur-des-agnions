using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ExcMenuController : MonoBehaviour
{
    string openTriggerTouches = "openTouchesMenu"; // Animation ouverture
    string closeTriggerTouches = "closeTouchesMenu"; // Animation fermeture
    bool isOpenSettTouches = false; // si l'ui SettTouches est ouvert ou pas

    string openTriggerAudio = "openAudioMenu"; // Animation ouverture
    string closeTriggerAudio = "closeAudioMenu"; // Animation fermeture
    bool isOpenSettAudio = false; // si l'ui SettAudio est ouvert ou pas

    string openTriggerGraph = "openGraphMenu"; // Animation ouverture
    string closeTriggerGraph = "closeGraphMenu"; // Animation fermeture
    bool isOpenSettGraph = false; // si l'ui SettGrap est ouvert ou pas

    string openTriggerSave = "openSaveMenu"; // Animation ouverture
    string closeTriggerSave = "closeSaveMenu"; // Animation fermeture
    bool isOpenSettSave = false; // si l'ui SettTouches est ouvert ou pas

    // Référence au Panel du menu - doit être assigné dans l'éditeur
    [SerializeReference] private GameObject menu;

    // Référence à l'Animator associé au menu - doit être assigné dans l'éditeur
    [SerializeReference] private Animator animator;

    // Indicateur pour suivre l'état actuel du menu - ouvert ou fermé
    bool isMenuActive = false;

    private void Update()
    {
        // Vérifie si la touche Échap a été pressée
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu(); // Appelle la méthode pour ouvrir ou fermer le menu
        }*/
    }

    public void ToggleMenu()
    {
        // Inverse l'état du menu : si actif, le désactive, sinon l'active
        isMenuActive = !isMenuActive;

        if (isMenuActive)
        {
            // Si le menu est activé, le rend visible
            menu.SetActive(true);

            // Déclenche l'animation d'ouverture pour le premier plan du menu
            StartCoroutine(OpenMenu());
        }
        else
        {
            // Si le menu est désactivé, déclenche l'animation de fermeture pour le premier plan
            StartCoroutine(CloseALL());
            StartCoroutine(CloseMenuAnimi());

            // Utilise un événement d'animation pour désactiver le menu
        }
    }
    IEnumerator OpenMenu()
    {
        animator.SetTrigger("openFrontEscMenu");
        yield return new WaitForSeconds(1); // Attendre que l'animation se termine
    }
    IEnumerator CloseMenuAnimi()
    {
        animator.SetTrigger("closeFrontEscMenu");
        yield return new WaitForSeconds(1); // Attendre que l'animation se termine
    }

    // Méthode appelée par un événement d'animation pour désactiver le menu
    public void CloseMenu()
    {
        // Désactive le menu après la fermeture
        menu.SetActive(false);

        StartCoroutine(CloseALL());

        // Met à jour l'état du enu pour refléter qu'il est fermé
        isMenuActive = false;
    }

    public void OpenCloseTouches() // lancer l'animation de fermeture ou d'ouverture
    {
        if (animator != null) // vérifie si ya un animator
        {
            if (!isOpenSettTouches)
            {
                StartCoroutine(TouchesClOp()); // Lance une Coroutine pour pouvoir faire le WaitForSecondes, Un void ne peut le faire
            } else
            {
                animator.SetTrigger(closeTriggerTouches);
            }
            isOpenSettTouches = !isOpenSettTouches;
        }
    }
    IEnumerator TouchesClOp()
    {
        if (isOpenSettAudio)
        {
            animator.SetTrigger(closeTriggerAudio);
            yield return new WaitForSeconds(1); // Attendre que l'animation se termine
            isOpenSettAudio = false;
        }
        else if (isOpenSettGraph)
        {
            animator.SetTrigger(closeTriggerGraph);
            yield return new WaitForSeconds(1); // Attendre que l'animation se termine
            isOpenSettGraph = false;
        }
        else if (isOpenSettSave)
        {
            animator.SetTrigger(closeTriggerSave);
            yield return new WaitForSeconds(1); // Attendre que l'animation se termine
            isOpenSettSave = false;
        }
        animator.SetTrigger(openTriggerTouches);
    }
    public void OpenCloseAudio() // lancer l'animation de fermeture ou d'ouverture
    {
        if (animator != null) // vérifie si ya un animator
        {
            if (!isOpenSettAudio)
            {
                StartCoroutine(AudioClOp()); // Lance une Coroutine pour pouvoir faire le WaitForSecondes, Un void ne peut le faire
            }
            else
            {
                animator.SetTrigger(closeTriggerAudio);
            }
            isOpenSettAudio = !isOpenSettAudio;
        }
    }
    IEnumerator AudioClOp()
    {
        if (isOpenSettTouches)
        {
            animator.SetTrigger(closeTriggerTouches);
            yield return new WaitForSeconds(1); // Attendre que l'animation se termine
            isOpenSettTouches = false;
        }
        else if (isOpenSettGraph)
        {
            animator.SetTrigger(closeTriggerGraph);
            yield return new WaitForSeconds(1); // Attendre que l'animation se termine
            isOpenSettGraph = false;
        }
        else if (isOpenSettSave)
        {
            animator.SetTrigger(closeTriggerSave);
            yield return new WaitForSeconds(1); // Attendre que l'animation se termine
            isOpenSettSave = false;
        }
        animator.SetTrigger(openTriggerAudio);
    }
    public void OpenCloseGraph() // lancer l'animation de fermeture ou d'ouverture
    {
        if (animator != null) // vérifie si ya un animator
        {
            if (!isOpenSettGraph)
            {
                StartCoroutine(GraphClOp()); // Lance une Coroutine pour pouvoir faire le WaitForSecondes, Un void ne peut le faire
            }
            else
            {
                animator.SetTrigger(closeTriggerGraph);
            }
            isOpenSettGraph = !isOpenSettGraph;
        }
    }
    IEnumerator GraphClOp()
    {
        if (isOpenSettTouches)
        {
            animator.SetTrigger(closeTriggerTouches);
            yield return new WaitForSeconds(1); // Attendre que l'animation se termine
            isOpenSettTouches = false;
        }
        else if (isOpenSettAudio)
        {
            animator.SetTrigger(closeTriggerAudio);
            yield return new WaitForSeconds(1); // Attendre que l'animation se termine
            isOpenSettAudio = false;
        }
        else if (isOpenSettSave)
        {
            animator.SetTrigger(closeTriggerSave);
            yield return new WaitForSeconds(1); // Attendre que l'animation se termine
            isOpenSettSave = false;
        }
        animator.SetTrigger(openTriggerGraph);
    }
    public void OpenCloseSave() // lancer l'animation de fermeture ou d'ouverture
    {
        if (animator != null) // vérifie si ya un animator
        {
            if (!isOpenSettSave)
            {
                StartCoroutine(SaveClOp()); // Lance une Coroutine pour pouvoir faire le WaitForSecondes, Un void ne peut le faire
            }
            else
            {
                animator.SetTrigger(closeTriggerSave);
            }
            isOpenSettSave = !isOpenSettSave;
        }
    }
    IEnumerator SaveClOp()
    {
        if (isOpenSettTouches)
        {
            animator.SetTrigger(closeTriggerTouches);
            yield return new WaitForSeconds(1); // Attendre que l'animation se termine
            isOpenSettTouches = false;
        }
        else if (isOpenSettAudio)
        {
            animator.SetTrigger(closeTriggerAudio);
            yield return new WaitForSeconds(1); // Attendre que l'animation se termine
            isOpenSettAudio = false;
        }
        else if (isOpenSettGraph)
        {
            animator.SetTrigger(closeTriggerGraph);
            yield return new WaitForSeconds(1); // Attendre que l'animation se termine
            isOpenSettGraph = false;
        }
        animator.SetTrigger(openTriggerSave);
    }





    public void ToutFermer() // lancer l'animation de fermeture ou d'ouverture
    {
        if (animator != null) // vérifie si ya un animator
        {
            StartCoroutine(CloseALL()); // Lance une Coroutine pour pouvoir faire le WaitForSecondes, Un void ne peut le faire
        }
    }
    IEnumerator CloseALL()
    {
        if (isOpenSettTouches)
        {
            animator.SetTrigger(closeTriggerTouches);
            isOpenSettTouches = false;
        } else if (isOpenSettAudio) 
        {
            animator.SetTrigger(closeTriggerAudio);
            isOpenSettAudio = false;
        } else if (isOpenSettGraph)
        {
            animator.SetTrigger(closeTriggerGraph);
            isOpenSettGraph = false;
        } else if (isOpenSettSave)
        {
            animator.SetTrigger(closeTriggerSave);
            isOpenSettSave = false;
        }
        yield return new WaitForSeconds(1); // Attendre que l'animation se termine
    }
}