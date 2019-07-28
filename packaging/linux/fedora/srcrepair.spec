%global debug_package %{nil}

%global nuget_version 5.0.2
%global dotnetzip_version 1.13.3
%global nlog_version 4.6.6

Name: srcrepair
Version: 35.0.0
Release: 1%{?dist}
Summary: A free open source tool for tuning and cleaning up Source engine games

License: GPLv3+
URL: https://github.com/xvitaly/%{name}
Source0: %{url}/archive/v%{version}.tar.gz#/%{name}-%{version}.tar.gz
Source1: https://dist.nuget.org/win-x86-commandline/v%{nuget_version}/nuget.exe#/nuget-%{nuget_version}.exe
Source2: https://www.nuget.org/api/v2/package/DotNetZip/%{dotnetzip_version}#/dotnetzip.%{dotnetzip_version}.nupkg
Source3: https://www.nuget.org/api/v2/package/NLog/%{nlog_version}#/nlog.%{nlog_version}.nupkg
BuildArch: noarch

BuildRequires: desktop-file-utils
BuildRequires: libappstream-glib
BuildRequires: ca-certificates
BuildRequires: mono-winforms
BuildRequires: mono-complete
BuildRequires: mono-devel

Requires: hicolor-icon-theme
Requires: mono-winforms

%description
SRC Repair (ex. TF2 Repair) is a free open source tool that can be
used for tuning and cleaning up Steam and Source engine games. You
can also create and edit .cfg files (configs), backup and restore
game settings, apply FPS configs or sprays into the game with one
click.

%prep
%autosetup -p1
mkdir nuget
cp -f {%{SOURCE1},%{SOURCE2},%{SOURCE3}} nuget/

cat /etc/ssl/certs/* > bundle.pem
cert-sync --user bundle.pem

mono nuget/nuget-%{nuget_version}.exe install DotNetZip -Source $PWD/nuget -OutputDirectory packages
mono nuget/nuget-%{nuget_version}.exe install NLog -Source $PWD/nuget -OutputDirectory packages

%build
xbuild /p:Configuration=Release %{name}.sln

%install
# Installing directories...
install -d %{buildroot}%{_bindir}
install -d %{buildroot}%{_prefix}/lib/%{name}
install -d %{buildroot}%{_prefix}/lib/%{name}/ru
install -d %{buildroot}%{_prefix}/lib/%{name}/cfgs
install -d %{buildroot}%{_datadir}/icons/hicolor/scalable/apps
install -d %{buildroot}%{_metainfodir}

# Installing base application files...
install -m 0644 -p %{name}/bin/Release/%{name}.exe %{buildroot}%{_prefix}/lib/%{name}
install -m 0644 -p %{name}/bin/Release/*.{dll,pdb,config} %{buildroot}%{_prefix}/lib/%{name}

# Installing assets...
install -m 0644 -p %{name}/bin/Release/{configs,games,huds}.xml %{buildroot}%{_prefix}/lib/%{name}
install -m 0644 -p %{name}/bin/Release/cfgs/*.cfg %{buildroot}%{_prefix}/lib/%{name}/cfgs

# Installing localizations...
install -m 0644 -p %{name}/bin/Release/ru/%{name}.resources.dll %{buildroot}%{_prefix}/lib/%{name}/ru

# Installing launcher, icon and desktop file...
install -m 0755 -p packaging/linux/%{name}.sh %{buildroot}%{_bindir}/%{name}
install -m 0644 -p packaging/linux/%{name}.appdata.xml %{buildroot}%{_metainfodir}
install -m 0644 -p assets/img_sources/%{name}.svg %{buildroot}%{_datadir}/icons/hicolor/scalable/apps
desktop-file-install --dir=%{buildroot}%{_datadir}/applications packaging/linux/%{name}.desktop

%check
appstream-util validate-relax --nonet %{buildroot}%{_metainfodir}/%{name}.appdata.xml

%files
%doc README.md
%license COPYING
%{_bindir}/%{name}
%{_prefix}/lib/%{name}
%{_datadir}/applications/%{name}.desktop
%{_datadir}/icons/hicolor/scalable/apps/%{name}.svg
%{_metainfodir}/%{name}.appdata.xml

%changelog
* Wed Jun 19 2019 Vitaly Zaitsev <vitaly@easycoding.org> - 35.0.0-1
- Initial SPEC release.
