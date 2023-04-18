using System;
using System.Collections;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

namespace Edgar.Unity.Examples.Metroidvania
{
    public class MetroidvaniaGameManager : GameManagerBase<MetroidvaniaGameManager>
    {
        public static MetroidvaniaGameManager Instance = new MetroidvaniaGameManager();
        public MetroidvaniaLevelType LevelType;
        private long generatorElapsedMilliseconds;

        // To make sure that we do not start the generator multiple times
        private bool isGenerating;

        public static readonly string LevelMapLayer = "LevelMap";
        //public static readonly string StaticEnvironmentLayer = "StaticEnvironment";
        public static readonly string StaticEnvironmentLayer = "StaticEnvironment";

        protected override void SingletonAwake()
        {
            if (LayerMask.NameToLayer(StaticEnvironmentLayer) == -1)
            {
                throw new Exception($"\"{StaticEnvironmentLayer}\" layer is needed for this example to work. Please create this layer.");
            }

            LoadNextLevel();

            var player = GameObject.Find("Player").GetComponent<Player>();
            var texto = (Text) Canvas.transform.Find("HP").GetComponent<Text>();
            player.hpChange = hp => { texto.text = "HP " + hp.ToString(); };

            Instance = this;
        }

        public void Update()
        {
            if (InputHelper.GetKeyDown(KeyCode.G) && !isGenerating)
            {
                LoadNextLevel();
            }

            if (InputHelper.GetKeyDown(KeyCode.U))
            {
                Canvas.SetActive(!Canvas.activeSelf);
            }
        }

        public override void LoadNextLevel()
        {
            isGenerating = true;

            // Show loading screen
            ShowLoadingScreen($"Metroidvania - {LevelType}", "loading..");

            // Find the generator runner
            var generator = GameObject.Find($"Platformer Generator").GetComponent<PlatformerGeneratorGrid2D>();
            //generator.CustomPostProcessTasks[0].

            // Start the generator coroutine
            StartCoroutine(GeneratorCoroutine(generator));

            //// Restar life to player
            //var player = GameObject.FindGameObjectWithTag("Player");
            //if (player != null)
            //{
            //    var sPlayer = player.GetComponent<Player>();
            //    if (sPlayer.Unit != null)
            //    {
            //        sPlayer.Unit.AddHP(sPlayer.Unit.HealthPoints);
            //    } else
            //    {
            //        Console.WriteLine("error");
            //    }
            //}
        }


        private IEnumerator GeneratorCoroutine(PlatformerGeneratorGrid2D generator)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var generatorCoroutine = this.StartSmartCoroutine(generator.GenerateCoroutine());

            yield return generatorCoroutine.Coroutine;

            yield return null;

            stopwatch.Stop();

            isGenerating = false;

            generatorCoroutine.ThrowIfNotSuccessful();

            generatorElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            RefreshLevelInfo();
            HideLoadingScreen();

            //var generatedLevel = GameObject.Find("Generated Level");
            //var tileObject = generatedLevel.transform.Find("Tilemaps").gameObject;

            //var walls = tileObject.transform.Find("Walls");
            //var plats = tileObject.transform.Find("Platforms");

            //if (walls != null)
            //{                
            //    var paredes = walls.gameObject.GetComponent<Rigidbody2D>();
            //    var paredes2 = walls.gameObject.GetComponent<CompositeCollider2D>();

            //    if (paredes != null)
            //    {
            //        paredes.sharedMaterial.friction = 0;
            //        paredes.sharedMaterial.bounciness = 0;
            //    }

            //    if (paredes2 != null)
            //    {
            //        paredes2.sharedMaterial.friction = 0;
            //        paredes2.sharedMaterial.bounciness = 0;
            //    }
            //}

            //if (plats != null)
            //{                
            //    var plataformas = plats.gameObject.GetComponent<Rigidbody2D>();
            //    var plataformas2 = plats.gameObject.GetComponent<CompositeCollider2D>();

            //    if (plataformas != null)
            //    {
            //        plataformas.sharedMaterial.friction = 0;
            //        plataformas.sharedMaterial.bounciness = 0;
            //    }

            //    if (plataformas2 != null)
            //    {
            //        plataformas2.sharedMaterial.friction = 0;
            //        plataformas2.sharedMaterial.bounciness = 0;
            //    }
            //}


            //var paredes = tileObject.GetComponent<Rigidbody2D>();
            //var paredes2 = tileObject.GetComponent<CompositeCollider2D>();

            //if (paredes != null)
            //{
            //    paredes.sharedMaterial.friction = 0;
            //    paredes.sharedMaterial.bounciness = 0;
            //}

            //if (paredes2 != null)
            //{
            //    paredes2.sharedMaterial.friction = 0;
            //    paredes2.sharedMaterial.bounciness = 0;
            //}
        }

        private void RefreshLevelInfo()
        {
            SetLevelInfo($"Generated in {generatorElapsedMilliseconds / 1000d:F}s\nLevel type: {LevelType}");
        }

        public bool LevelMapSupported()
        {
            var layer = LayerMask.NameToLayer(LevelMapLayer);

            if (layer == -1)
            {
                Debug.Log($"Level map is currently not supported. Please add a layer called \"{LevelMapLayer}\" to enable this feature and then restart the game.");
            }

            return layer != -1;
        }
    }
}