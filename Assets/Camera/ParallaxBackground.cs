using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour {
    
    
    [SerializeField]
    private FollowCamera _followCamera;

    [SerializeField]
    private BackgroundLayer[] _backgroundLayers;

    private void Update() {
        foreach (var backgroundLayer in _backgroundLayers) {
            backgroundLayer.SetPercentage(_followCamera.PlacementPercentage);
        }
    }
}
