# YWOS
R e a d m e
-----------
YWOS is just a simple script to manage things in Space Engineers(for Ships/Space Station) in Programmable Block, its FAR AWAY from finished and even more far away to Perfect
its my first C# project, so pleas be forgiving


How to Use:
1. Screen: Name a Screen "MenuLCD"
2. Place a ButtonPanel in front of the Screen Whit follow Buttons:
Link the Button to the Programming Block on Run whit follow Argmunents:
-UP: Argument: UP
-Down: Argument: Down
-Enter: Argument: Enter
-Back: Argument: Back

!!NO TIMER BLOCK NEEDED!!


Features:

Info/Warning Menu:
Show Info/Warning Messanges,whit A Prio,A Name and a Text, Warnings will Noticed in Menu too(at bottom)
Can be Addes from Extern, or Shows Warning from setting(Like for low Energy)

Show External Messages in Info/Warning Menu:
!!Warning: Use the the formatting of the example or Script will crash!!
MSG|TYPE|PRIO|TEXT|USER
Example: MSG|1|1|101|HELP|InfoScript
Types: 0 - Info, 1 - Warning


Show only Mode: 
Mode to Only Show,deaktivates all automatic actions,like turn off/on Geenerators
Aktivation: in Setting/Generas Settings or External whit the Argument "ShowOnly"



Menu Features:

Settings:

Energy Warning: 
Setting for warn you if Batter Charge go under set value

Genarator Setting:
Turns on Generators when Battery Charge go under set Value(1 Gnerator or All Generators)

Alarm Mode:
(Can be Aktivated External by using "Alarm" Run Argument )
Aktivate Alarm Mode Change Follow Things
- Turn all Lights in Set Value(Like red)
- Close all Doors
- Aktivate all external Weapons (Missle and Gatling turrets)

Auto Door Closer:
-Set Ticks for Close intervalle
-Works for Gates and Doors



Status Screens:
- Battery Status: Screen for all Battery witch shows Battery Charges
- Energy Status: Shows Running Generators,Solar Power and General Battery Charge
- Inventory Screen: Just show the General filling of inventory yet in percent
- Connector Screen: Shows Connectors, thair Status and if Conencted thair ships and the Ship Battery Status









