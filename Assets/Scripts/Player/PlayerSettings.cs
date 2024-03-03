using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
    public class PlayerSettings : Damageable
    {
        public static bool invincible;
        public static bool speedBoost;
        [Range(1f, 20.0f)]
        public static float mouseSens = 3;

        public PlayerController lesser;
        
        public void Invincible(bool newVal){
            invincible = newVal;
        }
        public void SpeedBoost(bool newVal){
            speedBoost = newVal;
            lesser.CurrSpeed = 5f;
        }
        public void MouseSens(float newVal){
            mouseSens = newVal;
        }
        
    }
