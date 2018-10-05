using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace am
{

[CustomEditor(typeof(ENPSetting))]
public class ENPSettingInspector : InspectorBase<ENPSetting>
{
    
    protected override void DrawCustomInspector(){
	var so = target as ENPSetting;

	DrawSimpleLabelField("EM-uNetPi ハード情報", "", null, 200f);
	DrawSimpleTextField(so, "制御IP", ref so.hwInfo.controlIp);
	DrawSimpleIntField(so, "制御ポート", ref so.hwInfo.controlPort);
	
	GUILayout.Space(5);
	GUILayout.Box(GUIContent.none, HrStyle.EditorLine, GUILayout.ExpandWidth(true), GUILayout.Height(1f));
	GUILayout.Space(5);
	DrawList<ENPSetting.NetworkParams>(so, so.paramList);
    }
    
    public override void OnInspectorGUI()
    {
	DrawCustomInspector();
    }

    protected override void DrawListNode<NodeT>(NodeT nodeBase){
	var so   = target as ENPSetting;
	var node = nodeBase as ENPSetting.NetworkParams;

	if(node.foldFlag = Foldout(node.foldFlag, node.name)){
	    
	    GUILayout.Space(3);
	    DrawSimpleTextField(so, "Name", ref node.name);
	    GUILayout.Space(3);

	    DrawSimpleLabelField("通信帯域（kbps）", "", null, 150f);
	    EditorGUILayout.BeginHorizontal();
	    {	
		DrawSimpleIntSlider(so, "UP",   ref node.bandUp, 32, 8096, 24f);
		DrawSimpleIntSlider(so, "DW", ref node.bandDw, 32, 8096, 24f);
	    }
	    EditorGUILayout.EndHorizontal();		
	    GUILayout.Space(3);
	    
	    DrawSimpleLabelField("パケット遅延（msec）", "", null, 150f);
	    EditorGUILayout.BeginHorizontal();
	    {	
		DrawSimpleIntSlider(so, "UP",   ref node.delayUp, 0, 3000, 24f);
		DrawSimpleIntSlider(so, "DW", ref node.delayDw, 0, 3000, 24f);
	    }
	    EditorGUILayout.EndHorizontal();
	    GUILayout.Space(3);
	    
	    DrawSimpleLabelField("パケットロスト（%）", "", null, 150f);
	    EditorGUILayout.BeginHorizontal();
	    {		
		DrawSimpleIntSlider(so, "UP",   ref node.lossUp, 0, 100, 24f);
		DrawSimpleIntSlider(so, "DW", ref node.lossDw, 0, 100, 24f);
	    }
	    EditorGUILayout.EndHorizontal();
	    GUILayout.Space(3);
	
	    DrawSimpleLabelField("切断エミュレート", "", null, 150f);
	    EditorGUILayout.BeginHorizontal();
	    {
		bool up = (node.disconnUp != 0) ? true: false;		
		DrawSimpleBoolField(so, "UP", ref up, 24f);
		node.disconnUp = up ? 1 : 0;
		bool dw = (node.disconnDw != 0) ? true: false; 		
		DrawSimpleBoolField(so, "DW", ref dw, 24f);
		node.disconnDw = dw ? 1 : 0;
	    }
	    EditorGUILayout.EndHorizontal();	
	}
	GUILayout.Space(3);		
	if(GUILayout.Button("Apply", EditorStyles.miniButton)){
	    var ret = so.SendControlPacket(node);
	    if(ret){
		var disconnUp = (node.disconnUp != 0) ? "有効" : "無効";
		var disconnDw = (node.disconnDw != 0) ? "有効" : "無効";
		m_callbackQueue.Push(() => {		
			EditorUtility.DisplayDialog
			("EM-uNetPi :: Param Apply", 
			 "Name : " + node.name + "\n"  
			 + "帯域 UP : " + node.bandUp.ToString() + " kbps / DW : " + node.bandDw.ToString() + " kbps\n" 
			 + "遅延 UP : " + node.delayUp.ToString() + " msec / DW : " + node.delayDw.ToString() + " msec\n" 
			 + "パケロス UP : " + node.lossUp.ToString() + " % / DW : " + node.lossDw.ToString() + " %\n" 
			 + "切断 UP : " + disconnUp + " / DW : " + disconnDw
			 ,"OK");
		    });
		m_isTermPollCallbackQueue = false;
                EditorApplication.delayCall += PollCallbackQueue;		
	    }
	    else {
		m_callbackQueue.Push(() => {				
			EditorUtility.DisplayDialog
			("EM-uNetPi :: Param Apply", 
			 "失敗"  
			 ,"OK");
		    });
		m_isTermPollCallbackQueue = false;
                EditorApplication.delayCall += PollCallbackQueue;		
	    }
	}
	GUILayout.Space(3);
    }

    
}
}    

/*
 * Local variables:
 * compile-command: ""
 * End:
 */
