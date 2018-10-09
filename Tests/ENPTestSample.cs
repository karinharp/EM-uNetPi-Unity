using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;
using System.Threading;
using System.Threading.Tasks;
using am;

namespace amTest
{

public class ENPTestSample {    

    [SerializeField]
    ENPSetting m_ENPSetting;
    
    [SetUp]
    public void Init(){
	m_ENPSetting = ScriptableObject.Instantiate(Resources.Load("ScriptableObjects/MNPSetting")) as ENPSetting;
	m_ENPSetting.Apply("mobile3G");
    }

    [UnityTest]
    public IEnumerator Bench(){
	yield return BenchAsync().ToEnumerator();
    }

    async Task BenchAsync(){
	/*
	 * DO your network test !
	 */
	await Task.Delay(3000);
    }
    
    [TearDown]
    public void Dispose(){
	m_ENPSetting.Apply("default");
    }
    
}
}
