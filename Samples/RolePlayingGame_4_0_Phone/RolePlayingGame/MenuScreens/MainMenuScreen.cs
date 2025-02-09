#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RolePlayingGameData;
#endregion

namespace RolePlaying
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Graphics Data


        private Texture2D backgroundTexture;
        private Vector2 backgroundPosition;

        private Texture2D descriptionAreaTexture;
        private Vector2 descriptionAreaPosition;
        private Vector2 descriptionAreaTextPosition;

        private Texture2D iconTexture;
        private Vector2 iconPosition;

        private Texture2D backTexture;
        private Vector2 backPosition;

        private Texture2D selectTexture;
        private Vector2 selectPosition;

        private Texture2D plankTexture1, plankTexture2, plankTexture3;


        #endregion


        #region Menu Entries


        MenuEntry newGameMenuEntry, exitGameMenuEntry; 
        MenuEntry saveGameMenuEntry, loadGameMenuEntry;
        MenuEntry controlsMenuEntry, helpMenuEntry;


        #endregion


        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base()
        {

            // add the New Game entry
            newGameMenuEntry = new MenuEntry("New Game");
            newGameMenuEntry.Description = "Start a New Game";
            newGameMenuEntry.Font = Fonts.HeaderFont;
            newGameMenuEntry.Position = ScaledVector2.GetScaledVector(765, 0f);
            newGameMenuEntry.Selected += NewGameMenuEntrySelected;
            MenuEntries.Add(newGameMenuEntry);

            // add the Save Game menu entry, 
            // if the game has started but is not in combat
            if (Session.IsActive && !CombatEngine.IsActive)
            {
                saveGameMenuEntry = new MenuEntry("Save Game");
                saveGameMenuEntry.Description = "Save the Game";
                saveGameMenuEntry.Font = Fonts.HeaderFont;
                saveGameMenuEntry.Position =  ScaledVector2.GetScaledVector(790, 0f);
                saveGameMenuEntry.Selected += SaveGameMenuEntrySelected;
                MenuEntries.Add(saveGameMenuEntry);
            }
            else
            {
                saveGameMenuEntry = null;
            }

            // add the Load Game menu entry
            loadGameMenuEntry = new MenuEntry("Load Game");
            loadGameMenuEntry.Description = "Load the Game";
            loadGameMenuEntry.Font = Fonts.HeaderFont;
            loadGameMenuEntry.Position = ScaledVector2.GetScaledVector(750, 0f);
            loadGameMenuEntry.Selected += LoadGameMenuEntrySelected;
            MenuEntries.Add(loadGameMenuEntry);

            // add the Controls menu entry
            controlsMenuEntry = new MenuEntry("Controls");
            controlsMenuEntry.Description = "View Game Controls";
            controlsMenuEntry.Font = Fonts.HeaderFont;
            controlsMenuEntry.Position = ScaledVector2.GetScaledVector(770, 0f);
            controlsMenuEntry.Selected += ControlsMenuEntrySelected;
            //MenuEntries.Add(controlsMenuEntry);

            // add the Help menu entry
            helpMenuEntry = new MenuEntry("Help");
            helpMenuEntry.Description = "View Game Help";
            helpMenuEntry.Font = Fonts.HeaderFont;
            helpMenuEntry.Position = ScaledVector2.GetScaledVector(750, 0f);
            helpMenuEntry.Selected += HelpMenuEntrySelected;
            MenuEntries.Add(helpMenuEntry);

            // create the Exit menu entry
            exitGameMenuEntry = new MenuEntry("Exit");
            exitGameMenuEntry.Description = "Quit the Game";
            exitGameMenuEntry.Font = Fonts.HeaderFont;
            exitGameMenuEntry.Position = ScaledVector2.GetScaledVector(770, 0f);
            exitGameMenuEntry.Selected += OnCancel;
            MenuEntries.Add(exitGameMenuEntry);


            // start the menu music
            AudioManager.PushMusic("MainTheme",true);
        }


        /// <summary>
        /// Load the graphics content for this screen.
        /// </summary>
        public override void LoadContent()
        {
            // load the textures
            ContentManager content = ScreenManager.Game.Content;
            backgroundTexture = content.Load<Texture2D>(@"Textures\MainMenu\MainMenu");
            descriptionAreaTexture = 
                content.Load<Texture2D>(@"Textures\MainMenu\MainMenuInfoSpace");
            iconTexture = content.Load<Texture2D>(@"Textures\MainMenu\GameLogo");
            plankTexture1 = 
                content.Load<Texture2D>(@"Textures\MainMenu\MainMenuPlank");
            plankTexture2 = 
                content.Load<Texture2D>(@"Textures\MainMenu\MainMenuPlank02");
            plankTexture3 = 
                content.Load<Texture2D>(@"Textures\MainMenu\MainMenuPlank03");
            backTexture = content.Load<Texture2D>(@"Textures\Buttons\rpgBtn");
            selectTexture = content.Load<Texture2D>(@"Textures\Buttons\rpgBtn");

            // calculate the texture positions
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            backgroundPosition = new Vector2(
                (viewport.Width - (backgroundTexture.Width * ScaledVector2.DrawFactor) ) / 2,
                (viewport.Height - (backgroundTexture.Height * ScaledVector2.DrawFactor)) / 2);
            descriptionAreaPosition = backgroundPosition + ScaledVector2.GetScaledVector(158, 130);
            descriptionAreaTextPosition = backgroundPosition + ScaledVector2.GetScaledVector(158, 350);
            iconPosition = backgroundPosition + ScaledVector2.GetScaledVector(170, 80);
            backPosition = backgroundPosition + ScaledVector2.GetScaledVector(220, 605);
            selectPosition = backgroundPosition + ScaledVector2.GetScaledVector(1060, 600);

            // set the textures on each menu entry
            newGameMenuEntry.Texture = plankTexture3;
            if (saveGameMenuEntry != null)
            {
                saveGameMenuEntry.Texture = plankTexture2;
            }
            loadGameMenuEntry.Texture = plankTexture1;
            controlsMenuEntry.Texture = plankTexture2;
            helpMenuEntry.Texture = plankTexture3;
            exitGameMenuEntry.Texture = plankTexture1;

            // now that they have textures, set the proper positions on the menu entries
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntries[i].Position = new Vector2(
                    MenuEntries[i].Position.X + 50,
                    (500 * ScaledVector2.ScaleFactor) - 
                    (((MenuEntries[i].Texture.Height * ScaledVector2.DrawFactor) + 10) * 
                    (MenuEntries.Count - 1 - i)));
            }

            base.LoadContent();
        }

        #endregion


        #region Updating


        /// <summary>
        /// Handles user input.
        /// </summary>
        public override void HandleInput()
        {
            bool resumeClicked = false;
            if(InputManager.IsButtonClicked(new Rectangle(
                (int)(backPosition.X), 
                (int)(backPosition.Y),
                (int)(backTexture.Width * ScaledVector2.DrawFactor),
                (int)(backTexture.Height * ScaledVector2.DrawFactor))))
                {
                    resumeClicked = true;
                }


            if (InputManager.IsActionTriggered(InputManager.Action.Back) || resumeClicked &&
                Session.IsActive)
            {
                Session.MapCache.Clear();
                AudioManager.PopMusic();
                ExitScreen();
                return;
            }

            base.HandleInput();
        }


        /// <summary>
        /// Event handler for when the New Game menu entry is selected.
        /// </summary>
        void NewGameMenuEntrySelected(object sender, EventArgs e)
        {
            ContentManager content = ScreenManager.Game.Content;
            if (Session.IsActive)
            {
                ExitScreen();
                //ScreenManager.Game.Content.Unload();
            }

            Session.MapCache.Clear();
        
           
            LoadingScreen.Load(ScreenManager, true, new GameplayScreen(
                content.Load<GameStartDescription>("MainGameDescription")));
        }


        /// <summary>
        /// Event handler for when the Save Game menu entry is selected.
        /// </summary>
        void SaveGameMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(
                new SaveLoadScreen(SaveLoadScreen.SaveLoadScreenMode.Save));
        }


        /// <summary>
        /// Event handler for when the Load Game menu entry is selected.
        /// </summary>
        void LoadGameMenuEntrySelected(object sender, EventArgs e)
        {
            SaveLoadScreen loadGameScreen = 
                new SaveLoadScreen(SaveLoadScreen.SaveLoadScreenMode.Load);
            loadGameScreen.LoadingSaveGame += new SaveLoadScreen.LoadingSaveGameHandler(
                loadGameScreen_LoadingSaveGame);
            ScreenManager.AddScreen(loadGameScreen);
        }


        /// <summary>
        /// Handle save-game-to-load-selected events from the SaveLoadScreen.
        /// </summary>
        void loadGameScreen_LoadingSaveGame(SaveGameDescription saveGameDescription)
        {
            if (Session.IsActive)
            {
                ExitScreen();
            }
            LoadingScreen.Load(ScreenManager, true, 
                new GameplayScreen(saveGameDescription));
        }


        /// <summary>
        /// Event handler for when the Controls menu entry is selected.
        /// </summary>
        void ControlsMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new ControlsScreen());
        }


        /// <summary>
        /// Event handler for when the Help menu entry is selected.
        /// </summary>
        void HelpMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new HelpScreen());
        }

        
        /// <summary>
        /// When the user cancels the main menu,
        /// or when the Exit Game menu entry is selected.
        /// </summary>
        protected override void OnCancel()
        {
            // add a confirmation message box
            string message = String.Empty;
            if (Session.IsActive)
            {
                message = 
                    "Are you sure you want to exit?  All unsaved progress will be lost.";
            }
            else
            {
                message = "Are you sure you want to exit?";
            }
            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);
            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;
            ScreenManager.AddScreen(confirmExitMessageBox);
        }


        /// <summary>
        /// Event handler for when the user selects Yes 
        /// on the "Are you sure?" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }
        

        #endregion


        #region Drawing


        /// <summary>
        /// Draw this screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            // draw the background images
            spriteBatch.Draw(backgroundTexture, backgroundPosition, null, Color.White, 0f,
                                Vector2.Zero, ScaledVector2.DrawFactor, SpriteEffects.None, 0f);

            spriteBatch.Draw(descriptionAreaTexture, descriptionAreaPosition, null, Color.White, 0f,
                                Vector2.Zero, ScaledVector2.DrawFactor,SpriteEffects.None,0f);

            spriteBatch.Draw(iconTexture, iconPosition, null, Color.White, 0f,
                                Vector2.Zero, ScaledVector2.DrawFactor,SpriteEffects.None,0f);

            // Draw each menu entry in turn.
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntry menuEntry = MenuEntries[i];
                bool isSelected = IsActive && (i == selectedEntry);
                menuEntry.Draw(this, isSelected, gameTime);
            }

            // draw the description text for the selected entry
            MenuEntry selectedMenuEntry = SelectedMenuEntry;
            if ((selectedMenuEntry != null) &&
                !String.IsNullOrEmpty(selectedMenuEntry.Description))
            {
                Vector2 textSize =
                    Fonts.DescriptionFont.MeasureString(selectedMenuEntry.Description);
                Vector2 textPosition2 = descriptionAreaTextPosition + new Vector2(
                    (float)Math.Floor((descriptionAreaTexture.Width - textSize.X) / 2),
                    0f);
            }

            // if we are in-game, draw the back instruction
            if (Session.IsActive )
            {
                spriteBatch.Draw(backTexture, backPosition, null, Color.White, 0f,
                    Vector2.Zero, ScaledVector2.DrawFactor, SpriteEffects.None, 0f);

                string text =  "Resume";
                Vector2 textPosition = Fonts.GetCenterPositionInButton(Fonts.ButtonNamesFont,text,
                    new Rectangle((int)backPosition.X,(int)backPosition.Y,backTexture.Width,backTexture.Height));

                spriteBatch.DrawString(Fonts.ButtonNamesFont,text,textPosition,Color.White);
            }

            spriteBatch.End();
        }

        #endregion
    }
}
