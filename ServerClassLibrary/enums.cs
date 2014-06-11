
namespace ServerClassLibrary
{
	public enum ServerAction : byte
	{
		None,
		ServerMessage,
		ServerKick,
		ServerShutdown,
		ServerResponse,
		ClientSay,
		ClientDisconnect,
		ClientCreateAcc,
		ClientCreateChar,
		ClientLogin,
		ClientLoginResponse,
		ClientLogout,
		ClientCollectItem,
		TestLargeSend,
		Walk,
		WalkResponse,
		Content, //16
		Warp,
		NUM_VALS,
		ERROR_STATE = 255
	}

	public enum ServerErrorResponse
	{
		Success = 0,

		//General errors
		DataFormatError,
		SQLQueryError,
		InvalidAccessError,

		//Account errors
		AccountAlreadyExists,
		AccountPasswordStrength,
		AccountCredentialMismatch,

		//Server errors
		ServerFatalError,
		ServerTooManyClients,

		NUM_VALS
	}

	public enum ContentType : byte
	{
		Players,
		Items,
		NUM_VALS
	}

	/// <summary>
	/// Names match their respective column names in the database
	/// Database names are in all lowercase
	/// </summary>
	public enum DBAccountIndex
	{
		ID,
		UserName,
		Password,
		LastIP,
		LoggedIn,
		BactCount,
		LocMap,
		LocX,
		LocY,
		AdminLevel
	}
}