Name: srcrepair
Version: 9.0
Release: 1
Group: Applications/Utilities
BuildArch: noarch
Summary: SRC Repair is a free tool that can be used for tuning & restoring Steam and all Source engine games
URL: http://www.easycoding.org/projects/srcrepair
License: GPL
Source: %{name}.tar.gz
BuildRoot: %{_tmppath}/%{name}-root
Vendor: EasyCoding Team
Packager: V1TSK <vitaly@easycoding.org>
Requires: mono-winforms

%description
SRC Repair is a free tool that can be used for tuning & restoring Steam and all Source engine games.
You can also create and edit .cfg files (configs), backup game or Steam settings, and apply your
FPS-config or spray into game. SRC Repair does not needs any configuration to work.

%prep
%setup -q -n %{name}

%build
rm -rf %{buildroot}
mkdir -p %{buildroot}/usr/bin/
mkdir -p %{buildroot}/usr/share/srcrepair/
mkdir -p %{buildroot}/usr/share/applications/
cp -fpr srcrepair %{buildroot}/usr/bin/
cp -fpr srcrepair.desktop %{buildroot}/usr/share/applications/
rm -f srcrepair
rm -f srcrepair.desktop
cp -fpr * %{buildroot}/usr/share/srcrepair/
chmod +x %{buildroot}/usr/bin/srcrepair

%install

%files
/usr/bin/srcrepair
/usr/share/applications/srcrepair.desktop
/usr/share/srcrepair/srcrepair.exe
/usr/share/srcrepair/srcrepair.exe.config
/usr/share/srcrepair/srcrepair.exe.sig
/usr/share/srcrepair/srcrepair.ico
/usr/share/srcrepair/srcrepair.pdb
/usr/share/srcrepair/ru/srcrepair.resources.dll
/usr/share/srcrepair/ru/srcrepair.resources.dll.sig
/usr/share/srcrepair/cfgs/bonhighfps.cfg
/usr/share/srcrepair/cfgs/bonhighfps_en.txt
/usr/share/srcrepair/cfgs/bonhighfps_ru.txt
/usr/share/srcrepair/cfgs/chrismaxfps.cfg
/usr/share/srcrepair/cfgs/chrismaxfps_en.txt
/usr/share/srcrepair/cfgs/chrismaxfps_ru.txt
/usr/share/srcrepair/cfgs/config_default.cfg
/usr/share/srcrepair/cfgs/dt_legal.cfg
/usr/share/srcrepair/cfgs/dt_legal_en.txt
/usr/share/srcrepair/cfgs/dt_legal_ru.txt
/usr/share/srcrepair/cfgs/m0refps.cfg
/usr/share/srcrepair/cfgs/m0refps_en.txt
/usr/share/srcrepair/cfgs/m0refps_ru.txt
/usr/share/srcrepair/cfgs/m0rehighfps.cfg
/usr/share/srcrepair/cfgs/m0rehighfps_en.txt
/usr/share/srcrepair/cfgs/m0rehighfps_ru.txt
/usr/share/srcrepair/cfgs/skullhighfps.cfg
/usr/share/srcrepair/cfgs/skullhighfps_en.txt
/usr/share/srcrepair/cfgs/skullhighfps_ru.txt
/usr/share/srcrepair/cfgs/team-fortress-ru.cfg
/usr/share/srcrepair/cfgs/team-fortress-ru_en.txt
/usr/share/srcrepair/cfgs/team-fortress-ru_ru.txt
/usr/share/srcrepair/cfgs/v1tsk's_generic.cfg
/usr/share/srcrepair/cfgs/v1tsk's_generic_en.txt
/usr/share/srcrepair/cfgs/v1tsk's_generic_ru.txt
/usr/share/srcrepair/changelog.txt
/usr/share/srcrepair/games.xml
/usr/share/srcrepair/GPL.txt
/usr/share/srcrepair/readme.txt
/usr/share/srcrepair/readme_en.txt
/usr/share/srcrepair/pubkey.asc
