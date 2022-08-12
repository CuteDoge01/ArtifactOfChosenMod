using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BepInEx;
using BepInEx.Logging;
using RoR2.ContentManagement;
using RoR2;
using static AOCMod.AssetLoad;
using static RoR2.Chat;
using static RoR2.Console;
using BepInEx.Configuration;

namespace AOCMod
{

    //[BepInDependency(R2API.R2API.PluginGUID)]

    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]

    public class AOC : BaseUnityPlugin
    {
        public static ConfigEntry<int> NumOfAspectsPerStageConf { get; set; }
        public static ConfigEntry<string> FirstArtifactGivenConf { get; set; }
        //We will be using 2 modules from R2API: ItemAPI to add our item and LanguageAPI to add our language tokens.
        //[R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI))]

        //BaseUnityPlugin itself inherits from MonoBehaviour, so you can use this as a reference for what you can declare and use in your plugin class: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html

        //The Plugin GUID should be a unique ID for this plugin, which is human readable (as it is used in places like the config).
        //If we see this PluginGUID as it is on thunderstore, we will deprecate this mod. Change the PluginAuthor and the PluginName !
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "CuteDoge";
        public const string PluginName = "ArtifactOfChosen";
        public const string PluginVersion = "1.0.7";

        private static ArtifactDef AoC;
        //AOC = ArtifactOfChosen

        public static PluginInfo pluginInfo;

        public void Awake()
        {
            pluginInfo = this.Info;
            NumOfAspectsPerStageConf = Config.Bind<int>(
                "General",
                "NumOfArtifactsPerStage",
                1,
                "This is the amount of artifacts thet will be given to the chosen player each stage"
            );
            pluginInfo = this.Info;
            FirstArtifactGivenConf = Config.Bind<string>(
                "General",
                "FirstArtifactGiven",
                "Random",
                "Specifies if you want to get a concrete artifact at the start. Available options: ZetAspectBlue, ZetAspectEarth, ZetAspectHaunted, ZetAspectLunar, ZetAspectPoison, ZetAspectRed, ZetAspectVoid, ZetAspectWhite. Anything else will make it random. Also integrations Aspects are supported too."
            );

        }


        // Start is called before the first frame update
        void Start()
        {
            PopulateAssets();
            AOCMod.ContentPackProvider.Initialize();
            Artifact.InitializeArtifact();
        }

        // Update is called once per frame
        void Update()
        {
            //if (i > 200)
            {
                //Chat.SendBroadcastChat(new SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { "200 tick" } });
                //i = 0;
            }

            //i++;
            //Chat.SendBroadcastChat(new SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] {  } });
            //Chat.AddMessage($"{i}");
        }

    }

}
