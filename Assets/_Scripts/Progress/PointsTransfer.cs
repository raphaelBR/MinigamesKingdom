using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsTransfer : MonoBehaviour
{
    public Text pointsLocalCount;
    public Text pointsBankCount;

    public float transferDuration = 1f;
    public AnimationCurve transferAnim;
    public CoinParticle particlesTransfer;

    int pointsLocal;
    int pointsBank;

    public void Init(int i)
    {
        pointsBank = Progress.progress.pointsA;
        pointsLocal = i;
        pointsLocalCount.text = pointsLocal.ToString();
        pointsBankCount.text = pointsBank.ToString();
        Progress.progress.pointsA += pointsLocal;
    }

    public void Transfer()
    {
        if (pointsLocal > 0)
        {
            particlesTransfer.Play(pointsLocal);
        }
    }

    public void CoinSpawn()
    {
        pointsLocal--;
        pointsLocalCount.text = pointsLocal.ToString();
    }

    public void CoinDespawn()
    {
        pointsBank++;
        pointsBankCount.text = pointsBank.ToString();
    }

    public void CoinStop()
    {
        pointsLocalCount.text = "";
    }
}
