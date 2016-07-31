#!/usr/bin/env python
# coding=utf-8

#
# Скрипт создания и обновления зеркала HUD.
#
# Copyright 2011 - 2016 EasyCoding Team (ECTeam).
# Copyright 2005 - 2016 EasyCoding Team.
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
import urllib, json, os, time, hashlib
from xml.dom import minidom
from datetime import datetime


def parsedb(dbname):
    result = []
    huddb = minidom.parse(dbname)
    for hud in huddb.getElementsByTagName('HUD'):
        result.append([hud.getElementsByTagName("InstallDir")[0].firstChild.data,
                       hud.getElementsByTagName("UpURI")[0].firstChild.data,
                       hud.getElementsByTagName("RepoPath")[0].firstChild.data,
                       int(hud.getElementsByTagName("LastUpdate")[0].firstChild.data)])
    return result


def gmt2unix(gtime):
    do = datetime.strptime(gtime, '%Y-%m-%dT%H:%M:%SZ')
    return int(time.mktime(do.timetuple()))


def getghinfo(repourl):
    url = repourl.replace('https://github.com/', 'https://api.github.com/repos/') + '/commits?per_page=1'
    response = urllib.urlopen(url).read()
    data = json.loads(response.decode())
    return [data[0]['sha'], gmt2unix(data[0]['commit']['committer']['date'])]


def downloadfile(url, name):
    dir = os.path.join(os.getcwd(), name)
    if not os.path.exists(dir):
        os.makedirs(dir)
    filepath = os.path.join(dir, '%s.zip' % name)
    urllib.urlretrieve(url, filepath)
    return filepath


def renamefile(fname, chash):
    dir = os.path.dirname(fname)
    result = os.path.join(dir, '%s_%s.zip' % (os.path.splitext(os.path.basename(fname))[0], chash[:8]))
    os.rename(fname, result)
    return result


def calculatehash(fname):
    return hashlib.sha1(open(fname, 'rb').read()).hexdigest()


def handlehud(name, url, repo, ltime):
    if repo.find('https://github.com/') != -1:
        r = getghinfo(repo)
        if r[1] > ltime:
            print('Available: %s, hash: %s, filename: %s.' % (name, r[0], renamefile(downloadfile(url, name), r[0])))
        else:
            print('%s is actual.' % name)
    else:
        f = downloadfile(url, name)
        print('Downloaded: %s, filename: %s.' % (name, renamefile(f, calculatehash(f))))


def main():
    try:
        for hud in parsedb('huds.xml'):
            handlehud(hud[0], hud[1], hud[2], hud[3])
    except:
        print('An error occurred. Try again later.')


if __name__ == '__main__':
    main()
