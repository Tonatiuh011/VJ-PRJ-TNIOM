using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using Unity.Mathematics;

public class SensorMovement : MonoBehaviour
{
    public string[] CollisionMasks = new string[] { };

    private int colCount = 0;
    private float disableTimer;

    private void OnEnable()
    {
        colCount = 0;
    }

    public bool State() => disableTimer > 0 ? false : colCount > 0;

    public void Disable(float duration) => disableTimer = duration;

    void OnTriggerEnter2D(Collider2D collision) 
    {
        if(IsCollisionOnMasks(CollisionMasks, collision.gameObject.layer))
            colCount++; 
    }

    void OnTriggerExit2D(Collider2D collision) 
    {
        if (IsCollisionOnMasks(CollisionMasks, collision.gameObject.layer))
            colCount--;
    }

    void Update() => disableTimer -= Time.deltaTime;

    private bool IsCollisionOnMasks(string[] masks, int currentLayer)
    {
        if (masks.Length == 0)
            return true;

        foreach(var mask in masks)
        {
            var iLayer = LayerMask.NameToLayer(mask);            
            return iLayer == currentLayer;
        }

        return false;
    }
}