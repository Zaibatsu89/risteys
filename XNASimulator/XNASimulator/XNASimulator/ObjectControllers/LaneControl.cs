using System.Collections.Generic;
using KruispuntGroep6.Simulator.Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using KruispuntGroep6.Simulator.Objects;

namespace KruispuntGroep6.Simulator.ObjectControllers
{
    class LaneControl
    {
        private Lists lists;
        private List<string> laneIDs;

        public LaneControl(Lists lists)
        {
            this.lists = lists;
            this.laneIDs = new List<string>();

            InitLanes();
        }

        private void InitLanes()
        {
            for (int i = 0; i < 8; i++)
            {
                laneIDs.Add("N" + i);
            }
            for (int i = 0; i < 8; i++)
            {
                laneIDs.Add("E" + i);
            }
            for (int i = 0; i < 8; i++)
            {
                laneIDs.Add("S" + i);
            }
            for (int i = 0; i < 8; i++)
            {
                laneIDs.Add("W" + i);
            }

            foreach (string ID in laneIDs)
            {
                lists.Lanes.Add(new Lane(ID));
            }
        }

        public void LoadLanes()
        {
            foreach (Lane lane in lists.Lanes)
            {
                LoadLane(lane);
            }
        }

        private void LoadLane(Lane lane)
        {
            switch (lane.laneID)
            {
                case "N0": 
                    break;
                case "N1": LoadLane(new Vector2(7, 0), lane);
                    break;
                case "N2": LoadLane(new Vector2(8, 0), lane);
                    break;
                case "N3": LoadLane(new Vector2(9, 0), lane);
                    break;
                case "N4": LoadLane(new Vector2(10, 0), lane);
                    break;
                case "N5": LoadLane(new Vector2(11, 0), lane);
                    break;
                case "N6": LoadLane(new Vector2(12, 0), lane);
                    break;
                case "N7":
                    break;

                case "E0":
                    break;
                case "E1": LoadLane(new Vector2(19, 7), lane);
                    break;
                case "E2": LoadLane(new Vector2(19, 8), lane);
                    break;
                case "E3": LoadLane(new Vector2(19, 9), lane);
                    break;
                case "E4": LoadLane(new Vector2(19, 10), lane);
                    break;
                case "E5": LoadLane(new Vector2(19, 11), lane);
                    break;
                case "E6": LoadLane(new Vector2(19, 12), lane);
                    break;
                case "E7":
                    break;

                case "W0":
                    break;
                case "W1": LoadLane(new Vector2(0, 7), lane);
                    break;
                case "W2": LoadLane(new Vector2(0, 8), lane);
                    break;
                case "W3": LoadLane(new Vector2(0, 9), lane);
                    break;
                case "W4": LoadLane(new Vector2(0, 10), lane);
                    break;
                case "W5": LoadLane(new Vector2(0, 11), lane);
                    break;
                case "W6": LoadLane(new Vector2(0, 12), lane);
                    break;
                case "W7":
                    break;

                case "S0":
                    break;
                case "S1": LoadLane(new Vector2(7, 19), lane);
                    break;
                case "S2": LoadLane(new Vector2(8, 19), lane);
                    break;
                case "S3": LoadLane(new Vector2(9, 19), lane);
                    break;
                case "S4": LoadLane(new Vector2(10, 19), lane);
                    break;
                case "S5": LoadLane(new Vector2(11, 19), lane);
                    break;
                case "S6": LoadLane(new Vector2(12, 19), lane);
                    break;
                case "S7": 
                    break;
            }
        }

        private void LoadLane(Vector2 gridposition, Lane lane)
        {
            Tile startTile = lists.Tiles[(int)gridposition.X, (int)gridposition.Y];

            lane.laneTiles.Add(startTile);
            lane.spawnTile = (startTile);

            if (lane.laneID.Contains("N"))
            {
                for (int i = 0; i < 7; i++)
                {
                    lane.laneTiles.Add(lists.Tiles[(int)gridposition.X, i]);
                }
            }
            else if (lane.laneID.Contains("E"))
            {
                for (int i = 19; i > 13; i--)
                {
                    lane.laneTiles.Add(lists.Tiles[i, (int)gridposition.Y]);
                }
            }
            else if (lane.laneID.Contains("W"))
            {
                for (int i = 0; i < 7; i++)
                {
                    lane.laneTiles.Add(lists.Tiles[i, (int)gridposition.Y]);
                }
            }
            else if (lane.laneID.Contains("S"))
            {
                for (int i = 19; i > 13; i--)
                {
                    lane.laneTiles.Add(lists.Tiles[(int)gridposition.X, i]);
                }
            }
        }

        public void Update(GameTime gametime)
        {
        }

        public void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
        }
    }
}