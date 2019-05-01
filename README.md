# IP-Network-8-Channel-Relay-Control-Board-Controlled-by-VB.net-in-LAN
IP-Network-8-Channel-Relay-Control-Board-Controlled-by-VB.net-in-LAN
This code is a sample project for 8 channel or 32 channel relay control in LAN by Sokcet. It's very easy to use. Only set the IP and Port and click "Scan" button for Initialization.

The software according to KinCony's KC868-H8/W or KC868-H32/W smart controller's protocol. The protocol is easy to use.When socket connected, send string to controller, the relay will be ON/OFF.You 

can change it for other devices. Also the software support not only network ,but also can control by RS232 port. It's same protocol. This project use VB2005 coding.


***main protocol for relay control:***

1.Check the device model
Send: RELAY-SCAN_DEVICE-NOW
Return: RELAY-SCAN_DEVICE-CHANNEL_8/CHANNEL_32, OK/ERROR

2.Open the working mode of the device server:
Send: RELAY-TEST-NOW
Return: HOST-TEST-START

3.Command set of device initialization:
Send: RELAY–SCAN_DEVICE–NOW
Send: RELAY-TEST-NOW

4.Separately control one relay to be ON and OFF:
Send: RELAY-SET-x (1 byte pack_num), x (1 byte relay serial number), x (1 byte action 0 / 1) Return: RELAY-SET-x (1 byte pack_num), X (1 byte relay serial number), x (1 byte action 0 / 1), OK/ERROR

5.Separately check the current switch status of one relay (as below picture):
Send: RELAY-READ-x (1 byte pack_num), x (1 byte relay sequence number)
Return: RELAY-READ-x (1 byte pack_num), x (1 byte relay sequence number), x (1 byte status 0 / 1), OK/ERROR

6.Checking trigger input status:
Send: RELAY-GET_INPUT-x (1 byte pack_num)
Return: RELAY-GET_INPUT-x (1 byte pack_num), x (1 byte state), OK/ERROR

7.Checking the serial number of the device:
Send: RELAY-HOST-NOW
Return: HOST-CHKLIC-56a890e6888793c918f151b5 (return the serial number).

8.One-time control of multiple relay on and off:

KC868-H8:
Send: RELAY-SET_ALL-x (1 byte pack_num), D0
Return: RELAY-SET_ALL-x (1 byte pack_num), D0, OK/ERROR

KC868-H32:
Send: RELAY-SET_ALL-x (1 byte pack_num), D3, D2, D1, D0
Return: RELAY-SET_ALL-x (1 byte pack_num), D3,D2,D1,D0,OK/ERROR

9.Read multiple relays current switch status at a time:
Send: RELAY-STATE-x (1 byte pack_num)
Return:
KC868-H8: RELAY-STATE-x (1 byte pack_num), D0, OK/ERROR
KC868-H32: RELAY-STATE-x (1 byte pack_num), D3, D2, D1, D0, OK/ERROR

![VB window](https://www.kincony.com/wp-content/uploads/2019/03/vb2005-1.jpg)

![Code window](https://www.kincony.com/wp-content/uploads/2019/03/vb2005-2.jpg)

![Network Relay Board](https://www.kincony.com/wp-content/uploads/2019/03/7-open-light.jpg)

![Turn ON Relay-1](https://www.kincony.com/wp-content/uploads/2019/03/6-Set-1.jpg)

***If you need the hardware in here:***

32 Channel relay controller: https://www.aliexpress.com/store/product/32-Channel-Relay-controller-Module-Ethernet-RS232-PC-Serial-Port-Smart-Home-Control-tcp-ip/807891_555335460.html?spm=a2g1y.12024536.productList_14403156.pic_0

8 Channel relay controller: https://www.aliexpress.com/store/product/8-Channel-Relay-controller-Module-Ethernet-RS232-PC-Serial-Port-Smart-Home-Control-tcp-ip/807891_555377986.html?spm=a2g1y.12024536.productList_14403156.pic_2

If you are looking for network relay remote control solutions , see more details can visit KinCony's webpage: https://www.kincony.com
