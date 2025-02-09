﻿using System.Collections.Generic;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Simulation;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Simulation.Factory;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Track;
using Il2CppAssets.Scripts.Unity.Bridge;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
#if BloonsTD6
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;
#elif BloonsAT
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles.Behaviors;
#endif

namespace BTD_Mod_Helper.Extensions
{
    /// <summary>
    /// Extensions for the InGame class
    /// </summary>
    public static partial class InGameExt
    {
        /// <summary>
        /// Returns whether or not the player is currently in a game.
        /// </summary>
        /// <param name="inGame"></param>
        /// <returns></returns>
        public static bool IsInGame(this InGame inGame)
        {
            return inGame.GetSimulation() != null;
        }

        /// <summary>
        /// Get the current Map
        /// </summary>
        public static Map GetMap(this InGame inGame)
        {
            return inGame.GetSimulation()?.Map;
        }

        /// <summary>
        /// Get the current Simulation for this InGame session
        /// </summary>
        public static Simulation GetSimulation(this InGame inGame)
        {
            return inGame.GetUnityToSimulation()?.simulation;
        }

        /// <summary>
        /// The Game.model that is being used for this InGame.instance
        /// </summary>
        public static GameModel GetGameModel(this InGame inGame)
        {
            return inGame.GetSimulation()?.model;
        }

        /// <summary>
        /// Get the main Factory that creates and manages all other Factories
        /// </summary>
        public static FactoryFactory GetMainFactory(this InGame inGame)
        {
            return inGame.GetSimulation().factory;
        }

        /// <summary>
        /// Get the Factory for a specific Type. Ex: Getting the Factory that makes Towers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inGame"></param>
        /// <returns></returns>
        public static Factory<T> GetFactory<T>(this InGame inGame) where T : RootObject, new()
        {
            return inGame.GetMainFactory().GetFactory<T>();
        }

        /// <summary>
        /// Get every Tower that has been created through the Tower Factory
        /// </summary>
        /// <param name="inGame"></param>
        /// <param name="name">Optionally only get Towers whose TowerModel name is this parameter</param>
        /// <returns></returns>
        public static List<Tower> GetTowers(this InGame inGame, string name = null)
        {
            var towers = inGame.GetAllObjectsOfType<Tower>();
            if (!string.IsNullOrEmpty(name))
                towers = towers?.FindAll(tower => tower.towerModel.name == name);

            return towers ?? new List<Tower>();
        }

        /// <summary>
        /// Get all TowerToSimulations
        /// </summary>
        /// <param name="inGame"></param>
        /// /// <param name="name">Optionally only get Towers whose TowerModel name is this parameter</param>
        /// <returns></returns>
        public static List<TowerToSimulation> GetAllTowerToSim(this InGame inGame, string name = null)
        {
            var towerToSims = inGame.GetUnityToSimulation().GetAllTowers()?.ToList();
            if (!string.IsNullOrEmpty(name))
                towerToSims = towerToSims?.FindAll(tower => tower.Def.name == name);

            return towerToSims ?? new List<TowerToSimulation>();
        }


        /// <summary>
        /// Get's all Bloons on the map
        /// </summary>
        /// <param name="inGame"></param>
        /// <returns></returns>
        public static List<Bloon> GetBloons(this InGame inGame)
        {
#if BloonsTD6
            return inGame.GetFactory<Bloon>().all.ToList();
#elif BloonsAT
            return inGame.GetSimulation().bloonManager.GetBloons().ToList();
#endif
        }

        /// <summary>
        /// Get's all existing BloonToSimulations
        /// </summary>
        /// <param name="inGame"></param>
        /// <returns></returns>
        public static List<BloonToSimulation> GetAllBloonToSim(this InGame inGame)
        {
            //return SessionData.Instance.bloonTracker.currentBloonToSims.Values.ToList();
            return inGame.GetUnityToSimulation().GetAllBloons().ToList();
        }

        /// <summary>
        /// Get's all existing Projectiles on the map
        /// </summary>
        /// <param name="inGame"></param>
        /// <returns></returns>
        public static List<Projectile> GetProjectiles(this InGame inGame)
        {
            return inGame.GetAllObjectsOfType<Projectile>();
        }

        /// <summary>
        /// Get the current TowerManager for this game session
        /// </summary>
        public static TowerManager GetTowerManager(this InGame inGame)
        {
            return inGame.GetSimulation().towerManager;
        }

        /// <summary>
        /// Get's all AbilityToSimulations currently in the game
        /// </summary>
        /// <param name="inGame"></param>
        /// <returns></returns>
        public static List<AbilityToSimulation> GetAbilities(this InGame inGame)
        {
#if BloonsTD6
            return inGame.GetUnityToSimulation()?.GetAllAbilities(false)?.ToList();
#elif BloonsAT
            return inGame.GetUnityToSimulation()?.GetAllAbilities()?.ToList();
#endif
        }

        /// <summary>
        /// Get's the UnityToSimulation for this game
        /// </summary>
        /// <param name="inGame"></param>
        /// <returns></returns>
        public static UnityToSimulation GetUnityToSimulation(this InGame inGame)
        {
#if BloonsTD6
            return inGame.bridge;
#elif BloonsAT
            return inGame.Simulation;
#endif
        }


        /// <summary>
        /// Gets all objects of type T. Does this by returning all objects created by the Factory of type T
        /// </summary>
        /// <typeparam name="T">The type of items you want</typeparam>
        public static List<T> GetAllObjectsOfType<T>(this InGame inGame) where T : RootObject, new()
        {
            var factory = inGame.GetMainFactory()?.GetFactory<T>();
#if BloonsTD6
            return factory?.all?.ToList() ?? new List<T>();
#elif BloonsAT
            return factory?.active?.ToList();
#endif
        }

        /// <summary>
        /// Sells multiple towers
        /// </summary>
        public static void SellTowers(this InGame inGame, List<Tower> towers)
        {
            towers.ForEach(inGame.SellTower);
        }

        /// <summary>
        /// Sells a tower
        /// </summary>
        public static void SellTower(this InGame inGame, Tower tower)
        {
            inGame.SellTower(tower.GetTowerToSim());
        }
    }
}
