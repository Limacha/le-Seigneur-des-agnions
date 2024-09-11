using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MenuAnim : MonoBehaviour
{
    public Animator animator; // l'animator

    public string openTriggerPlay = "PlayOpen"; // Animation ouverture
    public string closeTriggerPlay = "PlayClose"; // Animation fermeture
    private bool isOpenPlay = false; // si l'ui Play est ouvert ou pas

    public string openTriggerSett = "SettingsOpen"; // Animation ouverture
    public string closeTriggerSett = "SettingsClose"; // Animation fermeture
    private bool isOpenSett = false; // si l'ui Play est ouvert ou pas


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
                    StartCoroutine(AnimationPlayEndSETT());
                } else
                {
                    animator.SetTrigger(openTriggerPlay); // quand Fermer lancer l'animation ouvrir au click
                }
            }
            isOpenSett = false;
            isOpenPlay = !isOpenPlay;
        }
    }

    IEnumerator AnimationPlayEndSETT()
    {
        animator.SetTrigger(closeTriggerSett); // quand Fermer lancer l'animation ouvrir au click
        yield return new WaitForSeconds(2); // Attendre que l'animation se termine
        animator.SetTrigger(openTriggerPlay); // quand Fermer lancer l'animation ouvrir au click
    }

    public void ToggleAnimationSett() // lancer l'animation de fermeture ou d'ouverture
    {
        if (animator != null) // vérifie si ya un animator
        {
            if (isOpenSett)
            {
                animator.SetTrigger(closeTriggerSett); // quand ouvert lancer l'animation fermer au click
            }
            else
            {

                if (isOpenPlay)
                {
                    StartCoroutine(AnimationSettEndPLAY()); // Lance une Coroutine pour pouvoir faire le WaitForSecondes, Un void ne peut le faire.
                }
                else
                {
                    animator.SetTrigger(openTriggerSett); // quand Fermer lancer l'animation ouvrir au click
                }
            }
            isOpenPlay = false;
            isOpenSett = !isOpenSett;
        }
    }

    IEnumerator AnimationSettEndPLAY()
    {
        animator.SetTrigger(closeTriggerPlay); // quand Fermer lancer l'animation ouvrir au click
        yield return new WaitForSeconds(2); // Attendre que l'animation se termine
        animator.SetTrigger(openTriggerSett); // quand Fermer lancer l'animation ouvrir au click
    }
}
