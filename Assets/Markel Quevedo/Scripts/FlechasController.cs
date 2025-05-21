using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FlechasController : MonoBehaviour
{
    [System.Serializable]
    public struct FlechaData
    {
        public Vector3 posicion;         // Posición relativa al patrón
        public Vector3 eulerRotation;    // Rotación fija, independiente de la imagen
    }

    public GameObject flechaPrefab;
    public List<FlechaData> flechasDatos; // Configurable en el inspector

    private List<GameObject> flechasInstanciadas = new List<GameObject>();
    private ARTrackedImageManager imageManager;

    void OnEnable()
    {
        imageManager = FindObjectOfType<ARTrackedImageManager>();
        imageManager.trackedImagesChanged += OnImageChanged;
    }

    void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnImageChanged;
    }

    void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var tracked in args.added)
        {
            InstanciarFlechas(tracked.transform);
        }
    }

    void InstanciarFlechas(Transform imagenTransform)
    {
        flechasInstanciadas.Clear(); // Por si acaso
        for (int i = 0; i < flechasDatos.Count; i++)
        {
            Vector3 posicionMundo = imagenTransform.position + flechasDatos[i].posicion;
            Quaternion rotacionMundo = Quaternion.Euler(flechasDatos[i].eulerRotation);

            GameObject flecha = Instantiate(flechaPrefab, posicionMundo, rotacionMundo);
            flecha.SetActive(i == 0); // Solo la primera visible

            FlechaTrigger trigger = flecha.GetComponent<FlechaTrigger>();
            trigger.Index = i;
            trigger.Manager = this;

            flechasInstanciadas.Add(flecha);
        }
    }
    public void ActivarSiguienteFlecha(int index)
    {
        if (index + 1 < flechasInstanciadas.Count)
        {
            flechasInstanciadas[index + 1].SetActive(true);
        }
    }
}
