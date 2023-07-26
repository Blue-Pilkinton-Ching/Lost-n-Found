# Lost n Found

Unity Version 2022.3.4f

During Production build:
- Uncheck Test Mode in Vivox project settings
- Change Authentication to not create a random profile
- Change Vivox to not be creating development tolkens using LoginSession.GetLoginToken()
 https://docs.vivox.com/v5/general/unity/5_15_0/en-us/Default.htm#Unity/developer-guide/vivox-unity-sdk-basics/sign-in-to-game.htm?TocPath=Vivox%2520Unity%2520SDK%2520documentation%257CVivox%2520Unity%2520Developer%2520Guide%257CClient%2520SDK%2520basics%257C_____3

 To do:

 - Lower the poly count of the entity mesh


Entity Intention Value

0 is Idle
> 0 is looking around

> 0.5 is looking around to walking

> 1 is looking around to sprinting

> 1.5 is face player to sprinting

Optimising shaders and Textures
https://chat.openai.com/share/ddea7017-d056-424e-932d-6565870dde63