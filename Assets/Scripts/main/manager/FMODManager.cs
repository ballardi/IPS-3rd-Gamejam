using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement;
using TMPro;

public class FMODManager : MonoBehaviour
{
   [FMODUnity.BankRef]
   public List<string> Banks = new List<string>();

   public void Awake(){
    foreach (string b in Banks)
    {
        FMODUnity.RuntimeManager.LoadBank(b, true);
    }
    FMODUnity.RuntimeManager.CoreSystem.mixerSuspend();
    FMODUnity.RuntimeManager.CoreSystem.mixerResume();

    StartCoroutine(CheckBanksLoaded());
   }

   IEnumerator CheckBanksLoaded()
   {
    while (!FMODUnity.RuntimeManager.HasBankLoaded("Master.strings")){
        yield return null;
    }

     if(FMODUnity.RuntimeManager.HasBankLoaded("Master.strings")){
       SceneManager.LoadScene(1);
    }

   }
   
}
