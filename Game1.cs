using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Number2.Content;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;

namespace Number2 {
    public class Game1 : Game {
        SpriteFont font;
        public static Texture2D Gun;

        public Rectangle quad1;
        public Rectangle quad2;
        public Rectangle quad3;
        public Rectangle quad4;

        List<Enemy> quad1List = new List<Enemy>();
        List<Enemy> quad2List = new List<Enemy>();
        List<Enemy> quad3List = new List<Enemy>();
        List<Enemy> quad4List = new List<Enemy>();

        private GraphicsDeviceManager _graphics;
        private SpriteBatch create;

        public static Rectangle bounds;
        public static Texture2D Base;

        private Player Player;
        private Bullet Bullet;
        
        private Enemy Enemy;
        private Enemy Enemy2;
        private Enemy Enemy3;

        private BasicEnemy BasicEnemy;

        public float timeSpawn;

        List<Rectangle> xpList = new List<Rectangle>();
        List<Bullet> BulletList = new List<Bullet>();

        //Enemy Lists
        List<Enemy> AllEnemies= new List<Enemy>();

        List<Enemy> BasicEnemies = new List<Enemy>();
        List<Enemy> RangedEnemies = new List<Enemy>(); List<RangedAttack> RangedAttacks = new List<RangedAttack>();
        List<Enemy> SkellyEnemies = new List<Enemy>();
        List<Enemy> Trolls = new List<Enemy>();

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        protected override void Initialize() {

            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.ApplyChanges();

            Base = new Texture2D(GraphicsDevice, 1, 1);
            Base.SetData(new Color[] { Color.White });
            bounds = GraphicsDevice.Viewport.Bounds;

            quad1 = new Rectangle(bounds.Left, bounds.Top, bounds.Width/2, bounds.Height/2);
            quad2 = new Rectangle(bounds.Center.X, bounds.Top, bounds.Width / 2, bounds.Height / 2);
            quad3 = new Rectangle(bounds.Left, bounds.Center.Y, bounds.Width / 2, bounds.Height / 2);
            quad4 = new Rectangle(bounds.Center.X, bounds.Center.Y, bounds.Width / 2, bounds.Height / 2);

            var playerRect = new Rectangle(bounds.Center.X - 10, bounds.Center.Y - 10, 20, 20);
            Player = new Player(playerRect);

            base.Initialize();
        }

        protected override void LoadContent() {
            create = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/Retronoid");

            Gun = Content.Load<Texture2D>("Bow");

        }

        protected override void Update(GameTime gameTime) {

            //Ene Alive Dection
            for (int i = 0; i < BasicEnemies.Count; i++) {
                if (!BasicEnemies[i].isAlive) {
                    var xp = new Rectangle(new Point((int)BasicEnemies[i].rect.Center.X, (int)BasicEnemies[i].rect.Center.Y), new Point(4, 4));
                    xpList.Add(xp);
                    if (quad1List.Contains(BasicEnemies[i])) { quad1List.Remove(BasicEnemies[i]); }
                    if (quad2List.Contains(BasicEnemies[i])) { quad2List.Remove(BasicEnemies[i]); }
                    if (quad3List.Contains(BasicEnemies[i])) { quad3List.Remove(BasicEnemies[i]); }
                    if (quad4List.Contains(BasicEnemies[i])) { quad4List.Remove(BasicEnemies[i]); }
                    AllEnemies.Remove(BasicEnemies[i]);
                    BasicEnemies.RemoveAt(i);
                }
            }

            for (int i = 0; i < RangedEnemies.Count; i++) {
                if (!RangedEnemies[i].isAlive) {
                    var xp = new Rectangle(new Point((int)RangedEnemies[i].rect.Center.X, (int)RangedEnemies[i].rect.Center.Y), new Point(4, 4));
                    xpList.Add(xp);
                    if (quad1List.Contains(RangedEnemies[i])) { quad1List.Remove(RangedEnemies[i]); }
                    if (quad2List.Contains(RangedEnemies[i])) { quad2List.Remove(RangedEnemies[i]); }
                    if (quad3List.Contains(RangedEnemies[i])) { quad3List.Remove(RangedEnemies[i]); }
                    if (quad4List.Contains(RangedEnemies[i])) { quad4List.Remove(RangedEnemies[i]); }
                    AllEnemies.Remove(RangedEnemies[i]);
                    RangedEnemies.RemoveAt(i);
                }
            }
            
            for (int i = 0; i < SkellyEnemies.Count; i++) {
                if (!SkellyEnemies[i].isAlive) {
                    var xp = new Rectangle(new Point((int)SkellyEnemies[i].rect.Center.X, (int)SkellyEnemies[i].rect.Center.Y), new Point(4, 4));
                    xpList.Add(xp);
                    if (quad1List.Contains(SkellyEnemies[i])) { quad1List.Remove(SkellyEnemies[i]); }
                    if (quad2List.Contains(SkellyEnemies[i])) { quad2List.Remove(SkellyEnemies[i]); }
                    if (quad3List.Contains(SkellyEnemies[i])) { quad3List.Remove(SkellyEnemies[i]); }
                    if (quad4List.Contains(SkellyEnemies[i])) { quad4List.Remove(SkellyEnemies[i]); }
                    AllEnemies.Remove(SkellyEnemies[i]);
                    SkellyEnemies.RemoveAt(i);
                }
            }
            for (int i = 0; i < Trolls.Count; i++) {
                if (!Trolls[i].isAlive) {
                    var xp = new Rectangle(new Point((int)Trolls[i].rect.Center.X, (int)Trolls[i].rect.Center.Y), new Point(4, 4));
                    xpList.Add(xp);
                    if (quad1List.Contains(Trolls[i])) { quad1List.Remove(Trolls[i]); }
                    if (quad2List.Contains(Trolls[i])) { quad2List.Remove(Trolls[i]); }
                    if (quad3List.Contains(Trolls[i])) { quad3List.Remove(Trolls[i]); }
                    if (quad4List.Contains(Trolls[i])) { quad4List.Remove(Trolls[i]); }
                    AllEnemies.Remove(Trolls[i]);
                    Trolls.RemoveAt(i);
                }
            }

            //Shooting & bullet detection
            if (Mouse.GetState().LeftButton == ButtonState.Pressed) {
                if (Player.timeLastShot > 0) {
                    Player.timeLastShot -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else {
                    Bullet = new Bullet(Player);
                    BulletList.Add(Bullet);
                    Player.timeLastShot = Player.attackSpeed;
                }
            }
            else if (Mouse.GetState().LeftButton == ButtonState.Released) {
                Player.timeLastShot = 0;
            }

            //Bullet Detection
            for (int i = 0; i < BulletList.Count; i++) {
                BulletList[i].Update(Player);
                BulletList[i].bulletPostion += BulletList[i].bulletDirection * BulletList[i].bulletSpeed;

                if (BulletList[i].BulletCheck(BulletList, BasicEnemies, Player) && BasicEnemies.Count > 0) {
                    BulletList.RemoveAt(i);
                }
                else if (BulletList[i].BulletCheck(BulletList, RangedEnemies, Player) && RangedEnemies.Count > 0) {
                    BulletList.RemoveAt(i);
                }
                else if (BulletList[i].BulletCheck(BulletList, SkellyEnemies, Player) && SkellyEnemies.Count > 0) {
                    BulletList.RemoveAt(i);
                }
                else if (BulletList[i].BulletCheck(BulletList, Trolls, Player) && Trolls.Count > 0) {
                    BulletList.RemoveAt(i);
                }
            }

            for (int i = 0; i < BulletList.Count; i++) {
                if (!bounds.Contains(BulletList[i].bulletPostion)) {
                    BulletList.RemoveAt(i);
                }
            }

            //Player Dash
            if (Keyboard.GetState().IsKeyUp(Keys.Space) && !Player.isRolling) {
                Player.canRoll = true;
            }
            if (!Player.isRolling && Player.canRoll && Keyboard.GetState().IsKeyDown(Keys.Space) && Player.stamina > 0) {
                Player.canRoll = false;
                Player.isRolling = true;
                Player.lastTimeRoll = 0;
                Player.stamina -= 1;
            }
            if (Player.isRolling) {
                Player.dodgeRoll();
                Player.lastTimeRoll += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Player.isRolling && Player.lastTimeRoll >= 0.175f) {
                Player.PlayerSpeed = 2;
                Player.isRolling = false;
            }


            //temp ene Spawn
            if (timeSpawn > 0) {
                timeSpawn -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else {

                Enemy = new BasicEnemy();
                BasicEnemies.Add(Enemy);
                AllEnemies.Add(Enemy);

                    if (Player.playerLVL >= 3) {
                        Enemy = new RangedEnemy();
                        RangedEnemies.Add(Enemy);
                        AllEnemies.Add(Enemy);
                    }
                    if (Player.playerLVL > 5) {
                        Enemy = new Skeleton();
                        SkellyEnemies.Add(Enemy);
                        AllEnemies.Add(Enemy);
                    }
                    if (Player.playerLVL > 8) {
                        Enemy = new Troll();
                        Trolls.Add(Enemy);
                        AllEnemies.Add(Enemy);
                    }
                
                timeSpawn = 0;
            }

            //Enemy Updating

            foreach (BasicEnemy e in BasicEnemies) {
                e.UpdateBasics(Player);
            }
          
            foreach (RangedEnemy e in RangedEnemies) {
                e.UpdateRanged(Player, RangedAttacks);
            }
            for (int i = 0; i < RangedAttacks.Count; i++) {
                RangedAttacks[i].UpdateRangedAttack();
                if (RangedAttacks[i].attackDetect(Player) || !bounds.Contains(RangedAttacks[i].bulletPos)) {
                    RangedAttacks.RemoveAt(i);
                }
            }

            foreach (Skeleton s in SkellyEnemies) { 
                s.UpdateSkelly(Player);
            }

            foreach (Troll t in Trolls) { 
                t.UpdateTrolls(Player);
            }

            Enemy.UpdatePOS(quad1List, quad2List, quad3List, quad4List, quad1, quad2, quad3, quad4, AllEnemies);

            //----------------------------------------------------------------------------------------------------
            
            Player.Update(xpList);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            create.Begin();
            //create.Draw(Gun, new Vector2(0, 0), Color.White);

            /*
            create.Draw(Base, quad1, Color.DarkOliveGreen);
            create.Draw(Base, quad2, Color.MonoGameOrange);
            create.Draw(Base, quad3, Color.BlueViolet);
            create.Draw(Base, quad4, Color.DeepPink);

            create.DrawString(font, quad1List.Count.ToString(), new Vector2(bounds.Left, bounds.Top + 25), Color.White);
            create.DrawString(font, quad2List.Count.ToString(), new Vector2(bounds.Center.X + 100, bounds.Top + 25), Color.White);
            create.DrawString(font, quad3List.Count.ToString(), new Vector2(bounds.Left, bounds.Center.Y + 25), Color.White);
            create.DrawString(font, quad4List.Count.ToString(), new Vector2(bounds.Center.X + 100, bounds.Center.Y + 25), Color.White);
            */

            foreach (Rectangle xp in xpList) {
                create.Draw(Base, xp, Color.BlueViolet);
            }

            foreach (Bullet Bullet in BulletList) {
                Bullet.Draw(create);
            }
            //Basic Ene
            foreach (BasicEnemy e in BasicEnemies) {
                e.DrawBasics(create);
            }
            //Basic Ranged Ene
            foreach (RangedEnemy e in RangedEnemies) {
                e.DrawRanged(create);
            }
            foreach (RangedAttack attack in RangedAttacks) {
                attack.DrawRangedAttack(create);
            }
            foreach (Skeleton s in SkellyEnemies) { 
                s.DrawSkelly(create);
            }
            foreach (Troll t in Trolls) { 
                t.DrawTrolls(create);
            }
            create.DrawString(font, AllEnemies.Count.ToString(), new Vector2(bounds.Center.X, bounds.Center.Y), Color.White);

            //create.DrawString(font, RangedEnemies[0].Position.ToString(), new Vector2(bounds.Center.X, bounds.Center.Y), Color.White);

            //create.DrawString(font, Player.lastTimeRoll.ToString(), new Vector2(bounds.Center.X, bounds.Top + 25), Color.White);

            //create.DrawString(font, xpList.Count.ToString(), new Vector2(bounds.Center.X + 100, bounds.Center.Y + 25), Color.White);
            //create.DrawString(font, Player.playerXP.ToString(), new Vector2(bounds.Center.X + 100, bounds.Center.Y + 25), Color.White);
            //create.DrawString(font, BulletList.Count.ToString(), new Vector2(bounds.Center.X + 100, bounds.Center.Y + 25), Color.White);

            Player.Draw(create);
            create.DrawString(font, "LvL: " + Player.playerLVL, new Vector2(bounds.Center.X - 21, bounds.Top + 2), Color.LightGoldenrodYellow, 0, new Vector2(0, 0), 0.25f, SpriteEffects.None, 0);
            create.End();

            base.Draw(gameTime);
        }

       
    }
}