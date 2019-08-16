using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The interface used to reset the player’s data.
/// </summary>
public class ResetSettings : MonoBehaviour
{
    public CustomAnimation confirm;

    Loading load;

    public void Init()
    {
        confirm.Teleport(0);
    }

    public void Reset()
    {
        Progress.Wipe();
        Parameters.Wipe();
        FindObjectOfType<Loading>().Reload();
    }

}
