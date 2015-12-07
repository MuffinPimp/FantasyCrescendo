﻿using System.Collections.Generic;
using UnityEngine;
using Hourai.Events;
using UnityEngine.Networking;

namespace Hourai.SmashBrew {

    public class PlayerSpawnEvent : IEvent {

        public Player Player; 
        public GameObject PlayerObject;

    }

    public class Spawn : MonoBehaviour {

        private Mediator _eventManager;

        [SerializeField]
        private Transform[] _spawnPoints;
        
        void Awake() {
            _eventManager = GlobalEventManager.Instance;
            var i = 0;
            IEnumerator<Player> activePlayers = SmashGame.ActivePlayers.GetEnumerator();
            while (i < _spawnPoints.Length && activePlayers.MoveNext()) {
                Player player = activePlayers.Current;
                Character runtimeCharacter = player.Spawn(_spawnPoints[i]);
                i++;
                if (runtimeCharacter == null)
                    continue;

                _eventManager.Publish(new PlayerSpawnEvent { Player = player, PlayerObject = runtimeCharacter.gameObject });

                //TODO: Fix this hack, get netplay working
                runtimeCharacter.gameObject.SetActive(true);
            }
        }

    }

}