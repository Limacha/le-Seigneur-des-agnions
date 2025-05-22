using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MenuAnim : MonoBehaviour
{
    [SerializeReference] private Animator animator; // l'animator

    string openTriggerPlay = "PlayOpen"; // Animation ouverture
    string closeTriggerPlay = "PlayClose"; // Animation fermeture
    bool isOpenPlay = false; // si l'ui Play est ouvert ou pas

    string openTriggerSett = "SettingsOpen"; // Animation ouverture
    string closeTriggerSett = "SettingsClose"; // Animation fermeture
    bool isOpenSett = false; // si l'ui Sett est ouvert ou pas

    string openTriggerGraph = "GraphMenOpen"; // Animation ouverture
    string openTriggerAudio = "AudioMenOpen"; // Animation ouverture
    string openTriggerTouches = "TouchesMenOpen"; // Animation ouverture
    string closeTriggerGraph = "GraphMenClose"; // Animation fermeture
    string closeTriggerAudio = "AudioMenClose"; // Animation fermeture
    string closeTriggerTouches = "TouchesMenClose"; // Animation fermeture
    bool isOpenSettGraph = false; // si l'ui SettGrap est ouvert ou pas
    bool isOpenSettAudio = false; // si l'ui SettAudio est ouvert ou pas
    bool isOpenSettTouches = false; // si l'ui SettTouches est ouvert ou pas

    string openTriggerNew = "NewOpen"; // Animation ouverture
    string closeTriggerNew = "NewClose"; // Animation fermeture
    bool isOpenNew = false; // si l'ui New est ouvert ou pas


    public void ToggleAnimationPlay() // lancer l'animation de fermeture ou d'ouverture
    {
        
        if (animator != null) // v�rifie si ya un animator
        {
            if (isOpenPlay)
            {
                animator.SetTrigger(closeTriggerPlay); // quand ouvert lancer l'animation fermer au click
            }
            else
            {
                if (isOpenSett) {
                    StartCoroutine(CloseSettOpenPlay());
                } else if (isOpenNew)
                {
                    StartCoroutine(CloseNewOpenPlay());
                } 
                else
                {
                    animator.SetTrigger(openTriggerPlay); // quand Fermer lancer l'animation ouvrir au click
                }
            }
            isOpenSett = false;
            isOpenNew = false;
            isOpenPlay = !isOpenPlay;
        }
    }

    IEnumerator CloseSettOpenPlay()
    {
        StartCoroutine(CloseSettSettALL()); // Lance une Coroutine pour pouvoir faire le WaitForSecondes, Un void ne peut le faire.
        animator.SetTrigger(closeTriggerSett); // quand Fermer lancer l'animation ouvrir au click
        yield return new WaitForSeconds(2); // Attendre que l'animation se termine
        animator.SetTrigger(openTriggerPlay); // quand Fermer lancer l'animation ouvrir au click
    }
    IEnumerator CloseNewOpenPlay()
    {
        animator.SetTrigger(closeTriggerNew); // quand Fermer lancer l'animation ouvrir au click
        yield return new WaitForSeconds(2); // Attendre que l'animation se termine
        animator.SetTrigger(openTriggerPlay); // quand Fermer lancer l'animation ouvrir au click
    }

    public void ToggleAnimationSett() // lancer l'animation de fermeture ou d'ouverture
    {
        if (animator != null) // vérifie si ya un animator
        {
            if (isOpenSett)
            {
                StartCoroutine(CloseSettSettALL()); // Lance une Coroutine pour pouvoir faire le WaitForSecondes, Un void ne peut le faire.
                animator.SetTrigger(closeTriggerSett); // quand ouvert lancer l'animation fermer au click
            }
            else
            {
                if (isOpenPlay)
                {
                    StartCoroutine(ClosePlayOpenSett()); // Lance une Coroutine pour pouvoir faire le WaitForSecondes, Un void ne peut le faire.
                } else if (isOpenNew)
                {
                    StartCoroutine(CloseNewOpenSett()); // Lance une Coroutine pour pouvoir faire le WaitForSecondes, Un void ne peut le faire.
                }
                else
                {
                    animator.SetTrigger(openTriggerSett); // quand Fermer lancer l'animation ouvrir au click
                }
            }
            isOpenPlay = false;
            isOpenNew = false;
            isOpenSett = !isOpenSett;
        }
    }

    IEnumerator ClosePlayOpenSett()
    {
        animator.SetTrigger(closeTriggerPlay); // quand Fermer lancer l'animation ouvrir au click
        yield return new WaitForSeconds(2); // Attendre que l'animation se termine
        animator.SetTrigger(openTriggerSett); // quand Fermer lancer l'animation ouvrir au click
    }
    IEnumerator CloseNewOpenSett()
    {
        animator.SetTrigger(closeTriggerNew); // quand Fermer lancer l'animation ouvrir au click
        yield return new WaitForSeconds(2); // Attendre que l'animation se termine
        animator.SetTrigger(openTriggerSett); // quand Fermer lancer l'animation ouvrir au click
    }

    public void ToggleAnimationNew() // lancer l'animation de fermeture ou d'ouverture
    {
        if (animator != null) // vérifie si ya un animator
        {
            if (isOpenNew)
            {
                animator.SetTrigger(closeTriggerNew); // quand ouvert lancer l'animation fermer au click
            }
            else
            {
                if (isOpenPlay)
                {
                    StartCoroutine(ClosePlayOpenNew()); // Lance une Coroutine pour pouvoir faire le WaitForSecondes, Un void ne peut le faire.
                } else if (isOpenSett)
                {
                    StartCoroutine(CloseSettOpenNew()); // Lance une Coroutine pour pouvoir faire le WaitForSecondes, Un void ne peut le faire.
                }
                else
                {
                    animator.SetTrigger(openTriggerNew); // quand Fermer lancer l'animation ouvrir au click
                }
            }
            isOpenSett = false;
            isOpenPlay = false;
            isOpenNew = !isOpenNew;
        }
    }

    IEnumerator ClosePlayOpenNew()
    {
        animator.SetTrigger(closeTriggerPlay); // quand Fermer lancer l'animation ouvrir au click
        yield return new WaitForSeconds(2); // Attendre que l'animation se termine
        animator.SetTrigger(openTriggerNew); // quand Fermer lancer l'animation ouvrir au click
    }
    IEnumerator CloseSettOpenNew()
    {
        StartCoroutine(CloseSettSettALL()); // Lance une Coroutine pour pouvoir faire le WaitForSecondes, Un void ne peut le faire.
        animator.SetTrigger(closeTriggerSett); // quand Fermer lancer l'animation ouvrir au click
        yield return new WaitForSeconds(2); // Attendre que l'animation se termine
        animator.SetTrigger(openTriggerNew); // quand Fermer lancer l'animation ouvrir au click
    }

    public void ToggleAnimationSettGraph() // lancer l'animation de fermeture ou d'ouverture
    {

        if (animator != null) // v�rifie si ya un animator
        {
            if (isOpenSett)
            {
                StartCoroutine(CloseSettSettALL()); // Lance une Coroutine pour pouvoir faire le WaitForSecondes, Un void ne peut le faire.
                animator.SetTrigger(openTriggerGraph); // Si menu paramètre ouvert ouvrir parametre graphiques
            }
            isOpenSettGraph = !isOpenSettGraph;
        }
    }
    public void ToggleAnimationSettAudio() // lancer l'animation de fermeture ou d'ouverture
    {

        if (animator != null) // v�rifie si ya un animator
        {
            if (isOpenSett)
            {
                StartCoroutine(CloseSettSettALL()); // Lance une Coroutine pour pouvoir faire le WaitForSecondes, Un void ne peut le faire.
                animator.SetTrigger(openTriggerAudio); // Si menu paramètre ouvert ouvrir parametre audio
            }
            isOpenSettAudio = !isOpenSettAudio;
        }
    }
    public void ToggleAnimationSettTouches() // lancer l'animation de fermeture ou d'ouverture
    {

        if (animator != null) // v�rifie si ya un animator
        {
            if (isOpenSett)
            {
                StartCoroutine(CloseSettSettALL()); // Lance une Coroutine pour pouvoir faire le WaitForSecondes, Un void ne peut le faire.
                animator.SetTrigger(openTriggerTouches); // Si menu paramètre ouvert ouvrir parametre touches
            }
            isOpenSettTouches = !isOpenSettTouches;
        }
    }
    public void ToggleAnimationSettLeave() // lancer l'animation de fermeture ou d'ouverture
    {

        if (animator != null) // v�rifie si ya un animator
        {
            if (isOpenSett)
            {
                StartCoroutine(CloseSettSettALL()); // Lance une Coroutine pour pouvoir faire le WaitForSecondes, Un void ne peut le faire.
            }
        }
    }
    IEnumerator CloseSettSettALL()
    {
        if (isOpenSettGraph)
        {
            animator.SetTrigger(closeTriggerGraph); // Si menu paramètre ouvert fermer parametre graphiques
            yield return new WaitForSeconds(1); // Attendre que l'animation se termine
        }
        if (isOpenSettAudio)
        {
            animator.SetTrigger(closeTriggerAudio); // Si menu paramètre ouvert fermer parametre audio
            yield return new WaitForSeconds(1); // Attendre que l'animation se termine
        }
        if (isOpenSettTouches)
        {
            animator.SetTrigger(closeTriggerTouches); // Si menu paramètre ouvert fermer parametre touches
            yield return new WaitForSeconds(1); // Attendre que l'animation se termine
        }
        isOpenSettGraph = false;
        isOpenSettAudio = false;
        isOpenSettTouches = false;
    }
}
