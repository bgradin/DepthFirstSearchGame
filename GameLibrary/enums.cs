
namespace GameClassLibrary
{
	public enum GameState
	{
		Uninitialized,
		PregameMenu,
		Login,
		Register,
		LoggingIn,
		Registering,
		LoadingFromServer,
		HowToPlay,
		InGame,
		NUM_VALS
	}

	public enum RefreshContentType
	{
		Solids,
		Graphics,
		All,
		NUM_VALS
	}

	public enum Field
	{
		Image,
		Tile,
		GraphicTile,
		AnimatedTile,
		SpecialTile,
		NPCTile,
		InteractiveTile
	}

	#region From_Map_Editor

	public enum LAYERS
	{
		Graphic,
		Special,
		NPC,
		Interactive,
		Item,
		NUM_VALS
	}

	public enum Direction
	{
		Up, Down, Left, Right
	}

	public enum MapField
	{
		MapInfo,
		SpawnInfo,
		MapLayer,
		NUM_VALS
	}

	public enum MapType
	{
		Trench,
		TrenchCave,
		CoralReef,
		SeaFloor,
		Shallows,
		NUM_VALS
	}

	public enum TileType
	{
		Animated,
		Graphic,
		Special,
		NPC,
		Interactive,
		Tile,
		NUM_VALS
	}

	public enum ItemTileSpec : byte
	{
		BACTERIA,
		NONE,
		NUM_VALS
	}

	public enum SpecialTileSpec
	{
		WALL,
		WARP,
		NONE,
		NUM_VALS
	}

	public enum InteractiveTileSpec
	{
		ITEM,
		CUT,
		STRENGTH,
		PC,
		BERRY,
		SIGN,
		NUM_VALS
	}

	public enum WarpAnim
	{
		NONE,
		DOOR,
		FLY,
		NUM_VALS
	}

	public enum RenderOrder
	{
		Background,
		GraphicLayer,
		NPCLayer,
		EffectsLayer,
		OverlayLayer,
		PauseMenuLayer,
		ChatLayer,
		UILayer = 20, // Always draw UI last
		NUM_VALS
	}
	#endregion
}