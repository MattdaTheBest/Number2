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

namespace Number2 {
    internal class Enemy {
        protected Random rand = new Random();
        public bool isAlive;
        //Basic Stats
        public float Health;
        protected float maxHealth;
        protected float Attack;
        protected float Speed;
        //Basic oritentaion
        public Rectangle rect;
        protected Vector2 Size;
        
        public  Vector2 Position;
        public  Vector2 Direction;
        protected Vector2 Origin;
        protected float Rotation;
        //Shapes
        protected Rectangle healthBar;
        protected Rectangle healthBarBacking;
        public Vector2 SpawnPos() { 
            var spawnChoice = rand.Next(0,5);
            
            switch (spawnChoice) {

                case 1: 
                    Position = new Vector2(rand.Next(0, Game1.bounds.Right), Game1.bounds.Top - 10);
                break;

                case 2: 
                    Position = new Vector2(rand.Next(0, Game1.bounds.Right), Game1.bounds.Bottom + 10);
                break;

                case 3:
                    Position = new Vector2(Game1.bounds.Left - 10, rand.Next(0, Game1.bounds.Bottom));
                break;

                case 4:
                    Position = new Vector2(Game1.bounds.Right + 10, rand.Next(0, Game1.bounds.Bottom));
                break;
            } 
        return Position;    
        }

        public void HealthBar(Vector2 pos) {
            healthBar = new Rectangle(new Vector2(pos.X - 4, pos.Y - 10).ToPoint(), new Vector2((Health/maxHealth) * (8 + Size.X) , 3).ToPoint());
            healthBarBacking = new Rectangle(new Vector2(pos.X - 4, pos.Y - 10).ToPoint(), new Vector2((8 + Size.X), 3).ToPoint());
        }

        public void UpdatePOS(List<Enemy> quad1List, List<Enemy> quad2List, List<Enemy> quad3List, List<Enemy> quad4List, Rectangle quad1, Rectangle quad2, Rectangle quad3, Rectangle quad4, List<Enemy> Enemies) {

            foreach (Enemy e in Enemies) {
                if (quad1.Contains(e.rect) && !quad1List.Contains(e)) {
                    quad1List.Add(e);
                }
                else if (!quad1.Contains(e.rect) && quad1List.Contains(e)) {
                    quad1List.Remove(e);
                }
                //
                if (quad2.Contains(e.rect) && !quad2List.Contains(e)) {
                    quad2List.Add(e);
                }
                else if (!quad2.Contains(e.rect) && quad2List.Contains(e)) {
                    quad2List.Remove(e);
                }
                //
                if (quad3.Contains(e.rect) && !quad3List.Contains(e)) {
                    quad3List.Add(e);
                }
                else if (!quad3.Contains(e.rect) && quad3List.Contains(e)) {
                    quad3List.Remove(e);
                }
                //
                if (quad4.Contains(e.rect) && !quad4List.Contains(e)) {
                    quad4List.Add(e);
                }
                else if (!quad4.Contains(e.rect) && quad4List.Contains(e)) {
                    quad4List.Remove(e);
                }

                if (e.rect.Intersects(quad1) && !quad1List.Contains(e)) {
                    quad1List.Add(e);
                }

                if (e.rect.Intersects(quad2) && !quad2List.Contains(e)) {
                    quad2List.Add(e);
                }

                if (e.rect.Intersects(quad3) && !quad3List.Contains(e)) {
                    quad3List.Add(e);
                }

                if (e.rect.Intersects(quad4) && !quad4List.Contains(e)) {
                    quad4List.Add(e);
                }
            }

                //Collision
                foreach (Enemy e in quad1List) {
                    foreach (Enemy e2 in quad1List) {
                        if (e != e2) {

                            var yHitBox = new Rectangle();
                            yHitBox = (e.rect);
                            yHitBox.Inflate(-2, 0);
                            var xHitBox = new Rectangle();
                            xHitBox = (e.rect);
                            xHitBox.Inflate(0, -2);

                            var yHitBox2 = new Rectangle();
                            yHitBox2 = (e2.rect);
                            yHitBox2.Inflate(-2, 0);
                            var xHitBox2 = new Rectangle();
                            xHitBox2 = (e2.rect);
                            xHitBox2.Inflate(0, -2);

                            if (xHitBox2.Intersects(xHitBox) && xHitBox2.Left <= xHitBox.Right && xHitBox2.Left > xHitBox.Left) {
                                e.Position.X -= e.Speed;
                            }
                            if (xHitBox2.Intersects(xHitBox) && xHitBox2.Right >= xHitBox.Left && xHitBox2.Right < xHitBox.Right) {
                                e.Position.X += e.Speed;
                            }
                            //Y
                            if (yHitBox2.Intersects(yHitBox) && yHitBox2.Top <= yHitBox.Bottom && yHitBox2.Top > yHitBox.Top) {
                                e.Position.Y -= e.Speed; 
                            }
                            if (yHitBox2.Intersects(yHitBox) && yHitBox2.Bottom >= yHitBox.Top && yHitBox2.Bottom < yHitBox.Bottom) {
                                e.Position.Y += e.Speed;
                            }
                        }
                    }
                }

                foreach (Enemy e in quad2List) {
                    foreach (Enemy e2 in quad2List) {
                        if (e != e2) {

                            var yHitBox = new Rectangle();
                            yHitBox = (e.rect);
                            yHitBox.Inflate(-2, 0);
                            var xHitBox = new Rectangle();
                            xHitBox = (e.rect);
                            xHitBox.Inflate(0, -2);

                            var yHitBox2 = new Rectangle();
                            yHitBox2 = (e2.rect);
                            yHitBox2.Inflate(-2, 0);
                            var xHitBox2 = new Rectangle();
                            xHitBox2 = (e2.rect);
                            xHitBox2.Inflate(0, -2);

                        if (xHitBox2.Intersects(xHitBox) && xHitBox2.Left <= xHitBox.Right && xHitBox2.Left > xHitBox.Left) {
                            e.Position.X -= e.Speed;
                        }
                        if (xHitBox2.Intersects(xHitBox) && xHitBox2.Right >= xHitBox.Left && xHitBox2.Right < xHitBox.Right) {
                            e.Position.X += e.Speed;
                        }
                        //Y
                        if (yHitBox2.Intersects(yHitBox) && yHitBox2.Top <= yHitBox.Bottom && yHitBox2.Top > yHitBox.Top) {
                            e.Position.Y -= e.Speed;
                        }
                        if (yHitBox2.Intersects(yHitBox) && yHitBox2.Bottom >= yHitBox.Top && yHitBox2.Bottom < yHitBox.Bottom) {
                            e.Position.Y += e.Speed;
                        }
                    }
                    }
                }

                foreach (Enemy e in quad3List) {
                    foreach (Enemy e2 in quad3List) {
                        if (e != e2) {

                            var yHitBox = new Rectangle();
                            yHitBox = (e.rect);
                            yHitBox.Inflate(-2, 0);
                            var xHitBox = new Rectangle();
                            xHitBox = (e.rect);
                            xHitBox.Inflate(0, -2);

                            var yHitBox2 = new Rectangle();
                            yHitBox2 = (e2.rect);
                            yHitBox2.Inflate(-2, 0);
                            var xHitBox2 = new Rectangle();
                            xHitBox2 = (e2.rect);
                            xHitBox2.Inflate(0, -2);

                        if (xHitBox2.Intersects(xHitBox) && xHitBox2.Left <= xHitBox.Right && xHitBox2.Left > xHitBox.Left) {
                            e.Position.X -= e.Speed;
                        }
                        if (xHitBox2.Intersects(xHitBox) && xHitBox2.Right >= xHitBox.Left && xHitBox2.Right < xHitBox.Right) {
                            e.Position.X += e.Speed;
                        }
                        //Y
                        if (yHitBox2.Intersects(yHitBox) && yHitBox2.Top <= yHitBox.Bottom && yHitBox2.Top > yHitBox.Top) {
                            e.Position.Y -= e.Speed;
                        }
                        if (yHitBox2.Intersects(yHitBox) && yHitBox2.Bottom >= yHitBox.Top && yHitBox2.Bottom < yHitBox.Bottom) {
                            e.Position.Y += e.Speed;
                        }
                    }
                    }
                }

            foreach (Enemy e in quad4List) {
                foreach (Enemy e2 in quad4List) {
                    if (e != e2) {

                        var yHitBox = new Rectangle();
                        yHitBox = (e.rect);
                        yHitBox.Inflate(-2, 0);
                        var xHitBox = new Rectangle();
                        xHitBox = (e.rect);
                        xHitBox.Inflate(0, -2);

                        var yHitBox2 = new Rectangle();
                        yHitBox2 = (e2.rect);
                        yHitBox2.Inflate(-2, 0);
                        var xHitBox2 = new Rectangle();
                        xHitBox2 = (e2.rect);
                        xHitBox2.Inflate(0, -2);

                        if (xHitBox2.Intersects(xHitBox) && xHitBox2.Left <= xHitBox.Right && xHitBox2.Left > xHitBox.Left) {
                            e.Position.X -= e.Speed;
                        }
                        if (xHitBox2.Intersects(xHitBox) && xHitBox2.Right >= xHitBox.Left && xHitBox2.Right < xHitBox.Right) {
                            e.Position.X += e.Speed;
                        }
                        //Y
                        if (yHitBox2.Intersects(yHitBox) && yHitBox2.Top <= yHitBox.Bottom && yHitBox2.Top > yHitBox.Top) {
                            e.Position.Y -= e.Speed;
                        }
                        if (yHitBox2.Intersects(yHitBox) && yHitBox2.Bottom >= yHitBox.Top && yHitBox2.Bottom < yHitBox.Bottom) {
                            e.Position.Y += e.Speed;
                        }
                    }
                }
            }
           
        }
    }
}
