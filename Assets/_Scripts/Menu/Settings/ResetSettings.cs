using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSettings : MonoBehaviour
{
    Loading load;

    public void Reset()
    {
        Progress.Wipe();
        Parameters.Wipe();
        FindObjectOfType<Loading>().Reload();
    }

}
