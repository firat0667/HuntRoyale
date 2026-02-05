using System;
using System.Collections.Generic;
using UnityEngine;
using Firat0667.CaseLib.Key;
using Firat0667.CaseLib.Patterns;

namespace Firat0667.CaseLib.Game
{
    public class GameRegistry : FoundationSingleton<GameRegistry>,IFoundationSingleton 
    {

        private Dictionary<GameKey, object> _registry = new();
        private HashSet<GameKey> _persistentKeys = new();  // Stores persistent objects

        public bool Initialized { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event Action<string, object> OnRegister;  // Event: Triggered when a new object is registered
        public event Action<string> OnUnregister;        // Event: Triggered when an object is removed
        public event Action<string, object> OnRetrieved; // Event: Triggered when an object is retrieved

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Registers a new object.
        /// </summary>
        public void Register<T>(string key, T obj, bool isPersistent = false)
        {
            GameKey regKey = new GameKey(key); // GameKey kullanılıyor
            if (_registry.ContainsKey(regKey))
            {
                Debug.LogWarning($"[GameRegistry] {key} is already registered!");
                return;
            }

            _registry.Add(regKey, obj);
            if (isPersistent)
            {
                _persistentKeys.Add(regKey);
            }

            Debug.Log($"[GameRegistry] {key} successfully registered!");
            OnRegister?.Invoke(key, obj);
        }

        /// <summary>
        /// Retrieves a registered object.
        /// </summary>
        public T Get<T>(string key)
        {
            GameKey regKey = new GameKey(key); // GameKey kullanılıyor
            if (_registry.TryGetValue(regKey, out object obj))
            {
                Debug.Log($"[GameRegistry] {key} successfully retrieved!");
                OnRetrieved?.Invoke(key, obj);
                return (T)obj;
            }

            Debug.LogWarning($"[GameRegistry] {key} not found!");
            return default;
        }

        /// <summary>
        /// Removes an object from the registry.
        /// </summary>
        public void Unregister(string key)
        {
            GameKey regKey = new GameKey(key); // GameKey kullanılıyor
            if (_registry.ContainsKey(regKey))
            {
                _registry.Remove(regKey);
                _persistentKeys.Remove(regKey);
                Debug.Log($"[GameRegistry] {key} has been removed!");
                OnUnregister?.Invoke(key);
            }
            else
            {
                Debug.LogWarning($"[GameRegistry] {key} is not registered!");
            }
        }

        /// <summary>
        /// Clears all registered objects except persistent ones.
        /// </summary>
        public void Clear()
        {
            var tempRegistry = new Dictionary<GameKey, object>(_registry);

            foreach (var key in tempRegistry.Keys)
            {
                if (!_persistentKeys.Contains(key))
                {
                    _registry.Remove(key);
                    Debug.Log($"[GameRegistry] {key.ValueAsString} has been removed!");
                }
            }
        }
    }
}



/* Ne Zaman FoundationMaster, Ne Zaman GameRegistry Kullanılmalı?
 Eğer Singleton bir sistem ise → FoundationMaster Kullan

GameManager, UIManager, AudioManager, InputManager gibi tekil nesneler için FoundationMaster en iyisi.
 Eğer Sahneye Bağlı veya Geçici Bir Sistemse → GameRegistry Kullan

Örneğin, Spawner, NPC Manager, LevelController, QuestManager gibi sahneye özgü nesneler için GameRegistry iyi bir çözüm.
 Eğer Oyun içinde Dinamik Olarak Yönetilmesi Gereken Nesneler Varsa → GameRegistry Kullan

Örneğin, dinamik olarak sahnede yaratılan görevler, event’ler veya geçici UI nesneleri için GameRegistry uygun.
 Eğer Singleton Kullanılmayacak ama Merkezi Yönetim Gerekliyse → GameRegistry Kullan

Örneğin, bazı veri taşıyıcı nesneleri, Timer veya geçici UI bileşenlerini yönetmek için.
*/


/*
 *   private void Start()
    {
        // GameManager’ı GameRegistry’den çağırıyoruz.
        GameManager gm = GameRegistry.Instance.Get<GameManager>("GameManager");

        if (gm != null)
        {
            Debug.Log("GameManager bulundu!");
            gm.StartGame(); // GameManager'ın metodunu çağırıyoruz.
        }
        else
        {
            Debug.LogWarning("GameManager bulunamadı!");
        }
    }
 * 
 * 
 * GameRegistry.Instance.Unregister("GameManager");

 * */