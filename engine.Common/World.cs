using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using engine.Common.Entities;
using engine.Common.Entities.AI;

namespace engine.Common
{
    public struct WorldConfiguration
    {
        public int Width;
        public int Height;
        public bool CenterIndicator;
        public Menu StartMenu;
        public Menu EndMenu;
        public Menu HUD;
        public bool DisableZoom;
        public bool DisplayStats;
    }

    public class World
    {
        // Configuration
        //   Center marker
        //   HUD
        //   Zoom
        public World(WorldConfiguration config, Player[] players, Element[] obstacles, Background background)
        {
            // init
            Ephemerial = new List<EphemerialElement>();
            ZoomFactor = 1;
            Config = config;
            Details = new Dictionary<int, PlayerDetails>();

            // setup map
            Map = new Map(Config.Width, Config.Height, obstacles, background);

            // hook up map callbacks
            Map.OnEphemerialEvent += AddEphemerialElement;
            Map.OnElementHit += HitByAttack;
            Map.OnElementDied += PlayerDied;

            // process the players
            foreach(var player in players) AddItem(player);

            if (Human == null) throw new Exception("Must add at least 1 player (as human)");

            // setup window (based on placement on the map)
            WindowX = Human.X;
            WindowY = Human.Y;
            if (Human.Z > Constants.Ground) ZoomFactor = 0.05f;

            // start paused
            if (Config.StartMenu != null)
            {
                Menu = Config.StartMenu;
                Map.IsPaused = true;
            }
        }

        public void InitializeGraphics(IGraphics surface, ISounds sounds)
        {
            // graphics
            Surface = surface;
            Surface.SetTranslateCoordinates(TranslateCoordinates);

            // sounds
            Sounds = sounds;

            // initially render all the elements
            Paint();
        }

        public event Func<Menu> OnPaused;
        public event Action OnResumed;
        public event Action<Player, Element> OnContact;
        public event Func<Player, char, bool> OnBeforeKeyPressed;
        public event Func<Player, char, bool> OnAfterKeyPressed;

        public int Width { get { return Map.Width;  } }
        public int Height {  get { return Map.Height;  } }

        public Player Human { get; private set; }
        public int Alive
        {
            get
            {
                lock (Details)
                {
                    return Details.Where(p => p.Value.Player != null && !p.Value.Player.IsDead).Count();
                }
            }
        }

        public void Paint()
        {
            // exit early if there is no Surface to draw too
            if (Surface == null) return;

            // draw the map
            Map.Background.Draw(Surface);

            // add center indicator
            // TODO!
            if (Human.Z == Constants.Ground && Config.CenterIndicator)
            {
                var centerAngle = Collision.CalculateAngleFromPoint(Human.X, Human.Y, Map.Width / 2, Map.Height / 2);
                float x1, y1, x2, y2;
                var distance = Math.Min(Surface.Width, Surface.Height) * 0.9f;
                Collision.CalculateLineByAngle(Surface.Width / 2, Surface.Height / 2, centerAngle, (distance / 2), out x1, out y1, out x2, out y2);
                Surface.DisableTranslation();
                {
                    // draw an arrow
                    var endX = x2;
                    var endY = y2;
                    x1 = endX;
                    y1 = endY;
                    Collision.CalculateLineByAngle(x1, y1, (centerAngle + 180) % 360, 50, out x1, out y1, out x2, out y2);
                    Surface.Line(RGBA.Black, x1, y1, x2, y2, 10);

                    x1 = endX;
                    y1 = endY;
                    Collision.CalculateLineByAngle(x1, y1, (centerAngle + 135) % 360, 25, out x1, out y1, out x2, out y2);
                    Surface.Line(RGBA.Black, x1, y1, x2, y2, 10);

                    x1 = endX;
                    y1 = endY;
                    Collision.CalculateLineByAngle(x1, y1, (centerAngle + 225) % 360, 25, out x1, out y1, out x2, out y2);
                    Surface.Line(RGBA.Black, x1, y1, x2, y2, 10);
                }
                Surface.EnableTranslation();
            }

            lock (Details)
            {
                // draw all elements
                var hidden = new HashSet<int>();
                foreach (var elem in Map.WithinWindow(Human.X, Human.Y, Surface.Width * (1 / ZoomFactor), Surface.Height * (1 / ZoomFactor)))
                {
                    if (elem is Player) continue;
                    if (elem.IsDead) continue;
                    if (elem.IsTransparent)
                    {
                        // if the player is intersecting with this item, then do not display it
                        if (Map.IsTouching(Human, elem)) continue;

                        // check if one of the bots is hidden by this object
                        foreach(var detail in Details.Values)
                        {
                            // don't care about dead players
                            if (detail.Player.IsDead) continue;
                            // already hidden, do not need to recheck
                            if (hidden.Contains(detail.Player.Id)) continue;
                            // check
                            if (detail.Player is AI)
                            {
                                if (Map.IsTouching(detail.Player, elem))
                                {
                                    // this player is hidden
                                    hidden.Add(detail.Player.Id);
                                }
                            }
                        }
                    }
                    elem.Draw(Surface);
                }

                // draw the players
                int alive = 0;
                foreach (var detail in Details.Values)
                {
                    if (detail.Player.IsDead) continue;
                    alive++;
                    if (hidden.Contains(detail.Player.Id)) continue;
                    detail.Player.Draw(Surface);
                }
            } // lock(Details)

            // add any ephemerial elements
            lock (Ephemerial)
            {
                var toremove = new List<EphemerialElement>();
                var messageShown = false;
                foreach (var b in Ephemerial)
                {
                    if (b is OnScreenText)
                    {
                        // only show one message at a time
                        if (messageShown) continue;
                        messageShown = true;
                    }
                    b.Draw(Surface);
                    b.Duration--;
                    if (b.Duration < 0) toremove.Add(b);
                }
                foreach (var b in toremove)
                {
                    Ephemerial.Remove(b);
                }
            }

            // display the player counts
            if (Config.HUD != null)
            {
                Surface.DisableTranslation();
                {
                    Config.HUD.Draw(Surface);
                }
                Surface.EnableTranslation();
            }

            // display the player counts
            if (Config.DisplayStats)
            {
                Surface.DisableTranslation();
                {
                    Surface.Text(RGBA.Black, Surface.Width - 200, 10, string.Format("Alive {0}", Alive));
                    Surface.Text(RGBA.Black, Surface.Width - 200, 30, string.Format("Kills {0}", Human.Kills));
                }
                Surface.EnableTranslation();
            }

            // show a menu if present
            if (Map.IsPaused && Menu != null)
            {
                Menu.Draw(Surface);
            }
        }

        public void KeyPress(char key)
        {
            // inputs that are accepted while a menu is displaying
            if (Map.IsPaused)
            {
                switch(key)
                {
                    // menu
                    case Constants.Esc:
                        HideMenu();
                        break;
                }

                return;
            }

            // menu
            if (key == Constants.Esc)
            {
                ShowMenu();
                return;
            }

            // handle the user input
            bool result = false;
            float xdelta = 0;
            float ydelta = 0;

            // pass the key off to the caller to see if they know what to 
            // do in this case
            if (OnBeforeKeyPressed != null)
            {
                if (OnBeforeKeyPressed(Human, key)) return;
            }

            switch (key)
            {
                // move
                case Constants.Down:
                case Constants.Down2:
                case Constants.DownArrow:
                    ydelta = 1;
                    break;
                case Constants.Left:
                case Constants.Left2:
                case Constants.LeftArrow:
                    xdelta = -1;
                    break;
                case Constants.Right:
                case Constants.Right2:
                case Constants.RightArrow:
                    xdelta = 1;
                    break;
                case Constants.Up:
                case Constants.Up2:
                case Constants.UpArrow:
                    ydelta = -1;
                    break;

                case Constants.Switch:
                    // ActionEnum.SwitchWeapon;
                    result = SwitchTool(Human);
                    break;

                case Constants.Pickup:
                case Constants.Pickup2:
                    // ActionEnum.Pickup;
                    result = Pickup(Human);
                    break;

                case Constants.Drop3:
                case Constants.Drop2:
                case Constants.Drop4:
                case Constants.Drop:
                    // ActionEnum.Drop;
                    result = Drop(Human);
                    break;

                case Constants.Reload:
                case Constants.MiddleMouse:
                    // ActionEnum.Reload;
                    result = Reload(Human);
                    break;

                case Constants.Space:
                case Constants.LeftMouse:
                    // ActionEnum.Attack;
                    result = Attack(Human);
                    break;

                case Constants.RightMouse:
                    // use the mouse to move in the direction of the angle
                    float r = (Human.Angle % 90) / 90f;
                    xdelta = 1 * r;
                    ydelta = 1 * (1 - r);
                    if (Human.Angle > 0 && Human.Angle < 90) ydelta *= -1;
                    else if (Human.Angle > 180 && Human.Angle <= 270) xdelta *= -1;
                    else if (Human.Angle > 270) { ydelta *= -1; xdelta *= -1; }
                    break;
            }

            // pass the key off to the caller to see if they know what to 
            // do in this case
            if (OnAfterKeyPressed != null)
            {
                if (OnAfterKeyPressed(Human, key)) return;
            }

            // if a move command, then move
            if (xdelta != 0 || ydelta != 0)
            {
                // ActionEnum.Move;
                result = Move(Human, xdelta, ydelta);
            }
        }

        public void Mousewheel(float delta)
        {
            // disabled by the developer
            if (Config.DisableZoom) return;

            // block usage if a menu is being displayed
            if (Map.IsPaused) return;

            // only if on the ground
            if (Human.Z != Constants.Ground) return;

            // adjust the zoom
            if (delta < 0) ZoomFactor -= Constants.ZoomStep;
            else if (delta > 0) ZoomFactor += Constants.ZoomStep;

            // cap the zoom capability
            if (ZoomFactor < Constants.ZoomStep) ZoomFactor = Constants.ZoomStep;
            if (ZoomFactor > Constants.MaxZoomIn) ZoomFactor = Constants.MaxZoomIn;
        }

        public void Mousemove(float x, float y, float angle)
        {
            // block usage if a menu is being displayed
            if (Map.IsPaused) return;

            // use the angle to turn the human player
            Turn(Human, angle);
        }

        public void AddItem(Element item)
        {
            if (item is Player)
            {
                var details = new PlayerDetails() { Player = (item as Player) };

                lock (Details)
                {
                    // add to the set
                    Details.Add(item.Id, details);
                }

                // setup humans and AI
                if (item is AI)
                {
                    details.AITimer = new Timer(AIMove, item.Id, 0, Constants.GlobalClock);
                }
                else if (Human == null)
                {
                    Human = item as Player;
                }

                // initialize parachute (if necessary)
                if (item.Z > Constants.Ground)
                {
                    details.Parachute = new Timer(PlayerParachute, item.Id, 0, Constants.GlobalClock);
                }
            }

            Map.AddItem(item);
        }

        public void RemoveItem(Element item)
        {
            if (item is Player)
            {
                var player = (item as Player);

                // only allow a player to be removed if it is dead
                if (!player.IsDead) throw new Exception("A player must be dead to be removed");

                // drop ALL the players goodies
                Map.Drop(player);
                for (int i = 0; i < player.HandCapacity; i++)
                {
                    if (player.SwitchPrimary())
                    {
                        Map.Drop(player);
                    }
                }

                // set the players current ranking
                Human.Ranking = Alive;

                // clean up the dead players
                lock (Details)
                {
                    Details.Remove(player.Id);
                }
            }
            else throw new Exception("Invalid item to remove");
        }

        public void Play(string path)
        {
            // play sound
            Sounds.Play(path);
        }

        #region private
        class PlayerDetails
        {
            public Player Player;
            public Timer Parachute;
            public Timer AITimer;
        }

        private IGraphics Surface;
        private List<EphemerialElement> Ephemerial;
        private float ZoomFactor;
        private ISounds Sounds;
        private Map Map;
        private float WindowX;
        private float WindowY;
        private Dictionary<int, PlayerDetails> Details;
        private Menu Menu;
        private WorldConfiguration Config;

        private const string NothingSoundPath = "media/nothing.wav";
        private const string PickupSoundPath = "media/pickup.wav";

        // menu items
        private void ShowMenu()
        {
            Map.IsPaused = true;

            if (OnPaused != null)
            {
                Menu = OnPaused();
            }
        }

        private void HideMenu()
        {
            if (OnResumed != null)
            {
                OnResumed();
            }

            Map.IsPaused = false;
        }

        // callbacks to support time lapse actions
        private void PlayerParachute(object state)
        {
            // block usage if a menu is being displayed
            if (Map.IsPaused) return;

            // execute the parachute
            int id = (int)state;

            // grab the details
            PlayerDetails detail = null;
            lock (Details)
            {
                if (!Details.TryGetValue(id, out detail))
                {
                    // the player must be dead and caused the record to be cleaned up
                    return;
                }
            }

            if (detail.Player.Z <= Constants.Ground)
            {
                // ensure the player is on the ground
                detail.Player.Z = Constants.Ground;
                detail.Parachute.Dispose();

                // check if the player is touching an object, if so then move
                int count = 100;
                float xstep = 0.01f;
                float xmove = 10f;
                if (detail.Player.X > Map.Width / 2)
                {
                    // move the other way
                    xstep *= -1;
                    xmove *= -1;
                }
                do
                {
                    float xdelta = xstep;
                    float ydelta = 0;
                    if (Map.Move(detail.Player, ref xdelta, ref ydelta))
                    {
                        break;
                    }

                    // move over
                    detail.Player.X += xmove;
                    if (detail.Player.Id == Human.Id) WindowX += xmove;
                }
                while (count-- > 0);

                if (count <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("Failed to move after parachute");
                }

                return;
            }

            // decend
            detail.Player.Z -= (Constants.ZoomStep/10);

            if (detail.Player.Id == Human.Id)
            {
                // zoom in
                ZoomFactor += (Constants.ZoomStep / 10);
            }
        }

        // AI
        private void AIMove(object state)
        {
            // block usage if a menu is being displayed
            if (Map.IsPaused) return;

            // move the AI
            int id = (int)state;
            Stopwatch timer = new Stopwatch();

            // grab the details
            PlayerDetails detail = null;
            lock (Details)
            {
                if (!Details.TryGetValue(id, out detail))
                {
                    // the player must be dead and caused the record to be cleaned up
                    return;
                }
            }

            timer.Start();
            if (detail.Player is AI)
            {
                AI ai = detail.Player as AI;
                float xdelta = 0;
                float ydelta = 0;
                float angle = 0;

                // the timer is reentrant, so only allow one instance to run
                if (System.Threading.Interlocked.CompareExchange(ref ai.RunningState, 1, 0) != 0) return;

                if (ai.IsDead)
                {
                    // remove the AI player
                    RemoveItem(ai);

                    // stop the timer
                    detail.AITimer.Dispose();
                    return;
                }

                // NOTE: Do not apply the ZoomFactor (as it distorts the AI when debugging) - TODO may want to allow this while parachuting
                // TODO will likely want to translate into a copy of the list with reduced details
                List<Element> elements = Map.WithinWindow(ai.X, ai.Y, Constants.ProximityViewWidth, Constants.ProximityViewHeight).ToList();
                var angleToCenter = Collision.CalculateAngleFromPoint(ai.X, ai.Y, Config.Width / 2, Config.Height / 2);
                var inZone = Map.Background.Damage(ai.X, ai.Y) > 0;

                // get action from AI

                var action = ai.Action(elements, angleToCenter, inZone, ref xdelta, ref ydelta, ref angle);

                // turn
                ai.Angle = angle;

                // perform action
                bool result = false;
                Type item = null;
                switch (action)
                {
                    case ActionEnum.Drop:
                        item = Map.Drop(ai);
                        result |= (item != null);
                        ai.Feedback(action, item, result);
                        break;
                    case ActionEnum.Pickup:
                        item = Map.Pickup(ai);
                        result |= (item != null);
                        ai.Feedback(action, item, result);
                        break;
                    case ActionEnum.Reload:
                        var reloaded = ai.Reload();
                        result |= (reloaded == AttackStateEnum.Reloaded);
                        ai.Feedback(action, reloaded, result);
                        break;
                    case ActionEnum.Attack:
                        var attack = Map.Attack(ai);
                        result |= attack == AttackStateEnum.FiredAndKilled || attack == AttackStateEnum.FiredWithContact ||
                            attack == AttackStateEnum.MeleeAndKilled || attack == AttackStateEnum.MeleeWithContact;
                        ai.Feedback(action, attack, result);
                        break;
                    case ActionEnum.SwitchPrimary:
                        var swap = ai.SwitchPrimary();
                        result |= swap;
                        ai.Feedback(action, null, result);
                        break;
                    case ActionEnum.Move:
                    case ActionEnum.None:
                        break;
                    default: throw new Exception("Unknown ai action : " + action);
                }

                // have the AI move
                float oxdelta = xdelta;
                float oydelta = ydelta;
                var moved = Map.Move(ai, ref xdelta, ref ydelta);
                ai.Feedback(ActionEnum.Move, null, moved);

                // ensure the player stays within the map
                if (ai.X < 0 || ai.X > Map.Width || ai.Y < 0 || ai.Y > Map.Height)
                    System.Diagnostics.Debug.WriteLine("Out of bounds");

                // set state back to not running
                System.Threading.Volatile.Write(ref ai.RunningState, 0);
            }
            timer.Stop();

            if (timer.ElapsedMilliseconds > 100) System.Diagnostics.Debug.WriteLine("**AIMove Duration {0} ms", timer.ElapsedMilliseconds);
        }

        // support
        private bool TranslateCoordinates(bool autoScale, float x, float y, float width, float height, float other, out float tx, out float ty, out float twidth, out float theight, out float tother)
        {
            // transform the world x,y coordinates into scaled and screen coordinates
            tx = ty = twidth = theight = tother = 0;

            float zoom = (autoScale) ? ZoomFactor : 1;

            // determine scaling factor
            float scale = (1 / zoom);
            width *= zoom;
            height *= zoom;

            // Surface.Width & Surface.Height are the current windows width & height
            float windowHWidth = Surface.Width / 2.0f;
            float windowHHeight = Surface.Height / 2.0f;

            // now translate to the window
            tx = ((x - WindowX) * zoom) + windowHWidth;
            ty = ((y - WindowY) * zoom) + windowHHeight;
            twidth = width;
            theight = height;
            tother = other * zoom;

            return true;
        }

        private void AddEphemerialElement(EphemerialElement element)
        {
            lock (Ephemerial)
            {
                Ephemerial.Add(element);
            }
        }

        // human movements
        private bool SwitchTool(Player player)
        {
            if (player.IsDead) return false;
            return player.SwitchPrimary();
        }

        private bool Pickup(Player player)
        {
            if (player.IsDead) return false;
            if (Map.Pickup(player) != null)
            {
                // play sound
                Sounds.Play(PickupSoundPath);
                return true;
            }
            return false;
        }

        private bool Drop(Player player)
        { 
            if (player.IsDead) return false;
            return Map.Drop(player) != null;
        }

        private bool Reload(Player player)
        {
            if (player.IsDead) return false;
            var state = player.Reload();
            switch (state)
            {
                case AttackStateEnum.Reloaded:
                    if (player.Primary is RangeWeapon) Sounds.Play((player.Primary as RangeWeapon).ReloadSoundPath());
                    else throw new Exception("Invalid action for a non-gun");
                    break;
                case AttackStateEnum.None:
                case AttackStateEnum.NoRounds:
                    Sounds.Play(NothingSoundPath);
                    break;
                case AttackStateEnum.FullyLoaded:
                    // no sound
                    break;
                default: throw new Exception("Unknown GunState : " + state);
            }

            return (state == AttackStateEnum.Reloaded); 
        }

        private bool Attack(Player player)
        {
            if (player.IsDead) return false;
            var state = Map.Attack(player);

            // play sounds
            switch (state)
            {
                case AttackStateEnum.Melee:
                case AttackStateEnum.MeleeWithContact:
                case AttackStateEnum.MeleeAndKilled:
                    Sounds.Play(player.Fists.UsedSoundPath());
                    break;
                case AttackStateEnum.FiredWithContact:
                case AttackStateEnum.FiredAndKilled:
                case AttackStateEnum.Fired:
                    if (player.Primary is RangeWeapon) Sounds.Play((player.Primary as RangeWeapon).FiredSoundPath());
                    else throw new Exception("Invalid action for a non-gun");
                    break;
                case AttackStateEnum.NoRounds:
                case AttackStateEnum.NeedsReload:
                    if (player.Primary is RangeWeapon) Sounds.Play((player.Primary as RangeWeapon).EmptySoundPath());
                    else throw new Exception("Invalid action for a non-gun");
                    break;
                case AttackStateEnum.LoadingRound:
                case AttackStateEnum.None:
                    Sounds.Play(NothingSoundPath);
                    break;
                default: throw new Exception("Unknown GunState : " + state);
            }

            return (state == AttackStateEnum.MeleeAndKilled ||
                state == AttackStateEnum.MeleeWithContact ||
                state == AttackStateEnum.FiredAndKilled ||
                state == AttackStateEnum.FiredWithContact);
        }

        private bool Move(Player player, float xdelta, float ydelta)
        {
            if (player.IsDead) return false;
            if (Map.Move(player, ref xdelta, ref ydelta))
            {
                // move the screen
                WindowX += xdelta;
                WindowY += ydelta;

                return true;
            }
            else
            {
                // TODO may want to move back a bit in the opposite direction
                return false;
            }
        }

        private void Turn(Player player, float angle)
        {
            if (player.IsDead) return;
            player.Angle = angle;
        }

        // callbacks
        private void HitByAttack(Player player, Element element)
        {
            // play sound if the human is hit
            if (element is Player && element.Id == Human.Id)
            {
                Sounds.Play(Human.HurtSoundPath);
            }

            // notify the outside world that we hit something
            if (OnContact != null) OnContact(player, element);
        }

        private void PlayerDied(Element element)
        {
            // check for winner/death (element may be any element that can take damage)
            if (element is Player)
            {
                // remove this player
                RemoveItem(element);

                // pause
                if (Config.EndMenu != null)
                {
                    Menu = Config.EndMenu;
                    ShowMenu();
                }
            }
        }
        #endregion
    }
}
