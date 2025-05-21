using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlechaTrigger : MonoBehaviour
{
    public int Index;
    public FlechasController Manager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // o la cámara si no hay "Player"
        {
            gameObject.SetActive(false);
            Manager.ActivarSiguienteFlecha(Index);
        }
    }
}
