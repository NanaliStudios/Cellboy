using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FileSystem : MonoBehaviour {
	
	public static byte[] WriteGameDataFromFile( GameData myGamedata, string filename )
	{
		#if !WEB_BUILD
		string path = pathForDocumentsFile( filename );
		FileStream file = new FileStream (path, FileMode.Create, FileAccess.Write);
		
		//StreamWriter sw = new StreamWriter( file );


		BinaryFormatter b = new BinaryFormatter();
		b.Serialize(file, myGamedata);

		//cloud
		byte[] fileBytes = null;
		fileBytes = new byte[file.Length];

		int bytecount = 0;

		while(bytecount > 0)
		{
			int n = file.Read(fileBytes, bytecount, fileBytes.Length); 

			if(n == 0)
				break;
		}
		//sw.Write(); 
		
		//sw.Close();
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
			FileStream file = new FileStream (path, FileMode.Open, FileAccess.Read);
			StreamReader sr = new StreamReader( file );
			
			BinaryFormatter b = new BinaryFormatter();
			GameData ReadData = b.Deserialize(file) as GameData;
		
			file.Close();
			
			return ReadData;
		}
		
		else
		{
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
