using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Number2.Content {
    internal class Bullet {
        public int bulletSpeed = 2;

        public Rectangle rect;
        public Vector2 bulletPostion;
        public Vector2 bulletDirection;
        public Point bulletDimension;
        public Bullet(Player Player) {

            bulletDimension = new Point(4, 4);

            bulletSpeed = -4;

            var Temp = Player.Clone() as Player;
            bulletDirection = Temp.direction;
            bulletPostion = Temp.position;
        }

        public void Update(Player Player) {
            rect = new Rectangle(bulletPostion.ToPoint(), bulletDimension);
        }

        public void Draw(SpriteBatch create) {
            create.Draw(Game1.Base, rect, Color.Gold);
        }

        public bool BulletCheck(List<Bullet> bullet, List<Enemy> enemyList, Player player) {

            for (int i = 0; i < enemyList.Count; i++) {
                if (rect.Intersects(enemyList[i].rect)) {

                    if (enemyList[i].GetType() != typeof(Troll)) {
                        enemyList[i].Position -= bulletDirection * 2f;
                    }
                    enemyList[i].Health -= player.attack;
                    return true;
                }
            }
            return false;
        }
        


    }

} 
