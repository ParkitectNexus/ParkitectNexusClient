cd out
git clone https://github.com/probonopd/AppImageKit.git

sudo ./AppImageKit/build.sh

wget -c --trust-server-names https://download.fedoraproject.org/pub/fedora/linux/releases/23/Workstation/x86_64/iso/Fedora-Live-Workstation-x86_64-23-10.iso
wget -c --trust-server-names http://releases.ubuntu.com/14.04.4/ubuntu-14.04.4-desktop-amd64.iso?_ga=1.29637303.929741532.1454222602
wget-c --trust-server-names https://sfo1.dl.elementary.io/download/MTQ1NjgwODA1OA==/elementaryos-0.3.2-stable-amd64.20151209.iso

sudo ./AppImageKit/AppImageAssistant.AppDir/testappimage ./Fedora-Live-Workstation-x86_64-23-10.iso ./parkitectnexus 
sudo ./AppImageKit/AppImageAssistant.AppDir/testappimage ./ubuntu-14.04.4-desktop-amd64.iso?_ga=1.29637303.929741532.1454222602 ./parkitectnexus 
sudo ./AppImageKit/AppImageAssistant.AppDir/testappimage ./elementaryos-0.3.2-stable-amd64.20151209.iso ./parkitectnexus 
