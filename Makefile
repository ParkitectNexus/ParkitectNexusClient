PREFIX = /usr/

all: build

build:
	xbuild /p:Configuration=Release src/ParkitectNexus.Client.Base/ParkitectNexus.Client.Base.csproj /target:build
	xbuild /p:Configuration=Release src/ParkitectNexus.Data/ParkitectNexus.Data.csproj /target:build
	xbuild /p:Configuration=Release src/ParkitectNexus.Mod.ModLoader/ParkitectNexus.Mod.ModLoader.csproj /target:build
	xbuild /p:Configuration=Release src/ParkitectNexus.Client.Linux/ParkitectNexus.Client.Linux.csproj /target:build


install: build
		install -d $(DESTDIR)/opt/ParkitectNexus/ $(DESTDIR)$(PREFIX)/share/applications/
		install -m 775 src/ParkitectNexus.Client.Linux/bin/Release/*.dll $(DESTDIR)/opt/ParkitectNexus
		install -m 664 src/ParkitectNexus.Client.Linux/bin/Release/*.dll.mdb $(DESTDIR)/opt/ParkitectNexus
		install -m 664 src/ParkitectNexus.Client.Linux/bin/Release/*.config $(DESTDIR)/opt/ParkitectNexus
		install -m 775 src/ParkitectNexus.Client.Linux/bin/Release/*.exe $(DESTDIR)/opt/ParkitectNexus
		install -m 664 src/ParkitectNexus.Client.Linux/bin/Release/*.exe.mdb $(DESTDIR)/opt/ParkitectNexus
		install -m 644 -o root ./parkitectnexus.desktop $(DESTDIR)$(PREFIX)/share/applications/parkitectnexus.desktop
		install ./images/parkitectnexus_logo/parkitectnexus_logo-128x128.png $(DESTDIR)/opt/ParkitectNexus/parkitectnexus_logo.png

clean:
	xbuild /p:Configuration=Release src/ParkitectNexus.Client.Base/ParkitectNexus.Client.Base.csproj /target:clean
	xbuild /p:Configuration=Release src/ParkitectNexus.Data/ParkitectNexus.Data.csproj /target:clean
	xbuild /p:Configuration=Release src/ParkitectNexus.Mod.ModLoader/ParkitectNexus.Mod.ModLoader.csproj /target:clean
	xbuild /p:Configuration=Release src/ParkitectNexus.Client.Linux/ParkitectNexus.Client.Linux.csproj /target:clean
	rm -rf ./nuget.exe
distclean: clean

.PHONY: all install clean distclean


