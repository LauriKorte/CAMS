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

        

        public GameDefinitions gameDefinitions;
        private Assets assets;

        public TileMap tileMap
        {
            get;
        }

        public List<Actor> actors
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
            actorsToDelete.Add(actor);
        }

        public GameLogic(Assets ass, GameLogDelegate logger)
        {

            this.logger = logger;
            actors = new List<Actor>();
            tileMap = new TileMap();
            assets = ass;

            gameDefinitions = assets.loadGameDefinitions();

            items = assets.loadItems();

            foreach (var v in items.weapons)
            {
                Console.Write(v.name + " " + v.description);
            }

            start();
        }       

        private void updateActors()
        {
            foreach (var del in actorsToDelete)
            {
                actors.Remove(del);
            }
            foreach (var actor in actors)
            {
                actor.update(this);
            }

            updateLineOfSight();
        }

        private void updateLineOfSight()
        {
            List<Vector2> points = LineOfSight.GetCellsOnRadius(player.position, 7);
            
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
                    if (player.attack(this, actor))
                    {
                        updateActors();
                        return;
                    }
                    break;
                }
            }
            
            if (!isTileBlocking(trypos))
                player.position += off;
            updateActors();
        }

        public void start()
        {

            //Clear the actors
            actors.Clear();

            Random rand = new Random();
            //Map size should be uneven, odd (11,13,15,17)

            int msx = rand.Next(5) * 2;
            int msy = rand.Next(5) * 2;
            tileMap.initialize(new Vector2(15 + msx, 15 + msy));

            //otherwise the maze generator makes a mess
            MazeGenerator.Generate(tileMap, 25, 25);

            /*
            //Fill with empty prairie
            for (int i = 1; i < 15; i++)
                for (int j = 1; j < 15; j++)
                    tileMap.getTile(new Vector2(i,j)).type = 1;
            */
            //Make a new player
            player = new Player();
            player.name = gameDefinitions.playerName;
            player.postMortem = gameDefinitions.playerDeathMessage;
            player.nameArticle = "";
            player.nameDefArticle = "";
            player.goryPostMortem = gameDefinitions.playerGoryDeathMessage;
            player.image =  gameDefinitions.playerImage; //give it a fancy hat

            player.depth = 2;
            player.position.x = 1;
            player.position.y = 1;
            
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

            actors.Add(player);


            //Create a bunch of cougars
            Random r = new Random();
            for (int i = 0; i < 16; i++)
            {
                Enemy a = new Enemy();
                a.image = "ac_cougar";
                a.postMortem = "Evil glow fades from the cougar's eyes.";
                a.goryPostMortem = "The cougar is obliterated.";
                a.depth = 1;
                
                //TODO add check for tile collision here
                a.position.x = r.Next(tileMap.size.x - 2) + 1;
                a.position.y = r.Next(tileMap.size.y - 2) + 1;
                
                actors.Add(a);
            }
            updateLineOfSight();
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
    }
}
