using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PointsType
{
    TypeA,
    TypeB,
    TypeC
}

/// <summary>
/// Transfers points from a local counter to a global one using particles.
/// </summary>
public class PointsTransfer : MonoBehaviour
{
    public Text pointsLocalCount;
    public Text pointsBankCount;
    public PointsType type;
    public float transferDuration = 1f;
    public AnimationCurve transferAnim;
    public CoinParticle particlesTransfer;

    int pointsLocal;
    int pointsBank;

    public void Init(int i)
    {
        switch (type)
        {
            case PointsType.TypeA:
                pointsBank = Progress.progress.pointsA;
                break;
            case PointsType.TypeB:
                pointsBank = Progress.progress.pointsB;
                break;
            case PointsType.TypeC:
                pointsBank = Progress.progress.pointsC;
                break;
            default:
                break;
        }
        pointsLocal = i;
        switch (type)
        {
            case PointsType.TypeA:
                Progress.progress.pointsA += pointsLocal;
                break;
            case PointsType.TypeB:
                Progress.progress.pointsB += pointsLocal;
                break;
            case PointsType.TypeC:
                Progress.progress.pointsC += pointsLocal;
                break;
            default:
                break;
        }
        pointsLocalCount.text = pointsLocal.ToString();
        pointsBankCount.text = pointsBank.ToString();
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
