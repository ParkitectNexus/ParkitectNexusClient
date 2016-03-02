APP=ParkitectNexus
MONO_URL=https://kojipkgs.fedoraproject.org//packages/mono/4.2.2/3.fc24/x86_64/mono-core-4.2.2-3.fc24.x86_64.rpm

mkdir -p ./$APP/$APP.AppDir/usr/bin
cd ./$APP

cd $APP.AppDir/

wget "$MONO_URL"-O - | rpm2cpio | cpio -idm


