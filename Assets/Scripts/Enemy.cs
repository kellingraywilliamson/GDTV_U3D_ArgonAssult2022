using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private Transform parent;
    [SerializeField] private int scoreValue = 15;
    [SerializeField] private int hitPoints = 2;

    private ScoreBoard _scoreBoard;

    private void Start()
    {
        _scoreBoard = FindObjectOfType<ScoreBoard>();
    }

    private void OnParticleCollision(GameObject other)
    {
        ApplyScoreIncrease();

        if (--hitPoints <= 0)
        {
            RunVFX(deathVFX);
            DestroySelf();
        }
        else
        {
            RunVFX(hitVFX);
        }
    }

    private void ApplyScoreIncrease()
    {
        if (!_scoreBoard) return;
        _scoreBoard.IncreaseScore(amountToIncrease: scoreValue);
    }

    private void DestroySelf()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void RunVFX(GameObject vfxToRun)
    {
        GameObject vfx = Instantiate(vfxToRun, transform.position, Quaternion.identity);
        vfx.transform.parent = parent;
    }
}
