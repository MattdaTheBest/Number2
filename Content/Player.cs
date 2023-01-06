using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;
using System.Collections;

namespace Number2.Content {
    public class Player {
        public int playerLVL = 1;
        public int maxStamina;
        public int stamina;
        public float timeLastShot;
        public float attackSpeed; // attacks/second
        public float attack;
        public float Health;
        public float MaxHealth;

        public float playerXP;
        public float playerXPNeeded;

        public float stamRegen;

        Rectangle xpBar;
        Rectangle xpBarBacking;
        Rectangle healthBar;
        Rectangle healthBarBacking;
        Rectangle StamSqr1;
        Rectangle StamSqr2;
        Rectangle StamSqr3;

        public bool canRoll = true;
        public bool isRolling = false;
        public float lastTimeRoll = 0;

        public int PlayerSpeed = 2;

        public Rectangle rect;
        private float rotation;

        public Vector2 position = new Vector2(Game1.bounds.Center.X, Game1.bounds.Center.Y);
        public Vector2 direction;
        private Vector2 origin;

        private Vector2 mousePos;
        private Vector2 dPos;

        Point size = new Point(20, 20);

        public float gunRot;
        public Vector2 gunPos;
        public Vector2 gunOrg;

        Rectangle test;
        public Player(Rectangle rect) {
            isRolling = false;
            this.rect = rect;
            origin = new Vector2(rect.Width / 2, rect.Height / 2);
            timeLastShot = 0f;
            attackSpeed = .1f; //1.5f;
            attack = 4;
            playerXP = 0f;
            playerXPNeeded = 4 + playerLVL * 2;
            Health = 10;
            MaxHealth = Health;
            stamina = 2;
            maxStamina = stamina;

            test = new Rectangle((int)position.X + 100, (int)position.Y + 100, 5, 5);

            gunOrg = new Vector2(11,25);
        }
        public void Update(List<Rectangle> xpList) {
            playerXPNeeded = 6 + playerLVL * 2;
            mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            dPos = position - mousePos;

            XPBar();
            HealthBar();
            Stamina();

            if (Keyboard.GetState().IsKeyDown(Keys.W)) {
                position.Y -= PlayerSpeed;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S)) {
                position.Y += PlayerSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A)) {
                position.X -= PlayerSpeed;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D)) {
                position.X += PlayerSpeed;
            }

            rect = new Rectangle(position.ToPoint() - new Point(10,10), size);

            rotation = (float)Math.Atan2(dPos.Y, dPos.X) + MathHelper.ToRadians(90);

            direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - rotation));

            if (isRolling) {
                rect.Inflate(-2,-2);
            }

            for (int i = 0; i < xpList.Count; i++) {
                if (rect.Intersects(xpList[i])) {
                    playerXP += 1;
                    xpList.RemoveAt(i);
                }
            }

            if (playerXP == playerXPNeeded) {
                playerXP = 0;
                playerLVL += 1;
            }

            if (stamina < maxStamina) {
                stamRegen += 1;
                if (stamRegen == 120) {
                    stamRegen = 0;
                    stamina += 1;
                }
            }

            gunRot = rotation + MathHelper.ToRadians(90);
            gunPos = position;


        }

        public void XPBar() {
            xpBar = new Rectangle(new Vector2(Game1.bounds.Left + 25, Game1.bounds.Top + 10).ToPoint(), new Vector2((playerXP / playerXPNeeded) * Game1.bounds.Width - 50, 6).ToPoint());
            xpBarBacking = new Rectangle(new Vector2(Game1.bounds.Left + 25, Game1.bounds.Top + 10).ToPoint(), new Vector2(Game1.bounds.Width - 50, 6).ToPoint());
        }

        public void HealthBar() {
            healthBar = new Rectangle(new Vector2(Game1.bounds.Left + 25, Game1.bounds.Top + 22).ToPoint(), new Vector2((Health / MaxHealth) * 150, 6).ToPoint());
            //xpBarBacking = new Rectangle(new Vector2(pos.X - 4, pos.Y - 10).ToPoint(), new Vector2((8 + Size.X), 3).ToPoint());
        }

        public void Stamina() {
            StamSqr1 = new Rectangle(new Vector2(Game1.bounds.Left + 25, Game1.bounds.Top + 35).ToPoint(), new Vector2(15,15).ToPoint());
            StamSqr2 = new Rectangle(new Vector2(Game1.bounds.Left + 50, Game1.bounds.Top + 35).ToPoint(), new Vector2(15, 15).ToPoint());
            if (stamina == 3) {
            StamSqr3 = new Rectangle(new Vector2(Game1.bounds.Left + 75, Game1.bounds.Top + 35).ToPoint(), new Vector2(15, 15).ToPoint());
            }
                     
        }

        public void Draw(SpriteBatch create) {

            create.Draw(Game1.Base, position, rect, Color.Gray, rotation, origin, 1, SpriteEffects.None, 0f);

            create.Draw(Game1.Base, xpBarBacking, Color.Black);
            create.Draw(Game1.Base, xpBar, Color.BlueViolet);
            create.Draw(Game1.Base, healthBar, Color.MediumVioletRed);
            //create.Draw(Game1.Base, rect, Color.Brown);
            
            
            //create.Draw(Game1.Gun, gunPos, null, Color.Pink, gunRot, gunOrg, 1, SpriteEffects.None, 0f);

            if (stamina >= 1) {
                create.Draw(Game1.Base, StamSqr1, Color.MediumSeaGreen);
            }
            if (stamina >= 2) {
                create.Draw(Game1.Base, StamSqr2, Color.MediumSeaGreen);
            }
            if (stamina >= 3) {
                create.Draw(Game1.Base, StamSqr3, Color.MediumSeaGreen);
            }
            //create.Draw(Game1.Base, rect, Color.Brown);
        }

        public void dodgeRoll() {
            PlayerSpeed = 8;
        }

        public object Clone() {
            return this.MemberwiseClone();
        }
    }
}

