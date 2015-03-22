#!/bin/bash

function fetch_hud
{
    # Проверяем существование файла со скриншотом и если он не существует, загружаем...
    if [ ! -f "$3.png" ]; then
        # Загружаем скриншот с удалённого сервера...
        wget $1 -O $3.png > /dev/null 2> /dev/null  
    fi
    
    # Проверяем существование архива и если он не существует, загружаем...
    if [ ! -f "$3.zip" ]; then
        # Загружаем новую версию архива из апстрима...
        wget $2 -O $3.zip > /dev/null 2> /dev/null
    fi
}

fetch_hud http://huds.tf/img/main/7hud.png https://github.com/Sevin7/7HUD/archive/master.zip 7HUD
fetch_hud http://huds.tf/img/main/herganhud.png https://github.com/Hergan5/herganhud/archive/master.zip herganhud
fetch_hud http://huds.tf/img/main/pikleshud.png https://github.com/piklestf2/pikles-hud/archive/master.zip pikles-hud
fetch_hud http://huds.tf/img/main/broeselhudblue.png https://github.com/fblue/broeselhud_blue/archive/master.zip broeselhud_blue
fetch_hud http://huds.tf/img/main/zhud.png https://github.com/z4-/zhud/archive/master.zip zhud
fetch_hud http://huds.tf/img/main/basthud.png https://github.com/basbanaan/bastHUD/archive/master.zip basthud
fetch_hud http://huds.tf/img/main/takyahud.png https://github.com/takram/takyahud-classic/archive/master.zip takyahud
fetch_hud http://huds.tf/img/main/sirhud.png https://github.com/sirgrey/SirHUD/archive/master.zip sirhud
fetch_hud http://huds.tf/img/main/tf2hudplus.png https://github.com/SnowshoeIceboot/TF2HudPlus/archive/master.zip tf2hudplus
fetch_hud http://huds.tf/img/main/rpvhud.png https://github.com/harvardbb/rpvhud/archive/master.zip rpvhud
fetch_hud http://huds.tf/img/main/toonhud.png https://www.dropbox.com/s/1x742x8fjay2idn/ToonHUD.zip toonhud
fetch_hud http://huds.tf/img/main/warhud.png https://github.com/wareya/warHUD-TF/archive/master.zip warhud
fetch_hud http://huds.tf/img/main/doodlehud.png https://github.com/DougieDoodles/doodlehud/archive/master.zip doodlehud
fetch_hud http://huds.tf/img/main/voidhud.png https://github.com/TheStaticVoid/VoidHUD2.0/archive/master.zip voidhud
fetch_hud http://huds.tf/img/main/ahud.png https://github.com/n0kk/ahud/archive/master.zip ahud
fetch_hud http://huds.tf/img/main/cbhud.png https://github.com/ColdBalls/CBHUD/archive/master.zip cbhud
fetch_hud http://huds.tf/img/main/calmlikeabomb.png https://github.com/Slayer89/slayhud/archive/master.zip calmlikeabomb
fetch_hud http://huds.tf/img/main/ejphud.png https://github.com/basbanaan/EJP-HUD/archive/master.zip ejphud
fetch_hud http://huds.tf/img/main/flathud.png https://github.com/flatlinee/flatHUD-/archive/master.zip flathud
fetch_hud http://huds.tf/img/main/harvardhud.png https://github.com/harvardbb/harvardhud/archive/master.zip harvardhud
fetch_hud http://huds.tf/img/main/bwhud.png https://github.com/bw-/bw-HUD/archive/master.zip bwhud
fetch_hud http://huds.tf/img/main/budhud.png https://github.com/WhiskerBiscuit/budhud/archive/master.zip budhud
fetch_hud http://huds.tf/img/main/idhud.png https://github.com/Eniere/idhud/archive/master.zip idhud
fetch_hud http://huds.tf/img/main/jedihud.png https://gitgud.net/JediThug/jedihud/repository/archive.zip jedihud
