using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using Utf8Json;

namespace am
{
    
[CreateAssetMenu( fileName = "ENPSetting", menuName = "ENP/Setting", order = 1000 )]
public class ENPSetting : ScriptableObject
{
    [Serializable]
    public class HwInfo {
	public string controlIp   = "192.168.31.10";
	public int    controlPort = 10393;
    }

    [Serializable]
    public class NetworkParams {
	public string name;
	[Range(32,8096)]
	[DataMember(Name = "bandUp")]	
	public int bandUp = 1024;
	[Range(32,8096)]
	[DataMember(Name = "bandDw")]	
	public int bandDw = 1024;
	[Range(0,3000)]
	[DataMember(Name = "delayUp")]	
	public int delayUp = 0;
	[Range(0,3000)]
	[DataMember(Name = "delayDw")]	
	public int delayDw = 0;
	[Range(0,100)]
	[DataMember(Name = "lossUp")]	
	public int lossUp = 0;
	[Range(0,100)]
	[DataMember(Name = "lossDw")]	
	public int lossDw = 0;
	[Range(0,1)]
	[DataMember(Name = "disconnUp")]	
	public int disconnUp = 0;
	[Range(0,1)]
	[DataMember(Name = "disconnDw")]	
	public int disconnDw = 0;

	public bool foldFlag { get; set; }
    }

    [Serializable]
    public class EMNPiApiResp {
	[DataMember(Name = "status")]	
	public string status = "E_CHAOS";
    }
    
    public List<NetworkParams> paramList = new List<NetworkParams>();
    public HwInfo hwInfo = new HwInfo();

    /// <summary>
    ///   プリセット名から定義済パラメータを選択して、制御パケットを送る 
    /// </summary>
    public bool Apply(string name){
	var param = paramList.FirstOrDefault(node => node.name == name);
	if(param == null){ return false; }
	return SendControlPacket(param);
    }
    
    /// <summary>
    ///   パラメータを直接渡して、制御パケットを送る 
    /// </summary>
    public bool SendControlPacket(NetworkParams param){
	Socket sock = null;
	bool   ret  = false; 
	try {
	    var endPoint = new IPEndPoint(IPAddress.Parse(hwInfo.controlIp), hwInfo.controlPort) as EndPoint;
	    sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
	    sock.ReceiveTimeout = 1000;
	    var sendBuf = Encoding.ASCII.GetBytes(JsonSerializer.ToJsonString(param));
	    var sendRet = sock.SendTo(sendBuf, 0, sendBuf.Length, SocketFlags.None, endPoint);	    
	    if(sendRet < 0){ goto wayout; }
	    //UnityEngine.Debug.Log("Send Ret : " + sendRet.ToString());
	    var recvBuf = new byte[1024];
	    var recvRet = sock.ReceiveFrom(recvBuf, 0, recvBuf.Length, SocketFlags.None, ref endPoint);
	    if(sendRet < 0){ goto wayout; }
	    //UnityEngine.Debug.Log("Recv Ret : " + recvRet.ToString());
	    var resp = JsonSerializer.Deserialize<EMNPiApiResp>(recvBuf);
	    if(resp.status == "E_OK"){ ret = true; }
	}
	catch (SocketException e){ UnityEngine.Debug.Log("Socket Error : " + e.ToString()); }
	catch (Exception e){ UnityEngine.Debug.Log("Fatal Error : " + e.ToString()); }	    	    

      wayout:
	
	if(sock != null){ sock.Close();	}
	
	return ret;
    }

    /// <summary>
    ///   Export to JsonString
    /// </summary>
    public string ExportToJson(){
	return JsonSerializer.ToJsonString(paramList);
    }

    /// <summary>
    ///   Import from JsonString
    /// </summary>
    public void ImportFromJson(string json){
	paramList = JsonSerializer.Deserialize<List<NetworkParams>>(json);
    }

    void CreateParentDirectory(string itemPath){
	var path = Path.GetDirectoryName(itemPath);
	if(Directory.Exists(path)){ return; }
	Directory.CreateDirectory(path);
    }

    string GetSavePath(string key){
	var savePath = Application.persistentDataPath + Path.DirectorySeparatorChar;
	savePath +=  key + ".json";
	return savePath;
    }

    public async Task Save(string key){
	var json = ExportToJson();
	var savepath = GetSavePath(key);
	CreateParentDirectory(savepath);
	UnityEngine.Debug.Log("Save to : " + savepath);
	using(var sw = new StreamWriter(savepath, false, Encoding.GetEncoding("utf-8"))){ await sw.WriteAsync(json); }
    }

    public async Task Load(string key){
	var savepath = GetSavePath("ENPSetting");
	if(File.Exists(savepath)){
	    using(var sr = new StreamReader(savepath, Encoding.GetEncoding("utf-8"))){
		string json = await sr.ReadToEndAsync();
		ImportFromJson(json);
	    }
	}
    }
    
    
}
}

/*
 * Local variables:
 * compile-command: ""
 * End:
 */
