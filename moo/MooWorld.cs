using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.Common;
using engine.Common.Entities;

namespace moo
{
    enum AreaType : byte { Blank = 0, Food = 1, Gold = 2, Tree = 3, Rock = 4, Player = 5, Sword = 6};

    class MooWorld
    {
        public World World;
        public MooPlayer Human;

        public MooWorld()
        {
            Human = new MooPlayer() { Name = "Jacob", X = 0, Y = 0 };
        }

        public Player[] GetPlayers()
        {
            return new Player[]
            {
                Human
            };
        }

        public Element[] GetObstacles(out int width, out int height)
        {
            width = height = 10000;

            if (false)
            {
                // put the human in the upper right hand corner
                Human.X = Human.Y = 50;

                return new Element[]
                {
                new MooFood() { X = 110, Y = 110 },
                new MooGold() { X = 210, Y = 210 },
                new MooTree() { X = 310, Y = 310},
                new MooRock() { X = 410, Y = 410},
                // top
                new MooWall(WallDirection.Horiztonal, 500, 15) { X = 250, Y = 0 },
                // left
                new MooWall(WallDirection.Vertical, 500, 15) { X = 0, Y = 250 },
                // right
                new MooWall(WallDirection.Vertical, 500, 15) { X = 500, Y = 250 },
                // bottom
                new MooWall(WallDirection.Horiztonal, 500, 15) { X = 250, Y = 500 }
                };
            }
            else
            {
                // random gen
                var items = new List<Element>();

                // do not allow the chunk size to be smaller than the largest obstacle
                int chunkSize = 400;
                Random rand = new Random();

                if (width < chunkSize || height < chunkSize) throw new Exception("Must have at least " + chunkSize + " pixels to generate a board");

                // break the board up into 400x400 chunks, within those chunks we will then fill with a few particular patterns
                int cols = width / chunkSize;
                int rows = height / chunkSize;
                var count = cols * rows;
                var board = new AreaType[count];

                // add a specified number of items
                int index = 0;
                // players
                board[index++] = AreaType.Player;
                for (int i = 0; i < 2; i++) board[index++] = AreaType.Sword;
                // gold
                for(int i=0; i<(count * 0.01); i++) board[index++] = AreaType.Gold;
                // Tree
                for (int i = 0; i < (count * 0.05); i++) board[index++] = AreaType.Tree;
                // rock
                for (int i = 0; i < (count * 0.02); i++) board[index++] = AreaType.Rock;
                // food
                for (int i = 0; i < (count * 0.01); i++) board[index++] = AreaType.Food;

                // randomize
                for (int id1 = 0; id1 < index; id1++)
                {
                    int id2 = 0;
                    do
                    {
                        id2 = rand.Next() % count;
                    }
                    while (id2 == id1);

                    // swap
                    var tmp = board[id1];
                    board[id1] = board[id2];
                    board[id2] = tmp;
                }

                // build the board
                for (int i = 0; i < count; i++)
                {
                    int h = i / cols;
                    int w = i % cols;

                    float x = (h * chunkSize) + (chunkSize / 2);
                    float y = (w * chunkSize) + (chunkSize / 2);

                    if (board[i] == AreaType.Gold)
                    {
                        items.Add(new MooGold() { X = x, Y = y });
                    }
                    else if (board[i] == AreaType.Food)
                    {
                        items.Add(new MooFood() { X = x, Y = y });
                    }
                    else if (board[i] == AreaType.Tree)
                    {
                        items.Add(new MooTree() { X = x, Y = y });
                    }
                    else if (board[i] == AreaType.Rock)
                    {
                        items.Add(new MooRock() { X = x, Y = y });
                    }
                    else if (board[i] == AreaType.Player)
                    { 
                        Human.X = x;
                        Human.Y = y;
                    }
                    else if (board[i] == AreaType.Sword)
                    {
                        items.Add(new MooSword() { X = x, Y = y });
                    }
                }

                // make borders
                var thickness = 15;
                items.Add( new MooWall(WallDirection.Horiztonal, width, thickness) { X = width / 2, Y = thickness / 2, Z = float.MaxValue } );
                items.Add( new MooWall(WallDirection.Vertical, height, thickness) { X = thickness/2, Y = height / 2, Z=float.MaxValue } );
                items.Add( new MooWall(WallDirection.Horiztonal, width, thickness) { X = width / 2, Y = height - thickness/2, Z=float.MaxValue } );
                items.Add( new MooWall(WallDirection.Vertical, height, thickness) { X = width - thickness/2, Y = height / 2, Z=float.MaxValue } );

                return items.ToArray();
            }
        }

        public bool TakeAction(Player player, char key)
        {
            if (!(player is MooPlayer)) return false;
            var mp = player as MooPlayer;

            switch(key)
            {
                case '3':
                    // craft wood block

                    // check if there is enough wood materials
                    if (mp.Wood < MooWoodBox.CraftCost) return false;

                    // use wood to craft a box
                    mp.Wood -= MooWoodBox.CraftCost;

                    // if there is room in the hand (put in hand)
                    //  else drop on the ground
                    var wood = new MooWoodBox() { X = player.X, Y = player.Y };

                    if (!player.Take(wood))
                    {
                        World.AddItem(wood);
                    }

                    return true;

                case '4':
                    // craft rock block

                    // check if there is enough wood materials
                    if (mp.Rock < MooRockBox.CraftCost) return false;

                    // use rock to craft a box
                    mp.Rock -= MooRockBox.CraftCost;

                    // if there is room in the hand (put in hand)
                    //  else drop on the ground
                    var rock = new MooRockBox() { X = player.X, Y = player.Y };

                    if (!player.Take(rock))
                    {
                        World.AddItem(rock);
                    }

                    return true;

                case '8':
                    // craft sword

                    // check if there is enough wood materials
                    if (mp.Rock < MooSword.RockCraftCost ||
                        mp.Wood < MooSword.WoodCraftCost) return false;

                    // use wood and rock to craft a sword
                    mp.Rock -= MooSword.RockCraftCost;
                    mp.Wood -= MooSword.WoodCraftCost;

                    // if there is room in the hand (put in hand)
                    //  else drop on the ground
                    var sword = new MooSword() { X = player.X, Y = player.Y };

                    if (!player.Take(sword))
                    {
                        World.AddItem(sword);
                    }

                    return true;

                case 'r':
                case 'R':
                    // eat to increase health
                    if (mp.Health < Constants.MaxHealth)
                    {
                        if (mp.Food > 0)
                        {
                            mp.Health += MooFood.HealthPerFood;
                            mp.Food -= 1;
                        }
                    }

                    return true;

                case '9':
                    var rand = new Random();

                    // span a zombie horde
                    for(int i=0; i<10; i++)
                    {
                        var x = rand.Next() % World.Width;
                        var y = rand.Next() % World.Height;
                        float s = (float)(rand.Next() % 10) / 10f + 0.1f;

                        var zombie = new MooZombie(Human) { X = x, Y = y, Speed = s };

                        World.AddItem(zombie);
                    }

                    return true;

                case Constants.LeftMouse:
                    // place the block
                    if (mp.Primary != null && mp.Primary is MooCraftable)
                    {
                        var item = mp.DropPrimary() as MooCraftable;
                        item.Place(mp);
                        World.AddItem(item);

                        return true;
                    }

                    break;
            }

            return false;
        }

        public void Contact(Player player, Element element)
        {
            if (player is MooPlayer)
            {
                var mp = (player as MooPlayer);
                var xp = 5;
                var resources = 1;

                // double the resources if collected with an axe
                if (mp.Primary != null && mp.Primary is MooAxe)
                {
                    resources *= 2;
                }

                if (element is MooGold)
                {
                    // dboule the xp if it is gold
                    xp *= 2;
                }
                else if (element is MooTree)
                {
                    mp.Wood += (MooTree.Gathered * resources);
                }
                else if (element is MooFood)
                {
                    mp.Food += (MooFood.Gathered * resources);
                }
                else if (element is MooRock)
                {
                    mp.Rock += (MooRock.Gathered * resources);
                }
                else
                {
                    xp = 0;
                }
                mp.XP += xp;

                if (mp.XP > mp.XPMax)
                {
                    mp.Level++;
                    // keep level progression constant
                    //mp.XPMax += MooPlayer.XPIncrease;
                    mp.XP = 0;

                    // ever 3rd level, spawn zombies
                    if (mp.Level % 3 == 0)
                    {
                        TakeAction(mp, '9' /* spawn zombies*/);
                    }
                }
            }
        }
    }
}
