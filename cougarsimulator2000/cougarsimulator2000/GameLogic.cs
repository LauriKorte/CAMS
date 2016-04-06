using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Xml;
using System.Xml.Serialization;

namespace cougarsimulator2000
{
    public enum Input
    {
        North = 1,
        South = 2,
        East = 4,
        West = 8,
        NorthEast = North|East,
        NorthWest = North|West,
        SouthEast = South|East,
        SouthWest = South|West
    }

    class DuplicateIntComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            int result = x.CompareTo(y);

            if (result == 0)
                return 1;
            else
                return result;
        }
    }


    [XmlRoot("game")]
    public class GameDefinitions
    {
        [XmlElement("defaultPlayerWeapon")]
        public string defaultPlayerWeapon
        {
            get;
            set;
        }

        [XmlElement("playerName")]
        public string playerName
        {
            get;
            set;
        }

        [XmlElement("playerDeathMessage")]
        public string playerDeathMessage
        {
            get;
            set;
        }

        [XmlElement("playerGoryDeathMessage")]
        public string playerGoryDeathMessage
        {
            get;
            set;
        }

        [XmlElement("playerImage")]
        public string playerImage
        {
            get;
            set;
        }
        [XmlElement("defaultPlayerAmmo")]
        public int defaultPlayerAmmo
        {
            get;
            set;
        }
    }

    public class GameLogic
    {
        public Player player;
        public ItemList items;
        public EnemyList enemies;
        public GameDefinitions gameDefinitions;
        private Assets assets;
        public int currentTurn
        {
            get;
            set;
        }

        public int level
        {
            get;
            set;
        }

        public TileMap tileMap
        {
            get;
        }

        public List<Actor> actors
        {
            get;
        }

        public SortedList<int, Actor> actorTurns
        {
            get;
        }

        public void logGameMessage(params Object[] args)
        {
            logger(args);
        }
        
        private GameLogDelegate logger;

        private List<Actor> actorsToDelete = new List<Actor>();

        public void removeActor(Actor actor)
        {
            actor.isRemoved = true;
            actorsToDelete.Add(actor);
        }

        public GameLogic(Assets ass, GameLogDelegate logger)
        {

            this.logger = logger;
            actors = new List<Actor>();
            tileMap = new TileMap();
            actorTurns = new SortedList<int, Actor>(new DuplicateIntComparer());

            currentTurn = 0;
            level = 0;
            assets = ass;

            gameDefinitions = assets.loadGameDefinitions();

            items = assets.loadItems();
            enemies = assets.loadEnemies();

            start();
        }

        public void playSound(string snd)
        {
            assets.playSound(snd);
        }

        public void equip(Item selitem)
        {
            if (player.isDead)
            {
                logGameMessage("Ya ain't equippin' nuthin, son, 'cos ya dead!");
                return;
            }
            player.equip(this,selitem);
        }

        private void updatePlayerTurn(int time)
        {
            //This function assumes the player is next in the turn queue
            //and updates it's position in the queue accordingly
            Actor first = null;
            if (actorTurns.Count > 0)
            {
                var kv = actorTurns.First();
                first = kv.Value;
                currentTurn = kv.Key;
            }

            if (first is Player)
            {
                //Remove it from the first place
                actorTurns.RemoveAt(0);
                //And add it back
                actorTurns.Add(currentTurn + time, first);
            }
            else
                throw new Exception("updatePlayerTurn called when it isn't players turn");
        }

        private void updateActors()
        {
            foreach (var del in actorsToDelete)
            {
                actors.Remove(del);
            }
            //We get the first element in our sorted list
            Actor first = null;
            if (actorTurns.Count > 0)
            {
                //If there actually is an element
                var kv = actorTurns.First();
                first = kv.Value;
                currentTurn = kv.Key;
            }
            
            //We do it while the first element isn't player
            //or we've run out of elements
            while (first != player && first != null)
            {
                //If it's already removed
                if (first.isRemoved)
                {
                    //discard it
                    actorTurns.RemoveAt(0);
                }
                else
                {
                    //Update it
                    int add = first.update(this);
                    if (add <= 0)
                        add = 10;
                    
                    //Remove it from the first place
                    actorTurns.RemoveAt(0);
                    //And add it back
                    actorTurns.Add(currentTurn + add, first);
                }

                //And get the new first element
                Actor next = null;
                if (actorTurns.Count > 0)
                {
                    var kv = actorTurns.First();
                    next = kv.Value;
                    currentTurn = kv.Key;
                }
                first = next;
            }

            foreach (var del in actorsToDelete)
            {
                actors.Remove(del);
            }

            updateLineOfSight();
        }

        private void updateLineOfSight()
        {
            List<Vector2> points = LineOfSight.GetCellsOnRadius(player.position, 12);
            
            foreach (var t in tileMap.tiles)
                t.isVisible = false;
            foreach (var p in points)
            {
                IsBlockingCellDelegate ibcd = (x) =>
                {
                    try
                    {
                        Tile t = tileMap.getTile(x);
                        t.isVisible = true;
                    }
                    catch (Exception)
                    {
                    }

                    if (isTileBlockingNoActors(x))
                        return true;
                    return false;
                };
                LineOfSight.GetDiamondLineOfSight(
                    ibcd,
                    player.position, p);
            }
            foreach (var actor in actors)
            {
                if (actor == player)
                    continue;

                //Check the line of sight
                bool los = getLineOfSight(player, actor);
                
                actor.isVisible = los;
            }
        }

        public bool getLineOfSight(Actor from, Actor target)
        {
            //We check the line of sight 4 times
            //because people aren't points in real life

            //they're diamond shaped

            bool los = LineOfSight.GetDiamondLineOfSight(
                    (x) => (isTileBlockingNoActors(x)),
                    from.position, target.position, 0.75, 0.5);

            if (!los)
                los = los || LineOfSight.GetDiamondLineOfSight(
                    (x) => (isTileBlockingNoActors(x)),
                    from.position, target.position, 0.25, 0.5);

            if (!los)
                los = los || LineOfSight.GetDiamondLineOfSight(
                    (x) => (isTileBlockingNoActors(x)),
                    from.position, target.position, 0.5, 0.75);

            if (!los)
                los = los || LineOfSight.GetDiamondLineOfSight(
                    (x) => (isTileBlockingNoActors(x)),
                    from.position, target.position, 0.5, 0.25);
            return los;
        }

        public void enterInput(Input input)
        {
            if (player.isDead)
            {
                logGameMessage("Dead men do not move");
                return;
            }
            int i = (int)input;
            Vector2 off = new Vector2(0,0);
            if ((i & (int)Input.East) != 0)
                off.x++;

            if ((i & (int)Input.West) != 0)
                off.x--;

            if ((i & (int)Input.North) != 0)
                off.y--;

            if ((i & (int)Input.South) != 0)
                off.y++;

            Vector2 trypos = player.position + off;
            //Get a list of actors in the tile we're trying to move to
            
            var e = tileContainsActor(trypos);
            foreach (var actor in e)
            {
                if (actor.isEnemy)
                {
                    int attack = player.attack(this, actor);
                    updatePlayerTurn(attack);
                    
                    updateActors();
                    return;
                }
            }
            
            if (!isTileBlocking(trypos))
                player.position += off;
            updatePlayerTurn(player.moveSpeed);
            updateActors();
        }
        private void newLevel()
        {
            level++;

            //Clear the actors
            actors.Clear();
            actorTurns.Clear();

            Random rand = new Random();
            //Map size should be uneven, odd (11,13,15,17)

            int msx = rand.Next(5) * 2;
            int msy = rand.Next(5) * 2;
            tileMap.initialize(new Vector2(15 + msx, 15 + msy));

            //otherwise the maze generator makes a mess
            MazeGenerator.Generate(tileMap, 25, 25);

            //Make a new player... if we need to
            
            if (player == null || level == 1 || player.isDead)
            {
                player = new Player();
                player.name = gameDefinitions.playerName;
                player.postMortem = gameDefinitions.playerDeathMessage;
                player.nameArticle = "";
                player.nameDefArticle = "";
                player.goryPostMortem = gameDefinitions.playerGoryDeathMessage;
                player.image = gameDefinitions.playerImage; //give it a fancy hat
                player.depth = 2;

                ItemDefinition idef = items.getItemDefinition(gameDefinitions.defaultPlayerWeapon);
                if (idef != null)
                {
                    player.inventory.Add(new Item(idef));
                    WeaponDefinition wdef = idef as WeaponDefinition;
                    if (wdef != null)
                    {
                        player.weapon = wdef;
                        ItemDefinition adef = items.getItemDefinition(wdef.ammunitionType);
                        if (adef != null)
                        {
                            player.inventory.Add(new Item(adef, gameDefinitions.defaultPlayerAmmo));
                        }
                    }
                }
            }


            //Then move the player to the starting position
            player.position.x = 1;
            player.position.y = 1;

            //And register them
            actors.Add(player);
            actorTurns.Add(currentTurn, player);


            //Create a bunch of cougars
            Random r = new Random();
            if (enemies.enemies.Count > 0)
                for (int i = 0; i < 16; i++)
                {
                    EnemyDefinition def = enemies.enemies[r.Next(enemies.enemies.Count)];
                    Enemy a = new Enemy(def);

                    a.depth = 1;
                    a.position.x = r.Next(tileMap.size.x - 2) + 1;
                    a.position.y = r.Next(tileMap.size.y - 2) + 1;

                    if (!isTileBlocking(a.position))
                        addActor(a);
                }

            //Spawn loots!

            //We get the total weight of all items available
            //for the current level
            int wt = items.getTotalItemWeightForLevel(level);

            if (wt > 0)
                for (int i = 0; i < 37; i++)
                {
                    //then we get a random value
                    int rnd = r.Next(wt);

                    ItemDefinition def = null;

                    //And go through all the items...
                    foreach (var item in items.all)
                    {
                        //...available for the current level
                        if (item.minimumLevel <= level)
                        {
                            //And see if our random number
                            //happens to hit this item
                            rnd -= item.weight;
                            if (rnd < 0)
                            {
                                def = item;
                                break;
                            }
                        }
                    }
                    //If something went wrong...
                    if (def == null)
                        continue;  //(it shouldn't)

                    int cnt = def.spawnCount;
                    if (def.spawnCountVariation > 0)
                        cnt += r.Next(def.spawnCountVariation * 2 + 1) - def.spawnCountVariation;

                    Item a = new Item(def, cnt);
                    PickUp p = new PickUp(a);
                    p.position.x = r.Next(tileMap.size.x - 2) + 1;
                    p.position.y = r.Next(tileMap.size.y - 2) + 1;
                    addActor(p);
                }
            updateLineOfSight();
        }
        public void start()
        {
            level = 0;
            currentTurn = 0;

            newLevel();

        }

        public void addActor(Actor a)
        {
            actors.Add(a);
            actorTurns.Add(currentTurn + 1, a);
        }

      
        public bool isTileBlockingNoActors(Vector2 trypos)
        {
            //Just test the tilemap collision parameters
            if (trypos.x < 0 || trypos.y < 0 || trypos.x >= tileMap.size.x || trypos.y >= tileMap.size.y)
                return true;

            if (tileMap.getTile(trypos).type == 1)
                return false;

            return true;
        }

        public bool isTileBlocking(Vector2 trypos)
        {
            //Go through actors in the tile
            foreach (var a in tileContainsActor(trypos))
            {
                if (a.isBlocking == true)
                    return true;
            }

            return isTileBlockingNoActors(trypos);
        }


        public bool isTileBlockingExclude(Vector2 trypos, List<Actor> actors)
        {
            //Same thing as above, except we exclude a list of actors from the check

            foreach (var a in tileContainsActor(trypos))
            {
                if (actors.Contains(a))
                    continue;

                if (a.isBlocking == true)
                    return true;
            }

            return isTileBlockingNoActors(trypos);
        }

        public List<Actor> tileContainsActor(Vector2 trypos)
        {
            List<Actor> acts = new List<Actor>();
            foreach (Actor actor in actors)
            {
                if (actor.position == trypos)
                {
                    acts.Add(actor);
                }
            }
            return acts;
        }

        public void interact(Actor a)
        {
            if (!getLineOfSight(player,a))
            {
                logGameMessage("You perceived the target with your sixth sense, and can't actually see it.");
                return;
            }
            if (player.isDead)
            {
                logGameMessage("Dead men do not do things.");
                return;
            }
            if (a.isEnemy)
            {
                int pk = player.attack(this, a);
                updatePlayerTurn(pk);
                updateActors();
            }
            else if (a == player)
            {
                int pk = player.attack(this, a);
                updatePlayerTurn(pk);
                updateActors();
            }


        }
    }
}
