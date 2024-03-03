using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;

public class PlayerSettings : Damageable
    {
        public GameObject invincibleNotif;
        public Toggle invcTog;
        public static bool invincible;
        public GameObject speedNotif;
        public Toggle speeTog;
        public static bool speedBoost;
        [Range(1f, 20.0f)]
        public static float mouseSens = 3;

        public PlayerController lesser;

        private void Start()
        {
            invincibleNotif.SetActive(invincible);
            speedNotif.SetActive(speedBoost);
            speeTog.isOn = speedBoost;
            invcTog.isOn = invincible;
        }

        public void Invincible(bool newVal){
            invincible = newVal;
            invincibleNotif.SetActive(invincible);
        }
        public void SpeedBoost(bool newVal){
            speedBoost = newVal;
            lesser.CurrSpeed = 5f;
            speedNotif.SetActive(invincible);
        }
        public void MouseSens(float newVal){
            mouseSens = newVal;
        }
        
    }
