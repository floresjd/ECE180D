﻿using UnityEngine;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;

using System;

public class PlayerMQTT_Y : MonoBehaviour {
    private const string str_IP = "127.0.0.1";
    private const int int_Port = 1883;
    private const string topic = "localization";
	public static int cur_lane_num = GameState.middle_lane;

    private MqttClient client;
	// Use this for initialization
	void Start () {
		// create client instance 
		client = new MqttClient(IPAddress.Parse(str_IP), int_Port , false , null ); 
		
		// register to message received 
		client.MqttMsgPublishReceived += client_MqttMsgPublishReceived; 
		
		string clientId = Guid.NewGuid().ToString(); 
		client.Connect(clientId); 
		
		// subscribe to the topic "/home/temperature" with QoS 2 
		client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }); 

	}
	void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) 
	{ 
		int lane_num;
		if (Int32.TryParse(System.Text.Encoding.UTF8.GetString(e.Message), out lane_num)) {
			if (lane_num != cur_lane_num) {
				cur_lane_num = lane_num;
			}
		} else {
			// Figure out how to handle this
		}
	} 

}
