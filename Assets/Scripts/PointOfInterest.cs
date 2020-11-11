using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : Subject
{
    [SerializeField] private string poiName;

    //jika gameobject di disable akan me-notify observernya
    private void OnDisable()
    {
        Notify(poiName);
    }
}
