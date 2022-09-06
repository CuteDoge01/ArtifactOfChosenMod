using RoR2;
using BepInEx.Bootstrap;
//using MonoMod.RuntimeDetour;
using System.Collections.Generic;
using static RoR2.Chat;
using AoC;

namespace AOCMod
{
    public class Artifact
    {
        public static ArtifactDef MyArtifactDef = ContentPackProvider.contentPack.artifactDefs.Find("ArtifactOfChosen");

        private static System.Random random = new System.Random();
        private static int chosenID = -1;
        private static int aspectID = 0;
        private static SceneType lastSceneType = SceneType.Invalid;
        private static List<string> aspectNames = new List<string> { "ZetAspectBlue", "ZetAspectEarth", "ZetAspectHaunted", "ZetAspectLunar", "ZetAspectPoison", "ZetAspectRed", "ZetAspectVoid", "ZetAspectWhite" };
        private static string firstArtifactName;
        private static int numOfAspects;
        private static bool IsFirstStage = true;
        private static List<List<int>> aspectsGiven = new List<List<int>>();
        private static bool dirty = false;

        public static void InitializeArtifact()
        {
            AddIntegrations();
            firstArtifactName = AOCConfig.FirstArtifactGivenConf.Value;
            numOfAspects = AOCConfig.NumOfAspectsPerStageConf.Value;
            MyArtifactDef.nameToken = "AOC_ARTIFACT_NAME";
            MyArtifactDef.descriptionToken = "AOC_ARTIFACT_DESC";
            if (numOfAspects <= 0) numOfAspects = 1;
            Hooks();
        }

        public static void Hooks()
        {
            Stage.onStageStartGlobal += AOCProvideAspect;
            Run.onRunStartGlobal += RunStartInit;
            //RoR2.Stage.onServerStageComplete += AOCTakeAwayAspectFallback;
            TeleporterInteraction.onTeleporterChargedGlobal += AOCTakeAwayAspect;
            Language.onCurrentLanguageChanged += () =>
            {
                var lang = Language.GetOrCreateLanguage("en");
                lang.SetStringByToken("AOC_ARTIFACT_NAME", "Artifact of Chosen");
                lang.SetStringByToken("AOC_ARTIFACT_DESC", "Gain Elite Aspects that changes every stage.");
            };
        }

        private static void RunStartInit(Run _)
        {
            IsFirstStage = true;
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
            if (PluginLoaded("bubbet.bubbetsitems"))
            {
                aspectNames.Add("ZetAspectSepia");
            }
            if (PluginLoaded("com.groovesalad.GrooveSaladSpikestripContent"))
            {
                aspectNames.Add("ZetAspectPlated");
                aspectNames.Add("ZetAspectWarped");
                aspectNames.Add("ZetAspectVeiled");
                aspectNames.Add("ZetAspectAragonite");
            }
            var list = AOCConfig.list.Value.Split(',');
            var aspects = new List<string>();
            foreach (var raw in list)
            {
                var item = raw.Trim();
                if (aspectNames.Contains(item)) aspects.Add(item);
            }
            aspectNames = aspects;
        }

        private static void AOCProvideAspect(Stage stage)
        {
            if (RunArtifactManager.instance.IsArtifactEnabled(MyArtifactDef))
            {
                aspectID = -1;

                if (IsFirstStage)
                {
                    aspectsGiven.Clear();
                    for (var _ = 0; _ < PlayerCharacterMasterController.instances.Count; _++) aspectsGiven.Add(new List<int>());
                    dirty = false;
                    IsFirstStage = false;
                    if (AOCConfig.mode.Value == AOCConfig.AOCMode.Random) chosenID = random.Next(PlayerCharacterMasterController.instances.Count);
                    if (aspectNames.Contains(firstArtifactName)) aspectID = aspectNames.IndexOf(firstArtifactName);
                }

                if (dirty) AOCTakeAwayAspectInternal();
                lastSceneType = stage.sceneDef.sceneType;

                // get chosen
                if (AOCConfig.mode.Value == AOCConfig.AOCMode.RandomEachStage) chosenID = random.Next(PlayerCharacterMasterController.instances.Count);
                if (AOCConfig.mode.Value == AOCConfig.AOCMode.Host) chosenID = 0;

                // get aspect
                if (AOCConfig.multistack.Value) aspectID = random.Next(aspectNames.Count);
                if (chosenID == -1) for (var i = 0; i < PlayerCharacterMasterController.instances.Count; i++) // for all
                {
                    if (AOCConfig.mode.Value == AOCConfig.AOCMode.AllRandom && AOCConfig.multistack.Value) aspectID = random.Next(aspectNames.Count);
                    for (var _ = 0; _ < numOfAspects; _++) AOCProvideAspectInternal(i, aspectID == -1 ? random.Next(aspectNames.Count) : aspectID);
                }
                else
                {
                    for (var _ = 0; _ < numOfAspects; _++) AOCProvideAspectInternal(chosenID, aspectID == -1 ? random.Next(aspectNames.Count) : aspectID);
                    if (PlayerCharacterMasterController.instances.Count != 1) SendBroadcastChat(new SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { $"The artifact has chosen {PlayerCharacterMasterController.instances[chosenID].GetDisplayName()}" } });
                }
                dirty = true;
            }
        }

        private static void AOCProvideAspectInternal(int chosen, int aspect)
        {
            try
            {
                CharacterMaster.readOnlyInstancesList[chosen].inventory.GiveItemString(aspectNames[aspect]);
                aspectsGiven[chosen].Add(aspect);
            }
            catch { AOC.log.LogError($"Could not add the item to the inventory of the player ID { chosenID }, perhaps CharacterMaster.readOnlyInstancesList has not been created or the ID is wrong"); }
        }

        private static void AOCTakeAwayAspect(TeleporterInteraction _)
        {
            if (RunArtifactManager.instance.IsArtifactEnabled(MyArtifactDef) && !AOCConfig.postTeleporterAspect.Value) AOCTakeAwayAspectInternal();
        }

        private static void AOCTakeAwayAspectInternal()
        {
            try
            {
                for (int i = 0; i < PlayerCharacterMasterController.instances.Count; i++) foreach (var aspect in aspectsGiven[i])
                        CharacterMaster.readOnlyInstancesList[i].inventory.RemoveItem(ItemCatalog.FindItemIndex(aspectNames[aspect]));
            }
            catch { AOC.log.LogError($"Could not remove the item from the inventory of the player ID { chosenID }, perhaps CharacterMaster.readOnlyInstancesList has not been created or the ID is wrong"); }
            for (var i = 0; i < PlayerCharacterMasterController.instances.Count; i++) aspectsGiven[i].Clear();
            dirty = false;
        }
    }
}
