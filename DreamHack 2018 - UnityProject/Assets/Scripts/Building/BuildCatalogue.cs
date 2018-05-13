using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Building
{
    public class BuildCatalogue
    {
        public BuildCategory FoundationCategory { get; private set; }
        public BuildCategory InteriorCategory { get; private set; }
        public BuildCategory TechnologyCategory { get; private set; }
        public BuildCategory TrapsCategory { get; private set; }

        public BuildCategory[] Categories
        {
            get
            {
                return new BuildCategory[] { FoundationCategory, InteriorCategory, TechnologyCategory, TrapsCategory};
            }
        }

        public BuildCatalogue()
        {
            FoundationCategory = new BuildCategory("Foundation", "Foundation")
                .Add(new BuildEntry("Wall", typeof(TileObjects.WallTileObject)))
                .Add(new BuildEntry("Wall Convex Corner", typeof(TileObjects.WallConvexCornerTileObject)))
                .Add(new BuildEntry("Wall Concave Corner", typeof(TileObjects.WallConcaveCornerTileObject)))
                .Add(new BuildEntry("Door", typeof(TileObjects.DoorTileObject)))
                .Add(new BuildEntry("Floor", typeof(TileFloors.DefaultTileFloor)))
            ;

            InteriorCategory = new BuildCategory("Interior", "Interior");
            TechnologyCategory = new BuildCategory("Technology", "Technology")
                .Add(new BuildEntry("Miner", typeof(TileObjects.MinerTileObject)))
           ;
            TrapsCategory = new BuildCategory("Traps", "Traps");
        }
    }
}