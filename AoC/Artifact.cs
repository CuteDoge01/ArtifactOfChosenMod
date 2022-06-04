using RoR2;
using BepInEx.Bootstrap;
using UnityEngine;
//using MonoMod.RuntimeDetour;
using System.Reflection;
using System;
using System.Collections.Generic;
using static RoR2.Chat;
using RoR2.ExpansionManagement;

namespace AOCMod
{
    public class Artifact
    {
        public static ArtifactDef MyArtifactDef = ContentPackProvider.contentPack.artifactDefs.Find("ArtifactOfChosen");

        private static System.Random random = new System.Random();
        private static int chosenID = -1;
        private static int aspectID = 0;
        private static SceneType lastSceneType = RoR2.SceneType.Invalid;
        private static List<string> aspectNames = new List<string> { "ZetAspectBlue", "ZetAspectEarth", "ZetAspectHaunted", "ZetAspectLunar", "ZetAspectPoison", "ZetAspectRed", "ZetAspectVoid", "ZetAspectWhite" };

        public static void InitializeArtifact()
        {

            AddIntegrations();
            Hooks();
        }
        public static void Hooks()
        {
            RoR2.Stage.onStageStartGlobal += AOCProvideAspect;
            //RoR2.Stage.onServerStageComplete += AOCTakeAwayAspectFallback;
            TeleporterInteraction.onTeleporterChargedGlobal += AOCTakeAwayAspect;
        }

        public static bool PluginLoaded(string key)
        {
            bool foundPlugin = false;
            foreach (var plugin in Chainloader.PluginInfos)
            {
                var metadata = plugin.Value.Metadata;
                if (metadata.GUID.Equals(key))
                {
                    foundPlugin = true;
                    break;
                }
            }
            return foundPlugin;
        }

        private static void AddIntegrations()
        {
            if (PluginLoaded("com.Skell.GoldenCoastPlus"))
            {
                aspectNames.Add("ZetAspectGold");
            }
            if (PluginLoaded("com.KomradeSpectre.Aetherium"))
            {
                aspectNames.Add("ZetAspectSanguine");
            }
            if (PluginLoaded("com.groovesalad.GrooveSaladSpikestripContent"))
            {
                aspectNames.Add("ZetAspectPlated");
                aspectNames.Add("ZetAspectWarped");
            }
        }

        private static void AOCProvideAspect(Stage obj)
        {
            if (RunArtifactManager.instance.IsArtifactEnabled(MyArtifactDef))
            {
                //Chat.SendBroadcastChat(new SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { $"111 chosenID: {chosenID}, AspectID: {aspectID}, NAME: {GetZetAspectID(aspectID)}, MAX PlayerID: {PlayerCharacterMasterController.instances.Count - 1}" } });
                chosenID = random.Next(PlayerCharacterMasterController.instances.Count);
                aspectID = random.Next(aspectNames.Count);
                try
                {
                    //Chat.AddMessage($"Current max aspect ID count is: {aspectNames.Count}");
                    CharacterMaster.readOnlyInstancesList[chosenID].inventory.GiveItemString(aspectNames[aspectID]);
                }
                catch { Chat.SendBroadcastChat(new SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { $"Could not add the item to the inventory of the player ID { chosenID }, perhaps CharacterMaster.readOnlyInstancesList has not been created or the ID is wrong" } }); }

                Chat.SendBroadcastChat(new SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { $"The artifact has chosen {PlayerCharacterMasterController.instances[chosenID].GetDisplayName()}" } });
                
                if (lastSceneType == SceneType.Intermission)
                {
                    CharacterMaster.readOnlyInstancesList[chosenID].inventory.RemoveItem(ItemCatalog.FindItemIndex(aspectNames[aspectID]));
                }

                lastSceneType = obj.sceneDef.sceneType;
                //TPDespair.ZetAspects.Language.tokens.ContainsKey(ItemCatalog.GetItemDef(ItemCatalog.FindItemIndex(aspectNames[aspectID])).nameToken);
                //CharacterMaster.readOnlyInstancesList[chosenID].inventory.GiveItemString(GetZetAspectID(0));
                //     PlayerCharacterMasterController.instances[chosenID].master.GetBody().inventory.GiveItemString(GetZetAspectID(aspectID));
            }
        }

        private static void AOCTakeAwayAspect(TeleporterInteraction obj)
        {
            try
            {
                CharacterMaster.readOnlyInstancesList[chosenID].inventory.RemoveItem(ItemCatalog.FindItemIndex(aspectNames[aspectID]));
            }
            catch { Chat.SendBroadcastChat(new SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { $"Could not remove the item from the inventory of the player ID { chosenID }, perhaps CharacterMaster.readOnlyInstancesList has not been created or the ID is wrong" } }); }
        }
    }
}