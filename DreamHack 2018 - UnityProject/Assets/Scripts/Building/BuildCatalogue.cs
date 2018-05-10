﻿using System.Collections;
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
            get {
                return new BuildCategory[] { FoundationCategory, InteriorCategory, TechnologyCategory, TrapsCategory};
            }
        }

        public BuildCatalogue()
        {
            FoundationCategory = new BuildCategory("Foundation", "Foundation")
                .Add(new BuildEntry("Wall", typeof(TileObjects.WallTileObject), 0))
                .Add(new BuildEntry("Windowed Wall", typeof(TileObjects.WallTileObject), 1))
                .Add(new BuildEntry("Wall Corner", typeof(TileObjects.WallCornerTileObject), 0))
                .Add(new BuildEntry("Floor 1", typeof(TileFloors.DefaultTileFloor), 0))
                .Add(new BuildEntry("Floor 2", typeof(TileFloors.DefaultTileFloor), 1))
                .Add(new BuildEntry("Floor 3", typeof(TileFloors.DefaultTileFloor), 2))
            ;

            InteriorCategory = new BuildCategory("Interior", "Interior")
                .Add(new BuildEntry("Plant", typeof(TileObjects.CliffObject), 0))
            ;

            TechnologyCategory = new BuildCategory("Technology", "Technology");
            TrapsCategory = new BuildCategory("Traps", "Traps");
        }
    }
}