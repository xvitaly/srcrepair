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

from datetime import datetime
from hashlib import sha1
from json import loads
from os import path, getcwd, makedirs, rename
from shutil import rmtree
from time import mktime
from urllib import urlretrieve, urlopen
from xml.dom import minidom


def parsedb(dbname):
    result = []
    huddb = minidom.parse(dbname)
    for hud in huddb.getElementsByTagName('HUD'):
        result.append([hud.getElementsByTagName("InstallDir")[0].firstChild.data,
                       hud.getElementsByTagName("UpURI")[0].firstChild.data,
                       hud.getElementsByTagName("RepoPath")[0].firstChild.data,
                       int(hud.getElementsByTagName("LastUpdate")[0].firstChild.data),
                       path.basename(hud.getElementsByTagName("URI")[0].firstChild.data)])
    return result


def gmt2unix(gtime):
    do = datetime.strptime(gtime, '%Y-%m-%dT%H:%M:%SZ')
    return int(mktime(do.timetuple()))


def getghinfo(repourl):
    url = repourl.replace('https://github.com/', 'https://api.github.com/repos/') + '/commits?per_page=1'
    response = urlopen(url)
    respcode = response.getcode()
    if respcode != 200:
        raise Exception('GitHub API %d error code.' % respcode)
    data = loads(response.read().decode())
    return [data[0]['sha'], gmt2unix(data[0]['commit']['committer']['date'])]


def downloadfile(url, name):
    dir = path.join(getcwd(), 'huds', name)
    if not path.exists(dir):
        makedirs(dir)
    filepath = path.join(dir, '%s.zip' % name)
    urlretrieve(url, filepath)
    return filepath


def renamefile(fname, chash):
    dir = path.dirname(fname)
    result = path.join(dir, '%s_%s.zip' % (path.splitext(path.basename(fname))[0], chash[:8]))
    rename(fname, result)
    return result


def calculatehash(fname):
    return sha1(open(fname, 'rb').read()).hexdigest()


def handlehud(name, url, repo, ltime, lfname):
    if repo.find('https://github.com/') != -1:
        r = getghinfo(repo)
        if r[1] > ltime:
            print('%s has been updated. Hash: %s, time: %s, filename: %s.' % (name, r[0], r[1], path.basename(renamefile(downloadfile(url, name), r[0]))))
        else:
            print('%s is up to date.' % name)
    else:
        filednl = downloadfile(url, name)
        fullfile = renamefile(filednl, calculatehash(filednl))
        shortfile = path.basename(fullfile)
        if shortfile == lfname:
            print('%s downloaded. Filename: %s.' % (name, shortfile))
        else:
            rmtree(path.dirname(fullfile))
            print('%s is up to date.' % name)


def main():
    try:
        for hud in parsedb('huds.xml'):
            handlehud(hud[0], hud[1], hud[2], hud[3], hud[4])
    except Exception as ex:
        print('An error occurred: %s' % ex.message)


if __name__ == '__main__':
    main()
