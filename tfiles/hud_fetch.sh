#!/bin/bash

# 
# Скрипт создания зеркала HUD.
# 
# Copyright 2011 - 2015 EasyCoding Team (ECTeam).
# Copyright 2005 - 2015 EasyCoding Team.
# 
# Лицензия: GPL v3 (см. файл GPL.txt).
# Лицензия контента: Creative Commons 3.0 BY.
# 
# Запрещается использовать этот файл при использовании любой
# лицензии, отличной от GNU GPL версии 3 и с ней совместимой.
# 
# Официальный блог EasyCoding Team: http://www.easycoding.org/
# Официальная страница проекта: http://www.easycoding.org/projects/srcrepair
# 
# Более подробная инфорация о программе в readme.txt,
# о лицензии - в GPL.txt.
# 

function fetch_hud
{
    # Выводим текст, оповещающий пользователя о начале загрузки...
    echo -n "Downloading $3..."

    # Проверяем существование файла со скриншотом и если он не существует, загружаем...
    if [ ! -f "$3.png" ]; then
        # Загружаем скриншот с удалённого сервера...
        wget $1 -O $3.png > /dev/null 2> /dev/null  
    fi
    
    # Проверим существование каталога для HUD и если он не существует, создаём...
    if [ ! -d "$3" ]; then
        mkdir -p $3
    fi

    # Проверяем существование архива и если он не существует, загружаем...
    if [ ! -f "$3/$3.zip" ]; then
        # Загружаем новую версию архива из апстрима...
        wget $2 -O $3/$3.zip > /dev/null 2> /dev/null
    fi

    # Генерируем окончательное имя архива...
    nf=$(sha256sum $3/$3.zip | awk '{print $1}')
    mv $3/$3.zip $3/$3_${nf:0:8}.zip
    
    # Выводим текст, оповещающий пользователя о завершении загрузки...
    echo " Done."
}

fetch_hud http://huds.tf/7hud/img/main.png https://github.com/Sevin7/7HUD/archive/master.zip 7HUD
fetch_hud http://huds.tf/herganhud/img/main.png https://github.com/Hergan5/herganhud/archive/master.zip herganhud
fetch_hud http://huds.tf/pikleshud/img/main.png https://github.com/piklestf2/pikles-hud/archive/master.zip pikles-hud
fetch_hud http://huds.tf/broeselhudblue/img/main.png https://github.com/fblue/broeselhud_blue/archive/master.zip broeselhud_blue
fetch_hud http://huds.tf/zhud/img/main.png https://github.com/z4-/zhud/archive/master.zip zhud
fetch_hud http://huds.tf/basthud/img/main.png https://github.com/basbanaan/bastHUD/archive/master.zip basthud
fetch_hud http://huds.tf/takyahud/img/main.png https://github.com/takram/takyahud-classic/archive/master.zip takyahud
fetch_hud http://huds.tf/sirhud/img/main.png https://github.com/sirgrey/SirHUD/archive/master.zip sirhud
fetch_hud http://huds.tf/tf2hudplus/img/main.png https://github.com/SnowshoeIceboot/TF2HudPlus/archive/master.zip tf2hudplus
fetch_hud http://huds.tf/rpvhud/img/main.png https://github.com/harvardbb/rpvhud/archive/master.zip rpvhud
fetch_hud http://huds.tf/toonhud/img/main.png https://www.dropbox.com/s/1x742x8fjay2idn/ToonHUD.zip toonhud
fetch_hud http://huds.tf/warhud/img/main.png https://github.com/wareya/warHUD-TF/archive/master.zip warhud
fetch_hud http://huds.tf/doodlehud/img/main.png https://github.com/DougieDoodles/doodlehud/archive/master.zip doodlehud
fetch_hud http://huds.tf/voidhud/img/main.png https://github.com/TheStaticVoid/VoidHUD2.0/archive/master.zip voidhud
fetch_hud http://huds.tf/ahud/img/main.png https://github.com/n0kk/ahud/archive/master.zip ahud
fetch_hud http://huds.tf/cbhud/img/main.png https://github.com/ColdBalls/CBHUD/archive/master.zip cbhud
fetch_hud http://huds.tf/calmlikeabomb/img/main.png https://github.com/Slayer89/slayhud/archive/master.zip calmlikeabomb
fetch_hud http://huds.tf/ejphud/img/main.png https://github.com/basbanaan/EJP-HUD/archive/master.zip ejphud
fetch_hud http://huds.tf/flathud/img/main.png https://github.com/flatlinee/flatHUD-/archive/master.zip flathud
fetch_hud http://huds.tf/harvardhud/img/main.png https://github.com/harvardbb/harvardhud/archive/master.zip harvardhud
fetch_hud http://huds.tf/bwhud/img/main.png https://github.com/bw-/bw-HUD/archive/master.zip bwhud
fetch_hud http://huds.tf/budhud/img/main.png https://github.com/WhiskerBiscuit/budhud/archive/master.zip budhud
fetch_hud http://huds.tf/idhud/img/main.png https://github.com/Eniere/idhud/archive/master.zip idhud
fetch_hud http://huds.tf/jedihud/img/main.png https://gitgud.io/JediThug/JediHUD/repository/archive.zip jedihud
fetch_hud http://huds.tf/noto/img/main.png https://github.com/omnibombulator/noto/archive/master.zip noto
fetch_hud http://huds.tf/riethud/img/main.png https://github.com/TheRiet/rietHUD/archive/master.zip riethud
fetch_hud http://huds.tf/morghud/img/main.png https://github.com/ItsMorgus/MorgHUD/archive/master.zip morghud
fetch_hud http://huds.tf/minthud/img/main.png https://github.com/Rawrsor/DrooHUD/archive/master.zip minthud
fetch_hud http://huds.tf/kbnhud/img/main.png https://github.com/Jotunn/KBNHud/archive/master.zip kbnhud
fetch_hud http://huds.tf/medhud/img/main.png https://github.com/Intellectualbadass/medHUD/archive/master.zip medhud
fetch_hud http://huds.tf/prismhud/img/main.png https://github.com/JarateKing/PrismHUD/archive/master.zip prismhud
fetch_hud http://huds.tf/rayshud/img/main.png https://github.com/raysfire/rayshud/archive/master.zip rayshud
fetch_hud http://huds.tf/evehud/img/main.png http://files.gamebanana.com/guis/eve_hud_v371.zip evehud
fetch_hud http://huds.tf/biscottihud/img/main.png https://github.com/MedicodiBiscotti/biscottiHUD/archive/master.zip biscottihud
fetch_hud http://huds.tf/flamehud/img/main.png https://github.com/mattr0d/flamehud/archive/master.zip flamehud
fetch_hud http://huds.tf/frankenhud/img/main.png https://github.com/TheKins/frankenhud/archive/master.zip frankenhud
fetch_hud http://huds.tf/isaachud/img/main.png https://github.com/Xeletron/Isaac-Hud/archive/master.zip isaachud
fetch_hud http://huds.tf/jayhud/img/main.png https://github.com/kermit-tf/JayHUD/archive/master.zip jayhud
fetch_hud http://huds.tf/mannterface/img/main.png https://github.com/mannterface/Mannterface/archive/master.zip mannterface
fetch_hud http://huds.tf/yayahud/img/main.png https://github.com/Yttrium-tYcLief/yayahud/archive/master.zip yayahud
fetch_hud http://huds.tf/smesihud/img/main.png https://github.com/Smesi/SmesiHud/archive/master.zip smesihud
fetch_hud http://huds.tf/rainhud/img/main.png https://github.com/rainoflight/rainhud/archive/master.zip rainhud
fetch_hud http://huds.tf/solarhud/img/main.png https://github.com/Stochast1c/solarhud/archive/master.zip solarhud
fetch_hud http://huds.tf/yshud/img/main.png https://github.com/Yo5hi/ysHUD/archive/master.zip yshud
