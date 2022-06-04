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

namespace AOCMod
{

    //[BepInDependency(R2API.R2API.PluginGUID)]

    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]

    public class AOC : BaseUnityPlugin
    {

        //We will be using 2 modules from R2API: ItemAPI to add our item and LanguageAPI to add our language tokens.
        //[R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI))]

        //BaseUnityPlugin itself inherits from MonoBehaviour, so you can use this as a reference for what you can declare and use in your plugin class: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html


        //The Plugin GUID should be a unique ID for this plugin, which is human readable (as it is used in places like the config).
        //If we see this PluginGUID as it is on thunderstore, we will deprecate this mod. Change the PluginAuthor and the PluginName !
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "CuteDoge";
        public const string PluginName = "ArtifactOfChosen";
        public const string PluginVersion = "0.1.0";

        private static ArtifactDef AoC;
        //AOC = ArtifactOfChosen

        public static PluginInfo pluginInfo;

        public void Awake()
        {
            //Artifact art = new Artifact();
            //PopulateAssets();
            pluginInfo = this.Info;
            //R2API.ContentAddition.AddArtifactDef();

            //On.EntityStates.Huntress.ArrowRain.OnEnter += (orig, self) =>
            {
                // [The code we want to run]

                // This will be printed in the console.
                //Log.LogInfo("You used Huntress's Arrow Rain!");

                //Chat.AddMessage("You used Huntress's Arrow Rain!");

                // Call the original function (orig)
                // on the object it's normally called on (self)
                //orig(self);
            };

            //AOCMod.ContentPackProvider.Initialize();
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
