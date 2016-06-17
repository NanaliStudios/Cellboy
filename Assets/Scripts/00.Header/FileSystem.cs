using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FileSystem : MonoBehaviour {
	
	public static byte[] WriteGameDataFromFile( GameData myGamedata, string filename )
	{
		#if !WEB_BUILD
		string path = pathForDocumentsFile( filename );
		FileStream file = new FileStream (path, FileMode.Create, FileAccess.ReadWrite);
		
		//StreamWriter sw = new StreamWriter( file );


		BinaryFormatter b = new BinaryFormatter();
		b.Serialize(file, myGamedata);

		//cloud
		byte[] fileBytes = null;
		fileBytes = new byte[file.Length];


		int n = file.Read(fileBytes, 0, fileBytes.Length); 
		Debug.Log("file Reading");

		file.Close();

		return fileBytes;
		#endif
	}
	
	
	public static GameData ReadGameDataFromFile( string filename)//, int lineIndex )
	{
		#if !WEB_BUILD
		string path = pathForDocumentsFile( filename );
		
		if (File.Exists(path))
		{
			FileStream file = new FileStream (path, FileMode.Open, FileAccess.ReadWrite);
			
			BinaryFormatter b = new BinaryFormatter();
			GameData ReadData = b.Deserialize(file) as GameData;
		
			file.Close();
			
			return ReadData;
		}
		else
		{
			Debug.Log("Can't find SaveFile");
			return null;
		}
		#else
		return null;
		#endif 
	}

	public static string pathForDocumentsFile( string filename ) 
	{ 
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			string path = Application.dataPath.Substring( 0, Application.dataPath.Length - 5 );
			path = path.Substring( 0, path.LastIndexOf( '/' ) );
			return Path.Combine( Path.Combine( path, "Documents" ), filename );
		}
		
		else if(Application.platform == RuntimePlatform.Android)
		{
			string path = Application.persistentDataPath;
			path = path.Substring(0, path.LastIndexOf( '/' ) );
			return Path.Combine (path, filename);
		}
		
		else 
		{
			string path = Application.dataPath;
			path = path.Substring(0, path.LastIndexOf( '/' ) );
			return Path.Combine (path, filename);
		}
	}

}
