prefix = /usr/local

all: build

build:
	wget -nc https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
	mono nuget.exe install ./src/ParkitectNexus.Client.Linux/packages.config -outputdirectory ./packages
	mono nuget.exe install ./src/ParkitectNexus.Client.Base/packages.config -outputdirectory ./packages
	mono nuget.exe install ./src/ParkitectNexus.Data/packages.config -outputdirectory ./packages
	mono nuget.exe install ./src/ParkitectNexus.Mod.ModLoader/packages.config -outputdirectory ./packages
	xbuild /p:Configuration=Release ./src/ParkitectNexus.Client.Base/ParkitectNexus.Client.Base.csproj /target:build
	xbuild /p:Configuration=Release ./src/ParkitectNexus.Data/ParkitectNexus.Data.csproj /target:build
	xbuild /p:Configuration=Release ./src/ParkitectNexus.Mod.ModLoader/ParkitectNexus.Mod.ModLoader.csproj /target:build
	xbuild /p:Configuration=Release ./src/ParkitectNexus.Client.Linux/ParkitectNexus.Client.Linux.csproj /target:build
	rm -rf ./nuget.exe

install: build
		install -d $(DESTDIR)/opt/ParkitectNexus/ $(DESTDIR)/share/applications/
		install -m 775 src/ParkitectNexus.Client.Linux/bin/Release/*.dll $(DESTDIR)/opt/ParkitectNexus
		install -m 664 src/ParkitectNexus.Client.Linux/bin/Release/*.dll.mdb $(DESTDIR)/opt/ParkitectNexus
		install -m 775 src/ParkitectNexus.Client.Linux/bin/Release/*.exe $(DESTDIR)/opt/ParkitectNexus
		install -m 664 src/ParkitectNexus.Client.Linux/bin/Release/*.exe.mdb $(DESTDIR)/opt/ParkitectNexus
		install -m 644 -o root ./parkitectnexus.desktop $(DESTDIR)/share/applications/parkitectnexus.desktop
		install ./images/parkitectnexus_logo/parkitectnexus_logo-128x128.png $(DESTDIR)/opt/ParkitectNexus/parkitectnexus_logo.png

clean:
	git clean -x -d -f

uninstall:
		-rm -f $(DESTDIR)/opt/ParkitectNexus

.PHONY: all install clean distclean uninstall


