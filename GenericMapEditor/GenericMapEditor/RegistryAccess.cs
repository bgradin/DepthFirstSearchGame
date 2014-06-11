using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Windows.Forms;
using GameClassLibrary;

namespace GenericMapEditor
{
	enum RegistrySetting
	{
		WindowWidth = EditorConst.WINDOW_DEFAULT_WIDTH,
		WindowHeight = EditorConst.WINDOW_DEFAULT_HEIGHT,
		WindowX = EditorConst.WINDOW_DEFAULT_X,
		WindowY = EditorConst.WINDOW_DEFAULT_Y,
		WindowMaximized,
		RecentMaps,
		RecentTilesets
	}

	static class RegistryAccess
	{
		public static object GetValue(RegistrySetting setting, RegistryValueKind kind = RegistryValueKind.DWord)
		{
			return GetValueOrDefault(setting, (int)setting, kind);
		}

		public static object GetValueOrDefault(RegistrySetting setting, object defaultValue, RegistryValueKind kind = RegistryValueKind.DWord)
		{
			VerifyAccess();

			object ret = key.GetValue(setting.ToString());

			if (ret == null)
			{
				key.SetValue(setting.ToString(), defaultValue);
				return defaultValue;
			}
			else if (key.GetValueKind(setting.ToString()) != kind)
				throw new Exception("setting parameter doesn't match kind parameter");
			else
				return ret;
		}

		public static void SetValue(RegistrySetting setting, object value, RegistryValueKind kind = RegistryValueKind.DWord)
		{
			VerifyAccess();

			object ret = key.GetValue(setting.ToString());

			if (ret != null && key.GetValueKind(setting.ToString()) != kind)
				throw new Exception("setting parameter doesn't match kind parameter");

			key.SetValue(setting.ToString(), value, kind);
		}

		public static void AddRecentFile(string filename, RegistrySetting setting)
		{
			if (setting.ToString().Substring(0, 6) != "Recent")
				return;

			VerifyAccess();

			string[] recentList = (string[])GetValueOrDefault(setting, new string[]{""}, RegistryValueKind.MultiString);
			if (recentList == null)
				recentList = new string[0];

			int positionInRecentFiles = -1;
			for (int i = 0; i < recentList.Length; i++)
			{
				if (recentList[i] == filename)
				{
					positionInRecentFiles = i;
					break;
				}
			}

			if (recentList.Length < EditorConst.MAXIMUM_RECENT_FILES && recentList[0] != "" && positionInRecentFiles == -1)
				Array.Resize<string>(ref recentList, recentList.Length + 1);

			for (int i = positionInRecentFiles != -1 ? positionInRecentFiles : recentList.Length - 1; i > 0; i--)
				recentList[i] = recentList[i - 1];

			recentList[0] = filename;

			SetValue(setting, recentList, RegistryValueKind.MultiString);
		}

		public static string MakeShorter(string file)
		{
			if (file.Length < 15)
				return file;

			int endFirst = file.IndexOf('\\', 0, 3) + 1, beginSec = file.LastIndexOf('\\');
			return file.Substring(0, endFirst) + "..." + file.Substring(beginSec, file.Length - beginSec);
		}

		private static void VerifyAccess()
		{
			// Reload the key each time, in case something else change the key while we weren't looking
			key = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("GenericMapEditor");

			if (key == null)
				throw new Exception("Inconceivable!");
		}

		static RegistryKey key;
	}
}
