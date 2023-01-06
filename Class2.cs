using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Number2.Content;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Collections;
using System.Runtime.Intrinsics.Arm;

namespace Number2 {

    //Basic ene Type
    internal class BasicEnemy : Enemy {
        Vector2 dPos;
        public BasicEnemy() {
            isAlive = true;
            //Setting Stats
            Health = 10;
            maxHealth = Health;
            Speed = 1;
            Attack = 2;
            //Setting Size
            Size = new Vector2(18, 18);
            //Setting Orientation
            Position = SpawnPos();
            Origin = new Vector2(Size.X / 2, Size.Y / 2);


        }

        public void UpdateBasics(Player player) {
            dPos = Position - player.position;
            Rotation = (float)Math.Atan2(dPos.Y, dPos.X) + MathHelper.ToRadians(90);
            Direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - Rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - Rotation));
            Position -= Direction * Speed;

            HealthBar(Position);
            rect = new Rectangle(Position.ToPoint(), Size.ToPoint());

            if (Health <= 0) {
                isAlive = false;
            }
        }

        public void DrawBasics(SpriteBatch create) {
            create.Draw(Game1.Base, rect, Color.DarkGreen);
            create.Draw(Game1.Base, healthBarBacking, Color.Black);
            create.Draw(Game1.Base, healthBar, Color.MediumVioletRed);

        }
    }

    //Ranged ene Type

    internal class RangedEnemy : Enemy {
        Vector2 dPos;
        int Range;
        //Ranged Attack Variables
        private bool inRange;
        float AttackSpeed;
        float AttackCounter = 0;

        public RangedEnemy() {
            isAlive = true;
            //Setting Stats
            Health = 10;
            maxHealth = Health;
            Speed = 2;
            Attack = 2;
            AttackCounter = AttackSpeed;
            AttackSpeed = 120; // /60 for aps
            Range = rand.Next(275, 325);
            //Setting Size
            Size = new Vector2(14, 14);
            //Setting Orientation
            Position = SpawnPos();
            Origin = new Vector2(Size.X / 2, Size.Y / 2);
        }

        public void UpdateRanged(Player player, List<RangedAttack> attackList) {
            dPos = Position - player.position;
            Rotation = (float)Math.Atan2(dPos.Y, dPos.X) + MathHelper.ToRadians(90);
            Direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - Rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - Rotation));

            if ((player.position - Position).Length() > new Vector2(Range, Range).Length() || !Game1.bounds.Contains(Position)) {
                Position -= Direction * Speed;
            }
            else if ((player.position - Position).Length() < new Vector2(Range, Range).Length() && AttackCounter == AttackSpeed) {
                var attack = new RangedAttack(Position, Direction);
                AttackCounter = 0;
                attackList.Add(attack);
            }
            else if ((player.position - Position).Length() < Range - 100) {
                Position += Direction * Speed;
            }
            else {
                AttackCounter += 1;
            }

            HealthBar(Position);
            rect = new Rectangle(Position.ToPoint(), Size.ToPoint());

            if (Health <= 0) {
                isAlive = false;
            }
        }

        public void DrawRanged(SpriteBatch create) {
            //create.Draw(Game1.Base, Position, rect, Color.White, Rotation, Origin, 1, SpriteEffects.None, 0f);
            create.Draw(Game1.Base, rect, Color.Pink);
            create.Draw(Game1.Base, healthBarBacking, Color.Black);
            create.Draw(Game1.Base, healthBar, Color.MediumVioletRed);
        }

        public object Clone() {
            return this.MemberwiseClone();
        }
    }

    internal class RangedAttack : RangedEnemy {
        public Rectangle bullet;
        public Vector2 bulletPos;
        public Vector2 bulletDirection;
        public RangedAttack(Vector2 pos, Vector2 direction) {
            bulletDirection = direction;
            bulletPos = pos;
            bulletPos.X += Size.X / 2 - 2;
        }
        public void UpdateRangedAttack() {
            bulletPos -= bulletDirection * 2;
            bullet = new Rectangle(bulletPos.ToPoint(), new Point(5, 5));
        }
        public void DrawRangedAttack(SpriteBatch create) {
            create.Draw(Game1.Base, bullet, Color.Silver);
        }
        public bool attackDetect(Player player) {
            if (player.rect.Contains(bulletPos)) {
                return true;
            }
            else {
                return false;
            }
        }
    }

    //Skelly

    internal class Skeleton : Enemy {
        public bool firstDeath;
        bool canRevive;
        Vector2 dPos;
        float reAnimateTimer = 0;
        float reAnimateTiming = 120;

        Rectangle SkellyFrag1;
        Rectangle SkellyFrag2;
        Rectangle SkellyFrag3;

        public Skeleton() {
            isAlive = true;
            firstDeath = false;
            canRevive = true;

            //Setting Stats
            Health = 12;
            maxHealth = Health;
            Speed = 1.5f;
            Attack = 2.5f;
            //Setting Size
            Size = new Vector2(16, 16);
            //Setting Orientation
            Position = SpawnPos();
            Origin = new Vector2(Size.X / 2, Size.Y / 2);
        }
        public void UpdateSkelly(Player Player) {
            dPos = Position - Player.position;
            Rotation = (float)Math.Atan2(dPos.Y, dPos.X) + MathHelper.ToRadians(90);
            Direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - Rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - Rotation));
            Position -= Direction * Speed;

            HealthBar(Position);
            rect = new Rectangle(Position.ToPoint(), Size.ToPoint());

            if (Health <= 0 && !firstDeath) {
                firstDeath = true;
                SkellyReAnimate(Position);
            }
            else if (Health <= 0 && firstDeath && !canRevive) {
                isAlive = false;
            }

            if (firstDeath && reAnimateTimer != reAnimateTiming && canRevive) {
                reAnimateTimer += 1;
            }
            else if (firstDeath && reAnimateTimer == reAnimateTiming && canRevive) {
                reAnimateTimer = 0;
                canRevive = false;
                Health = maxHealth / 2;
                Size = new Vector2(16, 16);
                Speed = 1.25f;
            }
        }
        public void SkellyReAnimate(Vector2 Pos) {
            SkellyFrag1 = new Rectangle((int)Pos.X - 6, (int)Pos.Y - 3, 3, 3);
            SkellyFrag2 = new Rectangle((int)Pos.X + 3, (int)Pos.Y - 6, 5, 5);
            SkellyFrag3 = new Rectangle((int)Pos.X, (int)Pos.Y + 3, 8, 8);

            Size = new Vector2(0, 0);
            Speed = 0;
        }
        public void DrawSkelly(SpriteBatch create) {
            create.Draw(Game1.Base, rect, Color.AntiqueWhite);
            if (Speed != 0) {
                create.Draw(Game1.Base, healthBarBacking, Color.Black);
            }
            create.Draw(Game1.Base, healthBar, Color.MediumVioletRed);
            if (firstDeath && canRevive) {
                create.Draw(Game1.Base, SkellyFrag1, Color.AntiqueWhite);
                create.Draw(Game1.Base, SkellyFrag2, Color.AntiqueWhite);
                create.Draw(Game1.Base, SkellyFrag3, Color.AntiqueWhite);
            }

        }
    }

    internal class Troll : Enemy {
        Vector2 dPos;

        Rectangle LeftArm; Vector2 leftArmPos; float leftArmRot; Vector2 leftArmDirection; Vector2 dpos;
        Rectangle RightArm; Vector2 rightArmPos; float rightArmRot; Vector2 rightArmDirection;

        Rectangle test = new Rectangle(0, 0, 1, 1);
        public Troll() {
            isAlive = true;
            //Setting Stats
            Health = 100;
            maxHealth = Health;
            Speed = 0.25f;
            Attack = 2;
            //Setting Size
            Size = new Vector2(40, 40);
            //Setting Orientation
            Position = SpawnPos();
            Origin = new Vector2(Size.X / 2, Size.Y / 2);
        }
        public void UpdateTrolls(Player player) {
            dPos = Position - player.position;
            Rotation = (float)Math.Atan2(dPos.Y, dPos.X) + MathHelper.ToRadians(90);
            Direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - Rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - Rotation));
            Position -= Direction * Speed;

            HealthBar(Position);
            rect = new Rectangle(Position.ToPoint(), Size.ToPoint());

            if (Health <= 0) {
                isAlive = false;
            }
        }

        public void DrawTrolls(SpriteBatch create) {
            create.Draw(Game1.Base, rect, Color.DarkGreen);
            create.Draw(Game1.Base, healthBarBacking, Color.Black);
            create.Draw(Game1.Base, healthBar, Color.MediumVioletRed);
        }
    }
}

